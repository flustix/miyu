// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Reflection;
using Midori.Logging;
using Miyu.API;
using Miyu.API.Requests.Users;
using Miyu.Attributes;
using Miyu.Caches;
using Miyu.Models.Channels;
using Miyu.Models.Guilds;
using Miyu.Models.Guilds.Roles;
using Miyu.Models.Users;
using Miyu.Networking.Gateway;
using Miyu.Networking.WebSocket;
using Miyu.Utils;
using Miyu.Voice;

namespace Miyu;

public partial class MiyuClient
{
    internal static Logger Logger { get; } = Logger.GetLogger("Miyu");

    private const string gateway_url = "wss://gateway.discord.gg?v=10&encoding=json";

    public GatewayEventHandler Events { get; }
    public int Ping { get; private set; }

    public MiyuConfig Config { get; }

    public MiyuAPIClient API { get; }
    public DiscordUser Self { get; internal set; } = null!;

    public SnowflakeCache<DiscordChannel> Channels { get; }
    public SnowflakeCache<DiscordGuild> Guilds { get; }
    public SnowflakeCache<DiscordRole> Roles { get; }
    public SnowflakeCache<DiscordUser> Users { get; }

    public event Action? OnDisconnect;

    public Action<string, Action<string>>? BeforeHandle { get; set; }

    public VoiceManager VoiceManager { get; }
    private IWebSocketClient? socket;

    public MiyuClient(MiyuConfig config)
    {
        Config = config ?? throw new InvalidOperationException("no you can't just make the config null.");

        GatewayDebug = new DebugEventLogger("gtw")
        {
            CodeMappings = Enum.GetValues<GatewayOpCode>().ToDictionary(x => (int)x, x => x.ToString())
        };

        JsonUtils.RegisterConverter(new JsonClientApplicationConverter(this));
        API = new MiyuAPIClient(this);
        Events = new GatewayEventHandler(this);
        VoiceManager = new VoiceManager(this);

        Channels = new SnowflakeCache<DiscordChannel>(this);
        Guilds = new SnowflakeCache<DiscordGuild>(this);
        Roles = new SnowflakeCache<DiscordRole>(this);
        Users = new SnowflakeCache<DiscordUser>(this);

        RegisterListeners(this);
    }

    public void RegisterListeners(object obj)
    {
        const BindingFlags flags = BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public;

        var type = obj.GetType();
        var methods = type.GetMethods(flags)
                          .Where(m => m.GetCustomAttributes(typeof(EventListenerAttribute), false).Length > 0);

        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<EventListenerAttribute>()!;
            Logger.Add($"{method} {attr.TargetType}");
            Events.RegisterListener(attr.TargetType, obj, method);
        }
    }

    public void UnregisterListeners(object obj)
    {
        Events.UnregisterListener(obj);
    }

    public virtual IWebSocketClient CreateWebsocket()
    {
        return new WebSocketClient();
    }

    public async Task ConnectAsync()
    {
        if (string.IsNullOrWhiteSpace(Config.Token))
            throw new InvalidOperationException("maybe set a token? :>");

        await connect();
    }

    public void Disconnect()
    {
        if (socket != null)
        {
            socket.OnDisconnect -= onDisconnect;
            socket.DisconnectAsync();
        }

        socket = null;
    }

    public async Task<DiscordUser?> GetUser(ulong id)
    {
        var req = new UserRequest(id);
        var res = await API.Execute(req);

        if (res is not null)
            Users.AddOrUpdate(res);

        return res;
    }

    private async Task connect()
    {
        socket = CreateWebsocket();
        socket.OnDisconnect += onDisconnect;
        socket.OnMessage = m =>
        {
            if (BeforeHandle is null)
                handleGatewayMessage(m);
            else
                BeforeHandle(m, handleGatewayMessage);
        };

        setupHandlers();

        await socket.ConnectAsync(gateway_url);
    }

    private void onDisconnect()
    {
        if (BeforeHandle is null) OnDisconnect?.Invoke();
        else BeforeHandle("", _ => OnDisconnect?.Invoke());
    }
}
