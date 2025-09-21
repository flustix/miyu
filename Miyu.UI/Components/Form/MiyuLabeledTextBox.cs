// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Localisation;
using osuTK;

namespace Miyu.UI.Components.Form;

public partial class MiyuLabeledTextBox : FillFlowContainer
{
    public string Text => box.Text;

    private readonly MiyuTextBox box;

    public MiyuLabeledTextBox(LocalisableString label, TextInputType type = TextInputType.Text, bool required = false)
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Direction = FillDirection.Vertical;
        Spacing = new Vector2(8);

        InternalChildren = new Drawable[]
        {
            new MiyuFormLabel(label, required),
            box = new MiyuTextBox(type)
        };
    }
}
