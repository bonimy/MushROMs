using System;

namespace SnesXM.Emulator
{
    [Flags]
    public enum RegisterBits
    {
        Carry      = 1 << 0,
        Zero       = 1 << 1,
        Irq        = 1 << 2,
        Decimal    = 1 << 3,
        IndexFlag  = 1 << 4,
        MemoryFlag = 1 << 5,
        Overflow   = 1 << 6,
        Negative   = 1 << 7,
        Emulation  = 1 << 8,
    }
}
