// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using AutoMapper.Configuration.Annotations;
using Midori.Utils;
using Miyu.Caches;
using Miyu.Models.Channels;
using Miyu.Models.Guilds.Expressions;
using Miyu.Models.Guilds.Members;
using Miyu.Models.Guilds.Roles;
using Miyu.Networking.Gateway;
using Newtonsoft.Json;

namespace Miyu.Models.Guilds;

public class DiscordGuild : Snowflake
{
    [JsonProperty("name")]
    public string Name { get; internal set; } = null!;

    [JsonProperty("icon")]
    public string? Icon { get; internal set; }

    [JsonProperty("icon_hash")]
    public string? IconHash { get; internal set; }

    [JsonProperty("roles")]
    public List<DiscordRole> Roles { get; internal set; } = null!;

    [JsonProperty("members")]
    public List<DiscordMember> Members
    {
        get => MemberCache.Items.ToList();
        internal set => value.ForEach(x =>
        {
#pragma warning disable CS0472
            Debug.Assert(x.ID != null && x.ID != 0);
#pragma warning restore CS0472
            MemberCache.AddOrUpdate(x);
        });
    }

    [JsonProperty("channels")]
    public List<DiscordChannel> Channels { get; internal set; } = null!;

    [JsonProperty("emojis")]
    public List<DiscordEmote> Emojis { get; internal set; } = null!;

    [JsonProperty("unavailable")]
    public bool Unavailable { get; internal set; }

    [JsonProperty("owner_id")]
    public ulong OwnerID { get; internal set; }

    [Ignore]
    [JsonProperty("properties")]
    public DiscordGuild? Properties { get; internal set; }

    [JsonIgnore]
    public SnowflakeCache<DiscordMember> MemberCache { get; internal set; } = null!;

    private bool sentSubscriptions;

    public async Task SubscribeToEvents()
    {
        if (sentSubscriptions) return;

        await Client.SendPayload(new GatewayPayload
        {
            OpCode = GatewayOpCode.UpdateGuildSubscriptions,
            Data = new
            {
                subscriptions = new Dictionary<ulong, object>
                {
                    {
                        ID, new
                        {
                            typing = true,
                            activities = true,
                            threads = true
                        }
                    }
                }
            }
        }.Serialize());

        sentSubscriptions = true;
    }
}
