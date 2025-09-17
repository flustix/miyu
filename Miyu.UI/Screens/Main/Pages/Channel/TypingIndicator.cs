// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Attributes;
using Miyu.Events;
using Miyu.Events.Channels;
using Miyu.Events.Messages;
using Miyu.Models.Channels;
using Miyu.Models.Guilds;
using Miyu.UI.Graphics;
using Miyu.Utils.Extensions;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Miyu.UI.Screens.Main.Pages.Channel;

public partial class TypingIndicator : CompositeDrawable
{
    [Resolved]
    private MiyuClient client { get; set; } = null!;

    private readonly DiscordChannel channel;
    private readonly DiscordGuild? guild;
    private readonly Dictionary<ulong, double> startTimes = new();

    private bool changes;
    private MiyuTextFlow text = null!;

    public TypingIndicator(DiscordChannel channel, DiscordGuild? guild = null)
    {
        this.channel = channel;
        this.guild = guild;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChild = text = new MiyuTextFlow
        {
            X = 8,
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            FontSize = 12,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Weight = FontWeight.Medium,
            Alpha = 0
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        client.RegisterListeners(this);
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        client.UnregisterListeners(this);
    }

    [EventListener(EventType.TypingStart)]
    public void OnTypingStart(TypingStartEvent ev)
    {
        if (ev.ChannelID != channel.ID) return;
        if (ev.UserID == client.Self.ID) return;

        startTimes[ev.UserID] = Time.Current;
        changes = true;
    }

    [EventListener(EventType.MessageCreate)]
    public void OnMessageCreate(MessageCreateEvent ev)
    {
        if (ev.Channel.ID != channel.ID) return;

        startTimes.Remove(ev.Message.Author.ID);
        changes = true;
    }

    protected override void Update()
    {
        base.Update();

        var expired = startTimes.Where(x => x.Value + 10000 < Time.Current).ToList();
        expired.ForEach(x => startTimes.Remove(x.Key));

        if (!changes) changes = expired.Count >= 0;
        if (!changes) return;

        if (startTimes.Count == 0)
        {
            text.FadeOut(100);
            return;
        }

        text.Clear();
        text.FadeIn(100);

        var idx = 0;

        foreach (var (uid, _) in startTimes)
        {
            if (startTimes.Count > 1 && idx == startTimes.Count - 1)
                text.AddText(" and ");
            else if (idx != 0)
                text.AddText(", ");

            var member = guild?.MemberCache.Find(uid);
            var user = client.Users.Find(uid);
            var name = member?.Nickname ?? user?.DisplayName ?? user?.Username ?? "unknown";

            text.AddText<MiyuText>(name, s =>
            {
                var role = member?.GetTopRoleWithColor();

                if (role != null)
                {
                    var hex = role.Color.ToString("X6");
                    s.Colour = Colour4.FromHex(hex);
                }

                s.FontSize = 12;
                s.Weight = FontWeight.Bold;
            });

            idx++;
        }

        text.AddText(" ");
        text.AddText(startTimes.Count > 1 ? "are" : "is");
        text.AddText(" typing...");

        changes = false;
    }
}
