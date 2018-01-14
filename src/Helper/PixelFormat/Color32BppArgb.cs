// <copyright file="Color32BppArgb.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Text;
using static System.Math;

namespace Helper.PixelFormats
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Color32BppArgb
    {
        public const int SizeOf = sizeof(int);

        public static readonly Color32BppArgb Empty = new Color32BppArgb();

        public const int AlphaIndex = 3;

        public const int RedIndex = 2;

        public const int GreenIndex = 1;

        public const int BlueIndex = 0;

        private const int BitsPerChannel = 8;

        internal const int BitsPerAlpha = BitsPerChannel;

        internal const int BitsPerRed = BitsPerChannel;

        internal const int BitsPerGreen = BitsPerChannel;

        internal const int BitsPerBlue = BitsPerChannel;

        [FieldOffset(0)]
        private int _value;

        [FieldOffset(0)]
        private fixed byte _components[SizeOf];

        [FieldOffset(AlphaIndex)]
        private byte _alpha;

        [FieldOffset(RedIndex)]
        private byte _red;

        [FieldOffset(GreenIndex)]
        private byte _green;

        [FieldOffset(BlueIndex)]
        private byte _blue;

        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public byte Alpha
        {
            get
            {
                return _alpha;
            }

            set
            {
                _alpha = value;
            }
        }

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

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= SizeOf)
                {
                    SR.ErrorArrayBounds(nameof(index), index, SizeOf);
                }

                fixed (byte* components = _components)
                    return components[index];
            }

            set
            {
                if (index < 0 || index >= SizeOf)
                {
                    SR.ErrorArrayBounds(nameof(index), index, SizeOf);
                }

                fixed (byte* components = _components)
                    components[index] = value;
            }
        }

        private Color32BppArgb(int value)
            : this()
        {
            _value = value;
        }

        public Color32BppArgb(int red, int green, int blue) : this(Byte.MaxValue, red, green, blue)
        {
        }

        public Color32BppArgb(int alpha, int red, int green, int blue)
            : this()
        {
            _alpha = (byte)alpha;
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        public static implicit operator Color32BppArgb(int value)
        {
            return new Color32BppArgb(value);
        }

        public static implicit operator int(Color32BppArgb color32)
        {
            return color32.Value;
        }

        public static implicit operator Color32BppArgb(ColorF color)
        {
            return new Color32BppArgb(
                (int)Round(color.Alpha * Byte.MaxValue),
                (int)Round(color.Red * Byte.MaxValue),
                (int)Round(color.Green * Byte.MaxValue),
                (int)Round(color.Blue * Byte.MaxValue));
        }

        public static implicit operator ColorF(Color32BppArgb pixel)
        {
            return ColorF.FromArgb(
                pixel.Alpha / (float)Byte.MaxValue,
                pixel.Red / (float)Byte.MaxValue,
                pixel.Green / (float)Byte.MaxValue,
                pixel.Blue / (float)Byte.MaxValue);
        }

        public static bool operator ==(Color32BppArgb left, Color32BppArgb right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(Color32BppArgb left, Color32BppArgb right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color32BppArgb value)
            {
                return value == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value;
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
