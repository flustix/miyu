// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace Miyu.UI.Graphics;

public partial class CustomEmojiDrawable : CompositeDrawable, IHasPopover
{
    public ulong ID { get; }
    public bool Animated { get; }
    public bool DisplayBig { get; set; } = false;

    public CustomEmojiDrawable(string id, bool animated)
        : this(ulong.Parse(id), animated)
    {
    }

    public CustomEmojiDrawable(ulong id, bool animated)
    {
        ID = id;
        Animated = animated;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Size = new Vector2(DisplayBig ? 48 : 22);

        InternalChild = new DelayedImage($"https://cdn.discordapp.com/emojis/{ID}.png?size=56")
        {
            RelativeSizeAxes = Axes.Both,
            OnLoadComplete = s =>
            {
                s.FillMode = FillMode.Fit;
                s.Anchor = Anchor.Centre;
                s.Origin = Anchor.Centre;
            }
        };
    }

    public Popover GetPopover()
    {
        return new BasicPopover
        {
            AllowableAnchors = new[] { Anchor.CentreRight }
        };
    }
}
