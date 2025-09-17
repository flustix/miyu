// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Guilds;
using Miyu.UI.Components.Channels;
using Miyu.UI.Components.Pages;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace Miyu.UI.Screens.Main.Pages.Guild;

public partial class GuildPage : Page
{
    public DiscordGuild Guild { get; }

    private DependencyContainer dependencies = null!;
    private PageController subpage = null!;

    private const float list_width = 288;
    private Container channelListWrap = null!;

    private Bindable<bool> leftSideVisible = null!;

    public GuildPage(DiscordGuild guild)
    {
        Guild = guild;
    }

    [BackgroundDependencyLoader]
    private void load(AppScreen app)
    {
        leftSideVisible = app.LeftSideVisible;
        Guild.SubscribeToEvents().Wait();

        dependencies.CacheAs(this);
        dependencies.CacheAs(subpage = new PageController());

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Base
            },
            new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new Dimension[]
                {
                    new(GridSizeMode.AutoSize),
                    new()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        channelListWrap = new Container
                        {
                            Width = list_width,
                            RelativeSizeAxes = Axes.Y,
                            Masking = true,
                            Child = new ChannelList(Guild)
                            {
                                Width = list_width,
                                RelativeSizeAxes = Axes.Y,
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight
                            }
                        },
                        subpage
                    }
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        leftSideVisible.BindValueChanged(v => channelListWrap.ResizeWidthTo(v.NewValue ? list_width : 0, 300, Easing.OutQuint), true);
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
    {
        return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}
