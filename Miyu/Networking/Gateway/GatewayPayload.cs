// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Networking.Gateway;

internal class GatewayPayload : IWebSocketPayload<GatewayOpCode>
{
    public GatewayOpCode OpCode { get; set; }
    public object? Data { get; set; }
    public int? Sequence { get; set; }
    public string? EventName { get; set; }
}
