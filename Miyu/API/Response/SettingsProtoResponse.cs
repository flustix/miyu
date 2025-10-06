// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.API.Response;

public class SettingsProtoResponse
{
    [JsonProperty("settings")]
    public string Base64String { get; set; } = null!;

    public byte[] DecodeString() => Convert.FromBase64String(Base64String);
}
