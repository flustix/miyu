// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;

namespace Miyu.UI.Graphics.Menus.Context;

public partial class MiyuContextContainer : ContextMenuContainer
{
    protected override Menu CreateMenu() => new ContextMenu();

    private partial class ContextMenu : MiyuMenu
    {
        public ContextMenu()
            : base(Direction.Vertical)
        {
            MaskingContainer.CornerRadius = 8;
            ItemsContainer.CornerRadius = 8;
            ItemsContainer.Masking = true;
        }

        protected override Menu CreateSubMenu() => new ContextMenu();
    }
}
