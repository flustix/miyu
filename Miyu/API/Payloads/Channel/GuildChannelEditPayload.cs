// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Newtonsoft.Json;

namespace Miyu.API.Payloads.Channel;

public class GuildChannelEditPayload
{
    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("type")]
    public DiscordChannelType Type { get; set; }

    public int? Position { get; set; }

    public string Topic { get; set; } = "";

    public bool? NSFW { get; set; }

    public int? RateLimitPerUser { get; set; }

    public int Bitrate { get; set; }

    public int UserLimit { get; set; }
}
