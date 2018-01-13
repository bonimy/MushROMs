using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public interface IPpu
    {
        bool HTimerEnabled { get; }
        bool VTimerEnabled { get; }

        int HTimerPosition { get; }
        int VTimerPosition { get; }

        int RangeTimeOver { get; set; }

        int ScreenHeight { get; }

        int Hdma { get; set; }
    }
}
