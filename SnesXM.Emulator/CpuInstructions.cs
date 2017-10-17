using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public class CpuInstructions
    {
        private Registers Registers
        {
            get;
            set;
        }
        private CpuState State
        {
            get;
            set;
        }

        private int OpenBus
        {
            get;
            set;
        }

        public int Immediate8(AccessMode accessMode)
        {
            var result = State.PcBase[Registers.Pc];
            if (IsReadable(accessMode))
                OpenBus = result;
            Registers.PcW++;

            return result;
        }

        private static bool IsReadable(AccessMode accessMode)
        {
            return (accessMode & AccessMode.Read) != 0;
        }
    }
}
