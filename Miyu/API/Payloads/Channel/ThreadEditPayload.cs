// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.API.Payloads.Channel;

public class ThreadEditPayload
{
    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("archived")]
    public bool? Archived { get; set; }

    [JsonProperty("auto_archive_duration")]
    public int? AutoArchiveDuration { get; set; }

    [JsonProperty("locked")]
    public bool? Locked { get; set; }

    [JsonProperty("invitable")]
    public bool? Invitable { get; set; }

    [JsonProperty("rate_limit_per_user")]
    public int? RateLimitPerUser { get; set; }
}
