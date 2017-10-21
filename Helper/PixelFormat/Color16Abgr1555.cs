using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Helper.PixelFormats
{
    /// <summary>
    /// Represents a 15-bit RGB color as a <see cref="UInt16"/> with a bit layout of
    /// <c>[a b b b b b g g - g g g r r r r r]</c>. The most
    /// significant bit is ignored and can be either set or cleared.
    /// </summary>
    /// <remarks>
    /// This struct directly converts to and from <see cref="UInt16"/>. Therefore, a pointer of type
    /// <see cref="UInt16"/>* data can be successfully cast to a pointer of type <see cref="Color15BppAbgr1555"/>*.
    /// </remarks>
    /// <threadsafety static="true" instance="false"/>
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Color15BppAbgr1555
    {
        /// <summary>
        /// The size, in bytes, of <see cref="Color15BppAbgr1555"/>.
        /// This field is constant.
        /// </summary>
        public const int SizeOf = sizeof(short);

        /// <summary>
        /// Represents a <see cref="Color15BppAbgr1555"/> that has its <see cref="Value"/>
        /// set to 0.
        /// </summary>
        public static readonly Color15BppAbgr1555 Empty = new Color15BppAbgr1555();

        /// <summary>
        /// The number of bits each component consumes.
        /// This field is constant.
        /// </summary>
        private const int BitsPerChannel = 5;
        /// <summary>
        /// The number of bits the alpha channel consumes.
        /// This field is constant.
        /// </summary>
        private const int BitsPerAlpha = 1;
        /// <summary>
        /// The number of bits the red channel consumes.
        /// This field is constant.
        /// </summary>
        internal const int BitsPerRed = BitsPerChannel;
        /// <summary>
        /// The number of bits the green channel consumes.
        /// This field is constant.
        /// </summary>
        internal const int BitsPerGreen = BitsPerChannel;
        /// <summary>
        /// The number of bits the blue channel consumes.
        /// This field is constant.
        /// </summary>
        internal const int BitsPerBlue = BitsPerChannel;

        /// <summary>
        /// The number of bits the <see cref="Alpha"/> channel is shifted by in the component mask.
        /// This field is constant.
        /// </summary>
        private const int AlphaShift = BitsPerBlue + BitsPerGreen + BitsPerRed;
        /// <summary>
        /// The number of bits the <see cref="Red"/> channel is shifted by in the component mask.
        /// This field is constant.
        /// </summary>
        private const int RedShift = BitsPerBlue + BitsPerGreen;
        /// <summary>
        /// The number of bits the <see cref="Green"/> channel is shifted by in the component mask.
        /// This field is constant.
        /// </summary>
        private const int GreenShift = BitsPerRed;
        /// <summary>
        /// The number of bits the <see cref="Blue"/> channel is shifted by in the component mask.
        /// This field is constant.
        /// </summary>
        private const int BlueShift = 0;

        /// <summary>
        /// The bit-mask for the <see cref="Alpha"/> channel.
        /// This field is constant.
        /// </summary>
        private const int AlphaChannelMask = (1 << BitsPerAlpha) - 1;
        /// <summary>
        /// The bit-mask for the <see cref="Red"/> channel.
        /// This field is constant.
        /// </summary>
        private const int RedChannelMask = (1 << BitsPerRed) - 1;
        /// <summary>
        /// The bit-mask for the <see cref="Green"/> channel.
        /// This field is constant.
        /// </summary>
        private const int GreenChannelMask = (1 << BitsPerGreen) - 1;
        /// <summary>
        /// The bit-mask for the <see cref="Blue"/> channel.
        /// This field is constant.
        /// </summary>
        private const int BlueChannelMask = (1 << BitsPerBlue) - 1;

        /// <summary>
        /// The bit-mask of the <see cref="Alpha"/> component.
        /// This field is constant.
        /// </summary>
        private const int AlphaComponentMask = AlphaChannelMask << AlphaShift;
        /// <summary>
        /// The bit-mask of the <see cref="Red"/> component.
        /// This field is constant.
        /// </summary>
        private const int RedComponentMask = RedChannelMask << RedShift;
        /// <summary>
        /// The bit-mask of the <see cref="Green"/> component.
        /// This field is constant.
        /// </summary>
        private const int GreenComponentMask = GreenChannelMask << GreenShift;
        /// <summary>
        /// The bit-mask of the <see cref="Blue"/> component.
        /// This field is constant.
        /// </summary>
        private const int BlueComponentMask = BlueChannelMask << BlueShift;
        /// <summary>
        /// The bit-mask for the color.
        /// This field is constant.
        /// </summary>
        private const int ColorMask =
            AlphaComponentMask | RedComponentMask | GreenComponentMask | BlueComponentMask;

        /// <summary>
        /// The color value, as a 16-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// This is the fundamental type of this <see cref="Color15BppAbgr1555"/>.
        /// </remarks>
        [FieldOffset(0)]
        private ushort _value;
        /// <summary>
        /// The high <see cref="Byte"/> of <see cref="_value"/>.
        /// </summary>
        [FieldOffset(0)]
        private byte _high;
        /// <summary>
        /// The low <see cref="Byte"/> of <see cref="_value"/>.
        /// </summary>
        [FieldOffset(1)]
        private byte _low;

        /// <summary>
        /// Gets or sets the color value, as a 16-bit unsigned integer. The most
        /// significant bit has no meaning.
        /// </summary>
        /// <remarks>
        /// The most significant bit can be either 0 or 1. It does not affect operations on
        /// this <see cref="Color15BppAbgr1555"/>. However, its value will be preserved through this
        /// object's lifetime.
        /// </remarks>
        public int Value
        {
            get => _value;
            set => _value = (ushort)value;
        }

        /// <summary>
        /// Gets or sets the color value, as a 16-bit unsigned integer. The most significant
        /// bit is always ignored.
        /// </summary>
        public int PreservedValue
        {
            get => Value & ColorMask;
            set
            {
                Value &= ~ColorMask;
                Value |= (value & ColorMask);
            }
        }

        /// <summary>
        /// Gets or sets the high <see cref="Byte"/> of <see cref="Value"/>.
        /// </summary>
        public byte High
        {
            get => _high;
            set => _high = value;
        }
        /// <summary>
        /// Gets or sets the low <see cref="Byte"/> of <see cref="Value"/>.
        /// </summary>
        public byte Low
        {
            get => _low;
            set => _low = value;
        }

        public bool Alpha
        {
            get => (Value & AlphaComponentMask) != 0;
            set
            {
                if (value)
                    Value |= AlphaComponentMask;
                else
                    Value &= ~AlphaComponentMask;
            }
        }
        /// <summary>
        /// Gets or sets the red component of this <see cref="Color15BppAbgr1555"/> structure.
        /// </summary>
        /// <remarks>
        /// Valid values for a component range from 0 to 31 inclusive. If a value outside of this
        /// range is specified when setting the value, the highest three bits are cleared.
        /// </remarks>
        public int Red
        {
            get => (byte)((Value & RedComponentMask) >> RedShift);
            set
            {
                Value &= ~RedComponentMask;
                Value |= (ushort)((value & RedChannelMask) << RedShift);
            }
        }
        /// <summary>
        /// Gets or sets the green component of this <see cref="Color15BppAbgr1555"/> structure.
        /// </summary>
        /// <inheritdoc cref="Red"/>
        public int Green
        {
            get => (byte)((Value & GreenComponentMask) >> GreenShift);
            set
            {
                Value &= ~GreenComponentMask;
                Value |= (ushort)((value & GreenChannelMask) << GreenShift);
            }
        }
        /// <summary>
        /// Gets or sets the blue component of this <see cref="Color15BppAbgr1555"/> structure.
        /// </summary>
        /// <value>
        /// A value between 0 and 31 inclusive that specifies the five bits that describe this component.
        /// </value>
        /// <inheritdoc cref="Red"/>
        public int Blue
        {
            get => (byte)((Value & BlueComponentMask) >> BlueShift);
            set
            {
                Value &= ~BlueComponentMask;
                Value |= (ushort)((value & BlueChannelMask) << BlueShift);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color15BppAbgr1555"/> structure using the
        /// given color value.
        /// </summary>
        /// <param name="value">
        /// The color value.
        /// </param>
        /// <overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="Color15BppAbgr1555"/> struct.
        /// </summary>
        /// </overloads>
        private Color15BppAbgr1555(ushort value) : this()
        {
            Value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Color15BppAbgr1555"/> structure using the
        /// given color values. Only the lowest 5 bits of each value are used.
        /// </summary>
        /// <param name="red">
        /// The intensity of the <see cref="Red"/> channel.
        /// </param>
        /// <param name="green">
        /// The intensity of the <see cref="Green"/> channel.
        /// </param>
        /// <param name="blue">
        /// The intensity of the <see cref="Blue"/> channel.
        /// </param>
        public Color15BppAbgr1555(int red, int green, int blue)
            : this(
                (ushort)(
                ((red & RedChannelMask) << RedShift) |
                ((green & GreenChannelMask) << GreenShift) |
                ((blue & BlueChannelMask) << BlueShift)))
        {
        }

        /// <summary>
        /// Converts an <see cref="Int32"/> data type to a <see cref="Color15BppAbgr1555"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Int32"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color15BppAbgr1555"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color15BppAbgr1555(int value)
        {
            return new Color15BppAbgr1555((ushort)value);
        }
        /// <summary>
        /// Converts a <see cref="Color15BppAbgr1555"/> structure to an <see cref="Int32"/>
        /// data type.
        /// </summary>
        /// <param name="color15">
        /// The <see cref="Color15BppAbgr1555"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Int32"/> that results from the conversion.
        /// </returns>
        public static implicit operator int(Color15BppAbgr1555 color15)
        {
            return color15.Value;
        }

        /// <summary>
        /// Converts a <see cref="Color24BppRgb"/> structure to a <see cref="Color15BppAbgr1555"/>
        /// structure.
        /// </summary>
        /// <param name="color24">
        /// The <see cref="Color24BppRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color15BppAbgr1555"/> that results from the conversion.
        /// </returns>
        public static explicit operator Color15BppAbgr1555(Color24BppRgb color24)
        {
            // Each component goes from 8 bits of sensitivity to 5
            // So we shift right 3 bytes for the conversion.
            return new Color15BppAbgr1555(
                color24.Red >> (Color24BppRgb.BitsPerRed - BitsPerRed),
                color24.Green >> (Color24BppRgb.BitsPerGreen - BitsPerGreen),
                color24.Blue >> (Color24BppRgb.BitsPerBlue - BitsPerBlue));
        }
        /// <summary>
        /// Converts a <see cref="Color15BppAbgr1555"/> structure to a <see cref="Color24BppRgb"/>
        /// structure.
        /// </summary>
        /// <param name="color15">
        /// The <see cref="Color15BppAbgr1555"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color24BppRgb"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color24BppRgb(Color15BppAbgr1555 color15)
        {
            // Each component goes from 5 bits of sensitivity to 8
            // So we shift left 3 bytes for the conversion.
            return new Color24BppRgb(
                color15.Red << (Color24BppRgb.BitsPerRed - BitsPerRed),
                color15.Green << (Color24BppRgb.BitsPerGreen - BitsPerGreen),
                color15.Blue << (Color24BppRgb.BitsPerBlue - BitsPerBlue));
        }

        /// <summary>
        /// Converts a <see cref="Color32BppArgb"/> structure to a <see cref="Color15BppAbgr1555"/>
        /// structure.
        /// </summary>
        /// <param name="color32">
        /// The <see cref="Color32BppArgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color15BppAbgr1555"/> that results from the conversion.
        /// </returns>
        public static explicit operator Color15BppAbgr1555(Color32BppArgb color32)
        {
            // Same as the 24-bit color conversion; we ignore the alpha component.
            return new Color15BppAbgr1555(
                color32.Red >> (Color32BppArgb.BitsPerRed - BitsPerRed),
                color32.Green >> (Color32BppArgb.BitsPerGreen - BitsPerGreen),
                color32.Blue >> (Color32BppArgb.BitsPerBlue - BitsPerBlue));
        }
        /// <summary>
        /// Converts a <see cref="Color15BppAbgr1555"/> structure to a <see cref="Color32BppArgb"/>
        /// structure.
        /// </summary>
        /// <param name="color15">
        /// The <see cref="Color15BppAbgr1555"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color32BppArgb"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color32BppArgb(Color15BppAbgr1555 color15)
        {
            // Same as the 24-bit color conversion; we ignore the alpha component.
            return new Color32BppArgb(
                color15.Red << (Color32BppArgb.BitsPerRed - BitsPerRed),
                color15.Green << (Color32BppArgb.BitsPerGreen - BitsPerGreen),
                color15.Blue << (Color32BppArgb.BitsPerBlue - BitsPerBlue));
        }

        /// <summary>
        /// Compares two <see cref="Color15BppAbgr1555"/> objects. The result specifies whether
        /// the <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> components are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color15BppAbgr1555"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color15BppAbgr1555"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> have
        /// equal <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> components; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Color15BppAbgr1555 left, Color15BppAbgr1555 right)
        {
            // We compare the preserved values because we do not care about
            // the most significant bit.
            return left.PreservedValue == right.PreservedValue;
        }
        /// <summary>
        /// Compares two <see cref="Color15BppAbgr1555"/> objects. The result specifies whether
        /// the <see cref="Red"/>, <see cref="Green"/>, or
        /// <see cref="Blue"/> components are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color15BppAbgr1555"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color15BppAbgr1555"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> have
        /// unequal <see cref="Red"/>, <see cref="Green"/>, or
        /// <see cref="Blue"/> components; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Color15BppAbgr1555 left, Color15BppAbgr1555 right)
        {
            return !(left == right);
        }

        public static explicit operator Color15BppAbgr1555(ColorF color)
        {
            return new Color15BppAbgr1555(
                (int)Math.Round(color.Red * RedChannelMask),
                (int)Math.Round(color.Green * GreenChannelMask),
                (int)Math.Round(color.Blue * BlueChannelMask));
        }
        public static implicit operator ColorF(Color15BppAbgr1555 pixel)
        {
            return ColorF.FromArgb(
                pixel.Red / (float)RedChannelMask,
                pixel.Green / (float)GreenChannelMask,
                pixel.Blue / (float)BlueChannelMask);
        }

        /// <summary>
        /// Specifies whether this <see cref="Color15BppAbgr1555"/> is the same color as
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is the same color as this
        /// <see cref="Color15BppAbgr1555"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Color15BppAbgr1555))
                return false;

            return (Color15BppAbgr1555)obj == this;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="Color15BppAbgr1555"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="Color15BppAbgr1555"/>.
        /// </returns>
        public override int GetHashCode() =>
            // The hash code should not contain the most significant bit.
            PreservedValue;

        /// <summary>
        /// Converts this <see cref="Color15BppAbgr1555"/> to a human-readable <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> the represent this <see cref="Color15BppAbgr1555"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(nameof(Alpha));
            sb.Append(": ");
            sb.Append(Alpha);
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
