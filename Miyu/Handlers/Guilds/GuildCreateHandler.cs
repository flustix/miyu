// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Events.Guild;
using Miyu.Models.Guilds;
using Miyu.Networking.Gateway;
using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.Handlers.Guilds;

internal class GuildCreateHandler : SocketEventHandler
{
    public GuildCreateHandler(MiyuClient client, GatewayEventHandler handler)
        : base(client, handler)
    {
    }

    internal override void Handle(JObject data)
    {
        var guild = data.TurnTo<DiscordGuild>();
        Client.Guilds.AddOrUpdate(guild);

        if (guild.Properties is not null)
        {
            guild.Properties.MemberCache = guild.MemberCache;
            Client.Guilds.AddOrUpdate(guild.Properties);
        }

        foreach (var channel in guild.Channels)
        {
            channel.GuildID = guild.ID;
            Client.Channels.AddOrUpdate(channel);
        }

        foreach (var role in guild.Roles)
            Client.Roles.AddOrUpdate(role);

        guild.Channels = guild.Channels.Select(x => Client.Channels.Find(x.ID) ?? throw new Exception("Channel was not cached properly.")).ToList();

        Handler.DispatchEvent(new GuildCreateEvent(Client, guild));
    }
}
