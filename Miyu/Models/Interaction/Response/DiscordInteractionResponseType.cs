// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Models.Interaction.Response;

public enum DiscordInteractionResponseType
{
    /// <summary>
    ///     ACK a Ping
    /// </summary>
    Pong = 1,

    /// <summary>
    ///     respond to an interaction with a message
    /// </summary>
    ChannelMessageWithSource = 4,

    /// <summary>
    ///     ACK an interaction and edit a response later, the user sees a loading state
    /// </summary>
    DeferredChannelMessageWithSource = 5,

    /// <summary>
    ///     for components, ACK an interaction and edit the original message later; the user does not see a loading state
    /// </summary>
    DeferredMessageUpdate = 6,

    /// <summary>
    ///     for components, edit the message the component was attached to
    /// </summary>
    UpdateMessage = 7,

    /// <summary>
    ///     respond to an autocomplete interaction with suggested choices
    /// </summary>
    AutoCompleteResult = 8,

    /// <summary>
    ///     respond to an interaction with a popup modal
    /// </summary>
    Modal = 9,

    /// <summary>
    ///     respond to an interaction with an upgrade button, only available for apps with monetization enabled
    /// </summary>
    PremiumRequired = 10
}
