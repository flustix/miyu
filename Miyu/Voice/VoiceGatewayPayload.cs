// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Networking;

namespace Miyu.Voice;

public class VoiceGatewayPayload : IWebSocketPayload<VoiceOpCode>
{
    public VoiceOpCode OpCode { get; set; }
    public object? Data { get; set; }
    public int? Sequence { get; set; }
    public string? EventName { get; set; }
}
