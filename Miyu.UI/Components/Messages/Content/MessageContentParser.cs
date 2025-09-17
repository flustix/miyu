// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using Miyu.UI.Utils;

namespace Miyu.UI.Components.Messages.Content;

public static class MessageContentParser
{
    public static List<ContentParts.IBase> Parse(string input)
    {
        var parts = new List<ContentParts.IBase>();
        if (string.IsNullOrWhiteSpace(input)) return parts;

        var str = input;

        while (str.Length > 0)
        {
            var result = findClosest(str);

            if (result is null)
            {
                parts.Add(new ContentParts.Text(str));
                break;
            }

            var (idx, end, part) = result.Value;

            if (idx > 0)
            {
                var before = str[..idx];
                parts.Add(new ContentParts.Text(before));
            }

            parts.Add(part);
            str = str[end..];
        }

        return parts;
    }

    private static (int idx, int end, ContentParts.IBase part)? findClosest(string input)
    {
        var results = new List<(RegexType type, Match match)>();
        results.AddRange(Regexes.CustomEmote().Matches(input).Select(x => (RegexType.Emote, x)));
        results.AddRange(Regexes.UserMention().Matches(input).Select(x => (RegexType.UserMention, x)));

        if (results.Count == 0) return null;

        foreach (var (type, match) in results)
        {
            var str = match.Value;

            switch (type)
            {
                case RegexType.Emote:
                {
                    if (!ulong.TryParse(str.Split(':')[2].TrimEnd('>'), out var id))
                        continue;

                    var animated = str.StartsWith("<a:");
                    return (match.Index, match.Index + match.Length, new ContentParts.Emote(id, animated));
                }

                case RegexType.UserMention:
                {
                    if (!ulong.TryParse(str.Split('@').Last().TrimEnd('>'), out var id))
                        continue;

                    return (match.Index, match.Index + match.Length, new ContentParts.UserMention(id));
                }
            }
        }

        return null;
    }

    private enum RegexType
    {
        Emote,
        UserMention
    }
}
