using System;
using System.Collections.Generic;
using System.Text;
using Helper;

namespace SnesXM.Emulator
{
    public class Cpu
    {
        #region State
        CpuDebugModes Flags
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

        bool InDmaOrHdma
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

        CpuEvent WhichEvent
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
#endregion

        private ISettings Settings { get; }
        private ITimings Timings { get; }
        private IPpu Ppu { get; }
        private IInternalPpu InternalPpu { get; }
        private IDma Dma { get; }
        private ISuperFx SuperFx { get; }
        private IApu Apu { get; }
        private IMemoryMap Memory { get; }

        public Registers Registers
        {
            get;
            private set;
        }

        public Cpu()
        {
            Registers = new Registers();
        }

        public void Reset()
        {
            SoftReset();
            Registers.SL = 0xFF;
            Registers.P = 0;
            Registers.A = 0;
            Registers.X = 0;
            Registers.Y = 0;
            SetFlags(
                RegisterBits.MemoryFlag |
                RegisterBits.IndexFlag |
                RegisterBits.Emulation);
            ClearFlags(RegisterBits.Decimal);
        }

        public void SoftReset()
        {
            Cycles = 182;
            PreviousCycles = Cycles;
            VCounter = 0;
            Flags &= CpuDebugModes.DebugMode | CpuDebugModes.Trace;
            PcBase = null;
            NmiLine = false;
            IrqLine = false;
            IrqTransition = false;
            IrqLastState = false;
            IrqExternal = false;
            IrqPending = Timings.IrqPendingCount;
            MemSpeed = (int)MemSpeedCycles.SlowOneCycle;
            MemSpeedx2 = (int)MemSpeedCycles.SlowOneCycle * 2;
            FastRomSpeed = (int)MemSpeedCycles.SlowOneCycle;
            InDma = false;
            InHdma = false;
            InDmaOrHdma = false;
            InWramDmaorHdma = false;
            HdmaRanInDma = 0;
            CurrentDmaorHdmaChannel = -1;
            WhichEvent = CpuEvent.Render;
            NextEvent = Timings.RenderPos;
            WaitingForInterrupt = false;
            AutoSaveTimer = 0;
            SramModified = false;

            Registers.Pc = 0;
            throw new NotImplementedException();
        }

        void MainLoop()
        {
            _loop:
            if (NmiLine)
            {
                if (Timings.NmiTriggerPos <= Cycles)
                {
                    NmiLine = false;
                    Timings.NmiTriggerPos = 0xFFFF;
                    if (WaitingForInterrupt)
                    {
                        WaitingForInterrupt = false;
                        Registers.PcW++;
                    }

                    throw new NotImplementedException();
                }
            }

            if (IrqTransition || IrqExternal)
            {
                if (IrqPending == 0)
                {
                    if (WaitingForInterrupt)
                    {
                        WaitingForInterrupt = false;
                        Registers.PcW++;
                    }

                    IrqTransition = false;
                    IrqPending = Timings.IrqPendingCount;

                    if (!CheckAnyFlag(RegisterBits.Irq))
                        throw new NotImplementedException();
                }
                else
                    IrqPending--;
            }

            if ((Flags & CpuDebugModes.ScanKey) != 0)
                goto _endLoop;

            byte instruction;
            Opcode[] opcodes;

            if (PcBase != null)
            {
                instruction = PcBase[Registers.PcW];
                PreviousCycles = Cycles;
                Cycles += MemSpeed;
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }

            if (false)
            {
                throw new NotImplementedException();
            }

            Registers.PcW++;
            opcodes[instruction]();

            if (Settings.Sa1)
                throw new NotImplementedException();

            goto _loop;

            _endLoop:
            throw new NotImplementedException();
        }

