// <copyright file="ColorF.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;
using static Helper.MathHelper;
using Debug = System.Diagnostics.Debug;

namespace Helper
{
    public delegate ColorF ColorBlend(ColorF top, ColorF bottom);

    public struct ColorF
    {
        public static readonly ColorF Empty = new ColorF();

        private static readonly ColorF BalancedWeight = FromArgb(
            0,
            1 / 3f,
            1 / 3f,
            1 / 3f);

        private static readonly ColorF LumaWeight = FromArgb(
            0,
            LumaRed,
            LumaGreen,
            LumaBlue);

        private static readonly IReadOnlyDictionary<BlendMode, ColorBlend> BlendDictionary = new Dictionary<BlendMode, ColorBlend>()
        {
            { BlendMode.Alpha       , AlphaBlend },
            { BlendMode.Grayscale   , Grayscale },
            { BlendMode.Multiply    , Multiply },
            { BlendMode.Screen      , ScreenBlend },
            { BlendMode.Overlay     , Overlay },
            { BlendMode.HardLight   , HardLightBlend },
            { BlendMode.SoftLight   , SoftLightBlend },
            { BlendMode.ColorDodge  , ColorDodge },
            { BlendMode.LinearDodge , LinearDodge },
            { BlendMode.ColorBurn   , ColorBurn },
            { BlendMode.LinearBurn  , LinearBurn },
            { BlendMode.VividLight  , VividLightBlend },
            { BlendMode.LinearLight , LinearLightBlend },
            { BlendMode.Difference  , Difference },
            { BlendMode.Darken      , Darken },
            { BlendMode.Lighten     , Lighten },
            { BlendMode.DarkerColor , DarkerColorBlend },
            { BlendMode.LighterColor, LighterColorBlend },
            { BlendMode.Hue         , HueBlend },
            { BlendMode.Saturation  , SaturationBlend },
            { BlendMode.Luminosity  , LuminosityBlend },
            { BlendMode.Divide      , Divide }
        };

        public const float LumaRed = 0.299f;
        public const float LumaGreen = 0.587f;
        public const float LumaBlue = 0.114f;

        public const int NumberOfChannels = 4;
        public const int NumberOfColorChannels = NumberOfChannels - 1;

        public const int AlphaIndex = 3;
        public const int RedIndex = 2;
        public const int GreenIndex = 1;
        public const int BlueIndex = 0;

        public const float Tolerance = 1e-7f;

        public float Alpha
        {
            get;
            private set;
        }

        public float Red
        {
            get;
            private set;
        }

        public float Green
        {
            get;
            private set;
        }

