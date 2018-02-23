// <copyright file="Color32BppArgb.cs" company="Public Domain">
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
    using System.Text;
    using static System.Math;

    [StructLayout(LayoutKind.Explicit)]
    public struct Color32BppArgb : IEquatable<Color32BppArgb>
    {
        public const int SizeOf = sizeof(int);

        public const int AlphaIndex = 3;

        public const int RedIndex = 2;

        public const int GreenIndex = 1;

        public const int BlueIndex = 0;

        public static readonly Color32BppArgb Empty = default;

        internal const int BitsPerAlpha = BitsPerChannel;

        internal const int BitsPerRed = BitsPerChannel;

        internal const int BitsPerGreen = BitsPerChannel;

        internal const int BitsPerBlue = BitsPerChannel;

        private const int BitsPerChannel = 8;

        private const int AlphaShift = BitsPerAlpha * AlphaIndex;

        private const int RedShift = BitsPerRed * RedIndex;

        private const int GreenShift = BitsPerGreen * GreenIndex;

        private const int BlueShift = BitsPerBlue * BlueIndex;

        [FieldOffset(AlphaIndex)]
        private byte _alpha;

        [FieldOffset(RedIndex)]
        private byte _red;

        [FieldOffset(GreenIndex)]
        private byte _green;

        [FieldOffset(BlueIndex)]
        private byte _blue;

        public Color32BppArgb(int red, int green, int blue)
            : this(Byte.MaxValue, red, green, blue)
        {
        }

        public Color32BppArgb(int alpha, int red, int green, int blue)
        {
            _alpha = (byte)alpha;
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        private Color32BppArgb(int value)
            : this(
            value >> AlphaShift,
            value >> RedShift,
            value >> GreenShift,
            value >> BlueShift)
        {
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

        public int Value
        {
            get
            {
                return
                    (Alpha << (BitsPerChannel * AlphaIndex)) |
                    (Red << (BitsPerChannel * RedIndex)) |
                    (Green << (BitsPerChannel * GreenIndex)) |
                    (Blue << (BitsPerChannel * BlueIndex));
            }

            set
            {
                this = new Color32BppArgb(value);
            }
        }

        public static implicit operator Color32BppArgb(int value)
        {
            return new Color32BppArgb(value);
        }

        public static implicit operator int(Color32BppArgb color32)
        {
            return color32.Value;
        }

        public static explicit operator Color32BppArgb(Color color)
        {
            return new Color32BppArgb(
                color.A,
                color.R,
                color.G,
                color.B);
        }

        public static implicit operator Color(Color32BppArgb color32)
        {
            return Color.FromArgb(color32.Value);
        }

        public static explicit operator Color32BppArgb(ColorF colorF)
        {
            return new Color32BppArgb(
                (int)Round(colorF.Alpha * Byte.MaxValue),
                (int)Round(colorF.Red * Byte.MaxValue),
                (int)Round(colorF.Green * Byte.MaxValue),
                (int)Round(colorF.Blue * Byte.MaxValue));
        }

        public static implicit operator ColorF(Color32BppArgb color32)
        {
            return ColorF.FromArgb(
                color32.Alpha / (float)Byte.MaxValue,
                color32.Red / (float)Byte.MaxValue,
                color32.Green / (float)Byte.MaxValue,
                color32.Blue / (float)Byte.MaxValue);
        }

        public static bool operator ==(
            Color32BppArgb left,
            Color32BppArgb right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            Color32BppArgb left,
            Color32BppArgb right)
        {
            return !(left == right);
        }

        public bool Equals(Color32BppArgb obj)
        {
            return Value.Equals(obj.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color32BppArgb value)
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
