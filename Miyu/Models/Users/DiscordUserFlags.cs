// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Models.Users;

[Flags]
public enum DiscordUserFlags
{
    Staff = 1 << 0,
    Partner = 1 << 1,
    EarlySupporter = 1 << 9,
    CertifiedModerator = 1 << 18,
    TeamUser = 1 << 10,
    BotHttpInteractions = 1 << 19,
    ActiveDeveloper = 1 << 22,

    HypeSquad = 1 << 2,
    HypeSquadBravery = 1 << 6,
    HypeSquadBrilliance = 1 << 7,
    HypeSquadBalance = 1 << 8,

    BugHunterLevel1 = 1 << 3,
    BugHunterLevel2 = 1 << 14,

    VerifiedBot = 1 << 16,
    VerifiedBotDeveloper = 1 << 17
}
