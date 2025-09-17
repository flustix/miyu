// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.Models.Guilds.Expressions;
using Miyu.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace Miyu.UI.Screens.Main.Pages.Channel.Input;

public partial class EmojiPicker : Popover
{
    public override Vector2 Offset => new(-8);

    public Action<DiscordEmote>? OnEmojiSelected { get; init; }

    private readonly DiscordChannel channel;

    public EmojiPicker(DiscordChannel channel)
    {
        this.channel = channel;
    }

    [BackgroundDependencyLoader]
    private void load(MiyuClient client)
    {
        Width = 800;
        Background.Colour = Catppuccin.Current.Surface0;
        CornerRadius = 8;

        Content.AutoSizeAxes = Axes.Y;
        Content.RelativeSizeAxes = Axes.X;

        var bound = (InternalChild as Container)!;
        bound.Masking = true;
        bound.CornerRadius = 8;

        var flow = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Full,
            Spacing = new Vector2(4),
            Padding = new MarginPadding(8)
        };

        var guild = channel.GuildID is null ? null : client.Guilds.Find(channel.GuildID.Value);

        if (guild is not null)
        {
            flow.AddRange(guild.Emojis.OrderBy(x => x.Name?.ToLowerInvariant()).Select(x => new MiyuClickable
            {
                AutoSizeAxes = Axes.Both,
                Action = () => OnEmojiSelected?.Invoke(x),
                Child = new CustomEmojiDrawable(x.ID, x.Animated ?? false) { DisplayBig = true }
            }));
        }
        else
            flow.Add(new MiyuText { Text = "not in guild" });

        Child = flow;
    }

    protected override Drawable CreateArrow()
    {
        return Empty();
    }

    protected override void PopIn() // pop in 2
    {
        this.FadeInFromZero(150);
        // Content.MoveToX(100).MoveToX(0, 300, Easing.OutQuint);
    }

    protected override void PopOut()
    {
        this.FadeOut(150);
        // Content.MoveToX(100, 300, Easing.OutQuint);
    }
}
