// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Events.Channels;

public class TypingStartEvent : GenericEvent
{
    internal override EventType Type => EventType.TypingStart;

    public ulong ChannelID { get; }
    public ulong UserID { get; }

    public TypingStartEvent(MiyuClient client, ulong channel, ulong user)
        : base(client)
    {
        ChannelID = channel;
        UserID = user;
    }
}
