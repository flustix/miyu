// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Components.Pages;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace Miyu.UI.Screens.Main.Pages.Home;

public partial class HomePage : Page
{
    private DependencyContainer dependencies = null!;
    private PageController subpage = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        dependencies.CacheAs(this);

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Mantle
            },
            new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new Dimension[]
                {
                    new(GridSizeMode.Absolute, 288),
                    new()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new HomeSidebar(),
                        subpage = new PageController()
                    }
                }
            }
        };
    }

    public void SwitchPage(Page page)
    {
        subpage.SwitchPage(page);
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
    {
        return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}
