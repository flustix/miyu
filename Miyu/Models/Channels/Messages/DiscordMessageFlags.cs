// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Models.Channels.Messages;

[Flags]
public enum DiscordMessageFlags
{
    Crossposted = 1 << 0,
    IsCrosspost = 1 << 1,
    SuppressEmbeds = 1 << 2,
    SourceMessageDeleted = 1 << 3,
    Urgent = 1 << 4,
    HasThread = 1 << 5,
    Ephemeral = 1 << 6,
    Loading = 1 << 7,
    FailedToMentionSomeRolesInThread = 1 << 8,
    SuppressNotifications = 1 << 12,
    IsVoiceMessage = 1 << 13
}
