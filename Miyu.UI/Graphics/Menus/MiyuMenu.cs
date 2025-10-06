// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Graphics.Menus.Draw;
using Miyu.UI.Graphics.Menus.Items;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;
using osuTK;

namespace Miyu.UI.Graphics.Menus;

public partial class MiyuMenu : Menu
{
    public MiyuMenu(Direction direction, bool topLevelMenu = false)
        : base(direction, topLevelMenu)
    {
        BackgroundColour = Catppuccin.Current.Surface0;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        ItemsContainer.Padding = new MarginPadding(8);
    }

    protected override Menu CreateSubMenu() => new MiyuMenu(Direction.Vertical)
    {
        Anchor = Direction == Direction.Horizontal ? Anchor.TopLeft : Anchor.TopRight
    };

    protected override void UpdateSize(Vector2 newSize)
    {
        if (Direction == Direction.Vertical)
        {
            Width = 240;
            this.ResizeHeightTo(newSize.Y == 0 ? Height : newSize.Y);
        }
        else
        {
            Height = newSize.Y;
            this.ResizeWidthTo(newSize.X);
        }

        Logger.Log($"MiyuMenu resized to {newSize}");
    }

    protected override void AnimateOpen() => this.FadeInFromZero(100).ScaleTo(0.9f).ScaleTo(1f, 200, Easing.OutElasticQuarter);
    protected override void AnimateClose() => this.ScaleTo(0.9f, 200, Easing.OutQuint).FadeOut(100);

    protected override DrawableMenuItem CreateDrawableMenuItem(MenuItem item) => item switch
    {
        MenuActionItem action => new DrawableMenuActionItem(action),
        MenuSeparatorItem => new DrawableMenuSeparatorItem(item),
        _ => new BasicMenu.BasicDrawableMenuItem(item)
    };

    protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) => new MiyuScrollContainer
    {
        ScrollbarVisible = false
    };
}
