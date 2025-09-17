// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages;
using Miyu.Models.Guilds;
using Miyu.UI.Graphics;
using Miyu.UI.Graphics.Menus.Items;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Platform;

namespace Miyu.UI.Screens.Main.Pages.Channel;

public abstract partial class ChatMessageBase : CompositeDrawable, IHasContextMenu
{
    public virtual MenuItem[] ContextMenuItems
    {
        get
        {
            var list = new List<MenuItem>();

            if (!string.IsNullOrWhiteSpace(Message.Content))
                list.Add(new MenuActionItem("Copy Text", MiyuIcon.Type.Copy, () => clipboard.SetText(Message.Content)));

            list.Add(new MenuActionItem("Copy Message ID", MiyuIcon.Type.ID, () => clipboard.SetText($"{Message.ID}")));

            return list.ToArray();
        }
    }

    public abstract DiscordMessage Message { get; }

    [Resolved]
    protected MiyuClient Client { get; private set; } = null!;

    [Resolved]
    protected DiscordGuild? Guild { get; private set; }

    [Resolved]
    private Clipboard clipboard { get; set; } = null!;

    private HoverLayer? hover;

    protected IEnumerable<Drawable> CreateBackground()
    {
        yield return hover = new HoverLayer
        {
            TargetAlpha = .5f,
            Colour = Catppuccin.Current.Mantle
        };

        if (Message.Mentioned ?? Message.Mentions.Any(x => x.ID == Client.Self.ID))
        {
            yield return new Container
            {
                Name = "mention background",
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Catppuccin.Current.Yellow,
                        Alpha = 0.1f
                    },
                    new Box
                    {
                        Width = 2,
                        RelativeSizeAxes = Axes.Y,
                        Colour = Catppuccin.Current.Yellow
                    }
                }
            };
        }
    }

    protected override bool OnHover(HoverEvent e)
    {
        hover?.Show();
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        hover?.Hide();
    }
}
