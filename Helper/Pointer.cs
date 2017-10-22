// <copyright file="Pointer.cs>
//     Copyright (c) 2017 Nelson Garcia
// </copyright>

using System;

namespace Helper
{
    public class Pointer<T>
    {
        private T[] Array
        {
            get;
            set;
        }

        public T[] GetArray() => Array;

        public int Offset
        {
            get;
            set;
        }

        public int Length => Array.Length - Offset;

        public T this[int index]
        {
            get => Array[Offset + index];
            set => Array[Offset + index] = value;
        }

        public Pointer(int size) : this(new T[size])
        {
        }

        public Pointer(Pointer<T> pointer) : this(pointer, 0)
        {
        }

        public Pointer(Pointer<T> pointer, int offset)
        {
            if (pointer == null)
                throw new ArgumentNullException(nameof(pointer));

            Array = pointer.Array;
            Offset = pointer.Offset + offset;
        }

        public Pointer(T[] pointer) : this(pointer, 0)
        {
        }

        public Pointer(T[] pointer, int offset)
        {
            Array = pointer ?? throw new ArgumentNullException(nameof(pointer));
            Offset = offset;
        }

        public Pointer<T> Clone() => new Pointer<T>(Array, Offset);

        public void Clear(int length) =>
            Clear(0, length);

        public void Clear(int offset, int length) =>
            System.Array.Clear(Array, Offset + offset, length);

        public void Copy(Pointer<T> source, int length) =>
            Copy(0, source, 0, length);

        public void Copy(int destOffset, Pointer<T> source, int length) =>
            Copy(destOffset, source, 0, length);

        public void Copy(Pointer<T> source, int sourceOffset, int length) =>
            Copy(0, source, sourceOffset, length);

        public void Copy(int destOffset, Pointer<T> source, int sourceOffset, int length) =>
            System.Array.Copy(source.Array, sourceOffset, Array, Offset + destOffset, length);

        public void CopyTo(Array dest, int length) =>
            CopyTo(0, dest, 0, length);

        public void CopyTo(Array dest, int destOffset, int length) =>
            CopyTo(0, dest, destOffset, length);

        public void CopyTo(int sourceOffset, Array dest, int length) =>
            CopyTo(sourceOffset, dest, 0, length);

        public void CopyTo(int sourceOffset, Array dest, int destOffset, int length) =>
            System.Array.Copy(Array, sourceOffset, dest, destOffset, length);

        public void BlockCopy<U>(Pointer<U> source, int count)
        {
            var src = source.Array;
            var dst = Array;

            Buffer.BlockCopy(
                src,
                getLength(src, source.Length),
                dst,
                getLength(dst, Length),
                count);

            int getLength(Array array, int offset)
            {
                var length = array.Length;
                var byteLength = Buffer.ByteLength(array);

                // The size in bytes of each element in the array.
                var size = byteLength / length;

                return (length - offset) * size;
            }
        }

        public static implicit operator Pointer<T>(T[] array)
        {
            if (array == null)
                return null;

            return new Pointer<T>(array, 0);
        }

        public static Pointer<T> operator +(Pointer<T> sharedArray, int offset) =>
            new Pointer<T>(sharedArray.Array, sharedArray.Offset + offset);

        public static Pointer<T> operator +(int offset, Pointer<T> sharedArray) =>
            sharedArray + offset;

        public static Pointer<T> operator -(Pointer<T> sharedArray, int offset) =>
            sharedArray + (-offset);

        public static Pointer<T> operator -(int offset, Pointer<T> sharedArray) =>
            sharedArray - offset;

        public static Pointer<T> operator ++(Pointer<T> sharedArray) =>
            sharedArray + 1;

        public static Pointer<T> operator --(Pointer<T> sharedArray) =>
            sharedArray - 1;

        public static int operator -(Pointer<T> left, Pointer<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.Array != right.Array)
                throw new ArgumentException();

            return left.Offset - right.Offset;
        }

        public static bool operator ==(Pointer<T> left, Pointer<T> right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            if (left.Array != right.Array)
                return false;

            return left.Offset == right.Offset;
        }

        public static bool operator !=(Pointer<T> left, Pointer<T> right) =>
            !(left == right);

        public static bool operator <(Pointer<T> left, Pointer<T> right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            if (left.Array != right.Array)
                return false;

            return left.Offset < right.Offset;
        }

        public static bool operator >(Pointer<T> left, Pointer<T> right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            if (left.Array != right.Array)
                return false;

            return left.Offset > right.Offset;
        }

        public static bool operator >=(Pointer<T> left, Pointer<T> right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            if (left.Array != right.Array)
                return false;

            return left.Offset >= right.Offset;
        }

        public static bool operator <=(Pointer<T> left, Pointer<T> right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            if (left.Array != right.Array)
                return false;

            return left.Offset <= right.Offset;
        }

        public override bool Equals(object obj)
        {
            if (obj is Pointer<T> ap)
                return ap == this;
            return false;
        }

        public override int GetHashCode() =>
            Array.GetHashCode() ^ Offset.GetHashCode();
    }
}
