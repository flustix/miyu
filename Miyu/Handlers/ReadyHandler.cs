// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Logging;
using Miyu.Events;
using Miyu.Networking.Gateway;
using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.Handlers;

internal class ReadyHandler : SocketEventHandler
{
    public ReadyHandler(MiyuClient client, GatewayEventHandler handler)
        : base(client, handler)
    {
    }

    internal override void Handle(JObject data)
    {
        var ready = data.TurnTo<ReadyEvent.RawReadyEvent>();
        Client.Self = ready.User;
        Client.SessionID = ready.SessionID;
        Client.Users.AddOrUpdate(ready.User);
        Client.Self.Client = Client;

        foreach (var user in ready.Users)
        {
            user.Client = Client;
            Client.Users.AddOrUpdate(user);
        }

        foreach (var guild in ready.Guilds)
        {
            guild.Client = Client;
            Client.Guilds.AddOrUpdate(guild);

            if (!guild.Unavailable)
            {
                foreach (var channel in guild.Channels)
                {
                    channel.Client = Client;
                    channel.GuildID = guild.ID;
                    Client.Channels.AddOrUpdate(channel);
                }

                foreach (var role in guild.Roles)
                {
                    role.Client = Client;
                    Client.Roles.AddOrUpdate(role);
                }
            }

            if (guild.Properties is not null)
            {
                guild.Properties.MemberCache = guild.MemberCache;
                Client.Guilds.AddOrUpdate(guild.Properties);
            }

            Logger.Log($"got guild {guild.ID} ({guild.Name})", level: LogLevel.Debug);
        }

        if (ready.MergedMembers is not null)
        {
            for (var i = 0; i < ready.Guilds.Count; i++)
            {
                var sg = ready.Guilds[i];
                var members = ready.MergedMembers[i];
                var guild = Client.Guilds.Find(sg.ID)!;
                guild.Members = members;
            }
        }

        foreach (var channel in ready.PrivateChannels)
        {
            channel.Client = Client;
            Client.Channels.AddOrUpdate(channel);
        }

        Handler.DispatchEvent(new ReadyEvent(Client, ready));
    }
}
