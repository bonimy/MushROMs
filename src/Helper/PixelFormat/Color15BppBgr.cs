// <copyright file="Color15BppBgr.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Text;
using static System.Math;

namespace Helper.PixelFormats
{
    public unsafe struct Color15BppBgr
    {
        public const int SizeOf = sizeof(short);

        public static readonly Color15BppBgr Empty = new Color15BppBgr();

        private const int NumberOfChannels = 3;

        public const int RedIndex = 0;

        public const int GreenIndex = 1;

        public const int BlueIndex = 2;

        private const int BitsPerChannel = 5;

        private const int RedShift = BitsPerChannel * RedIndex;

        private const int GreenShift = BitsPerChannel * GreenIndex;

        private const int BlueShift = BitsPerChannel * BlueIndex;

        private const int ChannelMask = (1 << BitsPerChannel) - 1;

        private const int RedMask = ChannelMask << RedShift;

        private const int GreenMask = ChannelMask << GreenShift;

        private const int BlueMask = ChannelMask << BlueShift;

        private const int ColorMask = RedMask | GreenMask | BlueMask;

        private ushort _value;

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
                return (byte)(Value >> 8);
            }

            set
            {
                Value &= 0x00FF;
                Value |= (ushort)(value << 8);
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
                Value &= 0xFF00;
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
                            SR.ErrorArrayBounds(nameof(index), index, NumberOfChannels));
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
                            SR.ErrorArrayBounds(nameof(index), index, NumberOfChannels));
                }
            }
        }

        private Color15BppBgr(ushort value)
        {
            _value = value;
        }

        public Color15BppBgr(byte low, byte high) :
            this((ushort)(low | (high << 8)))
        {
        }

        public Color15BppBgr(int red, int green, int blue)
        {
            _value = (ushort)(
                ((red & ChannelMask) << RedShift) |
                ((green & ChannelMask) << GreenShift) |
                ((blue & ChannelMask) << BlueShift));
        }

        public static implicit operator Color15BppBgr(int value)
        {
            return new Color15BppBgr((ushort)value);
        }

        public static implicit operator int(Color15BppBgr color15)
        {
            return color15.Value;
        }

        public static explicit operator Color15BppBgr(Color24BppRgb color24)
        {
            // Each component goes from 8 bits of sensitivity to 5
            // So we shift right 3 bytes for the conversion.
            return new Color15BppBgr(
                color24.Red >> (8 - BitsPerChannel),
                color24.Green >> (8 - BitsPerChannel),
                color24.Blue >> (8 - BitsPerChannel));
        }

        public static implicit operator Color24BppRgb(Color15BppBgr color15)
        {
            // Each component goes from 5 bits of sensitivity to 8
            // So we shift left 3 bytes for the conversion.
            return new Color24BppRgb(
                color15.Red << (8 - BitsPerChannel),
                color15.Green << (8 - BitsPerChannel),
                color15.Blue << (8 - BitsPerChannel));
        }

        public static explicit operator Color15BppBgr(Color32BppArgb color32)
        {
            // Same as the 24-bit color conversion; we ignore the alpha component.
            return new Color15BppBgr(
                color32.Red >> (8 - BitsPerChannel),
                color32.Green >> (8 - BitsPerChannel),
                color32.Blue >> (8 - BitsPerChannel));
        }

        public static implicit operator Color32BppArgb(Color15BppBgr color15)
        {
            // Same as the 24-bit color conversion; we ignore the alpha component.
            return new Color32BppArgb(
                color15.Red << (8 - BitsPerChannel),
                color15.Green << (8 - BitsPerChannel),
                color15.Blue << (8 - BitsPerChannel));
        }

        public static bool operator ==(Color15BppBgr left, Color15BppBgr right)
        {
            // We compare the proper values because we do not care about
            // the most significant bit.
            return left.ProperValue == right.ProperValue;
        }

        public static bool operator !=(Color15BppBgr left, Color15BppBgr right)
        {
            return !(left == right);
        }

        public static explicit operator Color15BppBgr(ColorF color)
        {
            return new Color15BppBgr(
                (int)Round(color.Red * ChannelMask),
                (int)Round(color.Green * ChannelMask),
                (int)Round(color.Blue * ChannelMask));
        }

        public static implicit operator ColorF(Color15BppBgr pixel)
        {
            return ColorF.FromArgb(
                pixel.Red / (float)ChannelMask,
                pixel.Green / (float)ChannelMask,
                pixel.Blue / (float)ChannelMask);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color15BppBgr value)
            {
                return value == this;
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
