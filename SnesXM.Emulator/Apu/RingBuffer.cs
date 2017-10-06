using System;

namespace SnesXM.Emulator.Apu
{
    public abstract class RingBuffer
    {
        public int SpaceFilled
        {
            get;
            protected set;
        }
        public int Start
        {
            get;
            protected set;
        }
        public int SpaceEmpty => BufferSize - SpaceFilled;

        private byte[] Buffer
        {
            get;
            set;
        }
        protected int BufferSize => Buffer.Length;

        protected byte[] GetBuffer()
        {
            return Buffer;
        }

        protected RingBuffer(int bufferSize)
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            // Don't call Resize because it is a virtual method.
            Buffer = new byte[bufferSize];
            SpaceFilled = 0;
            Start = 0;
        }

        public virtual bool Push(byte[] data, int index, int length)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (index + length > data.Length)
                throw new ArgumentException();

            if (SpaceEmpty < length)
                return false;

            var end = (Start + SpaceFilled) % BufferSize;
            var firstWriteSize = Math.Min(length, BufferSize - end);

            Array.Copy(data, 0, Buffer, end, firstWriteSize);

            if (length > firstWriteSize)
            {
                Array.Copy(data, firstWriteSize, Buffer, 0, length - firstWriteSize);
            }

            SpaceFilled += length;

            return true;
        }

        protected virtual byte[] Pull(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (SpaceFilled < size)
                return null;

            var data = new byte[size];
            var end = BufferSize - Start;
            Array.Copy(Buffer, Start, data, 0, Math.Min(size, end));

            if (size > end)
                Array.Copy(Buffer, 0, data, end, size - end);

            Start += size;
            Start %= Buffer.Length;
            SpaceFilled -= size;

            return data;
        }

        public virtual void Clear()
        {
            Start = 0;
            SpaceFilled = 0;
            Array.Clear(Buffer, 0, BufferSize);
        }

        public virtual void Resize(int bufferSize)
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            Buffer = new byte[bufferSize];
            SpaceFilled = 0;
            Start = 0;
        }

        public virtual void CacheSilence()
        {
            Clear();
            SpaceFilled = BufferSize;
        }
    }
}
