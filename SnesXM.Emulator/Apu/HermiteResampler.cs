using System;
using Debug = System.Diagnostics.Debug;

namespace SnesXM.Emulator.Apu
{
    public class HermiteResampler : Resampler
    {
        private float _step;

        public override float TimeRatio
        {
            get => _step;
            set
            {
                _step = value;
                Clear();
            }
        }
        private float Fraction
        {
            get;
            set;
        }
        private int[] Left
        {
            get;
            set;
        }
        private int[] Right
        {
            get;
            set;
        }

        public override int Available =>
            (int)Math.Floor(((SpaceFilled >> 2) - Fraction) / TimeRatio) * 2;

        public HermiteResampler(int sampleSize) : base(sampleSize)
        {
            Left = new int[4];
            Right = new int[4];
            Clear();
        }

        public override void Read(byte[] data, int index, int sampleSize)
        {
            unsafe
            {
                fixed (byte* src = &data[index])
                fixed (byte* dest = GetBuffer())
                {
                    ReadInternal((short*)src, (short*)dest, sampleSize);
                }
            }
        }

        private unsafe void ReadInternal(short* data, short* buffer, int sampleSize)
        {
            Debug.Assert((sampleSize & 1) == 0);
            var srcIndex = Start >> 1;
            var maxSamples = BufferSize >> 1;
            var destIndex = 0;
            var consumed = 0;

            while (destIndex < sampleSize && consumed < BufferSize)
            {
                var sLeft = buffer[srcIndex];
                var sRight = buffer[srcIndex + 1];

                while (Fraction <= 1.0f && destIndex < sampleSize)
                {
                    var hLeft = Hermite(Fraction, Left[0], Left[1], Left[2], Left[3]);
                    var hRight = Hermite(Fraction, Right[0], Right[1], Right[2], Right[3]);

                    data[destIndex] = Int16Clamp(hLeft);
                    data[destIndex + 1] = Int16Clamp(hRight);

                    destIndex += 2;

                    Fraction += TimeRatio;
                }

                if (Fraction > 1.0f)
                {
                    Left[0] = Left[1];
                    Left[1] = Left[2];
                    Left[2] = Left[3];
                    Left[3] = sLeft;

                    Right[0] = Right[1];
                    Right[1] = Right[2];
                    Right[2] = Right[3];
                    Right[3] = sRight;

                    Fraction -= 1.0f;

                    srcIndex += 2;
                    srcIndex %= maxSamples;
                    consumed += 2;
                }
            }

            SpaceFilled -= consumed << 1;
            Start += consumed << 1;
            Start %= BufferSize;
        }

        private static short Int16Clamp(float value)
        {
            if (value < Int16.MinValue)
                return Int16.MinValue;

            if (value > Int16.MaxValue)
                return Int16.MaxValue;

            return (short)value;
        }

        public override void Clear()
        {
            base.Clear();
            Fraction = 1.0f;

            Left[0] =
            Left[1] =
            Left[2] =
            Left[3] = 0;

            Right[0] =
            Right[1] =
            Right[2] =
            Right[3] = 0;
        }

        private static float Hermite(float mu1, float a, float b, float c, float d)
        {
            var mu2 = mu1 * mu1;
            var mu3 = mu2 * mu1;

            var m0 = (c - a) * 0.5f;
            var m1 = (d - b) * 0.5f;

            var a0 = +2 * mu3 - 3 * mu2 + 0 * mu1 + 1;
            var a1 = +1 * mu3 - 2 * mu2 + 1 * mu1 + 0;
            var a2 = +1 * mu3 - 1 * mu2 + 0 * mu1 + 0;
            var a3 = -2 * mu3 + 3 * mu2 + 0 * mu1 + 0;

            return (a0 * b) + (a1 * m0) + (a2 * m1) + (a3 * c);
        }
    }
}