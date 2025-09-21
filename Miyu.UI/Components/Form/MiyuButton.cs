// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

namespace Miyu.UI.Components.Form;

public partial class MiyuButton : Button
{
    public MiyuButton(LocalisableString label, Action? action = null)
    {
        Height = 44;
        CornerRadius = 8;
        Masking = true;
        Action = action;

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Blue
            },
            new MiyuText
            {
                Text = label,
                FontSize = 16,
                Weight = FontWeight.Medium,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Catppuccin.Current.Mantle,
            }
        };
    }
}
