// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace Miyu.UI.Components.Overlays;

public partial class OverlayManager : CompositeDrawable
{
    public bool AnyOpen => stack.Count > 0;

    private readonly Stack<Drawable> stack = new();
    private InputBlockingContainer background = null!;
    private Container content = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.Both;
        InternalChildren = new Drawable[]
        {
            background = new InputBlockingContainer
            {
                RelativeSizeAxes = Axes.Both,
                ClickAction = _ => closeCurrent(),
                KeyDownAction = OnKeyDown,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Catppuccin.Current.Crust,
                    Alpha = 0.75f
                }
            },
            content = new Container
            {
                RelativeSizeAxes = Axes.Both
            }
        };
    }

    public void Push(Drawable draw)
    {
        draw.Anchor = draw.Origin = Anchor.Centre;
        content.Add(draw);

        if (stack.TryPeek(out var top))
            animateClose(top).OnComplete(_ => animateOpen(draw));
        else
            animateOpen(draw);

        stack.Push(draw);
    }

    private TransformSequence<T> animateOpen<T>(T draw) where T : Drawable
    {
        return draw.ScaleTo(0.9f).FadeInFromZero(100).ScaleTo(1f, 600, Easing.OutElasticHalf);
    }

    private TransformSequence<T> animateClose<T>(T draw) where T : Drawable
    {
        return draw.FadeOut(100).ScaleTo(0.9f, 300, Easing.OutQuint);
    }

    private void closeCurrent()
    {
        if (!stack.TryPeek(out var top)) return;

        animateClose(top).Expire();
    }

    protected override void Update()
    {
        base.Update();

        if (!stack.TryPeek(out var top))
        {
            background.Alpha = 0;
            return;
        }

        var alpha = top.Alpha;

        if (top.Parent == null)
        {
            stack.Pop();
            alpha = 0;
        }

        if (stack.Count > 1) background.Alpha = 1;
        else background.Alpha = alpha;
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Key != Key.Escape || e.Repeat) return false;
        if (stack.Count <= 0) return false;

        closeCurrent();
        return true;
    }
}
