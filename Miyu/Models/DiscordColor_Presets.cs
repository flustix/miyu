// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Miyu.Models;

public partial class DiscordColor
{
    public static DiscordColor White => new(255, 255, 255);

    public static DiscordColor Red => FromHex("#FF0000");
    public static DiscordColor Orange => FromHex("#FF8000");
    public static DiscordColor Yellow => FromHex("#FFFF00");
    public static DiscordColor Lime => FromHex("#80FF00");
    public static DiscordColor Green => FromHex("#00FF00");
    public static DiscordColor Mint => FromHex("#00FF80");
    public static DiscordColor Cyan => FromHex("#00FFFF");
    public static DiscordColor Sky => FromHex("#0080FF");
    public static DiscordColor Blue => FromHex("#0000FF");
    public static DiscordColor Purple => FromHex("#8000FF");
    public static DiscordColor Pink => FromHex("#FF00FF");
    public static DiscordColor Magenta => FromHex("#FF0080");

    public static DiscordColor NeoRed => FromHex("#FF5555");
    public static DiscordColor NeoOrange => FromHex("#FFAA55");
    public static DiscordColor NeoYellow => FromHex("#FFFF55");
    public static DiscordColor NeoLime => FromHex("#AAFF55");
    public static DiscordColor NeoGreen => FromHex("#55FF55");
    public static DiscordColor NeoMint => FromHex("#55FFAA");
    public static DiscordColor NeoCyan => FromHex("#55FFFF");
    public static DiscordColor NeoSky => FromHex("#55AAFF");
    public static DiscordColor NeoBlue => FromHex("#5555FF");
    public static DiscordColor NeoPurple => FromHex("#AA55FF");
    public static DiscordColor NeoPink => FromHex("#FF55FF");
    public static DiscordColor NeoMagenta => FromHex("#FF55AA");

    public static DiscordColor PastelRed => FromHex("#FF9999");
    public static DiscordColor PastelOrange => FromHex("#FFCC99");
    public static DiscordColor PastelYellow => FromHex("#FFFF99");
    public static DiscordColor PastelLime => FromHex("#CCFF99");
    public static DiscordColor PastelGreen => FromHex("#99FF99");
    public static DiscordColor PastelMint => FromHex("#99FFCC");
    public static DiscordColor PastelCyan => FromHex("#99FFFF");
    public static DiscordColor PastelSky => FromHex("#99CCFF");
    public static DiscordColor PastelBlue => FromHex("#9999FF");
    public static DiscordColor PastelPurple => FromHex("#CC99FF");
    public static DiscordColor PastelPink => FromHex("#FF99FF");
    public static DiscordColor PastelMagenta => FromHex("#FF99CC");

    public static DiscordColor DiscordBlurple => FromHex("#5865F2");

    public static DiscordColor[] Rainbow =>
        [Red, Orange, Yellow, Lime, Green, Mint, Cyan, Sky, Blue, Purple, Pink, Magenta];

    public static DiscordColor[] NeoRainbow =>
        [NeoRed, NeoOrange, NeoYellow, NeoLime, NeoGreen, NeoMint, NeoCyan, NeoSky, NeoBlue, NeoPurple, NeoPink, NeoMagenta];

    public static DiscordColor[] PastelRainbow =>
        [PastelRed, PastelOrange, PastelYellow, PastelLime, PastelGreen, PastelMint, PastelCyan, PastelSky, PastelBlue, PastelPurple, PastelPink, PastelMagenta];

    public static DiscordColor RainbowRandom => Rainbow[new Random().Next(0, Rainbow.Length)];
    public static DiscordColor NeoRandom => NeoRainbow[new Random().Next(0, NeoRainbow.Length)];
    public static DiscordColor PastelRandom => PastelRainbow[new Random().Next(0, PastelRainbow.Length)];
}
