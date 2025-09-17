// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Events;

public enum EventType
{
    Hello,
    Ready,
    Resumed,
    Reconnect,
    InvalidSession,

    ApplicationCommandPermissionsUpdate,

    AutoModerationRuleCreate,
    AutoModerationRuleUpdate,
    AutoModerationRuleDelete,
    AutoModerationActionExecution,

    ChannelCreate,
    ChannelUpdate,
    ChannelDelete,
    ChannelPinsUpdate,

    ThreadCreate,
    ThreadUpdate,
    ThreadDelete,
    ThreadListSync,
    ThreadMemberUpdate,
    ThreadMembersUpdate,

    EntitlementCreate,
    EntitlementUpdate,
    EntitlementDelete,

    GuildCreate,
    GuildUpdate,
    GuildDelete,

    GuildAuditLogEntryCreate,
    GuildBanAdd,
    GuildBanRemove,

    GuildEmojisUpdate,
    GuildStickersUpdate,

    GuildIntegrationsUpdate,

    GuildMemberAdd,
    GuildMemberRemove,
    GuildMemberUpdate,
    GuildMembersChunk,

    GuildRoleCreate,
    GuildRoleUpdate,
    GuildRoleDelete,

    GuildScheduledEventCreate,
    GuildScheduledEventUpdate,
    GuildScheduledEventDelete,
    GuildScheduledEventUserAdd,
    GuildScheduledEventUserRemove,

    GuildSoundboardSoundCreate,
    GuildSoundboardSoundUpdate,
    GuildSoundboardSoundDelete,
    GuildSoundboardSoundsUpdate,

    SoundboardSounds,

    IntegrationCreate,
    IntegrationUpdate,
    IntegrationDelete,

    InteractionCommand,
    InteractionComponent,
    InteractionAutoComplete,
    InteractionModal,

    InviteCreate,
    InviteDelete,

    MessageCreate,
    MessageUpdate,
    MessageDelete,
    MessageDeleteBulk,
    MessageReactionAdd,
    MessageReactionRemove,
    MessageReactionRemoveAll,
    MessageReactionRemoveEmoji,

    PresenceUpdate,

    StageInstanceCreate,
    StageInstanceUpdate,
    StageInstanceDelete,

    SubscriptionCreate,
    SubscriptionUpdate,
    SubscriptionDelete,

    TypingStart,

    UserUpdate,

    VoiceChannelEffectSend,
    VoiceStateUpdate,
    VoiceServerUpdate,

    WebhooksUpdate,

    MessagePollVoteAdd,
    MessagePollVoteRemove
}
