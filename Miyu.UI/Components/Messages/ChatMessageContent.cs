// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using Miyu.UI.Graphics;
using Miyu.UI.Utils;
using osu.Framework.Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Miyu.UI.Components.Messages;

public partial class ChatMessageContent : MiyuTextFlow
{
    public ChatMessageContent(string text)
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        var emoteRegex = Regexes.CustomEmote();
        var emoteMatches = emoteRegex.Matches(text);

        if (emoteMatches.Count == 0)
        {
            AddText(text);
            return;
        }

        var idx = 0;
        var addedText = "";
        var emojis = new List<CustomEmojiDrawable>();

        foreach (Match match in emoteMatches)
        {
            var raw = match.Value;
            var id = raw.Split(':')[2].TrimEnd('>');
            var animated = raw.StartsWith("<a:");
            var drawable = new CustomEmojiDrawable(id, animated);

            if (idx < match.Index)
            {
                var textPart = text.Substring(idx, match.Index - idx);
                addedText += textPart;
                AddText(textPart);
            }

            emojis.Add(drawable);
            AddPart(new TextPartManual(new MiyuClickable
            {
                AutoSizeAxes = Axes.Both,
                Action = drawable.ShowPopover,
                Child = drawable
            }.Yield()));
            idx = match.Index + match.Length;
        }

        if (idx < text.Length)
        {
            var remainingText = text.Substring(idx).Trim();

            if (!string.IsNullOrEmpty(remainingText))
            {
                addedText += remainingText;
                AddText(remainingText);
            }
        }

        if (string.IsNullOrWhiteSpace(addedText))
            emojis.ForEach(x => x.DisplayBig = true);
    }
}
