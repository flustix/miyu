// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Interaction;

namespace Miyu.Events.Interactions;

public class InteractionCommandEvent : GenericEvent
{
    internal override EventType Type => EventType.InteractionCommand;
    public DiscordInteraction Interaction { get; }

    public InteractionCommandEvent(MiyuClient client, DiscordInteraction interaction)
        : base(client)
    {
        Interaction = interaction;
    }
}
