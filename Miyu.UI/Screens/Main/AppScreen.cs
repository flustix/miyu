// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Attributes;
using Miyu.Events;
using Miyu.UI.Components.Guilds;
using Miyu.UI.Components.Pages;
using Miyu.UI.Config;
using Miyu.UI.Input;
using Miyu.UI.Screens.Main.Pages.Home;
using Miyu.UI.Screens.Main.User;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Screens;

namespace Miyu.UI.Screens.Main;

public partial class AppScreen : Screen, IKeyBindingHandler<MiyuBind>
{
    [Resolved]
    private MiyuClient client { get; set; } = null!;

    [Resolved]
    private MiyuApp app { get; set; } = null!;

    private DependencyContainer dependencies = null!;

    private const float list_width = 72;
    private Container? guildListWrap;
    private UserArea? user;

    private readonly BindableBool leftSideVisible = new(true);
    private readonly BindableBool rightSideVisible = new(true);

    public Bindable<bool> LeftSideVisible => leftSideVisible.GetBoundCopy();
    public Bindable<bool> RightSideVisible => rightSideVisible.GetBoundCopy();

    public float UserAreaHeight => user?.DrawHeight ?? 0;

    [BackgroundDependencyLoader]
    private void load(ClientConfig config)
    {
        client.Config.Token = config.Get<string>(ClientConfigEntry.Token);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        leftSideVisible.BindValueChanged(v =>
        {
            if (guildListWrap is null || user is null) return;

            guildListWrap.ResizeWidthTo(v.NewValue ? list_width : 0, 300, Easing.OutQuint);
            user.MoveToX(v.NewValue ? 0 : -user.DrawWidth, 300, Easing.OutQuint);
        });

        client.RegisterListeners(this);
        client.ConnectAsync().ContinueWith(_ => Schedule(() => app.Loading.StateText = "Waiting for data..."));
    }

    private void createContent()
    {
        dependencies.CacheAs(this);

        var pages = new PageController();
        pages.SwitchPage(new HomePage());
        dependencies.Cache(pages);

        user = new UserArea();
        dependencies.CacheAs(user);

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Crust
            },
            new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new Dimension[]
                {
                    new(GridSizeMode.AutoSize),
                    new()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        guildListWrap = new Container
                        {
                            Width = list_width,
                            Child = new GuildList
                            {
                                Width = list_width,
                                RelativeSizeAxes = Axes.Y,
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight
                            }
                        },
                        pages
                    }
                }
            },
            user
        };
    }

    protected override void Update()
    {
        base.Update();

        if (guildListWrap is null || user is null)
            return;

        guildListWrap.Height = DrawHeight - UserAreaHeight;
    }

    [EventListener(EventType.Ready)]
    public void OnReady(ReadyEvent ready)
    {
        app.Loading.StateText = "Creating content...";

        app.PrivateChannels = ready.Event.PrivateChannels;
        createContent();

        app.Loading.Hide();
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    public bool OnPressed(KeyBindingPressEvent<MiyuBind> e)
    {
        switch (e.Action)
        {
            case MiyuBind.ToggleLeftSide:
                leftSideVisible.Toggle();
                return true;
        }

        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<MiyuBind> e)
    {
    }
}
