// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Events;

public abstract class GenericEvent
{
    internal abstract EventType Type { get; }

    public MiyuClient Client { get; }

    protected GenericEvent(MiyuClient client)
    {
        Client = client;
    }
}
