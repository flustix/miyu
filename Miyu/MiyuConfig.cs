// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models;

namespace Miyu;

public class MiyuConfig
{
    public string Token { get; set; } = null!;
    public bool ClientToken { get; init; } = false;

    public DiscordIntents Intents { get; init; } = DiscordIntents.AllUnprivileged;
    public bool RegisterCommands { get; init; } = true;
}
