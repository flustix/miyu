// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models;
using Miyu.Models.Channels;
using Miyu.Models.Guilds;
using Miyu.UI.Components.Channels.Items;
using Miyu.UI.Components.Pages;
using Miyu.UI.Graphics;
using Miyu.UI.Input;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;

namespace Miyu.UI.Components.Channels;

public partial class ChannelList : CompositeDrawable, IKeyBindingHandler<MiyuBind>
{
    [Resolved]
    private MiyuClient client { get; set; } = null!;

    [Resolved]
    private PageController pages { get; set; } = null!;

    private DiscordGuild guild { get; }
    private List<IChannelListItem> items { get; } = new();

    public ChannelList(DiscordGuild guild)
    {
        this.guild = guild;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Masking = true;

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Mantle
            },
            new MiyuScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                ScrollbarVisible = false,
                Child = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding(8) { Top = 48 + 8 },
                    Direction = FillDirection.Vertical,
                    ChildrenEnumerable = createChannels().Select(x =>
                    {
                        items.Add(x);
                        return (Drawable)x;
                    })
                }
            },
            new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 48,
                Masking = true,
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Colour = Colour4.Black.Opacity(.25f),
                    Radius = 4
                },
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Catppuccin.Current.Mantle
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding(16),
                        Child = new MiyuText
                        {
                            Text = guild.Name,
                            RelativeSizeAxes = Axes.X,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Weight = FontWeight.SemiBold,
                            Truncate = true
                        }
                    }
                }
            }
        };
    }

    private IEnumerable<IChannelListItem> createChannels()
    {
        var channels = client.Channels.Items.Where(x => x.GuildID == guild.ID).ToList();
        var categories = channels.GroupBy(x => x.ParentID).OrderBy(x =>
        {
            if (x.Key is null) return -1;

            var cat = channels.FirstOrDefault(c => c.ID == x.Key);
            if (cat is null) return -1;

            return cat.Position;
        });

        var self = guild.MemberCache.Find(client.Self.ID);
        if (self is null) yield break;

        foreach (var category in categories)
        {
            var drawables = new List<Drawable>();

            if (category.Key is not null)
            {
                var cat = channels.FirstOrDefault(c => c.ID == category.Key);

                if (cat != null)
                {
                    var perms = cat.PermissionsFor(self);

                    if (!perms.ContainsPermission(DiscordPermissions.ViewChannel))
                        continue;

                    yield return new CategoryItem(cat, drawables);
                }
            }

            var sorted = category.Where(x => x.Type != DiscordChannelType.Category).ToList();
            sorted.Sort((a, b) =>
            {
                if (a.Type == DiscordChannelType.Voice && b.Type != DiscordChannelType.Voice)
                    return 1;
                if (b.Type == DiscordChannelType.Voice && a.Type != DiscordChannelType.Voice)
                    return -1;

                var result = 0;

                if (a.Position is not null && b.Position is not null)
                    result = a.Position.Value.CompareTo(b.Position.Value);

                return result != 0 ? result : a.ID.CompareTo(b.ID);
            });

            foreach (var channel in sorted)
            {
                var everyone = channel.PermissionsForEveryone();
                var perms = channel.PermissionsFor(self);

                if (!perms.ContainsPermission(DiscordPermissions.ViewChannel))
                    continue;

                var item = new ChannelItem(channel, !everyone.ContainsPermission(DiscordPermissions.ViewChannel));
                drawables.Add(item);
                yield return item;
            }
        }
    }

    private void cycleItems(int by = 0)
    {
        if (items.Count == 0) return;

        var page = pages.Current;
        if (page is null) return;

        var idx = items.FindIndex(x => x.MatchesPage(page));

        idx += by;
        idx = limitRange(idx);
        var item = items[idx];

        var count = 0;
        const int max_loops = 256;

        while (!item.Navigable)
        {
            idx += by;
            idx = limitRange(idx);
            item = items[idx];

            count++;
            if (count > max_loops) return;
        }

        item.Select();
        return;

        int limitRange(int val)
        {
            if (val >= items.Count) return 0;
            if (val < 0) return items.Count - 1;

            return val;
        }
    }

    public bool OnPressed(KeyBindingPressEvent<MiyuBind> e)
    {
        switch (e.Action)
        {
            case MiyuBind.SwitchChannelUp:
                cycleItems(-1);
                return true;

            case MiyuBind.SwitchChannelDown:
                cycleItems(1);
                return true;
        }

        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<MiyuBind> e)
    {
    }
}
