// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Networking;

internal interface IWebSocketPayload<T> where T : Enum
{
    [JsonProperty("op")]
    T OpCode { get; set; }

    [JsonProperty("d")]
    object? Data { get; set; }

    [JsonProperty("s")]
    int? Sequence { get; set; }

    [JsonProperty("t")]
    string? EventName { get; set; }
}
