using System;

namespace Helper
{
    public class UInt16ByteIndexer : IIndexer<ushort>
    {
        public Pointer<byte> Pointer
        {
            get;
            private set;
        }

        public ushort this[int index]
        {
            get => GetUInt16Internal(Pointer, index);
            set => SetUInt16Internal(Pointer, index, value);
        }

        public UInt16ByteIndexer(Pointer<byte> pointer)
        {
            Pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
        }

        public static ushort GetUInt16(Pointer<byte> pointer, int offset)
        {
            if (pointer == null)
                throw new ArgumentNullException(nameof(pointer));

            return GetUInt16Internal(pointer, offset);
        }
        private static ushort GetUInt16Internal(Pointer<byte> pointer, int index) =>
            BitConverter.ToUInt16(pointer.GetArray(), pointer.Offset + index);

        public static void SetUInt16(Pointer<byte> pointer, int index, int value)
        {
            if (pointer == null)
                throw new ArgumentNullException(nameof(pointer));

            SetUInt16Internal(pointer, index, value);
        }
        private static void SetUInt16Internal(Pointer<byte> pointer, int index, int value)
        {
            pointer[index + 0] = (byte)(value >> 0x00);
            pointer[index + 1] = (byte)(value >> 0x08);
        }
    }
}
