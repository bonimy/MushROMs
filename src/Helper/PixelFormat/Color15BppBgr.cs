// <copyright file="Color15BppBgr.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper.PixelFormat
{
    using System;
    using System.Drawing;
    using System.Text;
    using static System.Math;

    public struct Color15BppBgr : IEquatable<Color15BppBgr>
    {
        public const int SizeOf = sizeof(short);

        public const int LowIndex = 0;

        public const int HighIndex = 1;

        public static readonly Color15BppBgr Empty = default;

        private const int BitsPerByte = 8;

        private const int NumberOfChannels = 3;

        private const int RedIndex = 0;

        private const int GreenIndex = 1;

        private const int BlueIndex = 2;

        private const int BitsPerChannel = 5;

        private const int RedShift = BitsPerChannel * RedIndex;

        private const int GreenShift = BitsPerChannel * GreenIndex;

        private const int BlueShift = BitsPerChannel * BlueIndex;

        private const int ChannelMask = (1 << BitsPerChannel) - 1;

        private const int RedMask = ChannelMask << RedShift;

        private const int GreenMask = ChannelMask << GreenShift;

        private const int BlueMask = ChannelMask << BlueShift;

        private const int ColorMask = RedMask | GreenMask | BlueMask;

        private const int HighMask = Byte.MaxValue << (BitsPerByte * HighIndex);

        private const int LowMask = Byte.MaxValue << (BitsPerByte * LowIndex);

        private ushort _value;

        public Color15BppBgr(byte low, byte high)
            : this((ushort)(low | (high << BitsPerByte)))
        {
        }

        public Color15BppBgr(int red, int green, int blue)
        {
            _value = (ushort)(
                ((red & ChannelMask) << RedShift) |
                ((green & ChannelMask) << GreenShift) |
                ((blue & ChannelMask) << BlueShift));
        }

        private Color15BppBgr(ushort value)
        {
            _value = value;
        }

        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = (ushort)value;
            }
        }

        public int ProperValue
        {
            get
            {
                return Value & ColorMask;
            }

            set
            {
                Value &= ~ColorMask;
                Value |= value & ColorMask;
            }
        }

        public byte High
        {
            get
            {
                return (byte)(Value >> BitsPerByte);
            }

            set
            {
                Value &= HighMask;
                Value |= (ushort)(value << BitsPerByte);
            }
        }

        public byte Low
        {
            get
            {
                return (byte)Value;
            }

            set
            {
                Value &= LowMask;
                Value |= value;
            }
        }

        public byte Red
        {
            get
            {
                return (byte)((Value & RedMask) >> RedShift);
            }

            set
            {
                Value &= unchecked((ushort)(~RedMask));
                Value |= (ushort)((value & ChannelMask) << RedShift);
            }
        }

        public byte Green
        {
            get
            {
                return (byte)((Value & GreenMask) >> GreenShift);
            }

            set
            {
                Value &= unchecked((ushort)(~GreenMask));
                Value |= (ushort)((value & ChannelMask) << GreenShift);
            }
        }

        public byte Blue
        {
            get
            {
                return (byte)((Value & BlueMask) >> BlueShift);
            }

            set
            {
                Value &= unchecked((ushort)(~BlueMask));
                Value |= (ushort)((value & ChannelMask) << BlueShift);
            }
        }

        public static explicit operator Color15BppBgr(int value)
        {
            return new Color15BppBgr((ushort)value);
        }

        public static implicit operator int(Color15BppBgr color15)
        {
            return color15.Value;
        }

        public static explicit operator Color15BppBgr(
            Color24BppRgb color24)
        {
            return new Color15BppBgr(
                color24.Red >> (BitsPerByte - BitsPerChannel),
                color24.Green >> (BitsPerByte - BitsPerChannel),
                color24.Blue >> (BitsPerByte - BitsPerChannel));
        }

        public static implicit operator Color24BppRgb(
            Color15BppBgr color15)
        {
            return new Color24BppRgb(
                color15.Red << (BitsPerByte - BitsPerChannel),
                color15.Green << (BitsPerByte - BitsPerChannel),
                color15.Blue << (BitsPerByte - BitsPerChannel));
        }

        public static explicit operator Color15BppBgr(
            Color32BppArgb color32)
        {
            return new Color15BppBgr(
                color32.Red >> (BitsPerByte - BitsPerChannel),
                color32.Green >> (BitsPerByte - BitsPerChannel),
                color32.Blue >> (BitsPerByte - BitsPerChannel));
        }

        public static implicit operator Color32BppArgb(
            Color15BppBgr color15)
        {
            return new Color32BppArgb(
                color15.Red << (BitsPerByte - BitsPerChannel),
                color15.Green << (BitsPerByte - BitsPerChannel),
                color15.Blue << (BitsPerByte - BitsPerChannel));
        }

        public static explicit operator Color15BppBgr(Color color)
        {
            return new Color15BppBgr(
                color.R >> (BitsPerByte - BitsPerChannel),
                color.G >> (BitsPerByte - BitsPerChannel),
                color.B >> (BitsPerByte - BitsPerChannel));
        }

        public static implicit operator Color(Color15BppBgr color15)
        {
            return Color.FromArgb(
                color15.Red << (BitsPerByte - BitsPerChannel),
                color15.Green << (BitsPerByte - BitsPerChannel),
                color15.Blue << (BitsPerByte - BitsPerChannel));
        }

        public static explicit operator Color15BppBgr(ColorF colorF)
        {
            return new Color15BppBgr(
                (int)Round(colorF.Red * ChannelMask),
                (int)Round(colorF.Green * ChannelMask),
                (int)Round(colorF.Blue * ChannelMask));
        }

        public static implicit operator ColorF(Color15BppBgr color15)
        {
            return ColorF.FromArgb(
                color15.Red / (float)ChannelMask,
                color15.Green / (float)ChannelMask,
                color15.Blue / (float)ChannelMask);
        }

        public static bool operator ==(
            Color15BppBgr left,
            Color15BppBgr right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            Color15BppBgr left,
            Color15BppBgr right)
        {
            return !(left == right);
        }

        public bool Equals(Color15BppBgr obj)
        {
            return ProperValue.Equals(obj.ProperValue);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color15BppBgr value)
            {
                return Equals(value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            // We base equality on the proper value, not the base value.
            return ProperValue;
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
