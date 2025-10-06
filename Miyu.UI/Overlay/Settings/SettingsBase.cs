// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Config;
using Miyu.UI.Graphics;
using Miyu.UI.Input;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osuTK;

namespace Miyu.UI.Overlay.Settings;

public abstract partial class SettingsBase : VisibilityContainer, IKeyBindingHandler<MiyuBind>
{
    [Resolved]
    private ClientConfig config { get; set; } = null!;

    protected override bool StartHidden => true;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.Both;
        Anchor = Origin = Anchor.Centre;

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Catppuccin.Current.Mantle,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(2)
            },
            new GridContainer
            {
                AutoSizeAxes = Axes.X,
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                ColumnDimensions = new Dimension[]
                {
                    new(GridSizeMode.Absolute, 238 + 32), // pad 16
                    new(GridSizeMode.Absolute, 740) // pad 40
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Catppuccin.Current.Crust,
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Size = new Vector2(4)
                                }
                            }
                        },
                        new MiyuScrollContainer()
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Padding = new MarginPadding(40),
                                Direction = FillDirection.Vertical,
                                Children = new Drawable[]
                                {
                                    new BasicSliderBar<float>
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 16,
                                        Current = config.GetBindable<float>(ClientConfigEntry.Zoom),
                                        KeyboardStep = 0.1f
                                    },
                                    new BasicCheckbox
                                    {
                                        Current = config.GetBindable<bool>(ClientConfigEntry.Animations),
                                        LabelText = "animations"
                                    },
                                    new BasicDropdown<string>
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Current = config.GetBindable<string>(ClientConfigEntry.Theme),
                                        Items = new[] { "latte", "frappe", "macchiato", "mocha" }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    protected override void PopIn()
    {
        this.ScaleTo(1.2f).FadeIn(config.AnimLen(200))
            .ScaleTo(1f, config.AnimLen(800), Easing.OutElasticHalf);
    }

    protected override void PopOut()
    {
        this.FadeOut(config.AnimLen(200))
            .ScaleTo(1.2f, config.AnimLen(400), Easing.OutQuint);
    }

    public bool OnPressed(KeyBindingPressEvent<MiyuBind> e)
    {
        if (e.Action != MiyuBind.Back)
            return false;

        Hide();
        return true;
    }

    public void OnReleased(KeyBindingReleaseEvent<MiyuBind> e)
    {
    }
}
