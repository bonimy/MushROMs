// <copyright file="Color24BppRgb.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using static System.Math;

namespace Helper.PixelFormats
{
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public struct Color24BppRgb : IEquatable<Color24BppRgb>
    {
        public const int SizeOf = 3 * sizeof(byte);

        public static readonly Color24BppRgb Empty = new Color24BppRgb();

        public const int RedIndex = 2;

        public const int GreenIndex = 1;

        public const int BlueIndex = 0;

        private const int BitsPerChannel = 8;

        internal const int BitsPerRed = BitsPerChannel;

        internal const int BitsPerGreen = BitsPerChannel;

        internal const int BitsPerBlue = BitsPerChannel;

        private const int RedShift = BitsPerChannel * RedIndex;

        private const int GreenShift = BitsPerChannel * GreenIndex;

        private const int BlueShift = BitsPerChannel * BlueIndex;

        [FieldOffset(RedIndex)]
        private byte _red;

        [FieldOffset(GreenIndex)]
        private byte _green;

        [FieldOffset(BlueIndex)]
        private byte _blue;

        public byte Red
        {
            get
            {
                return _red;
            }

            set
            {
                _red = value;
            }
        }

        public byte Green
        {
            get
            {
                return _green;
            }

            set
            {
                _green = value;
            }
        }

        public byte Blue
        {
            get
            {
                return _blue;
            }

            set
            {
                _blue = value;
            }
        }

        public int Value
        {
            get
            {
                return
                    (Red << (BitsPerChannel * RedIndex)) |
                    (Green << (BitsPerChannel * GreenIndex)) |
                    (Blue << (BitsPerChannel * BlueIndex));
            }

            set
            {
                this = new Color24BppRgb(value);
            }
        }

        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case RedIndex:
                        return Red;

                    case GreenIndex:
                        return Green;

                    case BlueIndex:
                        return Blue;

                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(index),
                            index,
                            SR.ErrorArrayBounds(nameof(index), index, SizeOf));
                }
            }

            set
            {
                switch (index)
                {
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
                            index,
                            SR.ErrorArrayBounds(nameof(index), index, SizeOf));
                }
            }
        }

        private Color24BppRgb(int value) : this(
            value >> RedShift,
            value >> GreenShift,
            value >> BlueShift)
        {
        }

        public Color24BppRgb(int red, int green, int blue)
        {
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        public bool Equals(Color24BppRgb obj)
        {
            return Value.Equals(obj.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color24BppRgb value)
            {
                return Equals(value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
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

        public static explicit operator Color24BppRgb(int value)
        {
            return new Color24BppRgb(value);
        }

        public static implicit operator int(Color24BppRgb color24)
        {
            return color24.Value;
        }

        public static explicit operator Color24BppRgb(Color32BppArgb color32)
        {
            return new Color24BppRgb(color32.Value);
        }

        public static implicit operator Color32BppArgb(Color24BppRgb color24)
        {
            return color24.Value;
        }

        public static explicit operator Color24BppRgb(Color color)
        {
            return new Color24BppRgb(
                color.R,
                color.G,
                color.B);
        }

        public static implicit operator Color(Color24BppRgb color24)
        {
            return Color.FromArgb(color24.Value);
        }

        public static explicit operator Color24BppRgb(ColorF colorF)
        {
            return new Color24BppRgb(
                (int)Round(colorF.Red * Byte.MaxValue),
                (int)Round(colorF.Green * Byte.MaxValue),
                (int)Round(colorF.Blue * Byte.MaxValue));
        }

        public static implicit operator ColorF(Color24BppRgb color24)
        {
            return ColorF.FromArgb(
                color24.Red / (float)Byte.MaxValue,
                color24.Green / (float)Byte.MaxValue,
                color24.Blue / (float)Byte.MaxValue);
        }

        public static bool operator ==(Color24BppRgb left, Color24BppRgb right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color24BppRgb left, Color24BppRgb right)
        {
            return !(left == right);
        }
    }
}
