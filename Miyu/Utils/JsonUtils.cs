// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Utils;
using Newtonsoft.Json;

namespace Miyu.Utils;

public static class JsonUtils
{
    public static void RegisterConverter(JsonConverter conv)
    {
        Midori.Utils.JsonUtils.Converters.Add(conv);
    }

    public static T TurnTo<T>(this object obj) where T : class
    {
        return obj.Serialize().Deserialize<T>()!;
    }
}
