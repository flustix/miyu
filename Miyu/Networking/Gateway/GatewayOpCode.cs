// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Networking.Gateway;

internal enum GatewayOpCode
{
    Dispatch = 0,
    Heartbeat = 1,
    Identify = 2,
    PresenceUpdate = 3,
    VoiceStateUpdate = 4,

    /// <summary>
    ///     report a misbehaving voice server
    /// </summary>
    PingVoiceServer = 5,
    Resume = 6,
    Reconnect = 7,
    RequestGuildMembers = 8,
    InvalidSession = 9,
    Hello = 10, // hi :>
    HeartbeatAck = 11,

    /// <summary>
    ///     request a previous voice connection, unless if the `AUTO_CALL_CONNECT` capability is set
    /// </summary>
    RequestCallConnect = 13,

    /// <summary>
    ///     we start our stream
    /// </summary>
    CreateStream = 18,

    /// <summary>
    ///     we stop our stream or stop watching one
    /// </summary>
    DeleteStream = 19,

    /// <summary>
    ///     we start watching a stream
    /// </summary>
    WatchStream = 20,

    /// <summary>
    ///     ping a misbehaving stream server
    /// </summary>
    PingStreamServer = 21,

    /// <summary>
    ///     update the stream "paused" state, e.g. tabbed out or minimized
    /// </summary>
    UpdateStreamPaused = 22,

    /// <summary>
    ///     sends a command to another client session, seems to be used for consoles
    /// </summary>
    SendRemoteCommand = 29,

    /// <summary>
    ///     request a list of sounds from a list of guilds. fires a gateway event once ready
    /// </summary>
    RequestSoundboardSounds = 31,

    /// <summary>
    ///     requests the last messages from a channel. fires `LAST_MESSAGES` event
    /// </summary>
    LastMessagesRequest = 34,

    SearchRecentMembers = 35,
    RequestChannelStatuses = 36,

    /// <summary>
    ///     updates whether stuff like typing indicators and activities should be sent for guilds
    /// </summary>
    UpdateGuildSubscriptions = 37,

    RequestChannelMemberCount = 39
}
