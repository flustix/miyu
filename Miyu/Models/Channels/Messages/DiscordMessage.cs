// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages.Attachment;
using Miyu.Models.Guilds.Members;
using Miyu.Models.Users;
using Newtonsoft.Json;

namespace Miyu.Models.Channels.Messages;

public class DiscordMessage : Snowflake
{
    [JsonProperty("channel_id")]
    public ulong ChannelID { get; internal set; }

    [JsonProperty("author")]
    public DiscordUser Author { get; internal set; } = null!;

    [JsonProperty("content")]
    public string Content { get; internal set; } = "";

    [JsonProperty("mentioned")]
    public bool? Mentioned { get; internal set; }

    [JsonProperty("mentions")]
    public List<DiscordUser> Mentions { get; internal set; } = null!;

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; internal set; }

    [JsonProperty("member")]
    public DiscordMember? Member { get; internal set; }

    [JsonProperty("attachments")]
    public List<DiscordAttachment> Attachments { get; internal set; } = new();

    [JsonProperty("type")]
    public DiscordMessageType Type { get; internal set; }

    [JsonProperty("referenced_message")]
    public DiscordMessage? ReferencedMessage { get; internal set; }

    [JsonProperty("sticker_items")]
    public List<DiscordStickerItem>? Stickers { get; internal set; }
}
