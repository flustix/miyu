// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Events.Channels;
using Miyu.Models.Guilds.Members;
using Miyu.Networking.Gateway;
using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.Handlers.Channels;

internal class TypingStartHandler : SocketEventHandler
{
    public TypingStartHandler(MiyuClient client, GatewayEventHandler handler)
        : base(client, handler)
    {
    }

    internal override void Handle(JObject data)
    {
        var channel = data["channel_id"]!.ToObject<ulong>();
        var user = data["user_id"]!.ToObject<ulong>();

        var guildID = data["guild_id"]?.ToObject<ulong>();

        if (guildID != null && Client.Guilds.TryFind(guildID.Value, out var guild))
        {
            var member = data["member"]?.TurnTo<DiscordMember>();
            if (member != null) guild.MemberCache.AddOrUpdate(member);
        }

        Handler.DispatchEvent(new TypingStartEvent(Client, channel, user));
    }
}
