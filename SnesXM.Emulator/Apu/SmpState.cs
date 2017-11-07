using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator.Apu
{
    internal class SmpState
    {
        private byte[] Header
        {
            get;
            set;
        }

        private byte[] IdTag
        {
            get;
            set;
        }

        private byte VersionMinor
        {
            get;
            set;
        }

        private byte PcLow
        {
            get;
            set;
        }

        private byte PcHigh
        {
            get;
            set;
        }

        private byte A
        {
            get;
            set;
        }

        private byte X
        {
            get;
            set;
        }

        private byte Y
        {
            get;
            set;
        }

        private byte PsW
        {
            get;
            set;
        }

        private byte Sp
        {
            get;
            set;
        }

        private byte[] UnusedA
        {
            get;
            set;
        }

        private byte[] Id666
        {
            get;
            set;
        }

        private byte[] ApuRam
        {
            get;
            set;
        }

        private byte[] DspRegisters
        {
            get;
            set;
        }

        private byte[] UnusedB
        {
            get;
            set;
        }

        private byte[] IplRom
        {
            get;
            set;
        }

        public SmpState()
        {
            Header = new byte[33];
            IdTag = new byte[3];
            UnusedA = new byte[2];

            Id666 = new byte[210];

            ApuRam = new byte[0x10000];
            DspRegisters = new byte[0x80];
            UnusedA = new byte[0x40];
            IplRom = new byte[0x40];
        }
    }
}
