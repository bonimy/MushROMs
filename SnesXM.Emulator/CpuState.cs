using System;
using System.Collections.Generic;
using System.Text;
using Helper;

namespace SnesXM.Emulator
{
    class CpuState
    {
        int Flags
        {
            get;
            set;
        }

        int Cycles
        {
            get;
            set;
        }

        int PreviousCycles
        {
            get;
            set;
        }

        int VCounter
        {
            get;
            set;
        }

        public Pointer<byte> PcBase
        {
            get;
            set;
        }

        bool NmiLine
        {
            get;
            set;
        }

        bool IrqLine
        {
            get;
            set;
        }

        bool IrqTransition
        {
            get;
            set;
        }

        bool IrqLastState
        {
            get;
            set;
        }

        bool IrqExternal
        {
            get;
            set;
        }

        int IrqPending
        {
            get;
            set;
        }

        int MemSpeed
        {
            get;
            set;
        }

        int MemSpeedx2
        {
            get;
            set;
        }

        int FastRomSpeed
        {
            get;
            set;
        }

        bool InDma
        {
            get;
            set;
        }

        bool InHdma
        {
            get;
            set;
        }

        bool InDmaorHdma
        {
            get;
            set;
        }

        bool InWramDmaorHdma
        {
            get;
            set;
        }

        byte HdmaRanInDma
        {
            get;
            set;
        }

        int CurrentDmaorHdmaChannel
        {
            get;
            set;
        }

        byte WhichEvent
        {
            get;
            set;
        }

        int NextEvent
        {
            get;
            set;
        }

        bool WaitingForInterrupt
        {
            get;
            set;
        }

        int AutoSaveTimer
        {
            get;
            set;
        }

        bool SramModified
        {
            get;
            set;
        }
    }
}
