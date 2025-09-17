// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Networking.Gateway;

internal class ClientProperties
{
    [JsonProperty("os")]
    public string OS { get; set; } = string.Empty;

    [JsonProperty("browser")]
    public string Browser { get; set; } = string.Empty;

    [JsonProperty("device")]
    public string Device { get; set; } = string.Empty;

    [JsonProperty("system_locale")]
    public string SystemLocale { get; set; } = "en-US";

    [JsonProperty("browser_user_agent")]
    public string BrowserUserAgent { get; set; } = string.Empty;

    [JsonProperty("browser_version")]
    public string BrowserVersion { get; set; } = string.Empty;

    [JsonProperty("os_version")]
    public string OSVersion { get; set; } = string.Empty;

    [JsonProperty("referrer")]
    public string Referrer { get; set; } = string.Empty;

    [JsonProperty("referring_domain")]
    public string ReferringDomain { get; set; } = string.Empty;

    [JsonProperty("referrer_current")]
    public string ReferrerCurrent { get; set; } = string.Empty;

    [JsonProperty("referring_domain_current")]
    public string ReferringDomainCurrent { get; set; } = string.Empty;

    [JsonProperty("release_channel")]
    public string ReleaseChannel { get; set; } = "canary";

    [JsonProperty("client_build_number")]
    public int ClientBuildNumber { get; set; }

    [JsonProperty("client_event_source")]
    public string? ClientEventSource { get; set; }

    [JsonProperty("has_client_mods")]
    public bool HasClientMods { get; set; }
}
