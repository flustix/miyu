// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Requests.Channels;
using Miyu.Attributes;
using Miyu.Events;
using Miyu.Events.Messages;
using Miyu.Models.Channels;
using Miyu.Models.Channels.Messages;
using Miyu.Models.Guilds;
using Miyu.UI.Components.Messages;
using Miyu.UI.Components.Overlays;
using Miyu.UI.Components.Pages;
using Miyu.UI.Graphics;
using Miyu.UI.Screens.Main.Pages.Channel.Input;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace Miyu.UI.Screens.Main.Pages.Channel;

public partial class ChannelPage : Page
{
    [Resolved]
    private MiyuClient client { get; set; } = null!;

    [Resolved]
    private OverlayManager overlays { get; set; } = null!;

    public DiscordChannel Channel { get; }
    private DiscordGuild? guild { get; }

    private MiyuScrollContainer scroll = null!;
    private FillFlowContainer<ChatMessageBase> flow = null!;
    private MiyuText title = null!;
    private ChannelTextBox textBox = null!;

    private bool loading = true;
    private readonly List<DiscordMessage> queue = new();

    private double lastTyping;
    private int characterCount;

    public ChannelPage(DiscordChannel channel, DiscordGuild? guild = null)
    {
        Channel = channel;
        this.guild = guild;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Base
            },
            new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new Dimension[]
                {
                    new(GridSizeMode.Absolute, 48),
                    new(),
                    new(GridSizeMode.Absolute, 76)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(8),
                            Children = new Drawable[]
                            {
                                new Container
                                {
                                    Size = new Vector2(32),
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Child = new MiyuIcon(MiyuIcon.Type.Hash, 20)
                                    {
                                        Colour = Catppuccin.Current.Overlay2,
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre
                                    }
                                },
                                title = new MiyuText
                                {
                                    Text = Channel.Name ?? "",
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Weight = FontWeight.Medium
                                }
                            }
                        }
                    },
                    new Drawable[]
                    {
                        scroll = new MiyuScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            ScrollbarVisible = false,
                            Masking = true,
                            Child = flow = new FillFlowContainer<ChatMessageBase>
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Padding = new MarginPadding { Bottom = 16 }
                            }
                        }
                    },
                    new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding { Horizontal = 8, Bottom = 24 },
                            Children = new Drawable[]
                            {
                                textBox = new ChannelTextBox(Channel)
                                {
                                    PlaceholderText = $"Message #{Channel.Name}",
                                    RelativeSizeAxes = Axes.X,
                                    Height = 52
                                },
                                new MiyuClickable
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    X = -16,
                                    Action = () => textBox.ShowPopover(),
                                    Child = new MiyuText
                                    {
                                        Text = "emote"
                                    }
                                },
                                new TypingIndicator(Channel, guild)
                                {
                                    RelativeSizeAxes = Axes.X,
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.TopLeft,
                                    Height = 22
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        client.RegisterListeners(this);
        textBox.OnCommit += (box, _) =>
        {
            sendMessage(box.Text);
            characterCount = 0;
            box.Text = "";
        };

        Channel.GetMessages().ContinueWith(task => ScheduleAfterChildren(() =>
        {
            var messages = task.Result.ToList();
            queue.AddRange(messages);
            queue.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));

            loading = false;

            foreach (var message in queue)
                createMessage(message);
        }));
    }

    protected override void Update()
    {
        base.Update();

        if (characterCount != textBox.Text.Length && lastTyping + 10000 < Time.Current)
        {
            _ = client.API.Execute(new StartTypingRequest(Channel.ID));
            lastTyping = Time.Current;
        }

        characterCount = textBox.Text.Length;
        ensureTextBoxFocused();
    }

    private void ensureTextBoxFocused()
    {
        var input = GetContainingInputManager();
        var focus = GetContainingFocusManager();
        if (input is null || focus is null) return;

        if (overlays.AnyOpen)
        {
            if (input.FocusedDrawable != textBox)
                return;

            focus.ChangeFocus(null);
        }
        else
        {
            if (input.FocusedDrawable != null)
                return;

            focus.ChangeFocus(textBox);
        }
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        client.UnregisterListeners(this);
    }

    private void sendMessage(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return;

        var req = new CreateMessageRequest(Channel.ID, content);
        _ = client.API.Execute(req);
        lastTyping = 0;
    }

    [EventListener(EventType.MessageCreate)]
    public void OnMessage(MessageCreateEvent ev)
    {
        if (ev.Channel.ID != Channel.ID)
            return;

        createMessage(ev.Message);
    }

    private void createMessage(DiscordMessage message)
    {
        if (loading)
        {
            queue.Add(message);
            return;
        }

        var atEnd = scroll.IsScrolledToEnd(32);
        var last = flow.LastOrDefault();

        ChatMessageBase msg;

        switch (message.Type)
        {
            case DiscordMessageType.UserJoin:
                msg = new MemberJoinMessage(message);
                break;

            default:
                if (message.ReferencedMessage == null && last != null && message.Timestamp - last.Message.Timestamp <= TimeSpan.FromMinutes(5) && last.Message.Author.ID == message.Author.ID)
                    msg = new SmallChatMessage(message);
                else
                    msg = new ChatMessage(message);

                break;
        }

        msg.AlwaysPresent = true;
        flow.Add(msg);
        msg.FadeInFromZero(100);

        if (atEnd)
            ScheduleAfterChildren(() => scroll.ScrollToEnd());
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Repeat) return false;

        switch (e.Key)
        {
            case Key.Escape:
                scroll.ScrollToEnd();
                return true;
        }

        return false;
    }
}
