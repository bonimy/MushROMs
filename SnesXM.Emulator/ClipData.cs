namespace SnesXM.Emulator
{
    public class ClipData
    {
        private const int Size = Ppu.MaxClipSize;

        private byte _count;

        public int Count
        {
            get => _count;
            private set => _count = (byte)value;
        }

        internal byte[] DrawMode
        {
            get;
            private set;
        }

        internal ushort[] Left
        {
            get;
            private set;
        }

        internal ushort[] Right
        {
            get;
            private set;
        }

        public ClipData()
        {
            Count = 0;
            DrawMode = new byte[Size];
            Left = new ushort[Size];
            Right = new ushort[Size];
        }
    }
}