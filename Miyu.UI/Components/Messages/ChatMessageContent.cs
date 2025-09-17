// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Components.Messages.Content;
using Miyu.UI.Graphics;
using osu.Framework.Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace Miyu.UI.Components.Messages;

public partial class ChatMessageContent : MiyuTextFlow
{
    public ChatMessageContent(MiyuClient client, string text)
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        var parts = MessageContentParser.Parse(text);
        var emojis = new List<CustomEmojiDrawable>();
        var rawText = "";

        foreach (var part in parts)
        {
            switch (part)
            {
                case ContentParts.Text t:
                    rawText += t.Value;
                    AddText(t.Value);
                    break;

                case ContentParts.Emote e:
                    var drawable = new CustomEmojiDrawable(e.ID, e.Animated);
                    emojis.Add(drawable);
                    AddPart(new TextPartManual(new MiyuClickable
                    {
                        AutoSizeAxes = Axes.Both,
                        Action = drawable.ShowPopover,
                        Child = drawable
                    }.Yield()));
                    break;

                case ContentParts.UserMention um:
                    var user = client.Users.Find(um.ID);
                    var name = user?.DisplayName ?? user?.Username ?? um.ToString();

                    AddPart(new TextPartManual(new Container()
                    {
                        AutoSizeAxes = Axes.Both,
                        CornerRadius = 3,
                        Masking = true,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Catppuccin.Current.Blue,
                                Alpha = 0.3f
                            },
                            new MiyuText
                            {
                                Text = $"@{name}",
                                Weight = FontWeight.Medium,
                                Colour = Catppuccin.Current.Blue,
                                Margin = new MarginPadding { Horizontal = 2 }
                            }
                        }
                    }.Yield()));
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(rawText))
            emojis.ForEach(x => x.DisplayBig = true);
    }
}
