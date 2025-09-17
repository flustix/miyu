// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages;
using Miyu.UI.Graphics;
using Miyu.UI.Screens.Main.Pages.Channel;
using Miyu.Utils.Extensions;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Components.Messages;

public partial class SmallChatMessage : ChatMessageBase
{
    public override DiscordMessage Message { get; }

    private MiyuText timestamp = null!;

    public SmallChatMessage(DiscordMessage message)
    {
        Message = message;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        InternalChildren = CreateBackground().Concat(new[]
        {
            new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Padding = new MarginPadding(2)
                {
                    Left = 72,
                    Right = 24
                },
                Children = new Drawable[]
                {
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        ChildrenEnumerable = createContent()
                    },
                    new Container
                    {
                        Origin = Anchor.TopRight,
                        AutoSizeAxes = Axes.X,
                        Height = 22,
                        Position = new Vector2(-16, 0),
                        Child = timestamp = new MiyuText
                        {
                            Text = $"{Message.GetTimeOffset():hh:mm}",
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Colour = Catppuccin.Current.Overlay2,
                            FontSize = 12,
                            Alpha = 0
                        }
                    }
                }
            }
        }).ToArray();
    }

    private IEnumerable<Drawable> createContent()
    {
        if (!string.IsNullOrEmpty(Message.Content))
            yield return new ChatMessageContent(Message.Content);

        if (Message.Attachments.Any())
            yield return new MessageAttachments(Message.Attachments);

        if (Message.Stickers?.Any() ?? false)
            yield return new MessageStickers(Message.Stickers);
    }

    protected override bool OnHover(HoverEvent e)
    {
        timestamp.FadeIn(100);
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        timestamp.FadeOut(100);
        base.OnHoverLost(e);
    }
}
