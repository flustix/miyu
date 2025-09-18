// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Payloads.Channel;
using Miyu.API.Requests.Channels;
using Miyu.Models.Channels.Messages;
using Miyu.Models.Guilds;
using Miyu.Models.Guilds.Members;
using Miyu.Utils.Extensions;
using Newtonsoft.Json;

namespace Miyu.Models.Channels;

public class DiscordChannel : Snowflake
{
    [JsonProperty("type")]
    public DiscordChannelType Type { get; internal set; }

    [JsonProperty("guild_id")]
    public ulong? GuildID { get; internal set; }

    [JsonProperty("position")]
    public int? Position { get; internal set; }

    [JsonProperty("permission_overwrites")]
    public List<DiscordOverwrite>? PermissionOverwrites { get; internal set; }

    [JsonProperty("name")]
    public string? Name { get; internal set; }

    [JsonProperty("parent_id")]
    public ulong? ParentID { get; internal set; }

    [JsonProperty("last_message_id")]
    public ulong? LastMessageID { get; internal set; }

    [JsonProperty("permissions")]
    public DiscordPermissions? Permissions { get; internal set; }

    [JsonProperty("recipient_ids")]
    public List<ulong>? RecipientsIDs { get; internal set; }

    [JsonIgnore]
    public DiscordGuild? Guild => GuildID is null ? null : Client.Guilds.Find(GuildID.Value);

    public async Task<IEnumerable<DiscordMessage>> GetMessages(int limit = 50)
    {
        var list = new List<DiscordMessage>();
        var req = new GetMessagesRequest(ID, limit);
        var messages = await Client.API.Execute(req);

        if (messages is null)
            throw new Exception();

        if (GuildID is null)
            return messages;

        foreach (var message in messages)
        {
            message.Author = Client.Users.AddOrUpdate(message.Author);

            var guild = Client.Guilds.Find(GuildID.Value) ?? throw new InvalidOperationException("Received a message from a guild not in cache.");
            var member = guild.MemberCache.Find(message.Author.ID);

            if (message.Member != null)
            {
                message.Member.User = message.Author;
                message.Member.UserID = message.Author.ID;
                message.Member.Client = Client;
                member = guild.MemberCache.AddOrUpdate(message.Member);
            }

            message.Author.Client = Client;
            message.Member = member;
            list.Add(message);
        }

        return list;
    }

    public async Task ModifyChannelAsync(Action<GuildChannelEditPayload> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (Type is not DiscordChannelType.Text and not DiscordChannelType.Voice and not DiscordChannelType.Category
            and not DiscordChannelType.Announcement and not DiscordChannelType.AnnouncementThread and not DiscordChannelType.Stage
            and not DiscordChannelType.Directory and not DiscordChannelType.Forum and not DiscordChannelType.Media)
            throw new InvalidOperationException("This channel cannot be edited with this method.");

        var payload = new GuildChannelEditPayload
        {
            Name = Name,
            Type = Type
        };

        action(payload);
        await Client.API.Execute(new ModifyChannelRequest(ID, payload));
    }

    public DiscordPermissions PermissionsFor(DiscordMember mem)
    {
        // if (IsThread) return this.Parent.PermissionsFor(mem);
        if (this.IsPrivate() || Guild is null) return DiscordPermissions.None;
        if (Guild.OwnerID == mem.ID) return DiscordPermissions.Administrator;

        var everyone = Guild.Roles.FirstOrDefault(x => x.ID == GuildID) ?? throw new InvalidOperationException($"@everyone roles does not exist in guild {GuildID}.");
        var permissions = everyone.Permissions;

        var roles = mem.GetRolesSorted().Where(x => x.ID != everyone.ID).ToList();
        permissions |= roles.Aggregate(DiscordPermissions.None, (p, r) => p | r.Permissions);

        if (permissions.HasFlag(DiscordPermissions.Administrator))
            return DiscordPermissions.Administrator;

        if (PermissionOverwrites is null)
            return permissions;

        var everyoneOwr = PermissionOverwrites.FirstOrDefault(o => o.ID == everyone.ID);

        if (everyoneOwr != null)
        {
            permissions &= ~everyoneOwr.Deny;
            permissions |= everyoneOwr.Allow;
        }

        var roleOwr = roles
                      .Select(r => PermissionOverwrites.FirstOrDefault(o => o.ID == r.ID))
                      .OfType<DiscordOverwrite>()
                      .ToList();

        permissions &= ~roleOwr.Aggregate(DiscordPermissions.None, (p, r) => p | r.Deny);
        permissions |= roleOwr.Aggregate(DiscordPermissions.None, (p, r) => p | r.Allow);

        var memberOwr = PermissionOverwrites.FirstOrDefault(x => x.ID == mem.ID);
        if (memberOwr is null) return permissions;

        permissions &= ~memberOwr.Deny;
        permissions |= memberOwr.Allow;

        return permissions;
    }

    public DiscordPermissions PermissionsForEveryone()
    {
        // if (IsThread) return this.Parent.PermissionsForEveryone();
        if (this.IsPrivate() || Guild is null) return DiscordPermissions.None;

        var everyone = Guild.Roles.FirstOrDefault(x => x.ID == GuildID) ?? throw new InvalidOperationException($"@everyone roles does not exist in guild {GuildID}.");
        var permissions = everyone.Permissions;

        if (permissions.HasFlag(DiscordPermissions.Administrator)) return DiscordPermissions.Administrator;
        if (PermissionOverwrites is null) return permissions;

        var everyoneOwr = PermissionOverwrites.FirstOrDefault(o => o.ID == everyone.ID);
        if (everyoneOwr == null) return permissions;

        permissions &= ~everyoneOwr.Deny;
        permissions |= everyoneOwr.Allow;

        return permissions;
    }
}
