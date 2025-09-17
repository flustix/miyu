// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models;
using Miyu.Sharding;
using Newtonsoft.Json;

namespace Miyu.Networking.Gateway.Payloads;

internal class BotGatewayIdentify
{
    [JsonProperty("token")]
    public string Token { get; set; } = null!;

    [JsonProperty("properties")]
    public ClientProperties ClientProperties { get; } = new();

    [JsonProperty("compress")]
    public bool Compress { get; set; }

    [JsonProperty("large_threshold")]
    public int LargeThreshold { get; set; }

    [JsonProperty("shard")]
    public ShardInfo? ShardInfo { get; set; }

    /*[JsonProperty("presence")]
    public StatusUpdate Presence { get; set; } = null;*/

    [JsonProperty("intents")]
    public DiscordIntents Intents { get; set; }
}
