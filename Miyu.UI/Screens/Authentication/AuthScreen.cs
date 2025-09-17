// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Requests.Auth;
using Miyu.UI.Config;
using Miyu.UI.Screens.Main;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;

namespace Miyu.UI.Screens.Authentication;

public partial class AuthScreen : Screen
{
    [Resolved]
    private MiyuClient client { get; set; } = null!;

    [Resolved]
    private MiyuApp app { get; set; } = null!;

    [Resolved]
    private ClientConfig config { get; set; } = null!;

    private BasicTextBox username = null!;
    private BasicTextBox password = null!;

    private Bindable<string> tokenBind = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.Both;

        tokenBind = config.GetBindable<string>(ClientConfigEntry.Token);

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Crust
            },
            new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                CornerRadius = 8,
                Masking = true,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Catppuccin.Current.Base
                    },
                    new FillFlowContainer
                    {
                        Width = 420,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Padding = new MarginPadding(32),
                        Spacing = new Vector2(20),
                        Children = new Drawable[]
                        {
                            username = new BasicTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 44,
                                PlaceholderText = "Email or Phone Number"
                            },
                            password = new BasicTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 44,
                                PlaceholderText = "Password"
                            },
                            new BasicButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 44,
                                Text = "Log In",
                                Action = login
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

        if (!string.IsNullOrWhiteSpace(tokenBind.Value))
            continueToMain();
    }

    private async void login()
    {
        var res = await client.API.Execute(new LoginRequest(username.Text, password.Text));

        if (res?.Token is null)
            return;

        tokenBind.Value = res.Token;
        continueToMain();
    }

    private void continueToMain()
    {
        app.Loading.Show();
        app.Loading.FinishTransforms();

        this.Push(new AppScreen());
    }
}
