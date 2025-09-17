// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Buffers.Binary;
using System.Net;
using System.Text;
using Midori.Logging;
using Midori.Utils;
using Miyu.Attributes;
using Miyu.Events;
using Miyu.Events.Voice;
using Miyu.Models.Channels;
using Miyu.Models.Guilds;
using Miyu.Networking.Gateway;
using Miyu.Networking.Gateway.Payloads;
using Miyu.Networking.WebSocket;
using Miyu.Utils;
using Miyu.Voice.Payloads;
using Newtonsoft.Json.Linq;

namespace Miyu.Voice;

public class VoiceManager
{
    internal static Logger Logger { get; } = Logger.GetLogger("Miyu-Voice");

    private static readonly List<string> supportedModes =
    [
        "xsalsa20_poly1305_lite",
        "xsalsa20_poly1305_suffix",
        "xsalsa20_poly1305"
    ];

    private readonly MiyuClient client;
    internal DebugEventLogger VoiceDebug { get; }
    private IWebSocketClient? voiceWebSocket { get; set; }

    private ulong? guild;
    private string? token;

    private uint ssrc;
    private byte[] key = [];

    private UdpConnection udp = null!;

    private CancellationTokenSource? sendSource;
    private CancellationTokenSource? receiveSource;

    private Task? sendTask;
    private Task? receiveTask;

    public VoiceManager(MiyuClient client)
    {
        this.client = client;
        client.RegisterListeners(this);

        VoiceDebug = new DebugEventLogger("voice")
        {
            CodeMappings = Enum.GetValues<VoiceOpCode>().ToDictionary(x => (int)x, x => x.ToString())
        };
    }

    public void Connect(DiscordChannel channel, DiscordGuild? guild = null)
    {
        if (voiceWebSocket is not null)
            return;

        var payload = new GatewayPayload
        {
            OpCode = GatewayOpCode.VoiceStateUpdate,
            Data = new GatewayVoiceStateUpdate
            {
                GuildID = guild?.ID ?? 0,
                ChannelID = channel.ID,
                SelfDeaf = false,
                SelfMute = true
            }
        };

        _ = client.SendPayload(payload.Serialize());
    }

    [EventListener(EventType.VoiceServerUpdate)]
    public Task OnVoiceServer(VoiceServerUpdateEvent ev)
    {
        try
        {
            Logger.Add($"{ev}");

            token = ev.RawEvent.Token;
            guild = ev.RawEvent.GuildID;

            voiceWebSocket = client.CreateWebsocket();
            voiceWebSocket.OnDisconnect = () => voiceWebSocket = null;
            voiceWebSocket.OnMessage = m =>
            {
                if (client.BeforeHandle is null)
                    handlePayload(m);
                else
                    client.BeforeHandle(m, handlePayload);
            };

            _ = voiceWebSocket.ConnectAsync($"wss://{ev.RawEvent.Endpoint}?encoding=json&v=8");
        }
        catch (Exception ex)
        {
            Logger.Add($"Failed to connect to voice server: {ex.Message}", LogLevel.Error, ex);
            voiceWebSocket = null;
        }

        return Task.CompletedTask;
    }

    #region Heartbeat

    private int heartbeatInterval;
    private long sequence = 1;

    private async Task heartbeatLoop()
    {
        try
        {
            var socket = voiceWebSocket!;

            while (socket.Connected)
            {
                await Task.Delay(heartbeatInterval);
                await sendHeartbeat();
            }
        }
        catch (TaskCanceledException)
        {
            Logger.Add("heartbeat loop cancelled", LogLevel.Debug);
        }
    }

    private async Task sendHeartbeat()
    {
        await sendPayload(new VoiceGatewayPayload
        {
            OpCode = VoiceOpCode.Heartbeat,
            Data = new
            {
                t = DateTimeOffset.Now.ToUnixTimeSeconds(),
                seq_ack = sequence++
            }
        });
    }

    #endregion

    #region payloads

