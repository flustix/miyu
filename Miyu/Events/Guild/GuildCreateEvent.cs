// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Guilds;

namespace Miyu.Events.Guild;

public class GuildCreateEvent : GenericEvent
{
    internal override EventType Type => EventType.GuildCreate;

    public DiscordGuild Guild { get; }

    public GuildCreateEvent(MiyuClient client, DiscordGuild guild)
        : base(client)
    {
        Guild = guild;
    }
}
