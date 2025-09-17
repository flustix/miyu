// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Networking.Gateway.Payloads;

internal class GatewayHello
{
    [JsonProperty("heartbeat_interval")]
    public int HeartbeatInterval { get; private set; }

    [JsonProperty("_trace")]
    public IReadOnlyList<string> Trace { get; private set; } = null!;
}
