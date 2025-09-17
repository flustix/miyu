// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Midori.Logging;
using Midori.Utils;
using Newtonsoft.Json.Linq;

namespace Miyu.Utils;

internal class DebugEventLogger
{
    public Dictionary<int, string> CodeMappings { get; init; } = new();

    private string prefix { get; }
    private long count;

    public DebugEventLogger(string prefix)
    {
        this.prefix = prefix;
    }

    public void Log(string json, bool receive)
    {
        var i = Interlocked.Increment(ref count);

        var type = receive ? "rcv" : "snd";
        var file = $"{i}.{type}";

        var parse = json.Deserialize<JObject>();

        if (parse.TryGetValue("op", out var op) && !string.IsNullOrWhiteSpace(op.ToString()))
        {
            var code = op.ToObject<int>();
            var str = code.ToString();

            if (CodeMappings.TryGetValue(code, out var mp))
                str = mp.ToLower();

            file += $".{str}";
        }

        if (parse.TryGetValue("t", out var t) && !string.IsNullOrWhiteSpace(t.ToString()))
            file += $".{t}";

        var path = Path.Combine("events", prefix, $"{file}.json");
        var dir = Path.GetDirectoryName(path);

        if (dir != null && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllText(path, parse.Serialize(true));
        Logger.Log($"{type} {i} {json[..Math.Min(json.Length, 256)]}", LoggingTarget.Network, LogLevel.Debug);
    }
}
