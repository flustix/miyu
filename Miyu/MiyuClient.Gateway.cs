// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Logging;
using Midori.Utils;
using Miyu.Handlers;
using Miyu.Handlers.Applications.Interactions;
using Miyu.Handlers.Channels;
using Miyu.Handlers.Guilds;
using Miyu.Handlers.Messages;
using Miyu.Handlers.Voice;
using Miyu.Networking.Gateway;
using Miyu.Networking.Gateway.Payloads;
using Miyu.Sharding;
using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu;

public partial class MiyuClient
{
    internal DebugEventLogger GatewayDebug { get; }

    internal string? SessionID { get; set; }
    private Task? heartbeatTask;
    private DateTimeOffset lastHeartbeat;
    private int heartbeatInterval;
    private long sequence;

    private Dictionary<string, SocketEventHandler> handlers { get; } = new();

    private async void handleGatewayMessage(string message)
    {
        try
        {
            GatewayDebug.Log(message, true);
            var payload = message.Deserialize<GatewayPayload>();

            if (payload == null)
                throw new InvalidOperationException("expected gateway payload");

            sequence = payload.Sequence ?? sequence;

            switch (payload.OpCode)
            {
                case GatewayOpCode.Dispatch:
                    onDispatch(payload);
                    break;

                case GatewayOpCode.Heartbeat:
                    if (payload.Data is not long seq)
                        throw new InvalidOperationException("expected heartbeat payload");

                    await sendHeartbeat(seq);
                    break;

                case GatewayOpCode.Reconnect:
                    await reconnect();
                    break;

                case GatewayOpCode.InvalidSession:
                    await onInvalidSession();
                    break;

                case GatewayOpCode.Hello:
                    var jObj = payload.Data as JObject;
                    var hello = jObj?.ToObject<GatewayHello>();

                    if (hello == null)
                        throw new InvalidOperationException("expected hello payload");

                    await onHello(hello);
                    break;

                case GatewayOpCode.HeartbeatAck:
                    await onHeartbeatAck();
                    break;

                default:
                    Logger.Add($"unknown opcode '{payload.OpCode}'", LogLevel.Warning);
                    break;
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "error handling gateway message");
            await File.WriteAllTextAsync("gateway_error.json", message);
            // throw;
        }
    }

    private void onDispatch(GatewayPayload payload)
    {
        var name = payload.EventName?.ToLowerInvariant();

        if (name == null)
            return;

        if (payload.Data is not JObject data)
        {
            Logger.Add("Discord did not send an object.", LogLevel.Warning);
            return;
        }

        if (!handlers.TryGetValue(name, out var handler))
        {
            Logger.Add($"Unhandled event: {payload.EventName}", LogLevel.Warning);
            return;
        }

        handler.Handle(data);

        // await Events.Handle(payload);
    }

    private async Task onHello(GatewayHello hello)
    {
        heartbeatInterval = hello.HeartbeatInterval;
        heartbeatTask = Task.Run(heartbeatLoop);

        if (!string.IsNullOrEmpty(SessionID))
            await sendResume();
        else
            await sendIdentify();
    }

    private Task onHeartbeatAck()
    {
        var ping = (DateTimeOffset.UtcNow - lastHeartbeat).Milliseconds;
        Ping = ping;

        return Task.CompletedTask;
    }

    private Task reconnect()
    {
        return Task.CompletedTask;
    }

    private Task onInvalidSession()
    {
        Logger.Add("invalid session", LogLevel.Warning);

        return Task.CompletedTask;
    }

    private async Task heartbeatLoop()
    {
        try
        {
            while (true)
            {
                await sendHeartbeat(sequence);
                await Task.Delay(heartbeatInterval);
            }
        }
        catch (TaskCanceledException)
        {
            Logger.Add("heartbeat loop cancelled", LogLevel.Debug);
        }
    }

    private async Task sendHeartbeat(long seq)
    {
        var payload = new GatewayPayload
        {
            OpCode = GatewayOpCode.Heartbeat,
            Data = seq
        };

        sequence = seq;

        var json = payload.Serialize();
        await SendPayload(json);

        lastHeartbeat = DateTimeOffset.Now;
    }

    private async Task sendIdentify()
    {
        object? data = null;

        if (Config.ClientToken)
        {
            data = new ClientGatewayIdentify
            {
                Token = FormattingUtils.FormatToken(Config),
                Capabilities = 1734653,
                ClientProperties = new ClientProperties
                {
                    OS = "Windows",
                    Browser = "Firefox",
                    BrowserUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:142.0) Gecko/20100101 Firefox/142.0",
                    BrowserVersion = "142.0",
                    OSVersion = "10.0",
                    ClientBuildNumber = 443209
                },
                Compress = false
            };
        }
        else
        {
            data = new BotGatewayIdentify
            {
                Token = FormattingUtils.FormatToken(Config),
                Compress = false,
                Intents = Config.Intents,
                ShardInfo = new ShardInfo { ID = 0, Count = 1 },
                LargeThreshold = 250
            };
        }

        var payload = new GatewayPayload
        {
            OpCode = GatewayOpCode.Identify,
            Data = data
        };

        await SendPayload(payload.Serialize());
    }

    private async Task sendResume()
    {
        if (string.IsNullOrEmpty(SessionID))
            throw new InvalidOperationException("cannot resume without session id");

        var payload = new GatewayPayload
        {
            OpCode = GatewayOpCode.Resume,
            Data = new GatewayResume
            {
                Token = FormattingUtils.FormatToken(Config),
                SessionID = SessionID,
                Sequence = sequence
            }
        };

        await SendPayload(payload.Serialize());
    }

    internal async Task SendPayload(string json)
    {
        if (socket == null)
            throw new InvalidOperationException("not connected");

        GatewayDebug.Log(json, false);
        await socket.SendAsync(json);
    }

    private void setupHandlers()
    {
        handlers["guild_create"] = new GuildCreateHandler(this, Events);
        handlers["interaction_create"] = new InteractionCreateHandler(this, Events);
        handlers["message_create"] = new MessageCreateHandler(this, Events);
        handlers["ready"] = new ReadyHandler(this, Events);
        handlers["ready_supplemental"] = new ReadySupplementalHandler(this, Events);
        handlers["typing_start"] = new TypingStartHandler(this, Events);
        handlers["voice_server_update"] = new VoiceServerUpdateHandler(this, Events);

        try
        {
            if (Directory.Exists("events"))
                Directory.Delete("events", true);
        }
        catch
        {
        }
    }
}
