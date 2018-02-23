// <copyright file="ColorF.Initializers.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using Debug = System.Diagnostics.Debug;
using static System.Math;
using static Helper.MathHelper;

namespace Helper
{
    partial struct ColorF
    {
        public static ColorF FromArgb(float red, float green, float blue)
        {
            return FromArgb(1, red, green, blue);
        }

        public static ColorF FromArgb(float alpha, ColorF color)
        {
            return FromArgb(alpha, color.Red, color.Green, color.Green);
        }

        public static ColorF FromArgb(
            float alpha,
            float red,
            float green,
            float blue)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentNaNException(nameof(alpha));
            }

            if (Single.IsNaN(red))
            {
                throw new ArgumentNaNException(nameof(red));
            }

            if (Single.IsNaN(green))
            {
                throw new ArgumentNaNException(nameof(green));
            }

            if (Single.IsNaN(blue))
            {
                throw new ArgumentNaNException(nameof(blue));
            }

            return new ColorF(alpha, red, green, blue);
        }

        public static ColorF FromCmy(
            float cyan,
            float magenta,
            float yellow)
        {
            return FromCmy(1, cyan, magenta, yellow);
        }

        public static ColorF FromCmy(
            float alpha,
            float cyan,
            float magenta,
            float yellow)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentNaNException(nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw new ArgumentNaNException(nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw new ArgumentNaNException(nameof(magenta));
            }

            if (Single.IsNaN(yellow))
            {
                throw new ArgumentNaNException(nameof(yellow));
            }

            var red = 1 - cyan;
            var green = 1 - magenta;
            var blue = 1 - yellow;

            return new ColorF(alpha, red, green, blue);
        }

        public static ColorF FromCmyk(
            float cyan,
            float magenta,
            float yellow,
            float black)
        {
            return FromCmyk(1, cyan, magenta, yellow, black);
        }

        public static ColorF FromCmyk(
            float alpha,
            float cyan,
            float magenta,
            float yellow,
            float black)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentNaNException(nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw new ArgumentNaNException(nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw new ArgumentNaNException(nameof(yellow));
            }

            if (Single.IsNaN(yellow))
            {
                throw new ArgumentNaNException(nameof(magenta));
            }

            if (Single.IsNaN(black))
            {
                throw new ArgumentNaNException(nameof(black));
            }

            var white = 1 - black;
            var red = white * (1 - cyan);
            var green = white * (1 - magenta);
            var blue = white * (1 - yellow);

            return new ColorF(alpha, red, green, blue);
        }

        public static ColorF FromHcy(
            float hue,
            float chroma,
            float luma)
        {
            return FromHcy(1, hue, chroma, luma);
        }

        public static ColorF FromHcy(
            float alpha,
            float hue,
            float chroma,
            float luma)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentNaNException(nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw new ArgumentNaNException(nameof(hue));
            }

            if (Single.IsNaN(chroma))
            {
                throw new ArgumentNaNException(nameof(chroma));
            }

            if (Single.IsNaN(luma))
            {
                throw new ArgumentNaNException(nameof(luma));
            }

            chroma = Clamp(chroma, 0, 1);
            luma = Clamp(luma, 0, 1);
            var (r, g, b) = GetRgb(hue, chroma);

            var lumaR = r * LumaRedWeight;
            var lumaG = g * LumaGreenWeight;
            var lumaB = b * LumaBlueWeight;
            var match = luma - (lumaR + lumaG + lumaB);

            return new ColorF(alpha, match + r, match + g, match + b);
        }

        public static ColorF FromHsl(
            float hue,
            float saturation,
            float lightness)
        {
            return FromHsl(1, hue, saturation, lightness);
        }

        public static ColorF FromHsl(
            float alpha,
            float hue,
            float saturation,
            float lightness)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentNaNException(nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw new ArgumentNaNException(nameof(hue));
            }

            if (Single.IsNaN(saturation))
            {
                throw new ArgumentNaNException(nameof(saturation));
            }

            if (Single.IsNaN(lightness))
            {
                throw new ArgumentNaNException(nameof(lightness));
            }

            saturation = Clamp(saturation, 0, 1);
            lightness = Clamp(lightness, 0, 1);

            var chroma = (1 - Abs((2 * lightness) - 1)) * saturation;
            var (r, g, b) = GetRgb(hue, chroma);
            var match = lightness - (0.5f * chroma);

            return new ColorF(alpha, r + match, g + match, b + match);
        }

        private static (float r, float g, float b) GetRgb(float hue, float chroma)
        {
            if (chroma == 0)
            {
                return (0, 0, 0);
            }

            while (hue < 0)
            {
                hue += 1;
            }

            while (hue >= 1)
            {
                hue -= 1;
            }

            hue *= 6;
            if (hue >= 0 && hue < 1)
            {
                return (chroma, chroma * hue, 0);
            }

            if (hue >= 1 && hue < 2)
            {
                return (chroma * (2 - hue), chroma, 0);
            }

            if (hue >= 2 && hue < 3)
            {
                return (0, chroma, chroma * (hue - 2));
            }

            if (hue >= 3 && hue < 4)
            {
                return (0, chroma * (4 - hue), chroma);
            }

            if (hue >= 4 && hue < 5)
            {
                return (chroma * (hue - 4), 0, chroma);
            }

            if (hue >= 5 && hue < 6)
            {
                return (chroma, 0, chroma * (6 - hue));
            }

            Debug.Assert(false, "Impossible hue value");
            return (Single.NaN, Single.NaN, Single.NaN);
        }
    }
}
