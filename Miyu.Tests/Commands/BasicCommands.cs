// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel.DataAnnotations;
using Miyu.Attributes;
using Miyu.Models.Interaction;
using Miyu.Utils.Extensions;

namespace Miyu.Tests.Commands;

public class BasicCommands
{
    [SlashCommand("echo", "echos the input")]
    public async Task Echo(DiscordInteraction interaction, string input)
    {
        await interaction.ReplyContent(input);
    }

    [SlashCommand("number", "number with range")]
    public async Task Number(DiscordInteraction interaction, [Range(1, 10)] int num)
    {
        await interaction.ReplyContent($"Your number is {num}!", true);
    }
}
