// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.API.Requests.Auth;
using Miyu.UI.Components.Form;
using Miyu.UI.Config;
using Miyu.UI.Graphics;
using Miyu.UI.Screens.Main;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input;
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

    private Container content = null!;

    private FillFlowContainer loginForm = null!;
    private FillFlowContainer totpForm = null!;
    private FillFlowContainer tokenForm = null!;

    private MiyuText loginError = null!;
    private MiyuLabeledTextBox username = null!;
    private MiyuLabeledTextBox password = null!;

    private MiyuText totpError = null!;
    private MiyuLabeledTextBox totp = null!;

    private MiyuLabeledTextBox tokenBox = null!;

    private Bindable<string> tokenBind = null!;
    private string ticket = "";
    private string instance = "";

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        RelativeSizeAxes = Axes.Both;

        tokenBind = config.GetBindable<string>(ClientConfigEntry.Token);

        InternalChildren = new Drawable[]
        {
            new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = textures.Get("login-background"),
                FillMode = FillMode.Fill,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
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
                    content = new Container
                    {
                        Width = 480,
                        AutoSizeAxes = Axes.Y,
                        Padding = new MarginPadding(32),
                        Children = new Drawable[]
                        {
                            loginForm = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Spacing = new Vector2(20),
                                Children = new Drawable[]
                                {
                                    loginError = new MiyuText
                                    {
                                        Text = "error message",
                                        Colour = Catppuccin.Current.Red,
                                        Weight = FontWeight.Bold,
                                        FontSize = 16,
                                        Alpha = 0
                                    },
                                    username = new MiyuLabeledTextBox("Email or Phone Number", TextInputType.EmailAddress, true),
                                    password = new MiyuLabeledTextBox("Password", TextInputType.Password, true),
                                    new MiyuButton("Log In", login) { RelativeSizeAxes = Axes.X },
                                    new MiyuButton("Login with token", () =>
                                    {
                                        loginForm.Hide();
                                        tokenForm.Show();
                                    }) { RelativeSizeAxes = Axes.X },
                                }
                            },
                            totpForm = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Spacing = new Vector2(20),
                                Alpha = 0,
                                Children = new Drawable[]
                                {
                                    totpError = new MiyuText
                                    {
                                        Text = "error message",
                                        Colour = Catppuccin.Current.Red,
                                        Weight = FontWeight.Bold,
                                        FontSize = 16,
                                        Alpha = 0
                                    },
                                    totp = new MiyuLabeledTextBox("Enter Discord Auth Code", TextInputType.Number, true),
                                    new MiyuButton("Confirm", sendTotp) { RelativeSizeAxes = Axes.X }
                                }
                            },
                            tokenForm = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Spacing = new Vector2(20),
                                Alpha = 0,
                                Children = new Drawable[]
                                {
                                    tokenBox = new MiyuLabeledTextBox("User Token", TextInputType.Password, true),
                                    new MiyuButton("Confirm", () =>
                                    {
                                        if (string.IsNullOrWhiteSpace(tokenBox.Text))
                                            return;

                                        tokenBind.Value = tokenBox.Text;
                                        continueToMain();
                                    }) { RelativeSizeAxes = Axes.X },
                                    new MiyuButton("Back", () =>
                                    {
                                        tokenForm.Hide();
                                        loginForm.Show();
                                    }) { RelativeSizeAxes = Axes.X },
                                }
                            }
                        }
                    },
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        if (!string.IsNullOrWhiteSpace(tokenBind.Value))
            continueToMain();

        ScheduleAfterChildren(() =>
        {
            content.AutoSizeDuration = 300;
            content.AutoSizeEasing = Easing.OutQuint;
        });
    }

    private async void login()
    {
        loginError.Hide();
        var res = await client.API.Execute(new LoginRequest(username.Text, password.Text));

        if (string.IsNullOrWhiteSpace(res?.Token))
        {
            if (res?.Ticket != null)
            {
                ticket = res.Ticket;
                instance = res.LoginInstanceID!;

                if (res.Totp)
                {
                    loginForm.Hide();
                    totpForm.Show();
                }
                else
                {
                    loginError.Text = "Enable TOTP in your discord account.";
                    loginError.Show();
                }
            }
            else
            {
                loginError.Text = "Failed to log in!";
                loginError.Show();
            }

            return;
        }

        tokenBind.Value = res.Token;
        continueToMain();
    }

    private async void sendTotp()
    {
        totpError.Hide();
        var res = await client.API.Execute(new TotpRequest(totp.Text, ticket, instance));

        if (string.IsNullOrWhiteSpace(res?.Token))
        {
            totpError.Text = "Invalid Code!";
            totpError.Show();
            return;
        }

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
