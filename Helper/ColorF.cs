// <copyright file="ColorF.cs>
//     Copyright (c) 2017 Nelson Garcia
// </copyright>

using System;
using System.Text;
using Debug = System.Diagnostics.Debug;

namespace Helper
{
    /// <summary>
    /// Represents a color in ARGB (alpha, red, green, blue) space.
    /// </summary>
    /// <remarks>
    /// The primary purpose of this structure over the standard and well-made <see cref="Color"/>
    /// structure is the ability to blend colors using different blending techniques, increased precision
    /// on color components, color math, and instantiation from other color spaces.
    /// <para/>
    /// See <see cref="BlendMode"/> for a comprehensive description of each blend mode, or the respective
    /// method defined in this structure.
    /// <para/>
    /// This structure ranges each color component from 0 to 1 inclusive as a <see cref="Single"/>
    /// value.
    /// <para/>
    /// Many blending options are simply color math operations. Their implementations have accompanying
    /// overloaded operators.
    /// <para/>
    /// Beyond the standard alpha, red, green, and blue components, values for the intensity of
    /// cyan, magenta, and yellow, as well as the color's lightness, hue, saturation, chroma, and luma are
    /// also provided. Static methods are provided to initialize a color given a fully-descriptive set of
    /// this information as well.
    /// <para/>
    /// Most of the ideas implemented in this structure were obtained from <see href="https://en.wikipedia.org/wiki/RGB_color_space">
    /// Wikipedia's article on the RGB color space</see>.
    /// </remarks>
    /// <seealso cref="Color"/>
    /// <seealso cref="BlendMode"/>
    /// <threadsafety static="true" instance="false"/>
    public struct ColorF
    {
        /// <summary>
        /// Represents a <see cref="ColorF"/> that has its <see cref="Alpha"/>,
        /// <see cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/> components
        /// all set to zero.
        /// </summary>
        public static readonly ColorF Empty = new ColorF();

        /// <summary>
        /// Represents a <see cref="ColorF"/> whose RGB components are equal and sum to unity.
        /// </summary>
        private static readonly ColorF BalancedWeight = FromArgb(0, 1 / 3f, 1 / 3f, 1 / 3f);

        /// <summary>
        /// Represents a <see cref="Color"/> whose RGB components are luma scaled and sum to unity.
        /// </summary>
        private static readonly ColorF LumaWeight = FromArgb(0, LumaRed, LumaGreen, LumaBlue);

        public const float LumaRed = 0.299f;
        public const float LumaGreen = 0.587f;
        public const float LumaBlue = 0.114f;

        /// <summary>
        /// The number of channels that specifies a <see cref="ColorF"/> value.
        /// This field is constant.
        /// </summary>
        public const int NumberOfChannels = 4;

        /// <summary>
        /// The number of channels, excluding the <see cref="Alpha"/> component.
        /// </summary>
        public const int NumberOfColorChannels = NumberOfChannels - 1;

        /// <summary>
        /// The index of the <see cref="Alpha"/> component in <see cref="this[Int32]"/>.
        /// This field is constant.
        /// </summary>
        public const int AlphaIndex = 3;

        /// <summary>
        /// The index of the <see cref="Red"/> component in <see cref="this[Int32]"/>.
        /// This field is constant.
        /// </summary>
        public const int RedIndex = 2;

        /// <summary>
        /// The index of the <see cref="Green"/> component in <see cref="this[Int32]"/>.
        /// This field is constant.
        /// </summary>
        public const int GreenIndex = 1;

        /// <summary>
        /// The index of the <see cref="Blue"/> component in <see cref="this[Int32]"/>.
        /// This field is constant.
        /// </summary>
        public const int BlueIndex = 0;

        /// <summary>
        /// The minimum allowable difference between two values to consider a component comparison of two
        /// <see cref="ColorF"/> channels as equal.
        /// </summary>
        public const float Tolerance = 1e-7f;

