// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Newtonsoft.Json;

namespace Miyu.Models.Interaction.Application;

public class DiscordAppCommandOption
{
    [JsonProperty("type")]
    public DiscordAppCommandOptionType Type { get; internal set; }

    [JsonProperty("name")]
    public string Name { get; internal set; } = null!;

    [JsonProperty("description")]
    public string Description { get; internal set; } = null!;

    [JsonProperty("required")]
    public bool? Required { get; internal set; }

    // public List<DiscordAppCommandChoice>? Choices { get; internal set; }

    [JsonProperty("options")]
    public List<DiscordAppCommandOption>? Options { get; internal set; }

    [JsonProperty("channel_types")]
    public List<DiscordChannelType>? ChannelTypes { get; internal set; }

    [JsonProperty("min_value")]
    public double? MinValue { get; internal set; }

    [JsonProperty("max_value")]
    public double? MaxValue { get; internal set; }

    [JsonProperty("min_length")]
    public int? MinLength { get; internal set; }

    [JsonProperty("max_length")]
    public int? MaxLength { get; internal set; }

    [JsonProperty("autocomplete")]
    public bool? AutoComplete { get; internal set; }
}
