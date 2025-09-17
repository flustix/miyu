// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.API.Payloads.Auth;

public class LoginPayload
{
    [JsonProperty("login")]
    public string Email { get; set; } = "";

    [JsonProperty("password")]
    public string Password { get; set; } = "";
}
