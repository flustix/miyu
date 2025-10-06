// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages;
using Miyu.UI.Graphics;
using Miyu.Utils.Extensions;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Miyu.UI.Components.Messages;

public partial class MemberJoinMessage : ChatMessageBase
{
    public override DiscordMessage Message { get; }

    public MemberJoinMessage(DiscordMessage message)
    {
        Message = message;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Margin = new MarginPadding { Top = 16 };
        Padding = new MarginPadding(2)
        {
            Left = 72,
            Right = 24
        };

        InternalChildren = new Drawable[]
        {
            new Container
            {
                Size = new Vector2(72, 16),
                Position = new Vector2(0, 4),
                Origin = Anchor.TopRight,
                Child = new SpriteIcon
                {
                    Size = new Vector2(16),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Catppuccin.Current.Green,
                    Icon = FontAwesome.Solid.ArrowRight
                }
            },
            new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Horizontal,
                Children = new Drawable[]
                {
                    new MiyuText
                    {
                        Text = $"{Message.Author.DisplayName ?? Message.Author.Username} ",
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Weight = FontWeight.Medium
                    },
                    new MiyuText
                    {
                        Text = "is here.",
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Weight = FontWeight.Medium,
                        Colour = Catppuccin.Current.Subtext0
                    },
                    new MiyuText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Weight = FontWeight.Medium,
                        Colour = Catppuccin.Current.Overlay2,
                        Margin = new MarginPadding { Left = 4 },
                        Text = $"{Message.GetTimeOffset():hh:mm}",
                        FontSize = 12
                    }
                }
            }
        };
    }
}
