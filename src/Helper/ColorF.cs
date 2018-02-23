// <copyright file="ColorF.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Text;
using Helper.PixelFormats;
using static System.Math;
using static Helper.MathHelper;
using Debug = System.Diagnostics.Debug;

namespace Helper
{
    public partial struct ColorF : IEquatable<ColorF>
    {
        public static readonly ColorF Empty = new ColorF();

        public const float LumaRedWeight = 0.299f;
        public const float LumaGreenWeight = 0.587f;
        public const float LumaBlueWeight = 0.114f;

        public const int NumberOfChannels = 4;
        public const int NumberOfColorChannels = NumberOfChannels - 1;

        public float Alpha
        {
            get;
        }

        public float Red
        {
            get;
        }

        public float Green
        {
            get;
        }

        public float Blue
        {
            get;
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
                // Gray colors have no hue, but we return 0 for optimization.
                if (Chroma == 0)
                {
                    return 0;
                }

                var hue = 0f;
                if (Max == Red)
                {
                    hue = (Green - Blue) / Chroma;
                }
                else if (Max == Green)
                {
                    hue = ((Blue - Red) / Chroma) + 2;
                }
                else if (Max == Blue)
                {
                    hue = ((Red - Green) / Chroma) + 4;
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
                if (Chroma == 0)
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
                    (LumaRedWeight * Red) +
                    (LumaGreenWeight * Green) +
                    (LumaBlueWeight * Blue);
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

            Alpha = Clamp(alpha, 0, 1);
            Red = Clamp(red, 0, 1);
            Green = Clamp(green, 0, 1);
            Blue = Clamp(blue, 0, 1);
        }

        public ColorF Grayscale()
        {
            return Grayscale(this, FromArgb(1, 1, 1));
        }

        public ColorF RotateHue(ColorF bottom)
        {
            var hue = Hue + bottom.Hue;
            var result = FromHcy(Alpha, hue, bottom.Chroma, bottom.Luma);
            return AlphaBlend(result, bottom);
        }

        public ColorF Invert()
        {
            return new ColorF(Alpha, 1 - Red, 1 - Green, 1 - Blue);
        }

        public bool Equals(ColorF obj)
        {
            return
                Alpha.Equals(obj.Alpha) &&
                Red.Equals(obj.Red) &&
                Green.Equals(obj.Green) &&
                Blue.Equals(obj.Blue);
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
            var color = (Color32BppArgb)this;
            return color.GetHashCode();
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

        public static ColorF Negate(ColorF color)
        {
            return color.Invert();
        }

        public static ColorF operator ~(ColorF color)
        {
            return Negate(color);
        }

        public static ColorF Add(ColorF left, ColorF right)
        {
            return LinearDodge(left, right);
        }

        public static ColorF operator +(ColorF left, ColorF right)
        {
            return Add(left, right);
        }

        public static ColorF Subtract(ColorF left, ColorF right)
        {
            return Difference(left, right);
        }

        public static ColorF operator -(ColorF left, ColorF right)
        {
            return Subtract(left, right);
        }

        public static ColorF operator *(ColorF left, ColorF right)
        {
            return Multiply(left, right);
        }

        public static ColorF operator /(ColorF left, ColorF right)
        {
            return Divide(left, right);
        }

        public static bool operator ==(ColorF left, ColorF right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ColorF left, ColorF right)
        {
            return !(left == right);
        }
    }
}
