// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Channels.Messages.Embed;

public class DiscordEmbedField
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("value")]
    public string Value { get; set; } = string.Empty;

    [JsonProperty("inline")]
    public bool? Inline { get; set; } = false;
}
