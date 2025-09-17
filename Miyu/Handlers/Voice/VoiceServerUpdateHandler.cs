// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Events.Voice;
using Miyu.Networking.Gateway;
using Miyu.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.Handlers.Voice;

internal class VoiceServerUpdateHandler : SocketEventHandler
{
    public VoiceServerUpdateHandler(MiyuClient client, GatewayEventHandler handler)
        : base(client, handler)
    {
    }

    internal override void Handle(JObject data)
    {
        var ev = data.TurnTo<VoiceServerUpdateEvent.Raw>();
        Handler.DispatchEvent(new VoiceServerUpdateEvent(Client, ev));
    }
}
