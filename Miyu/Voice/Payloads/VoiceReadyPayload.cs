// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Voice.Payloads;

public class VoiceReadyPayload
{
    [JsonProperty("ip")]
    public string IPAddress { get; private set; } = null!;

    [JsonProperty("port")]
    public int Port { get; private set; }

    [JsonProperty("ssrc")]
    public uint Ssrc { get; set; }

    [JsonProperty("modes")]
    public IReadOnlyList<string> Modes { get; set; } = null!;
}
