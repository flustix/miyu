// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.UI.Graphics;
using Miyu.UI.Screens.Main.Pages.Channel;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Screens.Main.Pages.Home;

public partial class PrivateChannelEntry : MiyuClickable
{
    [Resolved]
    private HomePage home { get; set; } = null!;

    private readonly DiscordChannel channel;

    private HoverLayer hover = null!;
    private MiyuText text = null!;

    public PrivateChannelEntry(DiscordChannel channel)
    {
        this.channel = channel;
    }

    [BackgroundDependencyLoader]
    private void load(MiyuClient client)
    {
        RelativeSizeAxes = Axes.X;
        Height = 44;
        Padding = new MarginPadding { Horizontal = 8 };

        var rcpID = channel.RecipientsIDs!.First();
        var rcp = client.Users.Find(rcpID);

        InternalChildren = new Drawable[]
        {
            new Container
            {
                RelativeSizeAxes = Axes.Both,
                CornerRadius = 8,
                Masking = true,
                Children = new Drawable[]
                {
                    hover = new HoverLayer(),
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Padding = new MarginPadding(8),
                        Spacing = new Vector2(12),
                        Children = new Drawable[]
                        {
                            new CircularContainer
                            {
                                Size = new Vector2(32),
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Masking = true,
                                Child = string.IsNullOrWhiteSpace(rcp?.AvatarHash)
                                    ? new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Catppuccin.Current.Surface0
                                    }
                                    : new DelayedImage($"https://cdn.discordapp.com/avatars/{rcp.ID}/{rcp.AvatarHash}.png?size=64")
                                    {
                                        RelativeSizeAxes = Axes.Both
                                    }
                            },
                            text = new MiyuText
                            {
                                Text = rcp?.DisplayName ?? rcp?.Username ?? $"unknown user ({rcpID})",
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Colour = Catppuccin.Current.Subtext0
                            }
                        }
                    }
                }
            }
        };
    }

    protected override bool OnHover(HoverEvent e)
    {
        hover.Show();
        text.FadeColour(Catppuccin.Current.Text);
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        hover.Hide();
        text.FadeColour(Catppuccin.Current.Subtext0);
    }

    protected override bool OnClick(ClickEvent e)
    {
        home.SwitchPage(new ChannelPage(channel));
        return true;
    }
}
