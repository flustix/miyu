// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;

namespace Miyu.UI.Graphics.Menus.Draw;

public partial class DrawableMenuSeparatorItem : Menu.DrawableMenuItem
{
    public DrawableMenuSeparatorItem(MenuItem item)
        : base(item)
    {
        BackgroundColour = Colour4.Transparent;
        BackgroundColourHover = Colour4.Transparent;

        Foreground.AutoSizeAxes = Axes.Y;
        Foreground.RelativeSizeAxes = Axes.X;
        Foreground.Padding = new MarginPadding(8);
    }

    protected override Drawable CreateContent() => new Box
    {
        RelativeSizeAxes = Axes.X,
        Height = 1,
        Colour = Catppuccin.Current.Base
    };
}
