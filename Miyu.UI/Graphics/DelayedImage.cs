// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace Miyu.UI.Graphics;

public partial class DelayedImage : DelayedLoadUnloadWrapper
{
    public new Action<Sprite>? OnLoadComplete { get; set; } = null;

    public DelayedImage(string path)
        : base(() => new Image(path), 0, 2000)
    {
        DelayedLoadComplete += d => { OnLoadComplete?.Invoke((Sprite)d); };
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
        private void load(TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;
            FillMode = FillMode.Fill;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            try
            {
                Texture = textures.Get(path);
            }
            catch
            {
            }
        }
    }
}
