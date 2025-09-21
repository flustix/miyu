// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace Miyu.UI.Components.Form;

public partial class MiyuTextBox : TextBox
{
    protected override float LeftRightPadding => 10;
    protected override char MaskCharacter => '•';

    private ColourInfo borderColor
    {
        get => BorderColour;
        set => BorderColour = value;
    }

    public MiyuTextBox(TextInputType type = TextInputType.Text)
    {
        InputProperties = new TextInputProperties(type);

        RelativeSizeAxes = Axes.X;
        Height = 44;
        CornerRadius = 8;
        Masking = true;
        FontSize = 16 * MiyuText.SIZE_FACTOR;

        borderColor = Catppuccin.Current.Overlay0;
        BorderThickness = 2;

        AddInternal(new Box
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Catppuccin.Current.Crust,
            Depth = 1,
        });
    }

    protected override void OnFocus(FocusEvent e)
    {
        base.OnFocus(e);
        this.TransformTo(nameof(borderColor), Catppuccin.Current.Blue, 100);
    }

    protected override void OnFocusLost(FocusLostEvent e)
    {
        base.OnFocusLost(e);
        this.TransformTo(nameof(borderColor), Catppuccin.Current.Overlay0, 100);
    }

    protected override Drawable GetDrawableCharacter(char c) => new Container
    {
        AutoSizeAxes = Axes.Both,
        Child = new MiyuText
        {
            FontSize = 16,
            Weight = FontWeight.Normal,
            Text = c.ToString()
        }
    };

    protected override void NotifyInputError()
    {
    }

    protected override SpriteText CreatePlaceholder() => new MiyuText
    {
        FontSize = 16,
        Weight = FontWeight.Normal,
        Anchor = Anchor.CentreLeft,
        Origin = Anchor.CentreLeft,
        Colour = Catppuccin.Current.Subtext0
    };

    protected override Caret CreateCaret() => new MiyuCaret();

    private partial class MiyuCaret : Caret
    {
        public MiyuCaret()
        {
            Colour = Color4.Transparent;
            InternalChild = new Container
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                Height = 0.9f,
                Child = new Box { RelativeSizeAxes = Axes.Both },
            };
        }

        public override void DisplayAt(Vector2 position, float? selectionWidth)
        {
            if (selectionWidth != null)
            {
                this.MoveTo(new Vector2(position.X, position.Y))
                    .ResizeWidthTo(selectionWidth.Value)
                    .FadeColour(Catppuccin.Current.Blue)
                    .FadeTo(0.5f);
            }
            else
            {
                this.MoveTo(new Vector2(position.X, position.Y))
                    .FadeColour(Catppuccin.Current.Text)
                    .ResizeWidthTo(1)
                    .FadeTo(1f);
            }
        }
    }
}