        public float Blue
        {
            get;
            private set;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case AlphaIndex:
                        return Alpha;

                    case RedIndex:
                        return Red;

                    case GreenIndex:
                        return Green;

                    case BlueIndex:
                        return Blue;

                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(index),
                            SR.ErrorArrayBounds(nameof(index), index, NumberOfChannels));
                }
            }

            private set
            {
                switch (index)
                {
                    case AlphaIndex:
                        Alpha = value;
                        return;

                    case RedIndex:
                        Red = value;
                        return;

                    case GreenIndex:
                        Green = value;
                        return;

                    case BlueIndex:
                        Blue = value;
                        return;

                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(index),
                            SR.ErrorArrayBounds(nameof(index), index, NumberOfChannels));
                }
            }
        }

        public float Cyan
        {
            get
            {
                return 1 - Red;
            }
        }

        public float Magenta
        {
            get
            {
                return 1 - Green;
            }
        }

        public float Yellow
        {
            get
            {
                return 1 - Blue;
            }
        }

        public float Max
        {
            get
            {
                return Max(Red, Green, Blue);
            }
        }

        public float Min
        {
            get
            {
                return Min(Red, Green, Blue);
            }
        }

        public float Hue
        {
            get
            {
                var chroma = Chroma;
                var hue = 0f;

                // Gray colors have no hue, but we return 0 for optimization.
                if (NearlyEquals(chroma, 0, Tolerance))
                {
                    return hue;
                }

                var max = Max;
                if (max == Red)
                {
                    hue = ((Green - Blue) / chroma) + 0;
                }
                else if (max == Green)
                {
                    hue = ((Blue - Red) / chroma) + 2;
                }
                else
                {
                    // if (max == color.Blue)
                    hue = ((Red - Green) / chroma) + 4;
                }

                if (hue < 0)
                {
                    hue += 6;
                }

                return hue / 6;
            }
        }

        public float HueDegrees
        {
            get
            {
                return 360 * Hue;
            }
        }

        public float Saturation
        {
            get
            {
                // This number is undefined, but we return 0 for optimization.
                if (NearlyEquals(Chroma, 0, Tolerance))
                {
                    return 0;
                }

                return Chroma / (1 - Abs((2 * Lightness) - 1));
            }
        }

        public float Lightness
        {
            get
            {
                return (Max + Min) / 2;
            }
        }

        public float Luma
        {
            get
            {
                return
                    (LumaRed * Red) +
                    (LumaGreen * Green) +
                    (LumaBlue * Blue);
            }
        }

        public float Chroma
        {
            get
            {
                return Max - Min;
            }
        }

        private ColorF(float alpha, float red, float green, float blue)
        {
            Debug.Assert(!Single.IsNaN(alpha), "alpha cannot be NaN.");
            Debug.Assert(!Single.IsNaN(red), "red cannot be NaN.");
            Debug.Assert(!Single.IsNaN(green), "green cannot be NaN.");
            Debug.Assert(!Single.IsNaN(blue), "blue cannot be NaN.");

            Alpha = Clamp(alpha, 0, 1, Tolerance);
            Red = Clamp(red, 0, 1, Tolerance);
            Green = Clamp(green, 0, 1, Tolerance);
            Blue = Clamp(blue, 0, 1, Tolerance);
        }

        public ColorF BlendWith(ColorF bottom, BlendMode blendMode)
        {
            return Blend(this, bottom, blendMode);
        }

        public ColorF Blend(ColorF top, ColorF bottom, BlendMode blendMode)
        {
            return BlendDictionary[blendMode](top, bottom);
        }

        public ColorF AlphaBlend(ColorF bottom)
        {
            if (NearlyEquals(Alpha, 0, Tolerance))
            {
                return bottom;
            }

            if (NearlyEquals(Alpha, 1, Tolerance))
            {
                return this;
            }

            var result = Empty;
            result.Alpha = Alpha + (bottom.Alpha * (1 - Alpha));
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                result[i] = this[i] * Alpha;
                result[i] += bottom[i] * bottom.Alpha * (1 - Alpha);
                result[i] /= result.Alpha;
            }

            return result;
        }

        public static ColorF AlphaBlend(ColorF top, ColorF bottom)
        {
            return top.AlphaBlend(bottom);
        }

        public ColorF Grayscale()
        {
            const float Scale = 1 / 3f;
            return Grayscale(FromArgb(0, Scale, Scale, Scale));
        }

        public ColorF Grayscale(ColorF bottom)
        {
            var product =
                (Red * bottom.Red) +
                (Green * bottom.Green) +
                (Blue * bottom.Blue);

            var color = FromArgb(Alpha, product, product, product);
            return color & bottom;
        }

        public static ColorF Grayscale(ColorF top, ColorF bottom)
        {
            return top.Grayscale(bottom);
        }

        public ColorF Multiply(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                color[i] = this[i] * bottom[i];
            }

            return color & bottom;
        }

        public static ColorF Multiply(ColorF top, ColorF bottom)
        {
            return top.Multiply(bottom);
        }

        public ColorF Divide(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] == 0)
                {
                    color[i] = 1;
                }
                else
                {
                    color[i] = Min(bottom[i] / this[i], 1);
                }
            }

            return color & bottom;
        }

        public static ColorF Divide(ColorF top, ColorF bottom)
        {
            return top.Divide(bottom);
        }

        public ColorF ScreenBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                color[i] = 1 - ((1 - this[i]) * (1 - bottom[i]));
            }

            return color & bottom;
        }

        public static ColorF ScreenBlend(ColorF top, ColorF bottom)
        {
            return top.ScreenBlend(bottom);
        }

        public ColorF Overlay(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (bottom[i] < 0.5f)
                {
                    color[i] = 2 * bottom[i] * this[i];
                }
                else
                {
                    color[i] = 1 - (2 * (1 - bottom[i]) * (1 - this[i]));
                }
            }

            return color & bottom;
        }

        public static ColorF Overlay(ColorF top, ColorF bottom)
        {
            return top.Overlay(bottom);
        }

        public ColorF HardLightBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (bottom[i] > 0.5f)
                {
                    color[i] = 2 * bottom[i] * this[i];
                }
                else
                {
                    color[i] = 1 - (2 * (1 - bottom[i]) * (1 - this[i]));
                }
            }

            return color & bottom;
        }

        public static ColorF HardLightBlend(ColorF top, ColorF bottom)
        {
            return top.HardLightBlend(bottom);
        }

        public ColorF SoftLightBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (bottom[i] < 0.5f)
                {
                    color[i] = 2 * bottom[i] * this[i];
                    color[i] += bottom[i] * bottom[i] * (1 - (2 * this[i]));
                }
                else
                {
                    color[i] = 2 * bottom[i] * (1 - this[i]);
                    color[i] += (float)Sqrt(bottom[i]) * ((2 * this[i]) - 1);
                }
            }

            return color & bottom;
        }

        public static ColorF SoftLightBlend(ColorF top, ColorF bottom)
        {
            return top.SoftLightBlend(bottom);
        }

        public ColorF ColorDodge(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] < 1)
                {
                    color[i] = Min(bottom[i] / (1 - this[i]), 1);
                }
                else
                {
                    color[i] = 1;
                }
            }

            return color & bottom;
        }

        public static ColorF ColorDodge(ColorF top, ColorF bottom)
        {
            return top.ColorDodge(bottom);
        }

        public ColorF LinearDodge(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                color[i] = Min(this[i] + bottom[i], 1);
            }

            return color & bottom;
        }

        public static ColorF LinearDodge(ColorF top, ColorF bottom)
        {
            return top.LinearDodge(bottom);
        }

        public ColorF ColorBurn(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] > 0)
                {
                    color[i] = Max(1 - ((1 - bottom[i]) / this[i]), 0);
                }
                else
                {
                    color[i] = 0;
                }
            }

            return color & bottom;
        }

        public static ColorF ColorBurn(ColorF top, ColorF bottom)
        {
            return top.ColorBurn(bottom);
        }

        public ColorF LinearBurn(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                color[i] = Max(this[i] + bottom[i] - 1, 0);
            }

            return color & bottom;
        }

        public static ColorF LinearBurn(ColorF top, ColorF bottom)
        {
            return top.LinearBurn(bottom);
        }

        public ColorF Difference(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] > bottom[i])
                {
                    color[i] = this[i] - bottom[i];
                }
                else
                {
                    color[i] = bottom[i] - this[i];
                }
            }

            return color & bottom;
        }

        public static ColorF Difference(ColorF top, ColorF bottom)
        {
            return top.Difference(bottom);
        }

        public ColorF Darken(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                color[i] = Min(this[i], bottom[i]);
            }

            return color & bottom;
        }

        public static ColorF Darken(ColorF top, ColorF bottom)
        {
            return top.Darken(bottom);
        }

        public ColorF Lighten(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                color[i] = Max(this[i], bottom[i]);
            }

            return color & bottom;
        }

        public static ColorF Lighten(ColorF top, ColorF bottom)
        {
            return top.Lighten(bottom);
        }

        public ColorF VividLightBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] > 0.5f)
                {
                    if (this[i] < 1)
                    {
                        color[i] = Min(bottom[i] / (1 - this[i]), 1);
                    }
                    else
                    {
                        color[i] = 1;
                    }
                }
                else if (this[i] > 0)
                {
                    color[i] = Max(1 - ((1 - bottom[i]) / this[i]), 0);
                }
                else
                {
                    color[i] = 0;
                }
            }

            return color & bottom;
        }

        public static ColorF VividLightBlend(ColorF top, ColorF bottom)
        {
            return top.VividLightBlend(bottom);
        }

        public ColorF LinearLightBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                color[i] = Clamp(
                    (2 * this[i]) + bottom[i] - 1,
                    0,
                    1);
            }

            return color & bottom;
        }

        public static ColorF LinearLightBlend(ColorF top, ColorF bottom)
        {
            return top.LinearLightBlend(bottom);
        }

        public ColorF DarkerColorBlend(ColorF bottom)
        {
            return Luma < bottom.Luma ? this : bottom;
        }

        public static ColorF DarkerColorBlend(ColorF top, ColorF bottom)
        {
            return top.DarkerColorBlend(bottom);
        }

        public ColorF LighterColorBlend(ColorF bottom)
        {
            return Luma > bottom.Luma ? this : bottom;
        }

        public static ColorF LighterColorBlend(ColorF top, ColorF bottom)
        {
            return top.LighterColorBlend(bottom);
        }

        public ColorF RotateHue(ColorF bottom)
        {
            var hue = Hue + bottom.Hue;
            while (hue >= 1)
            {
                hue -= 1;
            }

            while (hue < 0)
            {
                hue += 1;
            }

            var color = FromHcy(Alpha, hue, bottom.Chroma, bottom.Luma);
            return color & bottom;
        }

        public ColorF HueBlend(ColorF bottom)
        {
            var color = FromHcy(Alpha, Hue, bottom.Chroma, bottom.Luma);
            return color & bottom;
        }

        public static ColorF HueBlend(ColorF top, ColorF bottom)
        {
            return top.HueBlend(bottom);
        }

        public ColorF SaturationBlend(ColorF bottom)
        {
            var color = FromHcy(Alpha, bottom.Hue, Chroma, bottom.Luma);
            return color & bottom;
        }

        public static ColorF SaturationBlend(ColorF top, ColorF bottom)
        {
            return top.SaturationBlend(bottom);
        }

        public ColorF LuminosityBlend(ColorF bottom)
        {
            var color = FromHcy(Alpha, bottom.Hue, bottom.Chroma, Luma);
            return color & bottom;
        }

        public static ColorF LuminosityBlend(ColorF top, ColorF bottom)
        {
            return top.LuminosityBlend(bottom);
        }

        public ColorF Invert()
        {
            return new ColorF(Alpha, 1 - Red, 1 - Green, 1 - Blue);
        }

        public static ColorF FromArgb(float red, float green, float blue)
        {
            return FromArgb(1, red, green, blue);
        }

        public static ColorF FromArgb(float alpha, ColorF color)
        {
            return FromArgb(alpha, color.Red, color.Green, color.Green);
        }

        public static ColorF FromArgb(float alpha, float red, float green, float blue)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            }

            if (Single.IsNaN(red))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(red)), nameof(red));
            }

            if (Single.IsNaN(green))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(green)), nameof(green));
            }

            if (Single.IsNaN(blue))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(blue)), nameof(blue));
            }

            alpha = Clamp(alpha, 0, 1, Tolerance);
            red = Clamp(red, 0, 1, Tolerance);
            green = Clamp(green, 0, 1, Tolerance);
            blue = Clamp(blue, 0, 1, Tolerance);

            return new ColorF(alpha, red, green, blue);
        }

        public static ColorF FromCmy(float cyan, float magenta, float yellow)
        {
            return FromCmy(1, cyan, magenta, yellow);
        }

        public static ColorF FromCmy(float alpha, float cyan, float magenta, float yellow)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(cyan)), nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(magenta)), nameof(magenta));
            }

            if (Single.IsNaN(yellow))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(yellow)), nameof(yellow));
            }

            alpha = Clamp(alpha, 0, 1, Tolerance);
            cyan = Clamp(cyan, 0, 1, Tolerance);
            magenta = Clamp(magenta, 0, 1, Tolerance);
            yellow = Clamp(yellow, 0, 1, Tolerance);

            var red = 1 - cyan;
            var green = 1 - magenta;
            var blue = 1 - yellow;

            return new ColorF(alpha, red, green, blue);
        }

        public static ColorF FromCmyk(float cyan, float magenta, float yellow, float black)
        {
            return FromCmyk(1, cyan, magenta, yellow, black);
        }

        public static ColorF FromCmyk(float alpha, float cyan, float magenta, float yellow, float black)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(cyan)), nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(magenta)), nameof(magenta));
            }

            if (Single.IsNaN(yellow))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(yellow)), nameof(yellow));
            }

            if (Single.IsNaN(black))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(black)), nameof(black));
            }

            alpha = Clamp(alpha, 0, 1, Tolerance);
            cyan = Clamp(cyan, 0, 1, Tolerance);
            magenta = Clamp(magenta, 0, 1, Tolerance);
            yellow = Clamp(yellow, 0, 1, Tolerance);
            black = Clamp(black, 0, 1, Tolerance);

            var white = 1 - black;
            var red = white * (1 - cyan);
            var green = white * (1 - magenta);
            var blue = white * (1 - yellow);

            return new ColorF(alpha, red, green, blue);
        }

        public static ColorF FromHcy(float hue, float chroma, float luma)
        {
            return FromHcy(1, hue, chroma, luma);
        }

        public static ColorF FromHcy(float alpha, float hue, float chroma, float luma)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            }

            if (Single.IsNaN(chroma))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(chroma)), nameof(chroma));
            }

            if (Single.IsNaN(luma))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(luma)), nameof(luma));
            }

            alpha = Clamp(alpha, 0, 1, Tolerance);
            hue = Clamp(hue, 0, 1, Tolerance);
            chroma = Clamp(chroma, 0, 1, Tolerance);
            luma = Clamp(luma, 0, 1, Tolerance);

            hue *= 6;
            var color = Empty;

            if (chroma > 0)
            {
                if (hue >= 0 && hue < 1)
                {
                    color.Red = chroma;
                    color.Green = chroma * hue;
                }
                else if (hue >= 1 && hue < 2)
                {
                    color.Blue = chroma * (2 - hue);
                    color.Green = chroma;
                }
                else if (hue >= 2 && hue < 3)
                {
                    color.Green = chroma;
                    color.Blue = chroma * (hue - 2);
                }
                else if (hue >= 3 && hue < 4)
                {
                    color.Green = chroma * (4 - hue);
                    color.Blue = chroma;
                }
                else if (hue >= 4 && hue < 5)
                {
                    color.Red = chroma * (hue - 4);
                    color.Blue = chroma;
                }
                else
                {
                    // if (hue >= 5 && hue < 6)
                    color.Red = chroma;
                    color.Blue = chroma * (6 - hue);
                }
            }

            var match = luma - color.Luma;

            return new ColorF(alpha, match + color.Red, match + color.Green, match + color.Blue);
        }

        public static ColorF FromHsl(float hue, float saturation, float lightness)
        {
            return FromHsl(1, hue, saturation, lightness);
        }

        public static ColorF FromHsl(float alpha, float hue, float saturation, float lightness)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            }

            if (Single.IsNaN(saturation))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(saturation)), nameof(saturation));
            }

            if (Single.IsNaN(lightness))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(lightness)), nameof(lightness));
            }

            alpha = Clamp(alpha, 0, 1, Tolerance);
            hue = Clamp(hue, 0, 1, Tolerance);
            saturation = Clamp(saturation, 0, 1, Tolerance);
            lightness = Clamp(lightness, 0, 1, Tolerance);

            hue *= 6;
            var chroma = (1 - Abs((2 * lightness) - 1)) * saturation;
            var red = 0f;
            var green = 0f;
            var blue = 0f;

            if (chroma > 0)
            {
                if (hue >= 0 && hue < 1)
                {
                    red = chroma;
                    green = chroma * hue;
                }
                else if (hue >= 1 && hue < 2)
                {
                    red = chroma * (2 - hue);
                    green = chroma;
                }
                else if (hue >= 2 && hue < 3)
                {
                    green = chroma;
                    blue = chroma * (hue - 2);
                }
                else if (hue >= 3 && hue < 4)
                {
                    green = chroma * (4 - hue);
                    blue = chroma;
                }
                else if (hue >= 4 && hue < 5)
                {
                    red = chroma * (hue - 4);
                    blue = chroma;
                }
                else
                {
                    // if (hue >= 5 && hue < 6)
                    red = chroma;
                    blue = chroma * (6 - hue);
                }
            }

            var match = lightness - (0.5f * chroma);

            return new ColorF(alpha, red + match, green + match, blue + match);
        }

        public static ColorF operator ~(ColorF color)
        {
            return color.Invert();
        }

        public static ColorF operator *(ColorF left, ColorF right)
        {
            return left.Multiply(right);
        }

        public static ColorF operator &(ColorF left, ColorF right)
        {
            if (NearlyEquals(left.Alpha, 0, Tolerance))
            {
                return right;
            }

            if (NearlyEquals(left.Alpha, 1, Tolerance))
            {
                return left;
            }

            var result = Empty;
            result.Alpha = left.Alpha + (right.Alpha * (1 - left.Alpha));
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                result[i] = left[i] * left.Alpha;
                result[i] += right[i] * right.Alpha * (1 - left.Alpha);
                result[i] /= result.Alpha;
            }

            return result;
        }

        public static bool operator ==(ColorF left, ColorF right)
        {
            for (var i = NumberOfChannels; --i >= 0;)
            {
                if (!NearlyEquals(left[i], right[i], Tolerance))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(ColorF left, ColorF right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is ColorF value)
            {
                return value == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var result = 0;

            for (var i = 0; i < NumberOfChannels; i++)
            {
                var channel = (int)Round(this[i] * Byte.MaxValue);

                result |= channel << (8 << i);
            }

            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(nameof(Alpha));
            sb.Append(": ");
            sb.Append(SR.GetString(Alpha));
            sb.Append(", ");
            sb.Append(nameof(Red));
            sb.Append(": ");
            sb.Append(SR.GetString(Red));
            sb.Append(", ");
            sb.Append(nameof(Green));
            sb.Append(": ");
            sb.Append(SR.GetString(Green));
            sb.Append(", ");
            sb.Append(nameof(Blue));
            sb.Append(": ");
            sb.Append(SR.GetString(Blue));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
