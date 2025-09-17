// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Users;
using Newtonsoft.Json;

namespace Miyu.Models.Guilds.Members;

public class DiscordMember : Snowflake
{
    // ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
    [JsonProperty("user")]
    public DiscordUser? User
    {
        get => Client?.Users.Find(ID);
        internal set => Client?.Users.AddOrUpdate(value ?? throw new InvalidOperationException());
    }
    // ReSharper restore ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

    [JsonProperty("user_id")]
    public ulong? UserID
    {
        get => ID;
        set => ID = value ?? 0;
    }

    [JsonProperty("nick")]
    public string? Nickname { get; internal set; }

    [JsonProperty("avatar")]
    public string? GuildAvatar { get; internal set; }

    [JsonProperty("roles")]
    public List<ulong> RoleIDs { get; internal set; } = new();

    [JsonProperty("joined_at")]
    public DateTimeOffset JoinedAt { get; internal set; }

    [JsonProperty("premium_since")]
    public DateTimeOffset? PremiumSince { get; internal set; }

    [JsonProperty("deaf")]
    public bool IsDeafened { get; internal set; }

    [JsonProperty("muted")]
    public bool IsMuted { get; internal set; }

    [JsonProperty("flags")]
    public int Flags { get; internal set; }

    [JsonProperty("pending")]
    public bool IsPending { get; internal set; }

    [JsonProperty("permissions")]
    public string Permissions { get; internal set; } = null!;

    [JsonProperty("communication_disabled_until")]
    public DateTimeOffset? TimeoutUntil { get; internal set; }
}