        /// <summary>
        /// Gets the alpha component of this <see cref="ColorF"/> structure. The range of this
        /// component is 0 (fully transparent) to 1 (fully opaque) inclusive.
        /// </summary>
        /// <seealso cref="this[Int32]"/>
        public float Alpha
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the red component of this <see cref="ColorF"/> structure. The range of this
        /// component is 0 to 1 inclusive.
        /// </summary>
        /// <seealso cref="Green"/>
        /// <seealso cref="Blue"/>
        /// <seealso cref="this[Int32]"/>
        public float Red
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the green component of this <see cref="ColorF"/> structure. The range of this
        /// component is 0 to 1 inclusive.
        /// </summary>
        /// <seealso cref="Red"/>
        /// <seealso cref="Blue"/>
        /// <seealso cref="this[Int32]"/>
        public float Green
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the blue component of this <see cref="ColorF"/> structure. The range of this
        /// component is 0 to 1 inclusive.
        /// </summary>
        /// <seealso cref="Red"/>
        /// <seealso cref="Green"/>
        /// <seealso cref="this[Int32]"/>
        public float Blue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the component of this <see cref="ColorF"/> structure at the specified
        /// index. The range of each component is 0 to 1 inclusive.
        /// </summary>
        /// <param name="index">
        /// One of <see cref="AlphaIndex"/>, <see cref="RedIndex"/>,
        /// <see cref="GreenIndex"/>, or <see cref="BlueIndex"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> is not one of <see cref="AlphaIndex"/>, <see cref="RedIndex"/>,
        /// <see cref="GreenIndex"/>, or <see cref="BlueIndex"/>.
        /// </exception>
        /// <seealso cref="Alpha"/>
        /// <seealso cref="Red"/>
        /// <seealso cref="Green"/>
        /// <seealso cref="Blue"/>
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
                Debug.Assert(false);
                return 0;
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
                Debug.Assert(false);
                return;
                }
            }
        }

        /// <summary>
        /// Gets the cyan component of this <see cref="ColorF"/> structure. The range of this
        /// component is 0 to 1 inclusive.
        /// </summary>
        /// <seealso cref="Magenta"/>
        /// <seealso cref="Yellow"/>
        public float Cyan => 1 - Red;

        /// <summary>
        /// Gets the magenta component of this <see cref="ColorF"/> structure. The range of this
        /// component is 0 to 1 inclusive.
        /// </summary>
        /// <seealso cref="Cyan"/>
        /// <seealso cref="Yellow"/>
        public float Magenta => 1 - Green;

        /// <summary>
        /// Gets the yellow component of this <see cref="ColorF"/> structure. The range of this
        /// component is 0 to 1 inclusive.
        /// </summary>
        /// <seealso cref="Cyan"/>
        /// <seealso cref="Magenta"/>
        public float Yellow => 1 - Blue;

        /// <summary>
        /// Gets the maximum of <see cref="Red"/>, <see cref="Green"/>, or <see cref="Blue"/>
        /// of this <see cref="ColorF"/> structure.
        /// </summary>
        /// <seealso cref="Min"/>
        public float Max => MathHelper.Max(Red, Green, Blue);

        /// <summary>
        /// Gets the minimum of <see cref="Red"/>, <see cref="Green"/>, or <see cref="Blue"/>
        /// of this <see cref="ColorF"/> structure.
        /// </summary>
        /// <seealso cref="Max"/>
        public float Min => MathHelper.Min(Red, Green, Blue);

        /// <summary>
        /// Gets the hue of this <see cref="ColorF"/> structure.
        /// Valid values range from 0 inclusive to 1 exclusive.
        /// </summary>
        /// <remarks>
        /// The range of the hue is designed as a color wheel. See <see cref="HueDegrees"/>
        /// for common values.
        /// <para/>
        /// If <see cref="Chroma"/> is zero, then <see cref="Hue"/> is undefined. An undefined
        /// hue is given the value <c>0</c> for simplicity.
        /// </remarks>
        /// <seealso cref="HueDegrees"/>
        /// <seealso cref="Chroma"/>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public float Hue
        {
            get
            {
                var chroma = Chroma;
                var hue = 0f;

                // gray colors have no hue, but we return 0 for optimization
                if (MathHelper.NearlyEquals(chroma, 0, Tolerance))
                    return hue;

                var max = Max;
                if (max == Red)
                    hue = (Green - Blue) / chroma;
                else if (max == Green)
                    hue = (Blue - Red) / chroma + 2;
                else // if (max == color.Blue)
                    hue = (Red - Green) / chroma + 4;

                if (hue < 0)
                    hue += 6;
                return hue / 6;
            }
        }

        /// <summary>
        /// Gets <see cref="Hue"/> scaled from 0 to 360.
        /// </summary>
        /// <remarks>
        /// The range of the hue is designed as a color wheel. Common values are given below:
        /// <para/>
        /// <list type="table">
        /// <listheader>
        /// <term>Color</term>
        /// <term>Hue</term>
        /// </listheader>
        /// <item>
        /// <description>Red</description>
        /// <description>0</description>
        /// </item>
        /// <item>
        /// <description>Yellow</description>
        /// <description>60</description>
        /// </item>
        /// <item>
        /// <description>Green</description>
        /// <description>120</description>
        /// </item>
        /// <item>
        /// <description>Cyan</description>
        /// <description>180</description>
        /// </item>
        /// <item>
        /// <description>Blue</description>
        /// <description>240</description>
        /// </item>
        /// <item>
        /// <description>Magenta</description>
        /// <description>300</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="Hue"/>
        /// <seealso cref="Chroma"/>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public float HueDegrees => 360 * Hue;

        /// <summary>
        /// Gets the saturation of this <see cref="ColorF"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        /// <remarks>
        /// If <see cref="Chroma"/> is zero, then <see cref="Saturation"/> is undefined. An undefined
        /// saturation is given the value <c>0</c> for simplicity.</remarks>
        /// <seealso cref="Chroma"/>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public float Saturation
        {
            get
            {
                // This number is undefined, but we return 0 for optimization.
                if (MathHelper.NearlyEquals(Chroma, 0, Tolerance))
                    return 0;

                return Chroma / (1 - Math.Abs(2 * Lightness - 1));
            }
        }

        /// <summary>
        /// Gets the lightness of this <see cref="ColorF"/> structure.
        /// </summary>
        /// <remarks>
        /// The lightness of a color is defined as the average of <see cref="Max"/> and <see cref="Min"/>.
        /// </remarks>
        /// <seealso cref="Max"/>
        /// <seealso cref="Min"/>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public float Lightness => (Max + Min) / 2;

        /// <summary>
        /// Gets the luma of this <see cref="ColorF"/> structure.
        /// </summary>
        /// <remarks>
        /// Luma is defined as the gamma-corrected, weighted sum of the red, green, and blue components
        /// of a color.
        /// </remarks>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        /// <seealso href="https://en.wikipedia.org/wiki/Luma_(video)">
        /// Wikipedia article of Luma
        /// </seealso>
        public float Luma => LumaRed * Red + LumaGreen * Green + LumaBlue * Blue;

        /// <summary>
        /// Gets the chroma of this <see cref="ColorF"/> structure.
        /// </summary>
        /// <remarks>
        /// The chroma of a <see cref="ColorF"/> is defined as the difference between <see cref="Max"/> and
        /// <see cref="Min"/>.
        /// </remarks>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public float Chroma => Max - Min;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorF"/> struct.
        /// </summary>
        /// <param name="alpha">
        /// The alpha component of this <see cref="ColorF"/>.
        /// </param>
        /// <param name="red">
        /// The red component of this <see cref="ColorF"/>.
        /// </param>
        /// <param name="green">
        /// The green component of this <see cref="ColorF"/>.
        /// </param>
        /// <param name="blue">
        /// The blue component of this <see cref="ColorF"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// One of the given components has value <see cref="Single.NaN"/>.
        /// </exception>
        /// <remarks>
        /// If any of the given components are outside of the inclusive range 0 to 1, then the
        /// component is set to the nearest value in that range.
        /// <para/>
        /// None of the components can be <see cref="Single.NaN"/> because it is not within the
        /// range 0 to 1, and its proximity to the range cannot be calculated using the standard
        /// distance metric.
        /// </remarks>
        private ColorF(float alpha, float red, float green, float blue)
        {
            Debug.Assert(!Single.IsNaN(alpha));
            Debug.Assert(!Single.IsNaN(red));
            Debug.Assert(!Single.IsNaN(green));
            Debug.Assert(!Single.IsNaN(blue));

            Alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            Red = MathHelper.Clamp(red, 0, 1, Tolerance);
            Green = MathHelper.Clamp(green, 0, 1, Tolerance);
            Blue = MathHelper.Clamp(blue, 0, 1, Tolerance);
        }

        public ColorF BlendWith(ColorF bottom, BlendMode blendMode)
        {
            switch (blendMode)
            {
            case BlendMode.Alpha:
            return AlphaBlend(bottom);

            case BlendMode.Multiply:
            return Multiply(bottom);

            case BlendMode.Divide:
            return Divide(bottom);

            case BlendMode.Screen:
            return ScreenBlend(bottom);

            case BlendMode.Overlay:
            return Overlay(bottom);

            case BlendMode.HardLight:
            return HardLightBlend(bottom);

            case BlendMode.SoftLight:
            return SoftLightBlend(bottom);

            case BlendMode.ColorDodge:
            return ColorDodge(bottom);

            case BlendMode.LinearDodge:
            return LinearDodge(bottom);

            case BlendMode.ColorBurn:
            return ColorBurn(bottom);

            case BlendMode.LinearBurn:
            return LinearBurn(bottom);

            case BlendMode.Difference:
            return Difference(bottom);

            case BlendMode.Darken:
            return Darken(bottom);

            case BlendMode.Lighten:
            return Lighten(bottom);

            case BlendMode.VividLight:
            return VividLightBlend(bottom);

            case BlendMode.LinearLight:
            return LinearLightBlend(bottom);

            case BlendMode.DarkerColor:
            return DarkerColorBlend(bottom);

            case BlendMode.LighterColor:
            return LighterColorBlend(bottom);

            case BlendMode.Hue:
            return HueBlend(bottom);

            case BlendMode.Saturation:
            return SaturationBlend(bottom);

            case BlendMode.Luminosity:
            return LuminosityBlend(bottom);

            default:
            return Empty;
            }
        }

        /// <summary>
        /// Alpha blends this <see cref="ColorF"/> over another <see cref="ColorF"/>.
        /// </summary>
        /// <param name="bottom">
        /// The <see cref="ColorF"/> structure that acts as the bottom layer to blend over.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> that blends this color over <paramref name="bottom"/>.
        /// </returns>
        public ColorF AlphaBlend(ColorF bottom)
        {
            if (MathHelper.NearlyEquals(Alpha, 0, Tolerance))
                return bottom;
            if (MathHelper.NearlyEquals(Alpha, 1, Tolerance))
                return this;

            var result = Empty;
            result.Alpha = Alpha + bottom.Alpha * (1 - Alpha);
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                result[i] = this[i] * Alpha + bottom[i] * bottom.Alpha * (1 - Alpha);
                result[i] /= result.Alpha;
            }
            return result;
        }

        public ColorF Grayscale()
        {
            const float scale = 1 / 3f;
            return Grayscale(FromArgb(0, scale, scale, scale));
        }

        public ColorF Grayscale(ColorF bottom)
        {
            var product = Red * bottom.Red + Green * bottom.Green + Blue * bottom.Blue;
            var color = FromArgb(Alpha, product, product, product);
            return color & bottom;
        }

        public ColorF Multiply(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
                color[i] = this[i] * bottom[i];
            return color & bottom;
        }

        public ColorF Divide(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] == 0)
                    color[i] = 1;
                else
                    color[i] = Math.Min(bottom[i] / this[i], 1);
            }
            return color & bottom;
        }

        public ColorF ScreenBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
                color[i] = 1 - (1 - this[i]) * (1 - bottom[i]);
            return color & bottom;
        }

        public ColorF Overlay(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (bottom[i] < 0.5f)
                    color[i] = 2 * bottom[i] * this[i];
                else
                    color[i] = 1 - 2 * (1 - bottom[i]) * (1 - this[i]);
            }
            return color & bottom;
        }

        public ColorF HardLightBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (bottom[i] > 0.5f)
                    color[i] = 2 * bottom[i] * this[i];
                else
                    color[i] = 1 - 2 * (1 - bottom[i]) * (1 - this[i]);
            }
            return color & bottom;
        }

        public ColorF SoftLightBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (bottom[i] < 0.5f)
                    color[i] = (2 * bottom[i] * this[i]) + (bottom[i] * bottom[i] * (1 - 2 * this[i]));
                else
                    color[i] = (2 * bottom[i] * (1 - this[i])) + ((float)Math.Sqrt(bottom[i]) * (2 * this[i] - 1));
            }
            return color & bottom;
        }

        public ColorF ColorDodge(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] < 1)
                    color[i] = Math.Min(bottom[i] / (1 - this[i]), 1);
                else
                    color[i] = 1;
            }
            return color & bottom;
        }

        public ColorF LinearDodge(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
                color[i] = Math.Min(this[i] + bottom[i], 1);
            return color & bottom;
        }

        public ColorF ColorBurn(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] > 0)
                    color[i] = Math.Max(1 - ((1 - bottom[i]) / this[i]), 0);
                else
                    color[i] = 0;
            }
            return color & bottom;
        }

        public ColorF LinearBurn(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
                color[i] = Math.Max(this[i] + bottom[i] - 1, 0);
            return color & bottom;
        }

        public ColorF Difference(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] > bottom[i])
                    color[i] = this[i] - bottom[i];
                else
                    color[i] = bottom[i] - this[i];
            }
            return color & bottom;
        }

        public ColorF Darken(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
                color[i] = Math.Min(this[i], bottom[i]);
            return color & bottom;
        }

        public ColorF Lighten(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
                color[i] = Math.Max(this[i], bottom[i]);
            return color & bottom;
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
                        color[i] = Math.Min(bottom[i] / (1 - this[i]), 1);
                    else
                        color[i] = 1;
                }
                else if (this[i] > 0)
                    color[i] = Math.Max(1 - (1 - bottom[i]) / this[i], 0);
                else
                    color[i] = 0;
            }
            return color & bottom;
        }

        public ColorF LinearLightBlend(ColorF bottom)
        {
            var color = Empty;
            color.Alpha = Alpha;
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                color[i] = MathHelper.Clamp(2 * this[i] + bottom[i] - 1, 0, 1);
            }
            return color & bottom;
        }

        public ColorF DarkerColorBlend(ColorF bottom) =>
            Luma < bottom.Luma ? this : bottom;

        public ColorF LighterColorBlend(ColorF bottom) =>
            Luma > bottom.Luma ? this : bottom;

        public ColorF RotateHue(ColorF bottom)
        {
            var hue = Hue + bottom.Hue;
            while (hue >= 1)
                hue -= 1;
            while (hue < 0)
                hue += 1;
            var color = FromHcy(Alpha, hue, bottom.Chroma, bottom.Luma);
            return color & bottom;
        }

        public ColorF HueBlend(ColorF bottom)
        {
            var color = FromHcy(Alpha, Hue, bottom.Chroma, bottom.Luma);
            return color & bottom;
        }

        public ColorF SaturationBlend(ColorF bottom)
        {
            var color = FromHcy(Alpha, bottom.Hue, Chroma, bottom.Luma);
            return color & bottom;
        }

        public ColorF LuminosityBlend(ColorF bottom)
        {
            var color = FromHcy(Alpha, bottom.Hue, bottom.Chroma, Luma);
            return color & bottom;
        }

        /// <summary>
        /// Inverts the red, green, and blue color components.
        /// </summary>
        /// <returns>
        /// A <see cref="ColorF"/> structure with the same alpha component as this <see cref="ColorF"/>,
        /// but with inverted red, green, and blue values.
        /// </returns>
        public ColorF Invert() =>
            new ColorF(Alpha, 1 - Red, 1 - Green, 1 - Blue);

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (red, green,
        /// and blue). The alpha value is implicitly 1 (fully opaque).
        /// </summary>
        /// <param name="red">
        /// The red component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="green">
        /// The green component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="blue">
        /// The blue component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <returns>
        /// A <see cref="ColorF"/> that describes the specified components.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// One of the given components has value <see cref="Single.NaN"/>.
        /// </exception>
        /// <remarks>
        /// If any of the given components are outside of the inclusive range 0 to 1, then the
        /// component is set to the nearest value in that range.
        /// <para/>
        /// None of the components can be <see cref="Single.NaN"/> because it is not within the
        /// range 0 to 1, and its proximity to the range cannot be calculated using the standard
        /// distance metric.
        /// </remarks>
        /// <overloads>
        /// Initializes a new instance of the <see cref="ColorF"/> structure.
        /// </overloads>
        public static ColorF FromArgb(float red, float green, float blue) =>
            FromArgb(1, red, green, blue);

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified <see cref="ColorF"/> and a
        /// new alpha value
        /// </summary>
        /// <param name="alpha">
        /// The alpha component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="color">
        /// The base <see cref="ColorF"/> value.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        public static ColorF FromArgb(float alpha, ColorF color) =>
            FromArgb(alpha, color.Red, color.Green, color.Green);

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (alpha, red, green,
        /// and blue)
        /// </summary>
        /// <param name="alpha">
        /// The alpha component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="red">
        /// The red component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="green">
        /// The green component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="blue">
        /// The blue component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        public static ColorF FromArgb(float alpha, float red, float green, float blue)
        {
            if (Single.IsNaN(alpha))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            if (Single.IsNaN(red))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(red)), nameof(red));
            if (Single.IsNaN(green))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(green)), nameof(green));
            if (Single.IsNaN(blue))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(blue)), nameof(blue));

            alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            red = MathHelper.Clamp(red, 0, 1, Tolerance);
            green = MathHelper.Clamp(green, 0, 1, Tolerance);
            blue = MathHelper.Clamp(blue, 0, 1, Tolerance);

            return new ColorF(alpha, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (cyan, magenta,
        /// and yellow). The alpha value is implicitly 1 (fully opaque).
        /// </summary>
        /// <param name="cyan">
        /// The cyan component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="magenta">
        /// The magenta component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="yellow">
        /// The yellow component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        /// <overloads>
        /// Initializes a new instance of the <see cref="ColorF"/> structure.
        /// </overloads>
        public static ColorF FromCmy(float cyan, float magenta, float yellow) =>
            FromCmy(1, cyan, magenta, yellow);

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (alpha, cyan, magenta,
        /// and yellow).
        /// </summary>
        /// <param name="alpha">
        /// The alpha component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="cyan">
        /// The cyan component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="magenta">
        /// The magenta component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="yellow">
        /// The yellow component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        public static ColorF FromCmy(float alpha, float cyan, float magenta, float yellow)
        {
            if (Single.IsNaN(alpha))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            if (Single.IsNaN(cyan))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(cyan)), nameof(cyan));
            if (Single.IsNaN(magenta))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(magenta)), nameof(magenta));
            if (Single.IsNaN(yellow))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(yellow)), nameof(yellow));

            alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            cyan = MathHelper.Clamp(cyan, 0, 1, Tolerance);
            magenta = MathHelper.Clamp(magenta, 0, 1, Tolerance);
            yellow = MathHelper.Clamp(yellow, 0, 1, Tolerance);

            var red = 1 - cyan;
            var green = 1 - magenta;
            var blue = 1 - yellow;

            return new ColorF(alpha, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (cyan, magenta,
        /// yellow, and black). The alpha value is implicitly 1 (fully opaque).
        /// </summary>
        /// <param name="cyan">
        /// The cyan component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="magenta">
        /// The magenta component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="yellow">
        /// The yellow component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="black">
        /// The yellow component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        /// <overloads>
        /// Initializes a new instance of the <see cref="ColorF"/> structure.
        /// </overloads>
        public static ColorF FromCmyk(float cyan, float magenta, float yellow, float black) =>
            FromCmyk(1, cyan, magenta, yellow, black);

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (alpha, cyan, magenta,
        /// yellow, and black).
        /// </summary>
        /// <param name="alpha">
        /// The alpha component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="cyan">
        /// The cyan component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="magenta">
        /// The magenta component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="yellow">
        /// The yellow component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="black">
        /// The yellow component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        public static ColorF FromCmyk(float alpha, float cyan, float magenta, float yellow, float black)
        {
            if (Single.IsNaN(alpha))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            if (Single.IsNaN(cyan))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(cyan)), nameof(cyan));
            if (Single.IsNaN(magenta))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(magenta)), nameof(magenta));
            if (Single.IsNaN(yellow))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(yellow)), nameof(yellow));
            if (Single.IsNaN(black))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(black)), nameof(black));

            alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            cyan = MathHelper.Clamp(cyan, 0, 1, Tolerance);
            magenta = MathHelper.Clamp(magenta, 0, 1, Tolerance);
            yellow = MathHelper.Clamp(yellow, 0, 1, Tolerance);
            black = MathHelper.Clamp(black, 0, 1, Tolerance);

            var white = 1 - black;
            var red = white * (1 - cyan);
            var green = white * (1 - magenta);
            var blue = white * (1 - yellow);

            return new ColorF(alpha, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (hue, chroma,
        /// and luma). The alpha value is implicitly 1 (fully opaque).
        /// </summary>
        /// <param name="hue">
        /// The hue component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="chroma">
        /// The chroma component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="luma">
        /// The luma component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        /// <overloads>
        /// Initializes a new instance of the <see cref="ColorF"/> structure.
        /// </overloads>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public static ColorF FromHcy(float hue, float chroma, float luma) =>
            FromHcy(1, hue, chroma, luma);

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (alpha, hue, chroma,
        /// and luma).
        /// </summary>
        /// <param name="alpha">
        /// The alpha component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="hue">
        /// The hue component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="chroma">
        /// The chroma component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="luma">
        /// The luma component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public static ColorF FromHcy(float alpha, float hue, float chroma, float luma)
        {
            if (Single.IsNaN(alpha))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            if (Single.IsNaN(hue))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            if (Single.IsNaN(chroma))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(chroma)), nameof(chroma));
            if (Single.IsNaN(luma))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(luma)), nameof(luma));

            alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            hue = MathHelper.Clamp(hue, 0, 1, Tolerance);
            chroma = MathHelper.Clamp(chroma, 0, 1, Tolerance);
            luma = MathHelper.Clamp(luma, 0, 1, Tolerance);

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
                else // if (hue >= 5 && hue < 6)
                {
                    color.Red = chroma;
                    color.Blue = chroma * (6 - hue);
                }
            }
            var match = luma - color.Luma;

            return new ColorF(alpha, match + color.Red, match + color.Green, match + color.Blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (hue, saturation,
        /// and lightness). The alpha value is implicitly 1 (fully opaque).
        /// </summary>
        /// <param name="hue">
        /// The hue component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="saturation">
        /// The saturation component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="lightness">
        /// The lightness component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        /// <overloads>
        /// Initializes a new instance of the <see cref="ColorF"/> structure.
        /// </overloads>
        /// <seealso cref="Hue"/>
        /// <seealso cref="Saturation"/>
        /// <seealso cref="Lightness"/>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public static ColorF FromHsl(float hue, float saturation, float lightness) =>
            FromHsl(1, hue, saturation, lightness);

        /// <summary>
        /// Creates a <see cref="ColorF"/> structure from the specified color values (alpha, hue, chroma,
        /// and luma).
        /// </summary>
        /// <param name="alpha">
        /// The alpha component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="hue">
        /// The hue component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="saturation">
        /// The saturation component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <param name="lightness">
        /// The lightness component value for the new <see cref="ColorF"/>. Cannot be <see cref="Single.NaN"/>.
        /// </param>
        /// <inheritdoc cref="FromArgb(Single, Single, Single)" select="returns|exception|remarks"/>
        /// <seealso cref="Hue"/>
        /// <seealso cref="Saturation"/>
        /// <seealso cref="Lightness"/>
        /// <seealso href="https://en.wikipedia.org/wiki/HSL_and_HSV">
        /// Wikipedia article on HSL and HSV
        /// </seealso>
        public static ColorF FromHsl(float alpha, float hue, float saturation, float lightness)
        {
            if (Single.IsNaN(alpha))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            if (Single.IsNaN(hue))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            if (Single.IsNaN(saturation))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(saturation)), nameof(saturation));
            if (Single.IsNaN(lightness))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(lightness)), nameof(lightness));

            alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            hue = MathHelper.Clamp(hue, 0, 1, Tolerance);
            saturation = MathHelper.Clamp(saturation, 0, 1, Tolerance);
            lightness = MathHelper.Clamp(lightness, 0, 1, Tolerance);

            hue *= 6;
            var chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
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
                else // if (hue >= 5 && hue < 6)
                {
                    red = chroma;
                    blue = chroma * (6 - hue);
                }
            }
            var match = lightness - 0.5f * chroma;

            return new ColorF(alpha, red + match, green + match, blue + match);
        }

        /// <inheritdoc cref="Invert"/>
        public static ColorF operator ~(ColorF color) =>
            color.Invert();

        public static ColorF operator *(ColorF left, ColorF right) =>
            left.Multiply(right);

        public static ColorF operator &(ColorF left, ColorF right)
        {
            if (MathHelper.NearlyEquals(left.Alpha, 0, Tolerance))
                return right;
            if (MathHelper.NearlyEquals(left.Alpha, 1, Tolerance))
                return left;

            var result = Empty;
            result.Alpha = left.Alpha + right.Alpha * (1 - left.Alpha);
            for (var i = NumberOfColorChannels; --i >= 0;)
            {
                result[i] = left[i] * left.Alpha + right[i] * right.Alpha * (1 - left.Alpha);
                result[i] /= result.Alpha;
            }
            return result;
        }

        /// <summary>
        /// Compares two <see cref="ColorF"/> objects. The result specifies whether
        /// the <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> channels are all equal within <see cref="Tolerance"/>.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorF"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorF"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/> have equal
        /// <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> channels within <see cref="Tolerance"/>; otherwise false.
        /// </returns>
        public static bool operator ==(ColorF left, ColorF right)
        {
            for (var i = NumberOfChannels; --i >= 0;)
                if (!MathHelper.NearlyEquals(left[i], right[i], Tolerance))
                    return false;
            return true;
        }

        /// <summary>
        /// Compares two <see cref="ColorF"/> objects. The result specifies whether
        /// the <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> channels are all equal within <see cref="Tolerance"/>.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorF"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorF"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/> have equal
        /// <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> channels within <see cref="Tolerance"/>; otherwise false.
        /// </returns>
        public static bool operator !=(ColorF left, ColorF right) =>
            !(left == right);

        /// <summary>
        /// Specifies whether this <see cref="ColorF"/> is the same color as
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj"/> is a <see cref="ColorF"/> structure and is the same
        /// color as this <see cref="ColorF"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorF))
                return false;

            return (ColorF)obj == this;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorF"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="ColorF"/>.
        /// </returns>
        /// <remarks>
        /// The hash code for this <see cref="ColorF"/> is equal to the hash code for its
        /// <see cref="Color"/> equivalent.
        /// </remarks>
        public override int GetHashCode()
        {
            var result = 0;

            for (var i = 0; i < NumberOfChannels; i++)
            {
                var channel = (int)Math.Round(this[i] * Byte.MaxValue);

                result |= channel << (8 << i);
            }

            return result;
        }

        /// <summary>
        /// Converts this <see cref="ColorF"/> to a human-readable <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> the represent this <see cref="ColorF"/>.
        /// </returns>
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
