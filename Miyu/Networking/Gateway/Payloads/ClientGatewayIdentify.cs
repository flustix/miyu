// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Networking.Gateway.Payloads;

internal class ClientGatewayIdentify
{
    [JsonProperty("token")]
    public string Token { get; set; } = null!;

    [JsonProperty("capabilities")]
    public int Capabilities { get; set; }

    [JsonProperty("properties")]
    public ClientProperties ClientProperties { get; set; } = new();

    [JsonProperty("compress")]
    public bool Compress { get; set; }
}
