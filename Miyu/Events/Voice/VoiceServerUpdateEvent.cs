// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace Miyu.Events.Voice;

public class VoiceServerUpdateEvent : GenericEvent
{
    internal override EventType Type => EventType.VoiceServerUpdate;

    public Raw RawEvent { get; }

    public VoiceServerUpdateEvent(MiyuClient client, Raw ev)
        : base(client)
    {
        RawEvent = ev;
    }

    public class Raw
    {
        [JsonProperty("token")]
        public string Token { get; init; } = null!;

        [JsonProperty("guild_id")]
        public ulong? GuildID { get; init; }

        [JsonProperty("endpoint")]
        public string Endpoint { get; init; } = null!;
    }
}
