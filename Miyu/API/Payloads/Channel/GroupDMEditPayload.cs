// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.API.Payloads.Channel;

public class GroupDMEditPayload
{
    [JsonProperty("name")]
    public string Name { get; set; } = "";

    /*[JsonProperty("icon")]
    public string Icon { get; set; } = "";*/
}
