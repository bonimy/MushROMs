using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public interface ITimings
    {
        int HMaxMaster
        {
            get;
            set;
        }
        int HMax
        {
            get;
            set;
        }
        int VMaxMaster
        {
            get;
            set;
        }
        int VMax
        {
            get;
            set;
        }
        int HBlankStart
        {
            get;
            set;
        }
        int HBlankEnd
        {
            get;
            set;
        }
        int HdmaInit
        {
            get;
            set;
        }
        int HdmaStart
        {
            get;
            set;
        }
        int NmiTriggerPos
        {
            get;
            set;
        }
        int IrqTriggerCycles
        {
            get;
            set;
        }
        int WramRefreshPos
        {
            get;
            set;
        }
        int RenderPos
        {
            get;
            set;
        }
        bool InterlaceField
        {
            get;
            set;
        }
        int DmaCpuSync
        {
            get;
            set;
        }
        int NmiDmaDelay
        {
            get;
            set;
        }
        int IrqPendingCount
        {
            get;
            set;
        }
        int ApuSpeedup
        {
            get;
            set;
        }
        bool ApuAllowTimeOverflow
        {
            get;
            set;
        }
    }
}
