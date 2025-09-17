// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Channels.Messages.Attachment;

public class DiscordAttachment : Snowflake
{
    [JsonProperty("url")]
    public string Url { get; internal set; } = null!;

    [JsonProperty("filename")]
    public string Name { get; internal set; } = null!;

    [JsonProperty("content_type")]
    public string? ContentType { get; internal set; }

    [JsonProperty("width")]
    public int? Width { get; internal set; }

    [JsonProperty("height")]
    public int? Height { get; internal set; }

    [JsonProperty("size")]
    public long SizeBytes { get; internal set; }
}
