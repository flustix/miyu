// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Networking.Gateway;
using Newtonsoft.Json.Linq;

namespace Miyu.Handlers;

internal abstract class SocketEventHandler
{
    protected MiyuClient Client { get; }
    protected GatewayEventHandler Handler { get; }

    protected SocketEventHandler(MiyuClient client, GatewayEventHandler handler)
    {
        Client = client;
        Handler = handler;
    }

    internal abstract void Handle(JObject data);
}
