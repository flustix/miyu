// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.Models.Channels;
using Miyu.UI.Components.Overlays;
using Miyu.UI.Config;
using Miyu.UI.Graphics;
using Miyu.UI.Graphics.Menus.Context;
using Miyu.UI.Input;
using Miyu.UI.Overlay;
using Miyu.UI.Screens.Authentication;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Screens;

namespace Miyu.UI;

public partial class MiyuApp : Game
{
    protected virtual Bindable<float> ZoomValue => config.GetBindable<float>(ClientConfigEntry.Zoom);

    private DependencyContainer dependencies = null!;
    private ClientConfig config = null!;
    private MiyuClient client = null!;
    private ScreenStack screens = null!;
    public LoadingOverlay Loading { get; private set; } = null!;

    public List<DiscordChannel> PrivateChannels { get; set; } = new();

    [BackgroundDependencyLoader]
    private void load()
    {
        Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(MiyuApp).Assembly), "Resources"));
        Resources.AddExtension("json");

        AddFont(Resources, "Fonts/Noto/Noto-Basic");
        AddFont(Resources, "Fonts/Noto/Noto-CJK-Basic");
        AddFont(Resources, "Fonts/Noto/Noto-CJK-Compatibility");
        AddFont(Resources, "Fonts/Noto/Noto-Hangul");
        AddFont(Resources, "Fonts/Noto/Noto-Math");
        AddFont(Resources, "Fonts/Noto/Noto-Thai");

        AddFont(Resources, "Fonts/ggsans/ggsans");
        AddFont(Resources, "Fonts/ggsans/ggsans-medium");
        AddFont(Resources, "Fonts/ggsans/ggsans-semibold");
        AddFont(Resources, "Fonts/ggsans/ggsans-bold");

        AddFont(Resources, "Fonts/Twemoji/Twemoji");

        foreach (var resource in Resources.GetAvailableResources())
            Logger.Log($"Found resource: {resource}", level: LogLevel.Debug);

        Catppuccin.Load(Resources.Get("Themes/catppuccin.json"));

        dependencies.CacheAs(this);
        dependencies.CacheAs(config = new ClientConfig(Host.Storage));
        dependencies.CacheAs(client = new MiyuClient(new MiyuConfig
        {
            RegisterCommands = false,
            ClientToken = true
        })
        {
            BeforeHandle = (m, a) => Schedule(() => a(m))
        });

        dependencies.CacheAs(new CachedTextureLoader(Host.Renderer, new CachedImageStore(Host.CacheStorage)));

        var overlays = new OverlayManager();
        dependencies.CacheAs(overlays);

        base.Add(new ZoomContainer(new KeybindingHandler
        {
            RelativeSizeAxes = Axes.Both,
            Child = new MiyuContextContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = new PopoverContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        screens = new ScreenStack(),
                        overlays,
                        Loading = new LoadingOverlay()
                    }
                }
            }
        }) { Zoom = ZoomValue });
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        client.OnDisconnect += () =>
        {
            Loading.StateText = "WebSocket disconnected. Reconnecting is not implemented yet. Restart the client.";
            Loading.Show();
        };
        screens.Push(new AuthScreen());
    }

    protected override bool OnExiting()
    {
        client.Disconnect();
        return base.OnExiting();
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
    {
        return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}
