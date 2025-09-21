// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osuTK;

namespace Miyu.UI.Components.Form;

public partial class MiyuFormLabel : FillFlowContainer
{
    private readonly LocalisableString label;
    private readonly bool required;

    public MiyuFormLabel(LocalisableString label, bool required = false)
    {
        this.label = label;
        this.required = required;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        Height = 20;
        Direction = FillDirection.Horizontal;
        Spacing = new Vector2(4);

        InternalChildren = new Drawable[]
        {
            new MiyuText
            {
                Text = label,
                FontSize = 16,
                Weight = FontWeight.SemiBold
            },
            new MiyuText
            {
                Text = "*",
                FontSize = 16,
                Weight = FontWeight.SemiBold,
                Colour = Catppuccin.Current.Red,
                Alpha = required ? 1 : 0
            },
        };
    }
}
