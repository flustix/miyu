// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Guilds.Expressions;
using Newtonsoft.Json;

namespace Miyu.Models.Channels.Messages.Attachment;

public class DiscordStickerItem : Snowflake
{
    [JsonProperty("name")]
    public string Name { get; internal set; } = null!;

    [JsonProperty("format_type")]
    public DiscordStickerFormat Format { get; internal set; }
}
