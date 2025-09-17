// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Voice.Payloads;

internal class VoiceSessionDescription
{
    [JsonProperty("audio_codec")]
    public string AudioCodec { get; internal set; } = null!;

    [JsonProperty("video_codec")]
    public string VideoCodec { get; internal set; } = null!;

    [JsonProperty("mode")]
    public string Mode { get; internal set; } = null!;

    [JsonProperty("media_session_id")]
    public string MediaSessionID { get; internal set; } = null!;

    [JsonProperty("secret_key")]
    public byte[] SecretKey { get; internal set; } = null!;

    [JsonProperty("dave_protocol_version")]
    public int DaveVersion { get; internal set; }

    [JsonProperty("secure_frames_version")]
    public int SecureFramesVersion { get; internal set; }
}
