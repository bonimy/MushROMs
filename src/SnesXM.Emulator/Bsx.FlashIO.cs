using System;
using Helper;

namespace SnesXM.Emulator
{
    partial class Bsx
    {
        private class FlashIOIndexer : IIndexer<int>
        {
            private Bsx Bsx
            {
                get;
                set;
            }

            public int this[int index]
            {
                get => Bsx.GetBypassFlashIO(index);
                set => Bsx.SetBypassFlashIO(index, (byte)value);
            }

            public FlashIOIndexer(Bsx bsx)
            {
                Bsx = bsx ?? throw new ArgumentNullException(nameof(bsx));
            }
        }

        private byte GetBypassFlashIO(int offset)
        {
            FlashRom = Memory.Rom + Multi.B.Offset;
            MapRom = Memory.Rom + Multi.B.Offset;

            if (Mmc[0x02] != 0)
                return MapRom[offset & 0x0FFFFF];
            else
                return MapRom[((offset & 0x1F0000) >> 1) | (offset & 0x7FFF)];
        }

        private void SetBypassFlashIO(int offset, byte value)
        {
            FlashRom = Memory.Rom + Multi.B.Offset;
            MapRom = Memory.Rom + Multi.B.Offset;

            if (Mmc[0x02] != 0)
                MapRom[offset & 0x0FFFFF] &= value;
            else
                MapRom[((offset & 0x1F0000) >> 1) | (offset & 0x7FFF)] &= value;
        }
    }
}
