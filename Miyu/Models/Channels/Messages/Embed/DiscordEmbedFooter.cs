// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Channels.Messages.Embed;

public class DiscordEmbedFooter
{
    [JsonProperty("text")]
    public string Text { get; internal set; } = string.Empty;

    [JsonProperty("icon_url")]
    public string? IconUrl { get; internal set; }

    [JsonProperty("proxy_icon_url")]
    public string? IconProxyUrl { get; internal set; }
}
