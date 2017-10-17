using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public interface IApu
    {
        void EndScanline();

        void SetReferenceTime(int cycles);
    }
}
