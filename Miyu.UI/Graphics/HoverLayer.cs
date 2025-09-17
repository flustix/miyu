// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace Miyu.UI.Graphics;

public partial class HoverLayer : Box
{
    public float TargetAlpha { get; init; } = .2f;

    public HoverLayer()
    {
        RelativeSizeAxes = Axes.Both;
        Colour = Catppuccin.Current.Text;
        Alpha = 0;
    }

    public override void Show()
    {
        this.FadeTo(TargetAlpha, 50);
    }

    public override void Hide()
    {
        this.FadeOut(200);
    }
}
