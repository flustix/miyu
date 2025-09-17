// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;

namespace Miyu.UI.Graphics;

public partial class MiyuTextFlow : TextFlowContainer
{
    public float FontSize { get; set; } = 16;
    public FontWeight Weight { get; set; } = FontWeight.Normal;

    public MiyuTextFlow()
    {
        ParagraphSpacing = 0;
    }

    protected override MiyuText CreateSpriteText()
    {
        return new MiyuText
        {
            FontSize = FontSize,
            Weight = Weight
        };
    }
}
