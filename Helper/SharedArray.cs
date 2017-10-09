using System;

namespace Helper
{
    public class SharedArray<T>
    {
        private T[] Array
        {
            get;
            set;
        }

        public int Offset
        {
            get;
            set;
        }

        public T this[int index]
        {
            get => Array[Offset + index];
            set => Array[Offset + index] = value;
        }

        public SharedArray(T[] array) : this(array, 0) { }

        public SharedArray(T[] array, int offset)
        {
            Array = array ?? throw new ArgumentNullException(nameof(array));
            Offset = offset;
        }

        public SharedArray(SharedArray<T> sharedArray) : this(sharedArray, 0) { }

        public SharedArray(SharedArray<T> sharedArray, int offset)
        {
            Array = sharedArray?.Array ?? throw new ArgumentNullException(nameof(sharedArray));
            Offset = offset + sharedArray.Offset;
        }

        public T[] GetArray()
        {
            return Array;
        }

        public static explicit operator SharedArray<T>(T[] array)
        {
            return new SharedArray<T>(array);
        }

        public static SharedArray<T> operator ++(SharedArray<T> sharedArray)
        {
            return new SharedArray<T>(sharedArray, 1);
        }

        public static SharedArray<T> operator --(SharedArray<T> sharedArray)
        {
            return new SharedArray<T>(sharedArray, -1);
        }

        public static SharedArray<T> operator +(SharedArray<T> sharedArray, int offset)
        {
            return new SharedArray<T>(sharedArray, offset);
        }

        public static SharedArray<T> operator +(int offset, SharedArray<T> sharedArray)
        {
            return new SharedArray<T>(sharedArray, offset);
        }

        public static SharedArray<T> operator -(SharedArray<T> sharedArray, int offset)
        {
            return new SharedArray<T>(sharedArray, -offset);
        }

        public static SharedArray<T> operator -(int offset, SharedArray<T> sharedArray)
        {
            return new SharedArray<T>(sharedArray, -offset);
        }

        public static int operator -(SharedArray<T> left, SharedArray<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.Array != right.Array)
                throw new ArgumentException();

            return left.Offset - right.Offset;
        }

        public static int operator -(SharedArray<T> left, T[] right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.Array != right)
                throw new ArgumentException();

            return left.Offset;
        }

        public static int operator -(T[] left, SharedArray<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left != right.Array)
                throw new ArgumentException();

            return -right.Offset;
        }

        public static bool operator ==(SharedArray<T> left, SharedArray<T> right)
        {
            return (left - right) == 0;
        }
        public static bool operator !=(SharedArray<T> left, SharedArray<T> right)
        {
            return !(left == right);
        }

        public static bool operator <(SharedArray<T> left, SharedArray<T> right)
        {
            return (left - right) < 0;
        }
        public static bool operator >(SharedArray<T> left, SharedArray<T> right)
        {
            return (left - right) > 0;
        }
        public static bool operator >=(SharedArray<T> left, SharedArray<T> right)
        {
            return !(left < right);
        }
        public static bool operator <=(SharedArray<T> left, SharedArray<T> right)
        {
            return !(left > right);
        }
    }
}