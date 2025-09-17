// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace Miyu.UI.Overlay;

public partial class LoadingOverlay : CompositeDrawable
{
    private MiyuText? state;
    private string text = "Establishing websocket...";

    public string StateText
    {
        set
        {
            text = value;
            if (state != null) state.Text = value;
        }
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.Both;
        AlwaysPresent = true;
        Alpha = 0;

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Base
            },
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new MiyuText
                    {
                        Text = "Connecting...",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Weight = FontWeight.SemiBold,
                        FontSize = 20
                    },
                    state = new MiyuText
                    {
                        Text = text,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
                }
            }
        };
    }

    public override void Show()
    {
        this.FadeInFromZero(800);
    }

    public override void Hide()
    {
        this.FadeOut(800);
    }
}
