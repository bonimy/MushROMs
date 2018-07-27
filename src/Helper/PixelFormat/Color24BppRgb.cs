// <copyright file="Color24BppRgb.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper.PixelFormat
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using static StringHelper;
    using static System.Math;

    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public struct Color24BppRgb : IEquatable<Color24BppRgb>
    {
        public const int SizeOf = 3 * sizeof(byte);

        public const int RedIndex = 2;

        public const int GreenIndex = 1;

        public const int BlueIndex = 0;

        public static readonly Color24BppRgb Empty = default;

        internal const int BitsPerRed = BitsPerChannel;

        internal const int BitsPerGreen = BitsPerChannel;

        internal const int BitsPerBlue = BitsPerChannel;

        internal const int BitsPerChannel = 8;

        private const int RedShift = BitsPerRed * RedIndex;

        private const int GreenShift = BitsPerGreen * GreenIndex;

        private const int BlueShift = BitsPerBlue * BlueIndex;

        [FieldOffset(RedIndex)]
        private byte _red;

        [FieldOffset(GreenIndex)]
        private byte _green;

        [FieldOffset(BlueIndex)]
        private byte _blue;

        public Color24BppRgb(int red, int green, int blue)
        {
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        private Color24BppRgb(int value)
            : this(
            value >> RedShift,
            value >> GreenShift,
            value >> BlueShift)
        {
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

        public static explicit operator Color24BppRgb(int value)
        {
            return new Color24BppRgb(value);
        }

        public static implicit operator int(Color24BppRgb color24)
        {
            return color24.Value;
        }

        public static explicit operator Color24BppRgb(
            Color32BppArgb color32)
        {
            return new Color24BppRgb(color32.Value);
        }

        public static implicit operator Color32BppArgb(
            Color24BppRgb color24)
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

        public static bool operator ==(
            Color24BppRgb left,
            Color24BppRgb right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            Color24BppRgb left,
            Color24BppRgb right)
        {
            return !(left == right);
        }

        public bool Equals(Color24BppRgb obj)
        {
            return
                Red.Equals(obj.Red) &&
                Green.Equals(obj.Green) &&
                Blue.Equals(obj.Blue);
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
            return Value;
        }

        public override string ToString()
        {
            return GetString(
                "{{R:{0},G:{1},B:{2}}}",
                Red,
                Green,
                Blue);
        }
    }
}
