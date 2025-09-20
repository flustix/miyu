// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Miyu.UI.Config;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace Miyu.UI.Graphics;

public partial class DelayedImage : DelayedLoadUnloadWrapper
{
    public new Action<Sprite>? OnLoadComplete { get; set; } = null;
    public Action<Sprite>? AnimateAppear { get; set; } = null;

    [Resolved]
    private ClientConfig config { get; set; } = null!;

    public DelayedImage(string path)
        : base(() => new Image(path), 0, 2000)
    {
        DelayedLoadComplete += d =>
        {
            OnLoadComplete?.Invoke((Sprite)d);
            if (config.Get<bool>(ClientConfigEntry.Animations))
                AnimateAppear?.Invoke((Sprite)d);
        };
    }

    [LongRunningLoad]
    private partial class Image : Sprite
    {
        private string path { get; }

        public Image(string path)
        {
            this.path = path;
        }

        [BackgroundDependencyLoader]
        private void load(CachedTextureLoader cache)
        {
            RelativeSizeAxes = Axes.Both;
            FillMode = FillMode.Fill;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            try
            {
                Texture = cache.Get(path);
            }
            catch
            {
            }
        }
    }
}
