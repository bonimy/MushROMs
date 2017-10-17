using System.Runtime.InteropServices;

namespace SnesXM.Emulator
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Pair16
    {
        public const int SizeOf = sizeof(ushort);

        [FieldOffset(0)]
        private byte _low;

        [FieldOffset(1)]
        private byte _high;

        [FieldOffset(0)]
        private ushort _word;

        public int Low
        {
            get => _low;
            set => _low = (byte)value;
        }

        public int High
        {
            get => _high;
            set => _high = (byte)value;
        }

        public int Word
        {
            get => _word;
            set => _word = (ushort)value;
        }

        public Pair16(int low, int high) : this()
        {
            Low = low;
            High = high;
        }

        public Pair16(int word) : this()
        {
            Word = word;
        }

        public static implicit operator Pair16(int value)
        {
            return new Pair16(value);
        }

        public static implicit operator int(Pair16 value)
        {
            return value.Word;
        }
    }
}