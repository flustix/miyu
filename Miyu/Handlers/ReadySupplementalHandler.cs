// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using Miyu.Models.Guilds.Members;
using Miyu.Networking.Gateway;
using Miyu.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Miyu.Handlers;

internal class ReadySupplementalHandler : SocketEventHandler
{
    public ReadySupplementalHandler(MiyuClient client, GatewayEventHandler handler)
        : base(client, handler)
    {
    }

    internal override void Handle(JObject data)
    {
        var supp = data.TurnTo<ReadySupplement>();

        for (var i = 0; i < supp.Guilds.Count; i++)
        {
            var sg = supp.Guilds[i];
            var members = supp.MergedMembers[i];
            var guild = Client.Guilds.Find(sg.ID)!;
            guild.Members = members;
        }
    }

    [UsedImplicitly]
    private class ReadySupplement
    {
        [JsonProperty("guilds")]
        public List<SupplementGuild> Guilds { get; set; } = null!;

        [JsonProperty("merged_members")]
        public List<List<DiscordMember>> MergedMembers { get; set; } = null!;
    }

    private class SupplementGuild
    {
        [JsonProperty("id")]
        public ulong ID { get; set; }
    }
}
