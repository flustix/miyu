// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Builders.Interactions;
using Miyu.Models.Channels.Messages.Embed;
using Miyu.Models.Interaction;
using Miyu.Models.Interaction.Application;
using Miyu.Models.Interaction.Data;
using Miyu.Models.Interaction.Response;

namespace Miyu.Utils.Extensions;

public static class DiscordInteractionExtensions
{
    #region Reply

    public static async Task ReplyContent(this DiscordInteraction interaction, string content, bool ephermal = false)
    {
        var res = new DiscordInteractionResponseBuilder(DiscordInteractionResponseType.ChannelMessageWithSource);
        await interaction.Reply(res.SetMessage(content, ephermal).Build());
    }

    public static async Task ReplyEmbed(this DiscordInteraction interaction, DiscordEmbed embed, bool ephermal = false)
    {
        var res = new DiscordInteractionResponseBuilder(DiscordInteractionResponseType.ChannelMessageWithSource);
        await interaction.Reply(res.SetMessage("", ephermal, [embed]).Build());
    }

    // public static Task ReplyChoices(this DiscordCommandInteraction interaction, List<DiscordAutoCompleteChoice> choices) => Task.CompletedTask;

    #endregion

    #region Options

    internal static string? GetString(this DiscordInteraction interaction, string name)
    {
        var opt = getOption(interaction, name, DiscordAppCommandOptionType.String);
        return opt?.Value?.ToObject<string>();
    }

    internal static int? GetInteger(this DiscordInteraction interaction, string name)
    {
        var opt = getOption(interaction, name, DiscordAppCommandOptionType.Integer);
        return opt?.Value?.ToObject<int>();
    }

    internal static bool? GetBool(this DiscordInteraction interaction, string name)
    {
        var opt = getOption(interaction, name, DiscordAppCommandOptionType.Boolean);
        return opt?.Value?.ToObject<bool>();
    }

    internal static T? GetNumber<T>(this DiscordInteraction interaction, string name)
        where T : struct
    {
        var opt = getOption(interaction, name, DiscordAppCommandOptionType.Number);
        return opt?.Value?.ToObject<T>();
    }

    private static InteractionOptionData? getOption(DiscordInteraction interaction, string name, DiscordAppCommandOptionType expected)
    {
        var opt = getOptions(interaction)?.FirstOrDefault(o => o.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        if (opt is null)
            return null;

        if (opt.Type != expected)
            throw new Exception($"Option '{opt.Name}' is not of expected type '{expected}' (is {opt.Type}).");

        return opt;
    }

    private static IEnumerable<InteractionOptionData>? getOptions(DiscordInteraction interaction)
    {
        var data = interaction.Data?.ToObject<InteractionCommandData>();
        if (data is null) throw new InvalidOperationException("Interaction data is null.");

        var options = data.Options?.ToList();
        if (options == null || options.Count == 0) return null;

        while (options.FirstOrDefault()?.Options?.Any() ?? false)
            options = options.First().Options!.ToList();

        return options;
    }

    #endregion
}
