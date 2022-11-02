using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TapoConnect
{
    public class TapoColor
    {

        [JsonPropertyName("hue")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Hue { get; }

        [JsonPropertyName("saturation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Saturation { get; }

        [JsonPropertyName("brightness")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Brightness { get; }

        [JsonPropertyName("color_temp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ColorTemp { get; }

        protected TapoColor(int hue, int saturation, int? brightness)
        {
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
            ColorTemp = null;
        }

        protected TapoColor(int colorTemp, int? brightness)
        {
            Hue = null;
            Saturation = null;
            Brightness = brightness;
            ColorTemp = colorTemp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">Supports values like "#42903a", "3600k", "hsl(220, 60, 80)", "rgb(200,100,20)" </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public static TapoColor FromString(string color)
        {
            if (color == null)
            {
                throw new ArgumentNullException(nameof(color));
            }

            color = color.ToLower();

            if (color.StartsWith('#')) return FromHex(color);
            if (color.EndsWith('k')) return FromTemperature(color, null);
            if (color.StartsWith("hsl")) return FromHsl(color);
            if (color.StartsWith("rgb")) return FromRgb(color);

            throw new Exception("Invalid Color");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temp">2500 - 6500.</param>
        /// <param name="brightness">0 - 100</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static TapoColor FromTemperature(int temp, int? brightness = null)
        {
            if (temp < 2500 || temp > 6500)
            {
                throw new ArgumentOutOfRangeException(nameof(temp), $"Value must be between 2500 and 6500. ({temp})");
            }

            if (brightness.HasValue && (brightness.Value < 0 || brightness.Value > 100))
            {
                throw new ArgumentOutOfRangeException(nameof(brightness), $"Value must be between 0 and 100. ({brightness.Value}).");
            }

            return new TapoColor(temp, brightness);
        }

        public static TapoColor FromTemperature(string temp, int? brightness = null)
        {
            if (temp == null)
            {
                throw new ArgumentNullException(nameof(temp));
            }

            var pattern = new Regex(@"^(\d*)\s*k?$");

            var result = pattern.Match(temp.ToLower());

            if (!result.Success)
            {
                throw new Exception($"Invalid temperature string: {temp}");
            }

            var k = Convert.ToInt16(result.Groups[1].Value);

            return FromTemperature(k, brightness);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hex">#000000 Not alowed.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public static TapoColor FromHex(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException(nameof(hex));
            }

            if (hex == "#000000")
                throw new Exception($"Invalid hex string: {hex} (Cannot be black)");

            var pattern = new Regex(@"^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$");

            var result = pattern.Match(hex.ToLower());

            if (!result.Success)
            {
                throw new Exception($"Invalid hex string: {hex}");
            }

            var r = Convert.ToInt16(result.Groups[1].Value, 16);
            var g = Convert.ToInt16(result.Groups[2].Value, 16);
            var b = Convert.ToInt16(result.Groups[3].Value, 16);

            return FromRgb(r, g, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r">0 - 255</param>
        /// <param name="g">0 - 255</param>
        /// <param name="b">0 - 255</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static TapoColor FromRgb(int r, int g, int b)
        {
            if (r < 0 || r > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(r), $"Value must be between 0 and 255. ({r}).");
            }

            if (g < 0 || g > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(g), $"Value must be between 0 and 255. ({g}).");
            }

            if (b < 0 || b > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(b), $"Value must be between 0 and 255. ({b}).");
            }

            var rF = r / 255f;
            var gF = g / 255f;
            var bF = b / 255f;

            var max = Math.Max(rF, Math.Max(gF, bF));
            var min = Math.Min(rF, Math.Min(gF, bF));

            double h, s, l = (max + min) / 2;

            if (max == min)
            {
                h = s = 0; // achromatic
            }
            else
            {
                var d = max - min;
                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);

                if (max == rF)
                {
                    h = (gF - bF) / d + (gF < bF ? 6 : 0);
                }
                else if (max == gF)
                {
                    h = (bF - rF) / d + 2;
                }
                else
                {
                    h = (rF - gF) / d + 4;
                }

                h /= 6;
            }

            s *= 100;
            s = Math.Round(s);
            l *= 100;
            l = Math.Round(l);
            h = Math.Round(360 * h);

            return new TapoColor((int)h, (int)s, (int)l);
        }

        public static TapoColor FromRgb(string rgb)
        {
            if (rgb == null)
            {
                throw new ArgumentNullException(nameof(rgb));
            }

            var pattern = new Regex(@"^(?:rgb\()?(\d+)(?:[\s,])+(\d+)(?:[\s,])+(\d+)\)?$");

            var result = pattern.Match(rgb.ToLower());

            if (!result.Success)
            {
                throw new Exception($"Invalid rgb string: {rgb}");
            }

            var r = Convert.ToInt16(result.Groups[1].Value);
            var g = Convert.ToInt16(result.Groups[2].Value);
            var b = Convert.ToInt16(result.Groups[3].Value);

            return FromRgb(r, g, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hue">0-360</param>
        /// <param name="saturation">0-100</param>
        /// <param name="lightness">0-100</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static TapoColor FromHsl(
            int hue,
            int saturation,
            int lightness)
        {
            if (hue < 0 || hue > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(hue), $"Value must be between 0 and 360. ({hue}).");
            }

            if (saturation < 0 || saturation > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(saturation), $"Value must be between 0 and 100. ({saturation}).");
            }

            if (lightness < 0 || lightness > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(saturation), $"Value must be between 0 and 100. ({lightness}).");
            }

            return new TapoColor(hue, saturation, lightness);
        }

        public static TapoColor FromHsl(string hsl)
        {
            if (hsl == null)
            {
                throw new ArgumentNullException(nameof(hsl));
            }

            var pattern = new Regex(@"^(?:hsl\()?(\d+)(?:[\s,])+(\d+)(?:[\s,])+(\d+)\)?$");

            var result = pattern.Match(hsl.ToLower());

            if (!result.Success)
            {
                throw new Exception($"Invalid hsl string: {hsl}");
            }

            var h = Convert.ToInt16(result.Groups[1].Value);
            var s = Convert.ToInt16(result.Groups[2].Value);
            var l = Convert.ToInt16(result.Groups[3].Value);

            return FromHsl(h, s, l);
        }
    }
}
