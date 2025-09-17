// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.UI.Components.Pages;
using Miyu.UI.Graphics;
using Miyu.UI.Screens.Main.Pages.Channel;
using Miyu.UI.Screens.Main.Pages.Guild;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Components.Channels.Items;

public partial class ChannelItem : MiyuClickable, IChannelListItem
{
    [Resolved]
    private MiyuClient client { get; set; } = null!;

    [Resolved]
    private PageController pages { get; set; } = null!;

    [Resolved(CanBeNull = true)]
    private GuildPage? guild { get; set; }

    public bool Navigable => Channel.Type == DiscordChannelType.Text;
    public DiscordChannel Channel { get; }
    private bool selected => pages.Current != null && MatchesPage(pages.Current);

    private bool locked { get; }
    private HoverLayer hover = null!;

    private MiyuIcon icon = null!;
    private MiyuText text = null!;

    public ChannelItem(DiscordChannel channel, bool locked = false)
    {
        Channel = channel;
        this.locked = locked;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        Height = 32;
        CornerRadius = 8;
        Masking = true;

        InternalChildren = new Drawable[]
        {
            hover = new HoverLayer(),
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Direction = FillDirection.Horizontal,
                Padding = new MarginPadding { Horizontal = 8, Vertical = 4 },
                Spacing = new Vector2(8),
                Children = new Drawable[]
                {
                    icon = new MiyuIcon(Channel.Type switch
                    {
                        DiscordChannelType.Voice when locked => MiyuIcon.Type.VoiceLock,
                        DiscordChannelType.Voice => MiyuIcon.Type.Voice,
                        DiscordChannelType.Forum or DiscordChannelType.Media => MiyuIcon.Type.Forum,
                        _ when locked => MiyuIcon.Type.HashLock,
                        _ => MiyuIcon.Type.Hash
                    }, 20)
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Colour = Catppuccin.Current.Subtext0
                    },
                    text = new MiyuText
                    {
                        Text = Channel.Name ?? "",
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Weight = FontWeight.Medium,
                        Colour = Catppuccin.Current.Subtext0
                    }
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        pages.OnPageChange += pageChanged;
        base.LoadComplete();
    }

    protected override void Dispose(bool isDisposing)
    {
        pages.OnPageChange -= pageChanged;
        base.Dispose(isDisposing);
    }

    protected override bool OnClick(ClickEvent e)
    {
        Select();
        return true;
    }

    protected override bool OnHover(HoverEvent e)
    {
        updateHighlight(true);
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        updateHighlight(false);
    }

    private void pageChanged(Page page)
    {
        updateHighlight(IsHovered);
    }

    private void updateHighlight(bool hovered)
    {
        var sel = selected;
        var show = hovered || sel;

        if (show) hover.Show();
        else hover.Hide();

        if (sel) icon.Colour = text.Colour = Catppuccin.Current.Text;
        else icon.Colour = text.Colour = Catppuccin.Current.Subtext0;
    }

    public bool MatchesPage(Page page)
    {
        return page is ChannelPage c && c.Channel == Channel;
    }

    public void Select()
    {
        if (guild is null) return;

        switch (Channel.Type)
        {
            case DiscordChannelType.Text:
                pages.SwitchPage(new ChannelPage(Channel, guild.Guild));
                break;

            case DiscordChannelType.Voice:
                client.VoiceManager.Connect(Channel, guild.Guild);
                break;
        }
    }
}
