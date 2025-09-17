// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Miyu.UI.Components.Pages;

public partial class PageController : CompositeDrawable
{
    public Page? Current => InternalChildren.LastOrDefault() as Page;

    public event Action<Page>? OnPageChange;

    public PageController()
    {
        RelativeSizeAxes = Axes.Both;
    }

    public void SwitchPage(Page page)
    {
        InternalChild = page;
        OnPageChange?.Invoke(page);
    }
}