        private void Reschedule()
        {
            switch (WhichEvent)
            {
            case CpuEvent.HBlankStart:
                WhichEvent = CpuEvent.HdmaStart;
                NextEvent = Timings.HdmaStart;
                break;

            case CpuEvent.HdmaStart:
                WhichEvent = CpuEvent.HCounterMax;
                NextEvent = Timings.HMax;
                break;

            case CpuEvent.HCounterMax:
                WhichEvent = CpuEvent.HdmaInit;
                NextEvent = Timings.HdmaInit;
                break;

            case CpuEvent.HdmaInit:
                WhichEvent = CpuEvent.Render;
                NextEvent = Timings.RenderPos;
                break;

            case CpuEvent.Render:
                WhichEvent = CpuEvent.WramRefresh;
                NextEvent = Timings.WramRefreshPos;
                break;

            case CpuEvent.WramRefresh:
                WhichEvent = CpuEvent.HBlankStart;
                NextEvent = Timings.HBlankStart;
                break;
            }
        }

        private void AddCycle(int cycles)
        {
            PreviousCycles = Cycles;
            Cycles += cycles;
            CheckInterrups();
            while (Cycles >= NextEvent)
                DoHEventProcessing();
        }

        private void CheckInterrups()
        {
            var irq = Ppu.HTimerEnabled || Ppu.VTimerEnabled;

            if (IrqLine && irq)
                IrqTransition = true;

            if (Ppu.HTimerEnabled)
            {
                var htime = Ppu.HTimerPosition;
                if (Cycles >= Timings.HMax && htime < PreviousCycles)
                    htime += Timings.HMax;

                if (PreviousCycles >= htime || Cycles < htime)
                    irq = false;
            }

            if (Ppu.VTimerEnabled)
            {
                var vcounter = VCounter;
                if (Cycles >= Timings.HMax && (!Ppu.HTimerEnabled || Ppu.HTimerPosition < PreviousCycles))
                {
                    vcounter++;
                    if (vcounter >= Timings.VMax)
                        vcounter = 0;
                }

                if (vcounter == Ppu.VTimerPosition)
                    irq = false;
            }

            if (IrqLastState && irq)
            {
                IrqLine = true;
            }

            IrqLastState = irq;
        }

        private void DoHEventProcessing()
        {
            switch (WhichEvent)
            {
            case CpuEvent.HBlankStart:
                Reschedule();
                break;

            case CpuEvent.HdmaStart:
                Reschedule();

                // ToDo: Make this a PPU function
                if (Ppu.Hdma != 0 && VCounter <= Ppu.ScreenHeight)
                {
                    Ppu.Hdma = Dma.DoHdma(Ppu.Hdma);
                }

                break;

            case CpuEvent.HCounterMax:
                // ToDo: Make this a SuperFX function
                if (Settings.SuperFx)
                {
                    if (!SuperFx.OneLineDone)
                        SuperFx.Execute();
                    SuperFx.OneLineDone = false;
                }

                Apu.EndScanline();
                Cycles -= Timings.HMax;
                PreviousCycles -= Timings.HMax;
                Apu.SetReferenceTime(Cycles);

                // ToDo: Make this a Timings function
                if ((Timings.NmiTriggerPos != 0xFFFF) && (Timings.NmiTriggerPos >= Timings.HMax))
                    Timings.NmiTriggerPos -= Timings.HMax;

                VCounter++;
                if (VCounter >= Timings.VMax)
                {
                    VCounter = 0;
                    Timings.InterlaceField ^= true;

                    if (InternalPpu.Interlace && !Timings.InterlaceField)
                        Timings.VMax = Timings.VMaxMaster + 1;
                    else
                        Timings.VMax = Timings.VMaxMaster;

                    Memory.FillRam[0x213F] ^= 0x80;
                    Ppu.RangeTimeOver = 0;

                    throw new NotImplementedException();
                }

                throw new NotImplementedException();

            default:
                throw new NotImplementedException();
            }
        }

        private void SetFlags(RegisterBits bits)
        {
            Registers.P |= (int)bits;
        }
        private void ClearFlags(RegisterBits bits)
        {
            Registers.P &= ~(int)bits;
        }
        private bool CheckAnyFlag(RegisterBits bits)
        {
            return (Registers.PL & (int)bits) != 0;
        }
        private bool CheckAllFlags(RegisterBits bits)
        {
            return (Registers.PL & (int)bits) == (int)bits;
        }
    }
}
