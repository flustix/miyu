// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace Miyu.UI.Graphics.Menus.Items;

public class MenuActionItem : MiyuMenuItemBase
{
    public MenuActionItem(LocalisableString text, MiyuIcon.Type icon, Action action)
        : this(text, icon, MenuItemType.Normal, action)
    {
    }

    public MenuActionItem(LocalisableString text, MiyuIcon.Type icon, MenuItemType type, Action action)
        : base(text, icon, type, action)
    {
    }
}
