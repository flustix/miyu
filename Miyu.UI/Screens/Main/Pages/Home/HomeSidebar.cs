// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Miyu.UI.Screens.Main.Pages.Home;

public partial class HomeSidebar : CompositeDrawable
{
    [BackgroundDependencyLoader]
    private void load(MiyuApp app)
    {
        RelativeSizeAxes = Axes.Both;
        InternalChild = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            ChildrenEnumerable = app.PrivateChannels.OrderByDescending(x => x.LastMessageID ?? 0).Select<DiscordChannel, Drawable>(x =>
            {
                if (x.Type != DiscordChannelType.DM)
                    return new MiyuText { Text = "DM Groups are unsupported right now." };

                return new PrivateChannelEntry(x);
            })
        };
    }
}
