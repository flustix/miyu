// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Channels.Messages.Embed;

public class DiscordEmbedThumbnail
{
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    [JsonProperty("proxy_url")]
    public string? ProxyUrl { get; set; }

    [JsonProperty("height")]
    public int? Height { get; set; }

    [JsonProperty("width")]
    public int? Width { get; set; }
}
