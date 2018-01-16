// <copyright file="Int24ByteIndexer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;

namespace Helper
{
    public class Int24ByteIndexer : IIndexer<Int24>, IReadOnlyIndexer<Int24>
    {
        public Pointer<byte> Pointer
        {
            get;
        }

        public Int24 this[int index]
        {
            get
            {
                return GetInt24Internal(Pointer, index);
            }

            set
            {
                SetInt24Internal(Pointer, index, value);
            }
        }

        public Int24ByteIndexer(Pointer<byte> pointer)
        {
            Pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
        }

        public static Int24 GetInt24(Pointer<byte> pointer, int offset)
        {
            if (pointer is null)
            {
                throw new ArgumentNullException(nameof(pointer));
            }

            return GetInt24Internal(pointer, offset);
        }

        private static Int24 GetInt24Internal(Pointer<byte> pointer, int index)
        {
            return
                (pointer[index + 0] << 0x00) |
                (pointer[index + 1] << 0x08) |
                (pointer[index + 2] << 0x10);
        }

        public static void SetInt24(Pointer<byte> pointer, int index, int value)
        {
            if (pointer is null)
            {
                throw new ArgumentNullException(nameof(pointer));
            }

            SetInt24Internal(pointer, index, value);
        }

        private static void SetInt24Internal(
            Pointer<byte> pointer,
            int index,
            int value)
        {
            pointer[index + 0] = (byte)(value >> 0x00);
            pointer[index + 1] = (byte)(value >> 0x08);
            pointer[index + 2] = (byte)(value >> 0x10);
        }
    }
}
