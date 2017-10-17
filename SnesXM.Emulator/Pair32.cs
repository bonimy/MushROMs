using System.Runtime.InteropServices;

namespace SnesXM.Emulator
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Pair32
    {
        public const int SizeOf = sizeof(int);

        [FieldOffset(0)]
        private byte _low;

        [FieldOffset(1)]
        private byte _high;

        [FieldOffset(2)]
        private byte _bank;

        [FieldOffset(0)]
        private ushort _word;

        [FieldOffset(0)]
        private int _value;

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

        public int Bank
        {
            get => _bank;
            set => _bank = (byte)value;
        }

        public int Word
        {
            get => _word;
            set => _word = (ushort)value;
        }

        public int Value
        {
            get => _value;
            set => _value = value;
        }

        public Pair32(int low, int high, int bank) : this()
        {
            Bank = bank;
            Low = low;
            High = high;
        }

        public Pair32(int bank, int word) : this()
        {
            Bank = bank;
            Word = word;
        }

        public Pair32(int value) : this()
        {
            Value = value;
        }

        public static implicit operator Pair32(int value)
        {
            return new Pair32(value);
        }

        public static implicit operator int(Pair32 value)
        {
            return value.Value;
        }
    }
}