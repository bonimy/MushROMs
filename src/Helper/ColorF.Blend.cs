// <copyright file="ColorF.Blend.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Math;

namespace Helper
{
    public delegate ColorF ColorBlend(ColorF top, ColorF bottom);

    public delegate float ChannelBlend(float top, float bottom);

    partial struct ColorF
    {
        private static readonly IReadOnlyDictionary<BlendMode, ColorBlend> BlendDictionary = new Dictionary<BlendMode, ColorBlend>()
        {
            { BlendMode.Alpha       , AlphaBlend },
            { BlendMode.Grayscale   , Grayscale },
            { BlendMode.Multiply    , Multiply },
            { BlendMode.Screen      , Screen },
            { BlendMode.Overlay     , Overlay },
            { BlendMode.HardLight   , HardLight },
            { BlendMode.SoftLight   , SoftLight },
            { BlendMode.ColorDodge  , ColorDodge },
            { BlendMode.LinearDodge , LinearDodge },
            { BlendMode.ColorBurn   , ColorBurn },
            { BlendMode.LinearBurn  , LinearBurn },
            { BlendMode.VividLight  , VividLight },
            { BlendMode.LinearLight , LinearLight },
            { BlendMode.Difference  , Difference },
            { BlendMode.Darken      , Darken },
            { BlendMode.Lighten     , Lighten },
            { BlendMode.DarkerColor , DarkerColor },
            { BlendMode.LighterColor, LighterColor },
            { BlendMode.Hue         , HueBlend },
            { BlendMode.Saturation  , SaturationBlend },
            { BlendMode.Luminosity  , LuminosityBlend },
            { BlendMode.Divide      , Divide }
        };

        public static ColorF Blend(ColorF top, ColorF bottom, BlendMode blendMode)
        {
            if (BlendDictionary.TryGetValue(blendMode, out var blend))
            {
                return blend(top, bottom);
            }

            throw new InvalidEnumArgumentException(
                nameof(blendMode),
                (int)blendMode,
                typeof(BlendMode));
        }

        public static ColorF AlphaBlend(ColorF top, ColorF bottom)
        {
            if (top.Alpha == 0)
            {
                return bottom;
            }

            if (top.Alpha == 1)
            {
                return top;
            }

            var t = top.Alpha;
            var u = bottom.Alpha;

            var a = t + (u * (1 - t));
            var r = Mix(top.Red, bottom.Red);
            var g = Mix(top.Green, bottom.Green);
            var b = Mix(top.Blue, bottom.Blue);

            return new ColorF(a, r, g, b);

            float Mix(float x, float y)
            {
                return ((x * t) + (y * u * (1 - t))) / a;
            }
        }

        public static ColorF Blend(ColorF top, ColorF bottom, ChannelBlend rule)
        {
            var a = top.Alpha;
            var r = rule(top.Red, bottom.Red);
            var g = rule(top.Green, bottom.Green);
            var b = rule(top.Blue, bottom.Blue);
            var result = new ColorF(a, r, g, b);

            return AlphaBlend(result, bottom);
        }

        public static ColorF Grayscale(ColorF top, ColorF bottom)
        {
            var gray = (1f / NumberOfColorChannels) * (
                (top.Red * bottom.Red) +
                (top.Green * bottom.Green) +
                (top.Blue * bottom.Blue));

            return Blend(top, bottom, (x, y) => gray);
        }

        public static ColorF Multiply(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, (x, y) => x * y);
        }

        public static ColorF Divide(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                if (x == 0)
                {
                    return 1;
                }

                return y / x;
            }
        }

        public static ColorF Screen(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return 1 - ((1 - x) * (1 - y));
            }
        }

        public static ColorF Overlay(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                if (y < 0.5f)
                {
                    return 2 * x * y;
                }

                return 1 - (2 * (1 - x) * (1 - y));
            }
        }

        public static ColorF HardLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                if (y > 0.5f)
                {
                    return 2 * x * y;
                }

                return 1 - (2 * (1 - x) * (1 - y));
            }
        }

        public static ColorF SoftLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                var result = 2 * y;
                if (y < 0.5f)
                {
                    result *= x;
                    result += x * x * (1 - (2 * y));
                }
                else
                {
                    result *= (1 - x);
                    result += (float)Sqrt(y) * ((2 * x) - 1);
                }

                return result;
            }
        }

        public static ColorF ColorDodge(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                if (x == 1)
                {
                    return 1;
                }

                return y / (1 - x);
            }
        }

        public static ColorF LinearDodge(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, (x, y) => x + y);
        }

        public static ColorF ColorBurn(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                if (x == 0)
                {
                    return 0;
                }

                return 1 - ((1 - y) / x);
            }
        }

        public static ColorF LinearBurn(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, (x, y) => x + y - 1);
        }

        public static ColorF Difference(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                return x > y ? (x - y) : (y - x);
            }
        }

        public static ColorF Darken(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Math.Min);
        }

        public static ColorF Lighten(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Math.Max);
        }

        public static ColorF VividLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, Mix);

            float Mix(float x, float y)
            {
                if (x == 1)
                {
                    return 1;
                }

                if (x == 0)
                {
                    return 0;
                }

                if (x > 0.5f)
                {
                    return y / (1 - x);
                }

                return 1 - ((1 - x) / y);
            }
        }

        public static ColorF LinearLight(ColorF top, ColorF bottom)
        {
            return Blend(top, bottom, (x, y) => (2 * x) + y - 1);
        }

        public static ColorF DarkerColor(ColorF top, ColorF bottom)
        {
            var result = top.Luma < bottom.Luma ? top : bottom;
            return AlphaBlend(result, bottom);
        }

        public static ColorF LighterColor(ColorF top, ColorF bottom)
        {
            var result = top.Luma > bottom.Luma ? top : bottom;
            return AlphaBlend(result, bottom);
        }

        public static ColorF HueBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, top.Hue, bottom.Chroma, bottom.Luma);
            return AlphaBlend(result, bottom);
        }

        public static ColorF SaturationBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, bottom.Hue, top.Chroma, bottom.Luma);
            return AlphaBlend(result, bottom);
        }

        public static ColorF LuminosityBlend(ColorF top, ColorF bottom)
        {
            var result = FromHcy(top.Alpha, bottom.Hue, bottom.Chroma, top.Luma);
            return AlphaBlend(result, bottom);
        }
    }
}
