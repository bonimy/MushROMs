using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public delegate void Opcode();

    public class InternalCpu
    {
        private Opcode[] Opcodes
        {
            get;
            set;
        }
        private byte[] OpcodeLengths
        {
            get;
            set;
        }
        public int Carry
        {
            get;
            set;
        }
        public int Zero
        {
            get;
            set;
        }
        public int Negative
        {
            get;
            set;
        }
        public int Overflow
        {
            get;
            set;
        }
        public int ShiftedPb
        {
            get;
            set;
        }
        public int ShiftedDb
        {
            get;
            set;
        }
        public int Frame
        {
            get;
            set;
        }
        public int FrameAdvanceCount
        {
            get;
            set;
        }

        public Opcode this[int index] => Opcodes[(byte)index];

        public void MainLoop()
        {
            _loop:



            goto _loop;
        }
    }
}
