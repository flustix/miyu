// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages.Attachment;
using Miyu.Models.Guilds.Expressions;
using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace Miyu.UI.Components.Messages;

public partial class MessageStickers : FillFlowContainer
{
    private List<DiscordStickerItem> stickers { get; }

    public MessageStickers(List<DiscordStickerItem> stickers)
    {
        this.stickers = stickers;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Padding = new MarginPadding { Vertical = 2 };
        Spacing = new Vector2(4);
        Direction = FillDirection.Vertical;

        foreach (var sticker in stickers)
        {
            if (sticker.Format != DiscordStickerFormat.Png)
            {
                Add(new Container
                {
                    Size = new Vector2(160),
                    CornerRadius = 4,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Catppuccin.Current.Mantle
                        },
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Direction = FillDirection.Vertical,
                            Children = new Drawable[]
                            {
                                new MiyuText
                                {
                                    FontSize = 12,
                                    Text = "Unable to display sticker",
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre
                                },
                                new MiyuText
                                {
                                    FontSize = 12,
                                    Text = $"Unknown format: {sticker.Format}",
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre
                                }
                            }
                        }
                    }
                });
                continue;
            }

            Add(new Container
            {
                Size = new Vector2(160),
                Child = new DelayedImage($"https://media.discordapp.net/stickers/{sticker.ID}.png?size=240&quality=lossless")
                {
                    RelativeSizeAxes = Axes.Both,
                    OnLoadComplete = s =>
                    {
                        s.FillMode = FillMode.Fit;
                        s.Anchor = Anchor.Centre;
                        s.Origin = Anchor.Centre;
                    }
                }
            });
        }
    }
}
