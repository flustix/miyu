// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.UI.Graphics.Menus.Items;

public class MenuSeparatorItem : MiyuMenuItemBase
{
    public MenuSeparatorItem()
        : base("", MiyuIcon.Type.Copy, MenuItemType.Normal, () => { })
    {
    }
}
