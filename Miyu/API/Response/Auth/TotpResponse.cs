// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.API.Response.Auth;

public class TotpResponse
{
    [JsonProperty("token")]
    public string? Token { get; set; }
}
