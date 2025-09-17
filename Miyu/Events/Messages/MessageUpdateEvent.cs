// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.Models.Channels.Messages;
using Miyu.Models.Guilds;
using Miyu.Models.Guilds.Members;

namespace Miyu.Events.Messages;

public class MessageUpdateEvent : GenericEvent
{
    internal override EventType Type => EventType.MessageUpdate;

    public DiscordMessage Message { get; }
    public DiscordChannel Channel { get; }
    public DiscordGuild? Guild { get; }
    public DiscordMember? Member { get; }

    public MessageUpdateEvent(MiyuClient client, DiscordMessage message, DiscordChannel channel, DiscordGuild? guild, DiscordMember? member)
        : base(client)
    {
        Message = message;
        Channel = channel;
        Guild = guild;
        Member = member;
    }
}
