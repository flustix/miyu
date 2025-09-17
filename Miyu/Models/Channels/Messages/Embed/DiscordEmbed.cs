// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Channels.Messages.Embed;

public class DiscordEmbed
{
    [JsonProperty("title")]
    public string? Title { get; internal set; }

    [JsonProperty("type")]
    public string? Type { get; internal set; }

    [JsonProperty("description")]
    public string? Description { get; internal set; }

    [JsonProperty("url")]
    public Uri? Url { get; internal set; }

    [JsonProperty("timestamp")]
    public DateTimeOffset? Timestamp { get; internal set; }

    [JsonProperty("color")]
    public DiscordColor? Color { get; internal set; }

    [JsonProperty("footer")]
    public DiscordEmbedFooter? Footer { get; internal set; }

    [JsonProperty("image")]
    public DiscordEmbedImage? Image { get; internal set; }

    [JsonProperty("thumbnail")]
    public DiscordEmbedThumbnail? Thumbnail { get; internal set; }

    [JsonProperty("video")]
    public DiscordEmbedVideo? Video { get; internal set; }

    [JsonProperty("provider")]
    public DiscordEmbedProvider? Provider { get; internal set; }

    [JsonProperty("author")]
    public DiscordEmbedAuthor? Author { get; internal set; }

    [JsonProperty("fields")]
    public List<DiscordEmbedField>? Fields { get; internal set; }
}
