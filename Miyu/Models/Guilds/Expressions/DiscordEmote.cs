// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Users;
using Newtonsoft.Json;

namespace Miyu.Models.Guilds.Expressions;

public class DiscordEmote : Snowflake
{
    [JsonProperty("name")]
    public string? Name { get; internal set; }

    [JsonProperty("roles")]
    public List<ulong>? Roles { get; internal set; }

    [JsonProperty("user")]
    public DiscordUser? User { get; internal set; }

    [JsonProperty("require_colors")]
    public bool? RequireColons { get; internal set; }

    [JsonProperty("managed")]
    public bool? Managed { get; internal set; }

    [JsonProperty("animated")]
    public bool? Animated { get; internal set; }

    [JsonProperty("available")]
    public bool? Available { get; internal set; }
}
