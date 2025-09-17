// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Miyu.Sharding;

[JsonConverter(typeof(ShardInfoConverter))]
public class ShardInfo
{
    public int ID { get; init; }
    public int Count { get; init; }
}

internal sealed class ShardInfoConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not ShardInfo shard)
            return;

        int[] obj = { shard.ID, shard.Count };
        serializer.Serialize(writer, obj);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var array = readArrayObject(reader, serializer);

        return new ShardInfo
        {
            ID = (int)array[0],
            Count = (int)array[1]
        };
    }

    private JArray readArrayObject(JsonReader reader, JsonSerializer serializer)
    {
        return serializer.Deserialize<JToken>(reader) is not JArray { Count: 2 } arr
            ? throw new JsonSerializationException("erm... array len 2 expected")
            : arr;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ShardInfo);
    }
}
