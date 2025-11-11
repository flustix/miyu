// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Components.Pages;
using Miyu.UI.Graphics;
using Miyu.UI.Input;
using Miyu.UI.Screens.Main.Pages.Guild;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Components.Guilds;

public partial class GuildList : CompositeDrawable, IKeyBindingHandler<MiyuBind>
{
    [Resolved]
    private MiyuClient client { get; set; } = null!;

    [Resolved]
    private PageController pages { get; set; } = null!;

    private readonly List<GuildIcon> guilds = new();
    private FillFlowContainer flow = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Crust
            },
            new MiyuScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                ScrollbarVisible = false,
                Child = flow = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Spacing = new Vector2(8),
                    Padding = new MarginPadding { Vertical = 12 },
                    Children = new Drawable[]
                    {
                        new HomeIcon(),
                        new Box
                        {
                            Size = new Vector2(32, 1),
                            Margin = new MarginPadding { Horizontal = 20 },
                            Colour = Catppuccin.Current.Surface0
                        }
                    }
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        var icons = client.Guilds.Items.Select(x =>
        {
            var icon = new GuildIcon(x);
            guilds.Add(icon);
            return icon;
        });

        flow.AddRange(icons);
    }

    private void cycleGuilds(int by = 0)
    {
        if (guilds.Count == 0) return;

        var page = pages.Current;

        if (page is not GuildPage gp)
        {
            guilds.First().SwitchTo();
            return;
        }

        var idx = guilds.FindIndex(x => x.Guild.ID == gp.Guild.ID);
        idx += by;

        if (idx >= guilds.Count)
            idx = 0;
        else if (idx < 0)
            idx = guilds.Count - 1;

        guilds[idx].SwitchTo();
    }

    public bool OnPressed(KeyBindingPressEvent<MiyuBind> e)
    {
        switch (e.Action)
        {
            case MiyuBind.SwitchGuildUp:
                cycleGuilds(-1);
                return true;

            case MiyuBind.SwitchGuildDown:
                cycleGuilds(1);
                return true;
        }

        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<MiyuBind> e)
    {
    }
}
