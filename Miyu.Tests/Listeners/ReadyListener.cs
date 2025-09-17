// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Logging;
using Miyu.Attributes;
using Miyu.Events;

namespace Miyu.Tests.Listeners;

public class ReadyListener
{
    private Logger logger => Program.Logger;

    [EventListener(EventType.Ready)]
    public void OnReady(ReadyEvent ready)
    {
        logger.Add($"Connected as {ready.Event.User.Username}#{ready.Event.User.Discriminator}");
        logger.Add($"Loaded {ready.Event.Guilds.Count} guild(s)");
        logger.Add($"Running on shard {(ready.Event.Shard?.ID ?? 0) + 1}/{ready.Event.Shard?.Count ?? 1}");
    }
}
