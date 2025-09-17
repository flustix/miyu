// Copyright (c) flustix <me@flux.moe>.Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;

namespace Miyu.UI.Graphics;

public class CachedTextureLoader : LargeTextureStore
{
    public CachedTextureLoader(IRenderer renderer, CachedImageStore store)
        : base(renderer, store, TextureFilteringMode.Nearest, false)
    {
    }
}
