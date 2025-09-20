// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using Midori.Logging;
using Midori.Utils;
using Miyu.Attributes;
using Miyu.Events;
using Miyu.Events.Voice;
using Miyu.Models.Channels;
using Miyu.Models.Guilds;
using Miyu.Native.libsodium;
using Miyu.Networking.Gateway;
using Miyu.Networking.Gateway.Payloads;
using Miyu.Networking.WebSocket;
using Miyu.Utils;
using Miyu.Voice.Codec;
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

    private byte[] key = [];
    private Encryption encryption;

    private ushort sequence;
    private uint timestamp;
    private uint ssrc;
    private uint nonce;

    private readonly VoiceAudioFormat format = new();

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

        transmitting = new ConcurrentDictionary<uint, object>();

        var payload = new GatewayPayload
        {
            OpCode = GatewayOpCode.VoiceStateUpdate,
            Data = new GatewayVoiceStateUpdate
            {
                GuildID = guild?.ID ?? 0,
                ChannelID = channel.ID,
                SelfDeaf = false,
                SelfMute = false
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
    private long heartbeatSequence = 1;

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
                seq_ack = heartbeatSequence++
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

        transmit = Channel.CreateBounded<VoicePacket>(new BoundedChannelOptions(25));

        sendSource = new CancellationTokenSource();
        sendTask = Task.Run(sendLoop, sendSource.Token);

        receiveSource = new CancellationTokenSource();
        receiveTask = Task.Run(receiveLoop, receiveSource.Token);
    }

    private async Task postSessionDescription(VoiceSessionDescription sd)
    {
        key = sd.SecretKey;

        encryption = sd.Mode switch
        {
            "xsalsa20_poly1305_lite" => Encryption.PolyLite,
            "xsalsa20_poly1305_suffix" => Encryption.PolySuffix,
            "xsalsa20_poly1305" => Encryption.Poly,
            _ => throw new ArgumentException()
        };

        // setup keepalive

        await sendSilence(3);

        // init = true
        // start ready wait
    }

    #endregion

    #region rcv/snd loop

    private long queueCount;
    private Channel<VoicePacket>? transmit;
    private TaskCompletionSource<bool>? playWait;
    private ConcurrentDictionary<uint, object> transmitting = null!;

    private async Task queuePacket(VoicePacket pack)
    {
        Debug.Assert(transmit != null);
        await transmit.Writer.WriteAsync(pack);
        queueCount++;
    }

    private async Task sendLoop()
    {
        var syncTicks = (double)Stopwatch.GetTimestamp();
        var syncRes = Stopwatch.Frequency * 0.005;
        var tickRes = 10000000.0 / Stopwatch.Frequency;

        Logger.Add($"sr: {syncRes}, tr: {tickRes}, hr: {Stopwatch.IsHighResolution}", LogLevel.Debug);

        var encoder = MiyuOpus.CreateEncoder(format);

        byte[] data = null!;
        var length = 0;

        try
        {
            while (sendSource?.Token is { IsCancellationRequested: false })
            {
                if (transmit is null) break;

                var available = transmit.Reader.TryRead(out var packet);
                if (available) Debug.Assert(packet != null);

                if (available)
                {
                    queueCount--;

                    if (playWait is null || playWait.Task.IsCompleted)
                        playWait = new TaskCompletionSource<bool>();

                    available = prepPacket(packet!.Bytes.Span, out data, out length);

                    if (packet.Rented != null)
                        ArrayPool<byte>.Shared.Return(packet.Rented);
                }

                var modifier = available ? packet!.Duration / 5 : 4;
                var cts = Math.Max(Stopwatch.GetTimestamp() - syncTicks, 0);

                if (cts < syncRes * modifier)
                    await Task.Delay(TimeSpan.FromTicks((long)((syncRes * modifier - cts) * tickRes)));

                syncTicks += syncRes * modifier;

                if (!available)
                    continue;

                await updateSpeaking(true);
                await udp.Send(data, length);
                ArrayPool<byte>.Shared.Return(data);

                if (!packet!.Silence && queueCount == 0)
                    await sendSilence(3);
                else if (queueCount == 0)
                {
                    await updateSpeaking(false);
                    playWait?.SetResult(true);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        bool prepPacket(Span<byte> pcm, out byte[] result, out int length)
        {
            var packet = ArrayPool<byte>.Shared.Rent(RTP.GetPacketSize(format.SampleCountToSampleSize(format.MaxFrameSize), encryption));
            var span = packet.AsSpan();

            RTP.EncodeHeader(sequence, timestamp, ssrc, span);
            var opus = span.Slice(RTP.HEADER_SIZE, pcm.Length);
            MiyuOpus.Encode(encoder, format, pcm, ref opus);

            sequence++;
            timestamp += (uint)format.GetFrameSize(format.GetSampleDuration(pcm.Length));

            var nonceSpan = (Span<byte>)stackalloc byte[Sodium.SecretBox.NonceSize];

            switch (encryption)
            {
                case Encryption.PolyLite:
                {
                    if (nonceSpan.Length != Sodium.SecretBox.NonceSize)
                        throw new Exception("target size does not match nonce size");

                    BinaryPrimitives.WriteUInt32BigEndian(nonceSpan, nonce++);
                    zeroFill(nonceSpan[4..]);
                    break;
                }

                default:
                    ArrayPool<byte>.Shared.Return(packet);
                    throw new ArgumentOutOfRangeException(nameof(encryption), "Invalid encryption mode.");
            }

            var encrypted = (Span<byte>)stackalloc byte[opus.Length + Sodium.SecretBox.MacSize];

            var state = Sodium.SecretBox.Encrypt(opus, encrypted, key, nonceSpan);
            if (state != 0) throw new CryptographicException("Failed to encrypt with sodium.");

            encrypted.CopyTo(span[RTP.HEADER_SIZE..]);
            span = span[..RTP.GetPacketSize(encrypted.Length, encryption)];

            switch (encryption)
            {
                case Encryption.PolyLite:
                {
                    nonceSpan[..4].CopyTo(span[^4..]);
                    break;
                }

                default:
                    ArrayPool<byte>.Shared.Return(packet);
                    throw new ArgumentOutOfRangeException(nameof(encryption), "Invalid encryption mode.");
            }

            result = packet;
            length = span.Length;
            return true;
        }
    }

    private async Task sendSilence(int count)
    {
        var size = format.GetSampleSize(20);

        for (var i = 0; i < count; i++)
        {
            var pk = new byte[size];
            var mem = pk.AsMemory();
            await queuePacket(new VoicePacket(mem, 20, true));
        }
    }

    private async Task receiveLoop()
    {
        try
        {
            while (receiveSource?.Token is { IsCancellationRequested: false })
            {
                var data = await udp.Receive();
                Logger.Add($"got packet with {data.Length} bytes");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        void procPacket(ReadOnlySpan<byte> data)
        {
            if (!RTP.IsHeader(data))
                return;

            RTP.DecodeHeader(data, out var seq, out var time, out var s, out var ext);
        }
    }

    private bool speaking = false;

    private async Task updateSpeaking(bool speaking)
    {
        if (voiceWebSocket is null)
            throw new InvalidOperationException("WebSocket is not connected.");

        if (this.speaking == speaking)
            return;

        await sendPayload(new VoiceGatewayPayload
        {
            OpCode = VoiceOpCode.Speaking,
            Data = new
            {
                speaking,
                delay = 0
            }
        });

        this.speaking = speaking;
    }

    #endregion

    private static void zeroFill(Span<byte> buffer)
    {
        var idx = 0;

        for (; idx < buffer.Length / 4; idx++)
            MemoryMarshal.Write(buffer, 0);

        var remaining = buffer.Length % 4;
        if (remaining == 0) return;

        for (; idx < buffer.Length; idx++)
            buffer[idx] = 0;
    }
}
