// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osuTK;

namespace Miyu.UI.Graphics;

public partial class ZoomContainer : CursorTypeContainer
{
    public Bindable<float> Zoom { get; init; } = new();

    public ZoomContainer(Drawable child)
    {
        RelativeSizeAxes = Axes.Both;
        InternalChild = child;
    }

    protected override void Update()
    {
        base.Update();

        var x = 1 / Zoom.Value;
        var y = 1 / Zoom.Value;

        Size = new Vector2(x, y);
        Scale = new Vector2(Zoom.Value);
    }
}
