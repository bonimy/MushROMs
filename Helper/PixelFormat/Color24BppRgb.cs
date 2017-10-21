using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Helper.PixelFormats
{
    /// <summary>
    /// Represents a 24-bit RGB color as three <see cref="Byte"/> values with a bit layout of
    /// <c>[r r r r r r r r - g g g g g g g g - b b b b b b b b]</c>.
    /// </summary>
    /// <remarks>
    /// This struct directly converts to an unsafe fixed array of three bytes. Therefore, a pointer
    /// conversion from <see cref="Byte"/>* to <see cref="Color24BppRgb"/>* can be successfully cast
    /// if the byte pointer accurately describes 24-bit RGB pixel data.
    /// </remarks>
    /// <threadsafety static="true" instance="false"/>
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public unsafe struct Color24BppRgb
    {
        /// <summary>
        /// The size, in bytes, of <see cref="Color24BppRgb"/>.
        /// This field is constant.
        /// </summary>
        public const int SizeOf = 3 * sizeof(byte);

        /// <summary>
        /// Represents a <see cref="Color24BppRgb"/> that has its <see cref="Red"/>,
        /// <see cref="Green"/>, and <see cref="Blue"/> values set to 0.
        /// </summary>
        public static readonly Color24BppRgb Empty = new Color24BppRgb();

        /// <summary>
        /// The index of <see cref="Red"/> in the components array.
        /// This field is constant.
        /// </summary>
        public const int RedIndex = 2;
        /// <summary>
        /// The index of <see cref="Green"/> in the components array.
        /// This field is constant.
        /// </summary>
        public const int GreenIndex = 1;
        /// <summary>
        /// The index of <see cref="Blue"/> in the components array.
        /// This field is constant.
        /// </summary>
        public const int BlueIndex = 0;

        /// <summary>
        /// The number of bits each component consumes.
        /// This field is constant.
        /// </summary>
        private const int BitsPerChannel = 8;
        /// <summary>
        /// The number of bits the red component consumes.
        /// This field is constant.
        /// </summary>
        internal const int BitsPerRed = BitsPerChannel;
        /// <summary>
        /// The number of bits the green component consumes.
        /// This field is constant.
        /// </summary>
        internal const int BitsPerGreen = BitsPerChannel;
        /// <summary>
        /// The number of bits the blue component consumes.
        /// This field is constant.
        /// </summary>
        internal const int BitsPerBlue = BitsPerChannel;

        /// <summary>
        /// The number of bits the <see cref="Red"/> component is shifted by.
        /// This field is constant.
        /// </summary>
        private const int RedShift = BitsPerChannel * RedIndex;
        /// <summary>
        /// The number of bits the <see cref="Green"/> component is shifted by.
        /// This field is constant.
        /// </summary>
        private const int GreenShift = BitsPerChannel * GreenIndex;
        /// <summary>
        /// The number of bits the <see cref="Blue"/> component is shifted by.
        /// This field is constant.
        /// </summary>
        private const int BlueShift = BitsPerChannel * BlueIndex;

        /// <summary>
        /// The color component array of this <see cref="Color24BppRgb"/> structure.
        /// </summary>
        /// <remarks>
        /// This is the fundamental type of this <see cref="Color24BppRgb"/>.
        /// </remarks>
        [FieldOffset(0)]
        private fixed byte _components[SizeOf];
        /// <summary>
        /// The red component of this <see cref="Color24BppRgb"/> structure.
        /// </summary>
        [FieldOffset(RedIndex)]
        private byte _red;
        /// <summary>
        /// The green component of this <see cref="Color24BppRgb"/> structure.
        /// </summary>
        [FieldOffset(GreenIndex)]
        private byte _green;
        /// <summary>
        /// The blue component of this <see cref="Color24BppRgb"/> structure.
        /// </summary>
        [FieldOffset(BlueIndex)]
        private byte _blue;

        /// <summary>
        /// Gets or sets the red component of this <see cref="Color24BppRgb"/> structure.
        /// </summary>
        public byte Red
        {
            get => _red;
            set => _red = value;
        }
        /// <summary>
        /// Gets or sets the green component of this <see cref="Color24BppRgb"/> structure.
        /// </summary>
        public byte Green
        {
            get => _green;
            set => _green = value;
        }
        /// <summary>
        /// Gets or sets the blue component of this <see cref="Color24BppRgb"/> structure.
        /// </summary>
        public byte Blue
        {
            get => _blue;
            set => _blue = value;
        }
        /// <summary>
        /// Gets or sets the component of this <see cref="Color24BppRgb"/> structure at the
        /// specified index.
        /// </summary>
        /// <param name="index">
        /// One of <see cref="RedIndex"/>, <see cref="GreenIndex"/>, or <see cref="BlueIndex"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> is not one of <see cref="RedIndex"/>, <see cref="GreenIndex"/>,
        /// or <see cref="BlueIndex"/>.
        /// </exception>
        /// <value>
        /// The component value at the specified index.
        /// </value>
        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= SizeOf)
                    SR.ErrorArrayBounds(nameof(index), index, SizeOf);

                fixed (byte* components = _components)
                    return components[index];
            }
            set
            {
                if (index < 0 || index >= SizeOf)
                    SR.ErrorArrayBounds(nameof(index), index, SizeOf);

                fixed (byte* components = _components)
                    components[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the color value, as a 24-bit unsigned integer. The most
        /// significant 8 bits have no meaning.
        /// </summary>
        public int Value
        {
            get => (Red << RedShift) |
                    (Green << GreenShift) |
                    (Blue << BlueShift);
            set => this = new Color24BppRgb(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color24BppRgb"/> struct.
        /// </summary>
        /// <param name="value">
        /// An RGB color value.
        /// </param>
        /// <summary>
        /// Initializes a new instance of the <see cref="Color24BppRgb"/> struct.
        /// </summary>
        private Color24BppRgb(int value)
            : this(
                value >> RedShift,
                value >> GreenShift,
                value >> BlueShift)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Color24BppRgb"/> struct using the specified
        /// red, green, and blue values.
        /// </summary>
        /// <param name="red">
        /// The red intensity.
        /// </param>
        /// <param name="green">
        /// The green intensity.
        /// </param>
        /// <param name="blue">
        /// The blue intensity.
        /// </param>
        /// <remarks>
        /// Each component is cast to <see cref="Byte"/> before assignment.
        /// </remarks>
        public Color24BppRgb(int red, int green, int blue)
        {
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        /// <summary>
        /// Converts an <see cref="Int32"/> data type to a <see cref="Color24BppRgb"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Int32"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color24BppRgb"/> that results from the conversion.
        /// </returns>
        public static explicit operator Color24BppRgb(int value)
        {
            return new Color24BppRgb(value);
        }
        /// <summary>
        /// Converts a <see cref="Color24BppRgb"/> structure to an <see cref="Int32"/>
        /// data type.
        /// </summary>
        /// <param name="color24">
        /// The <see cref="Color24BppRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Int32"/> that results from the conversion.
        /// </returns>
        public static implicit operator int(Color24BppRgb color24)
        {
            return color24.Value;
        }

        /// <summary>
        /// Converts a <see cref="Color32BppArgb"/> structure to a <see cref="Color24BppRgb"/>
        /// structure.
        /// </summary>
        /// <param name="color32">
        /// The <see cref="Color32BppArgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color24BppRgb"/> that results from the conversion.
        /// </returns>
        public static explicit operator Color24BppRgb(Color32BppArgb color32)
        {
            return new Color24BppRgb(color32.Value);
        }
        /// <summary>
        /// Converts a <see cref="Color24BppRgb"/> structure to a <see cref="Color32BppArgb"/>
        /// structure.
        /// </summary>
        /// <param name="color24">
        /// The <see cref="Color24BppRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color32BppArgb"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color32BppArgb(Color24BppRgb color24)
        {
            return color24.Value;
        }


        public static explicit operator Color24BppRgb(ColorF color)
        {
            return new Color24BppRgb(
                (int)(color.Red * Byte.MaxValue + 0.5f),
                (int)(color.Green * Byte.MaxValue + 0.5f),
                (int)(color.Blue * Byte.MaxValue + 0.5f));
        }
        public static implicit operator ColorF(Color24BppRgb pixel)
        {
            return ColorF.FromArgb(
                pixel.Red / (float)Byte.MaxValue,
                pixel.Green / (float)Byte.MaxValue,
                pixel.Blue / (float)Byte.MaxValue);
        }

        /// <summary>
        /// Compares two <see cref="Color24BppRgb"/> objects. The result specifies whether
        /// the <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> components are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color24BppRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color24BppRgb"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> have
        /// equal <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> components; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Color24BppRgb left, Color24BppRgb right)
        {
            return left.Value == right.Value;
        }
        /// <summary>
        /// Compares two <see cref="Color24BppRgb"/> objects. The result specifies whether
        /// the <see cref="Red"/>, <see cref="Green"/>, or
        /// <see cref="Blue"/> components are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color24BppRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color24BppRgb"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> have
        /// unequal <see cref="Red"/>, <see cref="Green"/>, or
        /// <see cref="Blue"/> components; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Color24BppRgb left, Color24BppRgb right)
        {
            return !(left.Value == right.Value);
        }

        /// <summary>
        /// Specifies whether this <see cref="Color24BppRgb"/> is the same color as
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is the same color as this
        /// <see cref="Color24BppRgb"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Color24BppRgb))
                return false;

            return (Color24BppRgb)obj == this;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="Color24BppRgb"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="Color24BppRgb"/>.
        /// </returns>
        public override int GetHashCode() => Value;

        /// <summary>
        /// Converts this <see cref="Color24BppRgb"/> to a human-readable <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> the represent this <see cref="Color24BppRgb"/>.
        /// </returns>
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
