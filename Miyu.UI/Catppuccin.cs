// Copyright (c) flustix <me@flux.moe>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text;
using Midori.Utils;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Logging;

namespace Miyu.UI;

public class Catppuccin
{
    public static Style Current => frappe;

    private static Style frappe { get; set; } = null!;

    public static void Load(byte[] bytes)
    {
        var str = Encoding.UTF8.GetString(bytes);
        Logger.Log($"Loaded Catppuccin theme: {str}", LoggingTarget.Runtime, LogLevel.Debug);

        var json = str.Deserialize<JObject>()!;

        frappe = parseStyle(json["frappe"]?.ToObject<JObject>());
        Logger.Log($"Parsed Catppuccin theme: {frappe}", LoggingTarget.Runtime, LogLevel.Debug);
    }

    private static Style parseStyle(JObject? data)
    {
        var colors = data?["colors"]?.ToObject<JObject>();
        if (colors == null) throw new Exception("Failed to parse colors");

        var fields = typeof(Style).GetProperties();
        var style = new Style();

        foreach (var (key, col) in colors)
        {
            var obj = col?.ToObject<JObject>();
            var hex = obj?["hex"]?.ToObject<string>();

            if (hex == null) throw new Exception($"Failed to parse color {key}");

            var colour = Colour4.FromHex(hex);
            var field = fields.FirstOrDefault(f => f.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

            if (field == null)
                throw new Exception($"Failed to find field {key} in Style");

            if (field.PropertyType != typeof(ColourInfo))
                throw new Exception($"Field {key} is not of type ColourInfo");

            field.SetValue(style, (ColourInfo)colour);
        }

        return style;
    }

    public class Style
    {
        public ColourInfo Rosewater { get; set; }
        public ColourInfo Flamingo { get; set; }
        public ColourInfo Pink { get; set; }
        public ColourInfo Mauve { get; set; }
        public ColourInfo Red { get; set; }
        public ColourInfo Maroon { get; set; }
        public ColourInfo Peach { get; set; }
        public ColourInfo Yellow { get; set; }
        public ColourInfo Green { get; set; }
        public ColourInfo Teal { get; set; }
        public ColourInfo Sky { get; set; }
        public ColourInfo Sapphire { get; set; }
        public ColourInfo Blue { get; set; }
        public ColourInfo Lavender { get; set; }

        public ColourInfo Text { get; set; }
        public ColourInfo Subtext1 { get; set; }
        public ColourInfo Subtext0 { get; set; }

        public ColourInfo Overlay2 { get; set; }
        public ColourInfo Overlay1 { get; set; }
        public ColourInfo Overlay0 { get; set; }

        public ColourInfo Surface2 { get; set; }
        public ColourInfo Surface1 { get; set; }
        public ColourInfo Surface0 { get; set; }

        public ColourInfo Base { get; set; }
        public ColourInfo Mantle { get; set; }
        public ColourInfo Crust { get; set; }

        public override string ToString()
        {
            return this.Serialize();
        }
    }
}
