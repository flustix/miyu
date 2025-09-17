// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osuTK;
using osuTK.Input;

namespace Miyu.UI.Graphics;

public partial class MiyuScrollContainer : MiyuScrollContainer<Drawable>
{
    public MiyuScrollContainer(Direction direction = Direction.Vertical)
        : base(direction)
    {
    }
}

public partial class MiyuScrollContainer<T> : BasicScrollContainer<T>, IHasCursorType
    where T : Drawable
{
    [Resolved]
    private GameHost host { get; set; } = null!;

    public CursorType Cursor => isDragging ? CursorType.SizeVertical : CursorType.Arrow;

    protected override bool IsDragging => base.IsDragging || isDragging;

    public bool AllowDragScrolling = true;

    private bool isDragging;
    private Vector2 dragStart;
    private Vector2 dragCurrent;

    public MiyuScrollContainer(Direction direction = Direction.Vertical)
        : base(direction)
    {
    }

    private bool shouldDrag(MouseButtonEvent e)
    {
        return e.Button == MouseButton.Middle && AllowDragScrolling;
    }

    protected override bool OnMouseDown(MouseDownEvent e)
    {
        if (!shouldDrag(e)) return false;

        dragStart = dragCurrent = e.MousePosition;
        isDragging = true;
        return true;
    }

    protected override void OnMouseUp(MouseUpEvent e)
    {
        if (e.Button == MouseButton.Middle)
            isDragging = false;
    }

    protected override bool OnMouseMove(MouseMoveEvent e)
    {
        if (!isDragging) return false;

        dragCurrent = e.MousePosition;
        return true;
    }

    protected override void Update()
    {
        base.Update();

        if (!isDragging)
            return;

        var dist = dragCurrent - dragStart;
        var amount = (ScrollDirection == Direction.Vertical ? dist.Y : dist.X) * (Time.Elapsed / 100);
        ScrollTo(Math.Clamp(Current + amount, -64, ScrollableExtent + 64), false);
    }
}
