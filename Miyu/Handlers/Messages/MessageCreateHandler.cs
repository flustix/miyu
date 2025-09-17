// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Events.Messages;
using Miyu.Models.Channels.Messages;
using Miyu.Models.Guilds;
using Miyu.Models.Guilds.Members;
using Miyu.Networking.Gateway;
using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.Handlers.Messages;

internal class MessageCreateHandler : SocketEventHandler
{
    public MessageCreateHandler(MiyuClient client, GatewayEventHandler handler)
        : base(client, handler)
    {
    }

    internal override void Handle(JObject data)
    {
        var message = data.TurnTo<DiscordMessage>();

        var channel = Client.Channels.Find(message.ChannelID) ?? throw new InvalidOperationException("Could not find the associated channel for this message.");
        DiscordGuild? guild = null;
        DiscordMember? member = null;

        if (channel.GuildID != null)
        {
            guild = Client.Guilds.Find(channel.GuildID.Value) ?? throw new InvalidOperationException("Received message from guild that isn't in cache.");
            member = guild.MemberCache.Find(message.Author.ID);

            if (message.Member != null)
            {
                message.Member.User = message.Author;
                message.Member.ID = message.Author.ID;
                message.Member.Client = Client;
                member = guild.MemberCache.AddOrUpdate(message.Member);
            }
        }

        message.Member = member;
        Handler.DispatchEvent(new MessageCreateEvent(Client, message, channel, guild, member));
    }
}
