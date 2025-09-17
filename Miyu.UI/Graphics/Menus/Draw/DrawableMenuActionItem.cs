// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Graphics.Menus.Items;

namespace Miyu.UI.Graphics.Menus.Draw;

public partial class DrawableMenuActionItem : DrawableMiyuMenuItem<MenuActionItem>
{
    public DrawableMenuActionItem(MenuActionItem item)
        : base(item)
    {
    }
}
