using System;
using Helper;

namespace SnesXM.Emulator
{
    public class Tile
    {
        private const int BitsPerByte = 8;
        private const int HrSize = 0x100;

        private IEmulator Emulator
        {
            get;
            set;
        }
        private IMemoryMap Memory => Emulator.MemoryMap;

        private int[][] PixelBits
        {
            get;
            set;
        }
        private byte[] HrBitOdd
        {
            get;
            set;
        }
        private byte[] HrBitEven
        {
            get;
            set;
        }

        public Tile(IEmulator emulator)
        {
            Emulator = emulator ?? throw new ArgumentNullException(nameof(emulator));

            PixelBits = new int[BitsPerByte][];
            for (int i = 0; i < BitsPerByte; i++)
                PixelBits[i] = new int[0x10];

            HrBitOdd = new byte[HrSize];
            HrBitEven = new byte[HrSize];
        }

        void Initialize()
        {
            for (int i = 0; i < 0x10; i++)
            {
                int b = 0;

                if ((i & 8) != 0)
                    b |= 1 << 0x00;
                if ((i & 4) != 0)
                    b |= 1 << 0x08;
                if ((i & 2) != 0)
                    b |= 1 << 0x10;
                if ((i & 1) != 0)
                    b |= 1 << 0x18;

                for (var bit = 0; bit < BitsPerByte; bit++)
                {
                    PixelBits[bit][i] = b << bit;
                }
            }

            for (int i = 0; i < HrSize; i++)
            {
                byte m = 0;
                byte s = 0;

                if ((i & 0x80) != 0)
                    s |= 8;
                if ((i & 0x40) != 0)
                    m |= 8;
                if ((i & 0x20) != 0)
                    s |= 4;
                if ((i & 0x10) != 0)
                    m |= 4;
                if ((i & 0x08) != 0)
                    s |= 2;
                if ((i & 0x04) != 0)
                    m |= 2;
                if ((i & 0x02) != 0)
                    s |= 1;
                if ((i & 0x01) != 0)
                    m |= 1;

                HrBitOdd[i] = m;
                HrBitEven[i] = s;
            }
        }

        private unsafe int ConvertTile2(byte* cache, int tileAddress)
        {
            var tp = Memory.Vram + tileAddress;
            var p = cache;
            var nonZero = 0;

            for (byte line = 8; line != 0; line--, tp += 2)
            {

            }

            return nonZero != 0 ? 1 : 2;
        }
    }
}
