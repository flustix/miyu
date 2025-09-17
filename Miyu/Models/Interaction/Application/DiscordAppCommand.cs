// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Interaction.Application;

public class DiscordAppCommand : Snowflake
{
    [JsonProperty("type")]
    public DiscordAppCommandType? Type { get; internal set; }

    [JsonProperty("application_id")]
    public ulong ApplicationID { get; internal set; }

    [JsonProperty("guild_id")]
    public ulong? GuildID { get; internal set; }

    [JsonProperty("name")]
    public string Name { get; internal set; } = null!;

    [JsonProperty("description")]
    public string Description { get; internal set; } = null!;

    [JsonProperty("options")]
    public List<DiscordAppCommandOption>? Options { get; internal set; }

    [JsonProperty("default_member_permissions")]
    public DiscordPermissions? DefaultMemberPermissions { get; internal set; }

    [JsonProperty("dm_permission")]
    public bool? AllowDMUsage { get; internal set; }

    [JsonProperty("nsfw")]
    public bool? NotSafeForWork { get; internal set; }

    [JsonProperty("version")]
    public ulong Version { get; internal set; }
}
