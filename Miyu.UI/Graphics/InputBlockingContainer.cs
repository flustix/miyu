// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace Miyu.UI.Graphics;

public partial class InputBlockingContainer : Container
{
    public Action<ClickEvent>? ClickAction { get; set; } = null;
    public Func<KeyDownEvent, bool>? KeyDownAction { get; set; } = null;

    protected override bool OnClick(ClickEvent e)
    {
        ClickAction?.Invoke(e);
        return true;
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        return KeyDownAction?.Invoke(e) ?? false;
    }

    protected override bool Handle(UIEvent e)
    {
        return e is not TouchEvent;
    }
}
