// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osuTK;

namespace Miyu.UI.Graphics;

public partial class ZoomContainer : CursorTypeContainer
{
    public Vector2 Zoom { get; set; }

    public ZoomContainer(Drawable child)
    {
        RelativeSizeAxes = Axes.Both;
        InternalChild = child;
    }

    protected override void Update()
    {
        base.Update();

        var x = 1 / Zoom.X;
        var y = 1 / Zoom.Y;

        Size = new Vector2(x, y);
        Scale = Zoom;
    }
}
