using System;

namespace Helper
{
    public class UInt24ByteIndexer : IIndexer<UInt24>
    {
        public Pointer<byte> Pointer
        {
            get;
            private set;
        }

        public UInt24 this[int index]
        {
            get => GetUInt24Internal(Pointer, index);
            set => SetUInt24Internal(Pointer, index, value);
        }

        public UInt24ByteIndexer(Pointer<byte> pointer)
        {
            Pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
        }

        public static UInt24 GetUInt24(Pointer<byte> pointer, int offset)
        {
            if (pointer == null)
                throw new ArgumentNullException(nameof(pointer));

            return GetUInt24Internal(pointer, offset);
        }
        private static UInt24 GetUInt24Internal(Pointer<byte> pointer, int index)
        {
            return
                (pointer[index + 0] << 0x00) |
                (pointer[index + 1] << 0x08) |
                (pointer[index + 2] << 0x10);
        }

        public static void SetUInt24(Pointer<byte> pointer, int index, int value)
        {
            if (pointer == null)
                throw new ArgumentNullException(nameof(pointer));

            SetUInt24Internal(pointer, index, value);
        }
        private static void SetUInt24Internal(Pointer<byte> pointer, int index, int value)
        {
            pointer[index + 0] = (byte)(value >> 0x00);
            pointer[index + 1] = (byte)(value >> 0x08);
            pointer[index + 2] = (byte)(value >> 0x10);
        }
    }
}
