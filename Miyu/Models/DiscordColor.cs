// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using Newtonsoft.Json;

namespace Miyu.Models;

[JsonConverter(typeof(DiscordColorConverter))]
public partial class DiscordColor
{
    public float R { get; }
    public float G { get; }
    public float B { get; }

    public DiscordColor(byte r, byte g, byte b)
    {
        R = r / (float)byte.MaxValue;
        G = g / (float)byte.MaxValue;
        B = b / (float)byte.MaxValue;
    }

    public DiscordColor(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }

    public uint ToRGB()
    {
        return ((uint)(Math.Min(1f, R) * byte.MaxValue) << 16) |
               ((uint)(Math.Min(1f, G) * byte.MaxValue) << 8) |
               (uint)(Math.Min(1f, B) * byte.MaxValue);
    }

    public static DiscordColor FromHex(string hex)
    {
        hex = hex.Replace("#", "");

        byte r;
        byte g;
        byte b;

        switch (hex.Length)
        {
            case 3:
                r = byte.Parse(hex[0].ToString(), NumberStyles.HexNumber);
                g = byte.Parse(hex[1].ToString(), NumberStyles.HexNumber);
                b = byte.Parse(hex[2].ToString(), NumberStyles.HexNumber);
                break;

            case 6:
                r = byte.Parse(hex[..2], NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                break;

            default:
                throw new ArgumentException("Invalid hex color.");
        }

        return new DiscordColor(r, g, b);
    }

    public static DiscordColor FromRGB(uint rgb)
    {
        return new DiscordColor((byte)((rgb >> 16) & 0xff), (byte)((rgb >> 8) & 0xff), (byte)(rgb & 0xff));
    }

    public static DiscordColor Hsl(float hue, float saturation, float lightness)
    {
        var c = (1f - Math.Abs(2f * lightness - 1f)) * saturation;
        var h = hue * 6f;
        var x = c * (1f - Math.Abs(h % 2f - 1f));

        float r, g, b;

        switch (h)
        {
            case >= 0f and < 1f:
                r = c;
                g = x;
                b = 0f;
                break;

            case >= 1f and < 2f:
                r = x;
                g = c;
                b = 0f;
                break;

            case >= 2f and < 3f:
                r = 0f;
                g = c;
                b = x;
                break;

            case >= 3f and < 4f:
                r = 0f;
                g = x;
                b = c;
                break;

            case >= 4f and < 5f:
                r = x;
                g = 0f;
                b = c;
                break;

            case >= 5f and <= 6f:
                r = c;
                g = 0f;
                b = x;
                break;

            default:
                r = g = b = 0f;
                break;
        }

        var m = lightness - c * 0.5f;
        return new DiscordColor(r + m, g + m, b + m);
    }
}

internal class DiscordColorConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue((value as DiscordColor)?.ToRGB());
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return reader.Value is null ? null : DiscordColor.FromRGB(Convert.ToUInt32(reader.Value));
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DiscordColor);
    }
}