    private async void handlePayload(string message)
    {
        try
        {
            VoiceDebug.Log(message, true);
            var payload = message.Deserialize<VoiceGatewayPayload>();
            if (payload == null) return;

            switch (payload.OpCode)
            {
                case VoiceOpCode.Hello:
                {
                    var jObj = payload.Data as JObject;
                    var hello = jObj?.ToObject<VoiceHelloPayload>();

                    if (hello == null)
                        throw new InvalidOperationException("expected hello payload");

                    await onHello(hello);
                    break;
                }

                case VoiceOpCode.Ready:
                {
                    var jObj = payload.Data as JObject;
                    var ready = jObj?.ToObject<VoiceReadyPayload>();

                    if (ready == null)
                        throw new InvalidOperationException("expected ready payload");

                    ssrc = ready.Ssrc;
                    await Task.Run(() => connectToUdp(ready));
                    _ = Task.Run(heartbeatLoop);
                    break;
                }

                case VoiceOpCode.SessionDescription:
                {
                    var jObj = payload.Data as JObject;
                    var sd = jObj?.ToObject<VoiceSessionDescription>();

                    if (sd == null)
                        throw new InvalidOperationException("expected sd payload");

                    key = sd.SecretKey;
                    _ = postSessionDescription(sd);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Add("error handling gateway message", LogLevel.Error, ex);
            await File.WriteAllTextAsync("gateway_error.json", message);
        }
    }

    private async Task onHello(VoiceHelloPayload hello)
    {
        await sendIdentify();
        heartbeatInterval = hello.HeartbeatInterval;
    }

    private async Task sendIdentify()
    {
        await sendPayload(new VoiceGatewayPayload
        {
            OpCode = VoiceOpCode.Identify,
            Data = new
            {
                server_id = guild,
                user_id = client.Self.ID,
                session_id = client.SessionID,
                token
            }
        });
    }

    private async Task sendPayload(VoiceGatewayPayload payload)
    {
        if (voiceWebSocket == null)
            throw new InvalidOperationException("Voice WebSocket is not connected.");

        var data = payload.Serialize();

        VoiceDebug.Log(data, false);
        await voiceWebSocket.SendAsync(data);
    }

    #endregion

    #region UDP

    private async void connectToUdp(VoiceReadyPayload ready)
    {
        udp = new UdpConnection();
        udp.Setup(new IPEndPoint(IPAddress.Parse(ready.IPAddress), ready.Port));

        {
            // send ip discovery
            var buffer = new byte[74];
            Array.Fill(buffer, (byte)0);

            var tb = BitConverter.GetBytes((ushort)0x1);
            var lb = BitConverter.GetBytes((ushort)70);
            var sb = BitConverter.GetBytes(ssrc);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(tb);
                Array.Reverse(lb);
                Array.Reverse(sb);
            }

            tb.CopyTo(buffer, 0); // type
            lb.CopyTo(buffer, 2); // length
            sb.CopyTo(buffer, 4); // SSRC

            await udp.Send(buffer, buffer.Length);
        }

        IPAddress detectedIP;
        int detectedPort;

        {
            var ipd = await udp.Receive();
            var dip = Encoding.UTF8.GetString(ipd, 8, 64).TrimEnd('\0');
            detectedIP = IPAddress.Parse(dip);
            detectedPort = BinaryPrimitives.ReadUInt16LittleEndian(ipd.AsSpan()[72..]);
        }

        Logger.Add($"Detected IP: {detectedIP}, Port: {detectedPort}", LogLevel.Debug);

        var selected = ready.Modes.FirstOrDefault(x => supportedModes.Contains(x));
        if (selected is null) throw new InvalidOperationException();

        Logger.Add($"Selected mode is {selected}.", LogLevel.Debug);

        await sendPayload(new VoiceGatewayPayload
        {
            OpCode = VoiceOpCode.SelectProto,
            Data = new
            {
                protocol = "udp",
                data = new
                {
                    address = detectedIP.ToString(),
                    port = (ushort)detectedPort,
                    mode = selected
                }
            }
        });

        sendSource = new CancellationTokenSource();
        sendTask = Task.Run(sendLoop, sendSource.Token);

        receiveSource = new CancellationTokenSource();
        receiveTask = Task.Run(receiveLoop, receiveSource.Token);
    }

    private async Task postSessionDescription(VoiceSessionDescription sd)
    {
    }

    #endregion

    #region rcv/snd loop

    private async Task sendLoop()
    {
        while (sendSource?.Token is { IsCancellationRequested: false })
        {
        }
    }

    private async Task receiveLoop()
    {
        while (receiveSource?.Token is { IsCancellationRequested: false })
        {
            var data = await udp.Receive();
            Logger.Add($"got packet with {data.Length} bytes");
        }
    }

    #endregion
}
