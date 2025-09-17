// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages;
using Miyu.Models.Channels.Messages.Embed;
using Newtonsoft.Json;

namespace Miyu.Builders.Interactions.Response;

internal class DiscordInteractionResponseMessage
{
    [JsonProperty("content")]
    public string? Content { get; set; }

    [JsonProperty("embeds")]
    public List<DiscordEmbed>? Embeds { get; set; } = new();

    [JsonProperty("flags")]
    public DiscordMessageFlags? Flags { get; set; } = 0;

    // public List<IDiscordMessageComponent>? Components { get; set; } = new();
    // public List<IDiscordAttachment>? Attachments { get; set; } = new();

    [JsonIgnore]
    public bool? Ephemeral
    {
        get => Flags?.HasFlag(DiscordMessageFlags.Ephemeral);
        set
        {
            if (value == true)
                Flags |= DiscordMessageFlags.Ephemeral;
            else
                Flags &= ~DiscordMessageFlags.Ephemeral;
        }
    }
}
