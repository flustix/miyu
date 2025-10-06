// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels.Messages.Attachment;
using Miyu.UI.Components.Overlays;
using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace Miyu.UI.Components.Messages;

public partial class MessageAttachments : FillFlowContainer
{
    private List<DiscordAttachment> attachments { get; }

    public MessageAttachments(List<DiscordAttachment> attachments)
    {
        this.attachments = attachments;
    }

    [BackgroundDependencyLoader]
    private void load(OverlayManager overlays)
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Padding = new MarginPadding { Vertical = 2 };
        Spacing = new Vector2(4);
        Direction = FillDirection.Vertical;

        foreach (var attachment in attachments)
        {
            if (string.IsNullOrWhiteSpace(attachment.ContentType) || !attachment.ContentType.StartsWith("image/"))
            {
                Add(new DefaultAttachment(attachment));
                continue;
            }

            float w = attachment.Width ?? 64;
            float h = attachment.Height ?? 64;

            const float max_w = 550;
            const float max_h = 350;

            if (w > max_w)
            {
                var factor = max_w / w;
                w = max_w;
                h *= factor;
            }

            if (h > max_h)
            {
                var factor = max_h / h;
                w *= factor;
                h = max_h;
            }

            AddInternal(new ImageAttachment(w, h, attachment.Url)
            {
                Action = () => overlays.Push(new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(64),
                    Child = new DelayedImage(attachment.Url)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        OnLoadComplete = d => d.FillMode = FillMode.Fit
                    }
                })
            });
        }
    }

    private partial class ImageAttachment : MiyuClickable
    {
        private readonly float width;
        private readonly float height;
        private readonly DelayedImage image;

        public ImageAttachment(float w, float h, string url)
        {
            width = w;
            height = h;

            Size = new Vector2(w, h);
            CornerRadius = 8;
            Masking = true;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Catppuccin.Current.Base
                },
                image = new DelayedImage(url)
                {
                    RelativeSizeAxes = Axes.Both,
                    AnimateAppear = s => s.FadeInFromZero(400)
                }
            };
        }

        protected override void Update()
        {
            base.Update();

            if (Parent is null) return;

            var available = Parent.ChildSize.X;

            if (available < width)
            {
                var factor = available / width;
                image.Width = Width = available;
                image.Height = Height = height * factor;
            }
            else
            {
                image.Width = Width = width;
                image.Height = Height = height;
            }
        }
    }

    private partial class DefaultAttachment : CompositeDrawable
    {
        private DiscordAttachment attachment { get; }

        public DefaultAttachment(DiscordAttachment attachment)
        {
            this.attachment = attachment;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Size = new Vector2(430, 74);
            CornerRadius = 2;
            Masking = true;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Catppuccin.Current.Mantle
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Padding = new MarginPadding(16),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Children = new Drawable[]
                    {
                        new MiyuText
                        {
                            Text = attachment.Name,
                            Colour = Catppuccin.Current.Blue
                        },
                        new MiyuText
                        {
                            Text = $"{toOptimalSizeString(attachment.SizeBytes)}",
                            Colour = Catppuccin.Current.Subtext0,
                            FontSize = 12
                        }
                    }
                }
            };
        }

        private static string toOptimalSizeString(long sizeB)
        {
            string[] suffixes = { "bytes", "KB", "MB", "GB", "TB" };

            var maxB = 1024L;
            double size = sizeB;

            for (var i = 0; i < suffixes.Length - 1; i++)
            {
                if (sizeB < maxB)
                    return $"{size:0.00} {suffixes[i]}";

                maxB *= 1024;
                size /= 1024;
            }

            return $"{size:0.00} {suffixes.Last()}";
        }
    }
}
