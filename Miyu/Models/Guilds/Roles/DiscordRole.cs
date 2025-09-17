// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Guilds.Roles;

public class DiscordRole : Snowflake
{
    [JsonProperty("name")]
    public string Name { get; internal set; } = null!;

    [JsonProperty("color")]
    public int Color { get; internal set; }

    [JsonProperty("hoist")]
    public bool Hoisted { get; internal set; }

    [JsonProperty("icon")]
    public string? Icon { get; internal set; }

    [JsonProperty("unicode_emoji")]
    public string? UnicodeEmoji { get; internal set; }

    [JsonProperty("position")]
    public int Position { get; internal set; }

    [JsonProperty("permissions")]
    public DiscordPermissions Permissions { get; internal set; }

    [JsonProperty("managed")]
    public bool Managed { get; internal set; }

    [JsonProperty("mentionable")]
    public bool Mentionable { get; internal set; }

    [JsonProperty("flags")]
    public DiscordRoleFlags Flags { get; internal set; }
}
