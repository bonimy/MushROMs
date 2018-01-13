using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator.Memory
{
    class MemoryMap
    {
        public const int Size = 0x1000000;

        public const int BlockSize = 0x1000;
        public const int NumberOfBlocks = Size / BlockSize;


    }
}
