// <copyright file="Int16ByteIndexer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;

namespace Helper
{
    public class Int16ByteIndexer : IIndexer<short>, IReadOnlyIndexer<short>
    {
        public Pointer<byte> Pointer
        {
            get;
            private set;
        }

        public short this[int index]
        {
            get
            {
                return GetInt16Internal(Pointer, index);
            }

            set
            {
                SetInt16Internal(Pointer, index, value);
            }
        }

        public Int16ByteIndexer(Pointer<byte> pointer)
        {
            Pointer = pointer ??
throw new ArgumentNullException(nameof(pointer));
        }

        public static short GetInt16(Pointer<byte> pointer, int offset)
        {
            if (pointer == null)
            {
                throw new ArgumentNullException(nameof(pointer));
            }

            return GetInt16Internal(pointer, offset);
        }

        private static short GetInt16Internal(Pointer<byte> pointer, int index)
        {
            return BitConverter.ToInt16(pointer.GetArray(), pointer.Offset + index);
        }

        public static void SetInt16(Pointer<byte> pointer, int index, int value)
        {
            if (pointer == null)
            {
                throw new ArgumentNullException(nameof(pointer));
            }

            SetInt16Internal(pointer, index, value);
        }

        private static void SetInt16Internal(Pointer<byte> pointer, int index, int value)
        {
            pointer[index + 0] = (byte)(value >> 0x00);
            pointer[index + 1] = (byte)(value >> 0x08);
        }
    }
}
