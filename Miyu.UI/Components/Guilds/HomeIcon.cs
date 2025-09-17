// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Components.Pages;
using Miyu.UI.Graphics;
using Miyu.UI.Screens.Main.Pages.Home;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Components.Guilds;

public partial class HomeIcon : MiyuClickable
{
    [Resolved]
    private PageController pages { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        Size = new Vector2(48);
        CornerRadius = 24;
        Masking = true;
        Margin = new MarginPadding { Left = 12 };

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Surface0
            },
            new MiyuIcon(MiyuIcon.Type.Discord, 24)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            }
        };
    }

    protected override bool OnClick(ClickEvent e)
    {
        pages.SwitchPage(new HomePage());
        return true;
    }
}
