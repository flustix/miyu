// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Networking.Gateway.Payloads;

public class GatewayVoiceStateUpdate
{
    [JsonProperty("guild_id")]
    public ulong GuildID { get; init; }

    [JsonProperty("channel_id")]
    public ulong ChannelID { get; init; }

    [JsonProperty("self_mute")]
    public bool SelfMute { get; init; }

    [JsonProperty("self_deaf")]
    public bool SelfDeaf { get; init; }
}
