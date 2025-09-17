// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Miyu.Models;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public class Snowflake
{
    /// <summary>
    ///     the unique id of this object
    /// </summary>
    [JsonProperty("id")]
    public ulong ID { get; internal set; }

    [JsonIgnore]
    internal MiyuClient Client { get; set; } = null!;
}
