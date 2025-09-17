// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Voice;

public enum VoiceOpCode
{
    Identify = 0,
    SelectProto = 1,
    Ready = 2,
    Heartbeat = 3,
    SessionDescription = 4,
    Speaking = 5,
    HeartbeatAtk = 6,
    Resume = 7,
    Hello = 8,
    Resumed = 9,

    ClientConnect = 11,
    Video = 12,
    ClientDisconnect = 13,

    Simulcast = 15,
    BackendVersion = 16,

    ClientFlags = 18,

    ClientPlatform = 20
}
