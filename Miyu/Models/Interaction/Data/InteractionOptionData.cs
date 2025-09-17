// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Interaction.Application;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Miyu.Models.Interaction.Data;

internal class InteractionOptionData
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("type")]
    public DiscordAppCommandOptionType Type { get; set; }

    [JsonProperty("value")]
    public JToken? Value { get; set; }

    [JsonProperty("options")]
    public List<InteractionOptionData>? Options { get; set; }

    [JsonProperty("focused")]
    public bool? Focused { get; set; }
}
