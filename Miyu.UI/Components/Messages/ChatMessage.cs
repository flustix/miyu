// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages;
using Miyu.Models.Guilds.Members;
using Miyu.UI.Graphics;
using Miyu.UI.Screens.Main.Pages.Channel;
using Miyu.Utils.Extensions;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Miyu.UI.Components.Messages;

public partial class ChatMessage : ChatMessageBase
{
    public override DiscordMessage Message { get; }
    private DiscordMember? member { get; }

    public ChatMessage(DiscordMessage message, DiscordMember? member = null)
    {
        Message = message;
        this.member = member;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Margin = new MarginPadding { Top = 16 };

        var avatarOffset = 2;

        if (Message.ReferencedMessage != null)
            avatarOffset += 20;

        InternalChildren = CreateBackground().Concat(new Drawable[]
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
                    new CircularContainer
                    {
                        Size = new Vector2(40),
                        Origin = Anchor.TopRight,
                        Position = new Vector2(-16, avatarOffset),
                        Masking = true,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Catppuccin.Current.Base
                            },
                            new DelayedImage($"https://cdn.discordapp.com/avatars/{Message.Author.ID}/{Message.Author.AvatarHash}.png?size=64")
                            {
                                RelativeSizeAxes = Axes.Both,
                                OnLoadComplete = s => s.FadeInFromZero(400)
                            }
                        }
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        ChildrenEnumerable = createContent()
                    }
                }
            }
        }).ToArray();
    }

    private IEnumerable<Drawable> createContent()
    {
        var role = member?.GetTopRoleWithColor();
        var color = Catppuccin.Current.Text;

        if (role?.Color != null)
        {
            var hex = role.Color.ToString("X6");
            color = Colour4.FromHex(hex);
        }

        if (Message.ReferencedMessage != null)
            yield return createReference(Message.ReferencedMessage);

        yield return new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            Height = 22,
            Direction = FillDirection.Horizontal,
            Children = new Drawable[]
            {
                new MiyuText
                {
                    Text = member?.Nickname ?? Message.Author.DisplayName ?? Message.Author.Username,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Weight = FontWeight.Medium,
                    Colour = color
                },
                new MiyuText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Weight = FontWeight.Medium,
                    Colour = Catppuccin.Current.Overlay2,
                    Margin = new MarginPadding { Left = 4, Top = 3 },
                    Text = $"{Message.GetTimeOffset():HH:mm}",
                    FontSize = 12
                }
            }
        };

        if (!string.IsNullOrEmpty(Message.Content))
            yield return new ChatMessageContent(Client, Message.Content);

        if (Message.Attachments.Any())
            yield return new MessageAttachments(Message.Attachments);

        if (Message.Stickers?.Any() ?? false)
            yield return new MessageStickers(Message.Stickers);
    }

    private Drawable createReference(DiscordMessage message)
    {
        var flow = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.Both,
            Margin = new MarginPadding { Bottom = 4 },
            Direction = FillDirection.Horizontal,
            Children = new Drawable[]
            {
                new Container
                {
                    Size = new Vector2(16),
                    CornerRadius = 8,
                    Masking = true,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Margin = new MarginPadding { Right = 4 },
                    Child = new DelayedImage($"https://cdn.discordapp.com/avatars/{message.Author.ID}/{message.Author.AvatarHash}.png?size=64")
                    {
                        RelativeSizeAxes = Axes.Both,
                        OnLoadComplete = s => s.FadeInFromZero(400)
                    }
                },
                new MiyuText
                {
                    Text = message.Author.DisplayName ?? message.Author.Username,
                    Weight = FontWeight.Medium,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Margin = new MarginPadding { Right = 4 },
                    FontSize = 14
                },
                new MiyuText
                {
                    Text = message.Content,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Colour = Catppuccin.Current.Subtext0,
                    FontSize = 14
                }
            }
        };

        return new Container
        {
            RelativeSizeAxes = Axes.X,
            Height = 20,
            Children = new Drawable[]
            {
                new SpriteIcon
                {
                    Size = new Vector2(12),
                    Origin = Anchor.TopRight,
                    Position = new Vector2(-8, 4),
                    Icon = FontAwesome.Solid.AngleDoubleRight,
                    Colour = Catppuccin.Current.Overlay2
                },
                flow
            }
        };
    }
}
