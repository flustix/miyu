// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Models;

[Flags]
public enum DiscordPermissions : ulong
{
    None = 0,

    /// <summary>
    ///     Allows creation of instant invites
    /// </summary>
    CreateInvite = 1 << 0,

    /// <summary>
    ///     Allows kicking members
    /// </summary>
    KickMembers = 1 << 1,

    /// <summary>
    ///     Allows banning members
    /// </summary>
    BanMembers = 1 << 2,

    /// <summary>
    ///     Allows all permissions and bypasses channel permission overwrites
    /// </summary>
    Administrator = 1 << 3,

    /// <summary>
    ///     Allows management and editing of channels
    /// </summary>
    ManageChannels = 1 << 4,

    /// <summary>
    ///     Allows management and editing of the guild
    /// </summary>
    ManageGuild = 1 << 5,

    /// <summary>
    ///     Allows for the addition of reactions to messages
    /// </summary>
    AddReactions = 1 << 6,

    /// <summary>
    ///     Allows for viewing of audit logs
    /// </summary>
    ViewAuditLog = 1 << 7,

    /// <summary>
    ///     Allows for using priority speaker in a voice channel
    /// </summary>
    PrioritySpeaker = 1 << 8,

    /// <summary>
    ///     Allows the user to go live
    /// </summary>
    Stream = 1 << 9,

    /// <summary>
    ///     Allows guild members to view a channel, which includes reading messages in text channels and joining voice channels
    /// </summary>
    ViewChannel = 1 << 10,

    /// <summary>
    ///     Allows for sending messages in a channel and creating threads in a forum (does not allow sending messages in
    ///     threads)
    /// </summary>
    SendMessages = 1 << 11,

    /// <summary>
    ///     Allows for sending of <c>/tts</c> messages
    /// </summary>
    SendTTSMessages = 1 << 12,

    /// <summary>
    ///     Allows for deletion of other users messages
    /// </summary>
    ManageMessages = 1 << 13,

    /// <summary>
    ///     Links sent by users with this permission will be auto-embedded
    /// </summary>
    EmbedLinks = 1 << 14,

    /// <summary>
    ///     Allows for uploading images and files
    /// </summary>
    AttachFiles = 1 << 15,

    /// <summary>
    ///     Allows for reading of message history
    /// </summary>
    ReadMessageHistory = 1 << 16,

    /// <summary>
    ///     Allows for using the <c>@everyone</c> tag to notify all users in a channel, and the <c>@here</c> tag to notify all
    ///     online users in a channel
    /// </summary>
    MentionEveryone = 1 << 17,

    /// <summary>
    ///     Allows the usage of custom emojis from other servers
    /// </summary>
    UseExternalEmojis = 1 << 18,

    /// <summary>
    ///     Allows for viewing guild insights
    /// </summary>
    ViewGuildInsights = 1 << 19,

    /// <summary>
    ///     Allows for joining of a voice channel
    /// </summary>
    Connect = 1 << 20,

    /// <summary>
    ///     Allows for speaking in a voice channel
    /// </summary>
    Speak = 1 << 21,

    /// <summary>
    ///     Allows for muting members in a voice channel
    /// </summary>
    MuteMembers = 1 << 22,

    /// <summary>
    ///     Allows for deafening of members in a voice channel
    /// </summary>
    DeafenMembers = 1 << 23,

    /// <summary>
    ///     Allows for moving of members between voice channels
    /// </summary>
    MoveMembers = 1 << 24,

    /// <summary>
    ///     Allows for using voice-activity-detection in a voice channel
    /// </summary>
    UseVad = 1 << 25,

    /// <summary>
    ///     Allows for modification of own nickname
    /// </summary>
    ChangeNickname = 1 << 26,

    /// <summary>
    ///     Allows for modification of other users nicknames
    /// </summary>
    ManageNicknames = 1 << 27,

    /// <summary>
    ///     Allows management and editing of roles
    /// </summary>
    ManageRoles = 1 << 28,

    /// <summary>
    ///     Allows for editing and deleting scheduled events created by all users
    /// </summary>
    ManageWebhooks = 1 << 29,

    /// <summary>
    ///     Allows for editing and deleting emojis, stickers, and soundboard sounds created by all users
    /// </summary>
    ManageExpressions = 1 << 30,

    /// <summary>
    ///     Allows members to use application commands, including slash commands and context menu commands.
    /// </summary>
    UseApplicationCommands = 1L << 31,

    /// <summary>
    ///     Allows for requesting to speak in stage channels. (This permission is under active development and may be changed
    ///     or removed.)
    /// </summary>
    RequestToSpeak = 1L << 32,

    /// <summary>
    ///     Allows for editing and deleting scheduled events created by all users
    /// </summary>
    ManageEvents = 1L << 33,

    /// <summary>
    ///     Allows for deleting and archiving threads, and viewing all private threads
    /// </summary>
    ManageThreads = 1L << 34,

    /// <summary>
    ///     Allows for creating public and announcement threads
    /// </summary>
    CreatePublicThreads = 1L << 35,

    /// <summary>
    ///     Allows for creating private threads
    /// </summary>
    CreatePrivateThreads = 1L << 36,

    /// <summary>
    ///     Allows the usage of custom stickers from other servers
    /// </summary>
    UseExternalStickers = 1L << 37,

    /// <summary>
    ///     Allows for sending messages in threads
    /// </summary>
    SendMessagesInThreads = 1L << 38,

    /// <summary>
    ///     Allows for using Activities (applications with the <c>EMBEDDED</c> flag) in a voice channel
    /// </summary>
    StartEmbeddedActivities = 1L << 39,

    /// <summary>
    ///     Allows for timing out users to prevent them from sending or reacting to messages in chat and threads, and from
    ///     speaking in voice and stage channels
    /// </summary>
    ModerateMembers = 1L << 40,

    /// <summary>
    ///     Allows for viewing role subscription insights
    /// </summary>
    ViewCreatorMonetizationAnalytics = 1L << 41,

    /// <summary>
    ///     Allows for using soundboard in a voice channel
    /// </summary>
    UseSoundboard = 1L << 42,

    /// <summary>
    ///     Allows for creating emojis, stickers, and soundboard sounds, and editing and deleting those created by the current
    ///     user. Not yet available to developers,
    ///     <a
    ///         href="https://discord.com/developers/docs/change-log#clarification-on-permission-splits-for-expressions-and-events">
    ///         see
    ///         changelog
    ///     </a>
    ///     .
    /// </summary>
    CreateExpressions = 1L << 43,

    /// <summary>
    ///     Allows for creating scheduled events, and editing and deleting those created by the current user. Not yet available
    ///     to developers,
    ///     <a
    ///         href="https://discord.com/developers/docs/change-log#clarification-on-permission-splits-for-expressions-and-events">
    ///         see
    ///         changelog
    ///     </a>
    ///     .
    /// </summary>
    CreateEvents = 1L << 44,

    /// <summary>
    ///     Allows the usage of custom soundboard sounds from other servers
    /// </summary>
    UseExternalSoundboard = 1L << 45,

    /// <summary>
    ///     Allows sending voice messages
    /// </summary>
    SendVoiceMessages = 1L << 46,

    /// <summary>
    ///     Allows creating polls
    /// </summary>
    SendPolls = 1L << 49
}

public static class DiscordPermissionsExtensions
{
    public static bool ContainsPermission(this DiscordPermissions existing, DiscordPermissions check)
    {
        return existing.HasFlag(DiscordPermissions.Administrator) || existing.HasFlag(check);
    }
}
