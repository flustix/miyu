// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.UI.Components.Pages;
using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Components.Channels.Items;

public partial class CategoryItem : MiyuClickable, IChannelListItem
{
    public bool Navigable => false;
    public DiscordChannel Channel { get; }

    private readonly List<Drawable> drawables;

    private bool visible = true;
    private MiyuText text = null!;
    private SpriteIcon icon = null!;

    public CategoryItem(DiscordChannel channel, List<Drawable> drawables)
    {
        Channel = channel;
        this.drawables = drawables;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        Height = 24;
        Padding = new MarginPadding { Horizontal = 8 };
        Margin = new MarginPadding { Top = 16 };

        InternalChild = new FillFlowContainer
        {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(4),
            Children = new Drawable[]
            {
                text = new MiyuText
                {
                    Text = $"{Channel?.Name}",
                    FontSize = 14,
                    Colour = Catppuccin.Current.Subtext0,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft
                },
                new Container
                {
                    Size = new Vector2(12),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Child = icon = new SpriteIcon
                    {
                        RelativeSizeAxes = Axes.Both,
                        Icon = FontAwesome.Solid.ChevronDown,
                        Colour = Catppuccin.Current.Subtext0,
                        Scale = new Vector2(0.8f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
                }
            }
        };
    }

    protected override bool OnHover(HoverEvent e)
    {
        text.FadeColour(Catppuccin.Current.Text);
        icon.FadeColour(Catppuccin.Current.Text);
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        text.FadeColour(Catppuccin.Current.Subtext0);
        icon.FadeColour(Catppuccin.Current.Subtext0);
    }

    protected override bool OnClick(ClickEvent e)
    {
        visible = !visible;

        foreach (var item in drawables)
            item.Alpha = visible ? 1 : 0;

        icon.RotateTo(visible ? 0 : -90, 200, Easing.OutQuint);
        return true;
    }

    public bool MatchesPage(Page page)
    {
        return false;
        // categories can't have pages
    }

    public void Select()
    {
    }
}
