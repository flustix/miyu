// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Interaction.Response;

public class DiscordInteractionResponse
{
    [JsonProperty("type")]
    public DiscordInteractionResponseType Type { get; internal set; }

    [JsonProperty("data")]
    public object? Data { get; internal set; }
}
