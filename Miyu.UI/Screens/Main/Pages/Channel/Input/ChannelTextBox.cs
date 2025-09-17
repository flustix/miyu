// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.UI.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osuTK.Graphics;
using osuTK.Input;

namespace Miyu.UI.Screens.Main.Pages.Channel.Input;

public partial class ChannelTextBox : BasicTextBox, IHasPopover, IHasCursorType
{
    public CursorType Cursor => CursorType.TextSelection;

    protected override float LeftRightPadding => 16;
    protected override Color4 SelectionColour => Catppuccin.Current.Blue;

    private DiscordChannel channel { get; }

    public ChannelTextBox(DiscordChannel channel)
    {
        this.channel = channel;

        BackgroundUnfocused = Catppuccin.Current.Mantle;
        BackgroundFocused = Catppuccin.Current.Mantle;
        BackgroundCommit = Catppuccin.Current.Mantle;
        ReleaseFocusOnCommit = false;

        CornerRadius = 8;
        Masking = true;

        FontSize = 16 * 1.4f;
        Placeholder.Colour = Catppuccin.Current.Text.TopLeft.Linear.Opacity(0.5f);
        Placeholder.Font = new FontUsage("ggsans", FontSize);

        // BorderColour = Catppuccin.Current.Surface0;
        // BorderThickness = 1;
    }

    protected override Drawable GetDrawableCharacter(char c)
    {
        var container = new FallingDownContainer
        {
            AutoSizeAxes = Axes.Both,
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Child = new MiyuText
            {
                Text = c.ToString(),
                FontSize = 16
            }
        };

        return container;
    }

    public Popover GetPopover()
    {
        return new EmojiPicker(channel)
        {
            AutoSizeAxes = Axes.Both,
            OnEmojiSelected = e => Text += $"<{(e.Animated ?? false ? "a" : "")}:{e.Name}:{e.ID}> "
        };
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        return e.Key != Key.Escape && base.OnKeyDown(e);
    }
}
