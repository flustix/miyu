// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Guilds;
using Miyu.UI.Components.Messages.Content;
using Miyu.UI.Graphics;
using Miyu.Utils.Extensions;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace Miyu.UI.Components.Messages;

public partial class ChatMessageContent : MiyuTextFlow
{
    private readonly string text;

    public ChatMessageContent(string text)
    {
        this.text = text;
    }

    [BackgroundDependencyLoader]
    private void load(MiyuClient client, DiscordGuild? guild)
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
                    var member = guild?.MemberCache.Find(um.ID);

                    var name = member?.Nickname ?? user?.DisplayName ?? user?.Username ?? um.ID.ToString();
                    var color = Catppuccin.Current.Blue;

                    var role = member?.GetTopRoleWithColor();

                    if (role != null)
                    {
                        var hex = role.Color.ToString("X6");
                        color = Colour4.FromHex(hex);
                    }

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
                                Colour = color,
                                Alpha = 0.3f
                            },
                            new MiyuText
                            {
                                Text = $"@{name}",
                                Weight = FontWeight.Medium,
                                Colour = color,
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
