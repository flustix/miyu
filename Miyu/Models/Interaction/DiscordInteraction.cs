// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Requests.Interactions;
using Miyu.Models.Channels.Messages;
using Miyu.Models.Guilds.Members;
using Miyu.Models.Interaction.Response;
using Miyu.Models.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Miyu.Models.Interaction;

public class DiscordInteraction : Snowflake
{
    [JsonProperty("application_id")]
    public ulong ApplicationID { get; set; }

    [JsonProperty("type")]
    public DiscordInteractionType Type { get; set; }

    [JsonProperty("data")]
    public JObject? Data { get; set; }

    [JsonProperty("guild_id")]
    public ulong? GuildID { get; set; }

    [JsonProperty("channel_id")]
    public ulong? ChannelID { get; set; }

    [JsonProperty("member")]
    public DiscordMember? Member { get; set; }

    [JsonProperty("user")]
    public DiscordUser? User { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;

    [JsonProperty("version")]
    public int Version { get; set; }

    [JsonProperty("message")]
    public DiscordMessage? Message { get; set; }

    [JsonProperty("app_permissions")]
    public DiscordPermissions? ApplicationPermissions { get; set; }

    [JsonProperty("locale")]
    public string? Locale { get; set; }

    [JsonProperty("guild_locale")]
    public string? GuildLocale { get; set; }

    [JsonProperty("attachment_size_limit")]
    public int AttachmentSizeLimit { get; set; }

    public async Task Reply(DiscordInteractionResponse response)
    {
        await Client.API.Execute(new InteractionCallbackRequest(ID, Token, response));
    }
}

/*[MapTo(typeof(IDiscordCommandInteractionData))]
internal class DiscordCommandInteractionDataImpl : IDiscordCommandInteractionData
{
    public string Name { get; set; } = string.Empty;
    public List<IDiscordCommandInteractionOptionsData>? Options { get; set; } = new();
}

[MapTo(typeof(IDiscordCommandInteractionOptionsData))]
internal class DiscordCommandInteractionOptionsDataImpl : IDiscordCommandInteractionOptionsData
{
    public string Name { get; set; } = string.Empty;
    public DiscordAppCommandOptionType Type { get; set; }
    public JToken? Value { get; set; }
    public List<IDiscordCommandInteractionOptionsData>? Options { get; } = new();
    public bool? Focused { get; set; }
}*/
