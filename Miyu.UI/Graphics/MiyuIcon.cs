// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Miyu.UI.Graphics;

public partial class MiyuIcon : Sprite
{
    private readonly Type type;

    public MiyuIcon(Type type, float size)
    {
        this.type = type;
        Size = new Vector2(size);
        Colour = Catppuccin.Current.Text;
    }

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        Texture = textures.Get($"Icons/{type.GetDescription()}");
    }

    public enum Type
    {
        [Description("copy")]
        Copy,

        [Description("discord")]
        Discord,

        [Description("forum")]
        Forum,

        [Description("hash")]
        Hash,

        [Description("hash-lock")]
        HashLock,

        [Description("id")]
        ID,

        [Description("lock")]
        Lock,

        [Description("voice")]
        Voice,

        [Description("voice-lock")]
        VoiceLock,

        [Description("reply")]
        Reply,

        [Description("forward")]
        Forward,

        [Description("emoji")]
        Emoji,

        [Description("sticker")]
        Sticker,

        [Description("plus")]
        Plus
    }
}
