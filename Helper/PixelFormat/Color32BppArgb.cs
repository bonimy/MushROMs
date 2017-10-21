using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Helper.PixelFormats
{
    /// <summary>
    /// Represents a 24-bit RGB color as three <see cref="Byte"/> values with a bit layout of
    /// <c>[a a a a a a a a - r r r r r r r r - g g g g g g g g - b b b b b b b b]</c>.
    /// </summary>
    /// <remarks>
    /// This struct directly converts to <see cref="Int32"/>. Therefore, a pointer
    /// conversion from <see cref="Int32"/>* to <see cref="Color32BppArgb"/>* can be
    /// successfully cast.
    /// </remarks>
    /// <threadsafety static="true" instance="false"/>
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Color32BppArgb
    {
        /// <summary>
        /// The size, in bytes, of <see cref="Color32BppArgb"/>.
        /// This field is constant.
        /// </summary>
        public const int SizeOf = sizeof(int);

        /// <summary>
        /// Represents a <see cref="Color24BppRgb"/> that has its <see cref="Alpha"/>, <see cref="Red"/>,
        /// <see cref="Green"/>, and <see cref="Blue"/> values set to 0.
        /// </summary>
        public static readonly Color32BppArgb Empty = new Color32BppArgb();

        /// <summary>
        /// The index of <see cref="Alpha"/> in the components array.
        /// This field is constant.
        /// </summary>
        public const int AlphaIndex = 3;
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
        /// The number of bits the alpha component consumes.
        /// This field is constant.
        /// </summary>
        internal const int BitsPerAlpha = BitsPerChannel;
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
        /// The color value, as a 32-bit integer.
        /// </summary>
        /// <remarks>
        /// This is the fundamental type of this <see cref="Color32BppArgb"/>.
        /// </remarks>
        [FieldOffset(0)]
        private int _value;
        /// <summary>
        /// The color component array of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        [FieldOffset(0)]
        private fixed byte _components[SizeOf];
        /// <summary>
        /// The alpha component of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        [FieldOffset(AlphaIndex)]
        private byte _alpha;
        /// <summary>
        /// The red component of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        [FieldOffset(RedIndex)]
        private byte _red;
        /// <summary>
        /// The green component of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        [FieldOffset(GreenIndex)]
        private byte _green;
        /// <summary>
        /// The blue component of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        [FieldOffset(BlueIndex)]
        private byte _blue;

        /// <summary>
        /// gets or sets the color value, as a 32-bit integer.
        /// </summary>
        /// <remarks>
        /// This is the fundamental type of this <see cref="Color32BppArgb"/>.
        /// </remarks>
        public int Value
        {
            get => _value;
            set => _value = value;
        }
        /// <summary>
        /// Gets or sets the alpha component of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        public byte Alpha
        {
            get => _alpha;
            set => _alpha = value;
        }
        /// <summary>
        /// Gets or sets the red component of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        public byte Red
        {
            get => _red;
            set => _red = value;
        }
        /// <summary>
        /// Gets or sets the green component of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        public byte Green
        {
            get => _green;
            set => _green = value;
        }
        /// <summary>
        /// Gets or sets the blue component of this <see cref="Color32BppArgb"/> structure.
        /// </summary>
        public byte Blue
        {
            get => _blue;
            set => _blue = value;
        }

        /// <summary>
        /// Gets or sets the blue component of this <see cref="Color32BppArgb"/> structure at the
        /// specified index.
        /// </summary>
        /// <param name="index">
        /// One of <see cref="AlphaIndex"/>, <see cref="RedIndex"/>, <see cref="GreenIndex"/>,
        /// or <see cref="BlueIndex"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> is not one of <see cref="AlphaIndex"/>,
        /// <see cref="RedIndex"/>, <see cref="GreenIndex"/>, or <see cref="BlueIndex"/>.
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
        /// Initializes a new instance of the <see cref="Color32BppArgb"/> struct.
        /// </summary>
        /// <param name="value">
        /// An ARGB color value.
        /// </param>
        /// <summary>
        /// Initializes a new instance of the <see cref="Color32BppArgb"/> struct.
        /// </summary>
        private Color32BppArgb(int value)
            : this()
        {
            _value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Color32BppArgb"/> struct using the specified
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
        /// <para/><para/>
        /// <see cref="Alpha"/> is set to be fully opaque.
        /// </remarks>
        public Color32BppArgb(int red, int green, int blue) : this(Byte.MaxValue, red, green, blue)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Color32BppArgb"/> struct using the specified
        /// alpha, red, green, and blue values.
        /// </summary>
        /// <param name="alpha">
        /// The alpha intensity.
        /// </param>
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
        public Color32BppArgb(int alpha, int red, int green, int blue)
            : this()
        {
            _alpha = (byte)alpha;
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        /// <summary>
        /// Converts an <see cref="Int32"/> data type to a <see cref="Color32BppArgb"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Int32"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color32BppArgb"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color32BppArgb(int value)
        {
            return new Color32BppArgb(value);
        }
        /// <summary>
        /// Converts a <see cref="Color32BppArgb"/> structure to an <see cref="Int32"/>
        /// data type.
        /// </summary>
        /// <param name="color32">
        /// The <see cref="Color32BppArgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Int32"/> that results from the conversion.
        /// </returns>
        public static implicit operator int(Color32BppArgb color32)
        {
            return color32.Value;
        }

        public static implicit operator Color32BppArgb(ColorF color)
        {
            return new Color32BppArgb(
                (int)(color.Alpha * Byte.MaxValue + 0.5f),
                (int)(color.Red * Byte.MaxValue + 0.5f),
                (int)(color.Green * Byte.MaxValue + 0.5f),
                (int)(color.Blue * Byte.MaxValue + 0.5f));
        }
        public static implicit operator ColorF(Color32BppArgb pixel)
        {
            return ColorF.FromArgb(
                pixel.Alpha / (float)Byte.MaxValue,
                pixel.Red / (float)Byte.MaxValue,
                pixel.Green / (float)Byte.MaxValue,
                pixel.Blue / (float)Byte.MaxValue);
        }

        /// <summary>
        /// Compares two <see cref="Color32BppArgb"/> objects. The result specifies whether
        /// the <see cref="Red"/>, <see cref="Green"/>, or
        /// <see cref="Blue"/> components are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color32BppArgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color32BppArgb"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> have
        /// equal <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> components; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Color32BppArgb left, Color32BppArgb right)
        {
            return left.Value == right.Value;
        }
        /// <summary>
        /// Compares two <see cref="Color32BppArgb"/> objects. The result specifies whether
        /// the <see cref="Red"/>, <see cref="Green"/>, or
        /// <see cref="Blue"/> components are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color32BppArgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color32BppArgb"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> have
        /// unequal <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, or
        /// <see cref="Blue"/> components; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Color32BppArgb left, Color32BppArgb right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="Color32BppArgb"/> is the same color as
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is the same color as this
        /// <see cref="Color32BppArgb"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Color32BppArgb))
                return false;

            return (Color32BppArgb)obj == this;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="Color32BppArgb"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="Color32BppArgb"/>.
        /// </returns>
        public override int GetHashCode() => Value;

        /// <summary>
        /// Converts this <see cref="Color32BppArgb"/> to a human-readable <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> the represent this <see cref="Color32BppArgb"/>.
        /// </returns>
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
