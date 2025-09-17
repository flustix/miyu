// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Builders.Interactions.Response;
using Miyu.Models.Channels.Messages.Embed;
using Miyu.Models.Interaction.Response;
using Newtonsoft.Json;

namespace Miyu.Builders.Interactions;

internal class DiscordInteractionResponseBuilder
{
    [JsonProperty("type")]
    public DiscordInteractionResponseType Type { get; }

    [JsonProperty("data")]
    public object? Data { get; private set; }

    public DiscordInteractionResponseBuilder(DiscordInteractionResponseType type)
    {
        Type = type;
    }

    // TODO: components, mentions, attachments
    public DiscordInteractionResponseBuilder SetMessage(string content, bool ephemeral = false, List<DiscordEmbed>? embeds = null)
    {
        if (embeds?.Count > 10)
            throw new ArgumentOutOfRangeException(nameof(embeds), "Embeds cannot exceed 10");

        Data = new DiscordInteractionResponseMessage
        {
            Content = content,
            Ephemeral = ephemeral,
            Embeds = embeds
        };

        return this;
    }

    public DiscordInteractionResponse Build()
    {
        return new DiscordInteractionResponse
        {
            Type = Type,
            Data = Data
        };
    }
}
