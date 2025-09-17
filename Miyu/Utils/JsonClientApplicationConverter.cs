// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Caches;
using Miyu.Models.Guilds;
using Miyu.Models.Guilds.Members;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Miyu.Utils;

internal class JsonClientApplicationConverter : JsonConverter
{
    private MiyuClient client { get; }

    public JsonClientApplicationConverter(MiyuClient client)
    {
        this.client = client;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var value = JObject.Load(reader);

        var sr = new JsonSerializer();

        foreach (var conv in serializer.Converters)
        {
            if (conv is not JsonClientApplicationConverter)
                sr.Converters.Add(conv);
        }

        object obj;

        switch (objectType.Name)
        {
            case "DiscordGuild":
                obj = new DiscordGuild
                {
                    Client = client,
                    MemberCache = new SnowflakeCache<DiscordMember>(client)
                };
                break;

            case "DiscordMember":
                obj = new DiscordMember { Client = client };
                break;

            default:
                throw new InvalidOperationException();
        }

        using var read = value.CreateReader();
        sr.Populate(read, obj);
        return obj;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DiscordGuild) || objectType == typeof(DiscordMember);
    }
}
