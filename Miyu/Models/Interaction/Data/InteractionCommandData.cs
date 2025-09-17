// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using Miyu.Models.Interaction.Application;
using Newtonsoft.Json;

namespace Miyu.Models.Interaction.Data;

[UsedImplicitly]
internal class InteractionCommandData : Snowflake
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("type")]
    public DiscordAppCommandType Type { get; set; }

    [JsonProperty("options")]
    public List<InteractionOptionData>? Options { get; set; }
}
