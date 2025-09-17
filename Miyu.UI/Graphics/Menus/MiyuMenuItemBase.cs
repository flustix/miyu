// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

namespace Miyu.UI.Graphics.Menus;

public abstract class MiyuMenuItemBase : MenuItem
{
    public MenuItemType Type { get; }
    public MiyuIcon.Type Icon { get; }

    protected MiyuMenuItemBase(LocalisableString text, MiyuIcon.Type icon, MenuItemType type, Action action)
        : base(text, action)
    {
        Type = type;
        Icon = icon;
    }
}
