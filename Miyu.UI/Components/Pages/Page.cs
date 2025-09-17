// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Miyu.UI.Components.Pages;

public partial class Page : CompositeDrawable
{
    public Page()
    {
        RelativeSizeAxes = Axes.Both;
        Anchor = Origin = Anchor.Centre;
    }
}
