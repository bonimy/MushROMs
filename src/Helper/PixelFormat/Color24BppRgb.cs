// <copyright file="Color24BppRgb.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Text;
using static System.Math;

namespace Helper.PixelFormats
{
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public unsafe struct Color24BppRgb
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

        [FieldOffset(0)]
        private fixed byte _components[SizeOf];

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

        public int Value
        {
            get
            {
                return (Red << RedShift) |
                   (Green << GreenShift) |
                   (Blue << BlueShift);
            }

            set
            {
                this = new Color24BppRgb(value);
            }
        }

        private Color24BppRgb(int value)
            : this(
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

        public static explicit operator Color24BppRgb(ColorF color)
        {
            return new Color24BppRgb(
                (int)Round(color.Red * Byte.MaxValue),
                (int)Round(color.Green * Byte.MaxValue),
                (int)Round(color.Blue * Byte.MaxValue));
        }

        public static implicit operator ColorF(Color24BppRgb pixel)
        {
            return ColorF.FromArgb(
                pixel.Red / (float)Byte.MaxValue,
                pixel.Green / (float)Byte.MaxValue,
                pixel.Blue / (float)Byte.MaxValue);
        }

        public static bool operator ==(Color24BppRgb left, Color24BppRgb right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(Color24BppRgb left, Color24BppRgb right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color24BppRgb value)
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
