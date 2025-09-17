// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Users;

public class DiscordUser : Snowflake
{
    [JsonProperty("username")]
    public string Username { get; internal set; } = null!;

    [JsonProperty("discriminator")]
    public string Discriminator { get; internal set; } = null!;

    [JsonProperty("global_name")]
    public string? DisplayName { get; internal set; }

    [JsonProperty("avatar")]
    public string? AvatarHash { get; internal set; }

    [JsonProperty("banner")]
    public string? BannerHash { get; internal set; }

    [JsonProperty("accent_color")]
    public int? AccentColor { get; internal set; }

    [JsonProperty("bot")]
    public bool? BotAccount { get; internal set; }

    [JsonProperty("system")]
    public bool? SystemAccount { get; internal set; }

    [JsonProperty("mfa_enabled")]
    public bool? MultifactorEnabled { get; internal set; }

    [JsonProperty("locale")]
    public string? Locale { get; internal set; }

    [JsonProperty("verified")]
    public bool EmailVerified { get; internal set; }

    [JsonProperty("email")]
    public string? Email { get; internal set; }

    [JsonProperty("flags")]
    public DiscordUserFlags Flags { get; internal set; }

    [JsonProperty("premium_type")]
    public DiscordPremiumType PremiumType { get; internal set; }

    [JsonProperty("public_flags")]
    public DiscordUserFlags PublicFlags { get; internal set; }

    [JsonProperty("avatar_decoration")]
    public object? AvatarDecoration { get; internal set; }

    [JsonIgnore]
    public string AvatarUrl => $"https://cdn.discordapp.com/avatars/{ID}/{AvatarHash}.png?size=1024";
}
