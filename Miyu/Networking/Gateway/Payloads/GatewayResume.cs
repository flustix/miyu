// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Networking.Gateway.Payloads;

internal class GatewayResume
{
    /// <summary>
    ///     session token
    /// </summary>
    [JsonProperty("token")]
    public string Token { get; set; } = null!;

    /// <summary>
    ///     session id
    /// </summary>
    [JsonProperty("session_id")]
    public string SessionID { get; set; } = null!;

    /// <summary>
    ///     last sequence number received
    /// </summary>
    [JsonProperty("seq")]
    public long Sequence { get; set; }
}
