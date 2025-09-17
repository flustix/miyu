// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.Models.Channels.Messages;

namespace Miyu.Utils.Extensions;

public static class DiscordChannelExtensions
{
    public static bool IsGuildChannel(this DiscordChannel channel)
    {
        return channel.Type
            is DiscordChannelType.Text
            or DiscordChannelType.Voice
            or DiscordChannelType.Category
            or DiscordChannelType.Announcement
            or DiscordChannelType.AnnouncementThread
            or DiscordChannelType.PublicThread
            or DiscordChannelType.PrivateThread
            or DiscordChannelType.Stage
            or DiscordChannelType.Directory
            or DiscordChannelType.Forum
            or DiscordChannelType.Media;
    }

    public static bool IsPrivate(this DiscordChannel channel)
    {
        return channel.Type is DiscordChannelType.DM or DiscordChannelType.GroupDM;
    }

    public static DateTimeOffset GetTimeOffset(this DiscordMessage message)
    {
        return new DateTimeOffset(message.Timestamp);
    }
}
