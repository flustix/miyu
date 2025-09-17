// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.Models.Guilds;
using Miyu.Models.Guilds.Members;
using Miyu.Models.Users;
using Miyu.Sharding;
using Newtonsoft.Json;

namespace Miyu.Events;

public class ReadyEvent : GenericEvent
{
    internal override EventType Type => EventType.Ready;

    public RawReadyEvent Event { get; }

    public ReadyEvent(MiyuClient client, RawReadyEvent ev)
        : base(client)
    {
        Event = ev;
    }

    public class RawReadyEvent
    {
        [JsonProperty("v")]
        public int Version { get; set; }

        [JsonProperty("user")]
        public DiscordUser User { get; set; } = null!;

        [JsonProperty("guilds")]
        public List<DiscordGuild> Guilds { get; set; } = new();

        [JsonProperty("users")]
        public List<DiscordUser> Users { get; set; } = new();

        [JsonProperty("private_channels")]
        public List<DiscordChannel> PrivateChannels { get; set; } = new();

        [JsonProperty("session_id")]
        public string SessionID { get; set; } = null!;

        [JsonProperty("resume_gateway_url")]
        public string ResumeUrl { get; set; } = null!;

        [JsonProperty("shard")]
        public ShardInfo? Shard { get; set; }

        [JsonProperty("application")]
        public object Application { get; set; } = null!;

        [JsonProperty("merged_members")]
        public List<List<DiscordMember>>? MergedMembers { get; set; }
    }
}
