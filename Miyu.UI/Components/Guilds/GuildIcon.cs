// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Guilds;
using Miyu.UI.Components.Pages;
using Miyu.UI.Graphics;
using Miyu.UI.Screens.Main.Pages.Guild;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Components.Guilds;

public partial class GuildIcon : MiyuClickable
{
    [Resolved]
    private PageController pages { get; set; } = null!;

    public DiscordGuild Guild { get; }

    private bool selected => pages.Current is GuildPage gp && gp.Guild.Equals(Guild);

    private Container circular = null!;
    private Circle line = null!;

    public GuildIcon(DiscordGuild guild)
    {
        Guild = guild;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        Height = 48;

        Drawable icon;

        if (!string.IsNullOrEmpty(Guild.Icon))
        {
            icon = new DelayedImage($"https://cdn.discordapp.com/icons/{Guild.ID}/{Guild.Icon}.png")
            {
                RelativeSizeAxes = Axes.Both,
                AnimateAppear = s => s.FadeInFromZero(200)
            };
        }
        else
        {
            icon = new MiyuText
            {
                FontSize = 20,
                Text = string.Join("", Guild.Name.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x[0])),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
        }

        InternalChildren = new[]
        {
            line = new Circle
            {
                Size = new Vector2(8, 40),
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.Centre,
                Colour = Catppuccin.Current.Text
            },
            circular = new Container
            {
                Size = new Vector2(48),
                CornerRadius = 24,
                Masking = true,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Children = new[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Catppuccin.Current.Surface0
                    },
                    icon
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        pages.OnPageChange += pageChange;
    }

    protected override void Dispose(bool isDisposing)
    {
        pages.OnPageChange -= pageChange;
        base.Dispose(isDisposing);
    }

    private void pageChange(Page page)
    {
        updateSelected();
    }

    protected override bool OnClick(ClickEvent e)
    {
        SwitchTo();
        return true;
    }

    public void SwitchTo()
    {
        pages.SwitchPage(new GuildPage(Guild));
    }

    private void updateSelected()
    {
        var sel = selected;
        circular.TransformTo(nameof(circular.CornerRadius), sel ? 12f : 24f, 300, Easing.OutQuint);
        line.ResizeHeightTo(40).ScaleTo(sel ? 1f : 0f, 300, Easing.OutQuint);
    }
}
