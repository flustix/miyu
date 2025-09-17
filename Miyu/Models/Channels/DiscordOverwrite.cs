// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Models.Channels;

public class DiscordOverwrite
{
    [JsonProperty("id")]
    public ulong ID { get; internal set; }

    [JsonProperty("type")]
    public DiscordOverwriteType Type { get; internal set; }

    [JsonProperty("allow")]
    public DiscordPermissions Allow { get; internal set; }

    [JsonProperty("deny")]
    public DiscordPermissions Deny { get; internal set; }
}

public enum DiscordOverwriteType
{
    Role = 0,
    Member = 1
}
