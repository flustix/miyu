// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Channels.Messages.Embed;

public class DiscordEmbedAuthor
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("icon_url")]
    public string? IconUrl { get; set; }

    [JsonProperty("proxy_icon_url")]
    public string? ProxyIconUrl { get; set; }
}
