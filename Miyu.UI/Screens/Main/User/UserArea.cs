// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Screens.Main.User;

public partial class UserArea : CompositeDrawable
{
    [Resolved]
    private AppScreen app { get; set; } = null!;

    public const float PADDING = 8;

    [BackgroundDependencyLoader]
    private void load(MiyuClient client)
    {
        Width = 360;
        AutoSizeAxes = Axes.Y;
        Anchor = Origin = Anchor.BottomLeft;
        Padding = new MarginPadding(PADDING);

        InternalChild = new Container
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Masking = true,
            BorderThickness = 1,
            BorderColour = Catppuccin.Current.Surface0,
            CornerRadius = 8,
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Catppuccin.Current.Base
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 56,
                            Padding = new MarginPadding(8),
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(8),
                            Children = new Drawable[]
                            {
                                new Container
                                {
                                    Size = new Vector2(40),
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    CornerRadius = 20,
                                    Masking = true,
                                    Child = new DelayedImage($"https://cdn.discordapp.com/avatars/{client.Self.ID}/{client.Self.AvatarHash}.png?size=64")
                                    {
                                        RelativeSizeAxes = Axes.Both
                                    }
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Direction = FillDirection.Vertical,
                                    Children = new Drawable[]
                                    {
                                        new MiyuText
                                        {
                                            Text = client.Self.DisplayName ?? client.Self.Username,
                                            FontSize = 14,
                                            Weight = FontWeight.Medium
                                        },
                                        new MiyuText
                                        {
                                            Text = client.Self.Username,
                                            FontSize = 12,
                                            Weight = FontWeight.Normal,
                                            Alpha = client.Self.DisplayName == client.Self.Username ? 0 : 1
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    protected override bool OnClick(ClickEvent e)
    {
        app.Settings?.Show();
        return true;
    }
}
