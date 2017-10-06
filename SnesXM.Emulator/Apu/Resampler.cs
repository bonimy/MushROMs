namespace SnesXM.Emulator.Apu
{
    public abstract class Resampler : RingBuffer
    {
        public int MaxWrite => SpaceEmpty >> 1;

        public abstract float TimeRatio { get; set; }

        public abstract int Available { get; }

        protected Resampler(int sampleSize) : base(sampleSize << 1)
        { }

        public abstract void Read(byte[] data, int index, int sampleSize);

        public override bool Push(byte[] data, int index, int sampleSize)
        {
            if (MaxWrite < sampleSize)
                return false;

            return base.Push(data, index, sampleSize << 1);
        }

        public override void Resize(int sampleSize)
        {
            base.Resize(sampleSize << 1);
        }
    }
}