// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace Miyu.UI.Graphics.Menus.Draw;

public abstract partial class DrawableMiyuMenuItem<T> : Menu.DrawableMenuItem
    where T : MiyuMenuItemBase
{
    protected new T Item => (base.Item as T)!;

    protected DrawableMiyuMenuItem(T item)
        : base(item)
    {
        CornerRadius = 2;
        Masking = true;

        BackgroundColour = Color4.Transparent;
        BackgroundColourHover = Catppuccin.Current.Surface2.MultiplyAlpha(0.25f);
    }

    protected override Drawable CreateContent() => new GridContainer
    {
        Size = new Vector2(224, 36),
        Alpha = Item.Enabled ? 1f : .6f,
        ColumnDimensions = new Dimension[]
        {
            new(),
            new(GridSizeMode.Absolute, 36)
        },
        Content = new[]
        {
            new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(8) { Right = 0 },
                    Child = new MiyuText
                    {
                        RelativeSizeAxes = Axes.X,
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Text = Item.Text.Value,
                        FontSize = 14,
                        Weight = FontWeight.Medium,
                        Truncate = true
                    }
                },
                new MiyuIcon(Item.Icon, 20)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            }
        }
    };
}
