// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Models.Channels;

public enum DiscordChannelType
{
    Text = 0,
    DM = 1,
    Voice = 2,
    GroupDM = 3,
    Category = 4,
    Announcement = 5,

    AnnouncementThread = 10,
    PublicThread = 11,
    PrivateThread = 12,

    Stage = 13,
    Directory = 14,
    Forum = 15,
    Media = 16
}
