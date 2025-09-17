// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Events.Interactions;
using Miyu.Models.Interaction;
using Miyu.Networking.Gateway;
using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.Handlers.Applications.Interactions;

internal class InteractionCreateHandler : SocketEventHandler
{
    public InteractionCreateHandler(MiyuClient client, GatewayEventHandler handler)
        : base(client, handler)
    {
    }

    internal override void Handle(JObject data)
    {
        var type = data["type"]?.ToObject<DiscordInteractionType>();

        if (type is null)
            return;

        switch (type)
        {
            case DiscordInteractionType.Ping:
                break;

            case DiscordInteractionType.ApplicationCommand:
            {
                var cmd = data.TurnTo<DiscordInteraction>();
                cmd.Client = Client;
                cmd.User ??= cmd.Member?.User;

                Client.GetUser(cmd.User!.ID).Wait();
                Client.Users.AddOrUpdate(cmd.User!);

                Handler.DispatchEvent(new InteractionCommandEvent(Client, cmd));
                break;
            }

            case DiscordInteractionType.MessageComponent:
                break;

            case DiscordInteractionType.AutoComplete:
                break;

            case DiscordInteractionType.Modal:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
