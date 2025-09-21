// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Miyu.UI.Graphics;

public partial class MiyuText : SpriteText
{
    public const float SIZE_FACTOR = 1.4f;

    public float FontSize
    {
        set
        {
            Font = Font.With("ggsans", value * SIZE_FACTOR);
            GlyphOffset = new Vector2(0, -value * 0.1f);
        }
    }

    public FontWeight Weight
    {
        set
        {
            var weight = value == FontWeight.Normal ? "" : value.ToString().ToLower();
            Font = Font.With("ggsans", weight: weight);
        }
    }

    public MiyuText()
    {
        Colour = Catppuccin.Current.Text;
        FontSize = 16;
    }
}

public enum FontWeight
{
    /// <summary>
    ///     400
    /// </summary>
    Normal,

    /// <summary>
    ///     500
    /// </summary>
    Medium,

    /// <summary>
    ///     600
    /// </summary>
    SemiBold,

    /// <summary>
    ///     700
    /// </summary>
    Bold
}
