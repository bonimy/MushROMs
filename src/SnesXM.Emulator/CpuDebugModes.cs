using System;

namespace SnesXM.Emulator
{
    [Flags]
    public enum CpuDebugModes
    {
        None         = 0,
        DebugMode    = 1 << 0,
        Trace        = 1 << 1,
        SingleStep   = 1 << 2,
        Break        = 1 << 3,
        ScanKey      = 1 << 4,
        Halted       = 1 << 12,
        FrameAdvance = 1 << 9
    }
}
