// <copyright file="M6502.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes.Emulator
{
    using System;

    public class M6502 : INesProcessor
    {
        public const int StatusOk = Int32.MaxValue;

        public const int NmiVector = 0xFFFA;

        public const int ResetVector = 0xFFFC;

        public const int IrqVector = 0xFFFE;

        private static readonly byte[] OpcodeSizes = new byte[0x100]
        {
            0, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 2, 2, 0,
            1, 1, 0, 0, 0, 1, 1, 0, 0, 2, 0, 0, 0, 2, 2, 0,
            2, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 2, 2, 2, 0,
            1, 1, 0, 0, 0, 1, 1, 0, 0, 2, 0, 0, 0, 2, 2, 0,
            0, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 2, 2, 2, 0,
            1, 1, 0, 0, 0, 1, 1, 0, 0, 2, 0, 0, 0, 2, 2, 0,
            0, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 2, 2, 2, 0,
            1, 1, 0, 0, 0, 1, 1, 0, 0, 2, 0, 0, 0, 2, 2, 0,
            0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 2, 2, 2, 0,
            1, 1, 0, 0, 1, 1, 1, 0, 0, 2, 0, 0, 0, 2, 0, 0,
            1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 0, 0, 2, 2, 2, 0,
            1, 1, 0, 0, 1, 1, 1, 0, 0, 2, 0, 0, 2, 2, 2, 0,
            1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 2, 2, 2, 0,
            1, 1, 0, 0, 0, 1, 1, 0, 0, 2, 0, 0, 0, 2, 2, 0,
            1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 2, 2, 2, 0,
            1, 1, 0, 0, 0, 1, 1, 0, 0, 2, 0, 0, 0, 2, 2, 0,
        };

        private static readonly byte[] CycleCost = new byte[]
        {
            7, 6, 0, 0, 0, 3, 5, 0, 3, 2, 2, 0, 0, 4, 6, 0,
            2, 5, 0, 0, 0, 4, 6, 0, 1, 4, 0, 0, 0, 4, 7, 0,
            6, 6, 0, 0, 3, 3, 5, 0, 4, 2, 2, 0, 4, 4, 6, 0,
            2, 5, 0, 0, 0, 4, 6, 0, 2, 4, 0, 0, 0, 4, 7, 0,
            6, 6, 0, 0, 0, 3, 5, 0, 3, 2, 2, 0, 3, 4, 6, 0,
            2, 5, 0, 0, 0, 4, 6, 0, 2, 4, 0, 0, 0, 4, 7, 0,
            6, 6, 0, 0, 0, 3, 5, 0, 4, 2, 2, 0, 5, 4, 6, 0,
            2, 5, 0, 0, 0, 4, 6, 0, 2, 4, 0, 0, 0, 4, 7, 0,
            0, 6, 0, 0, 3, 3, 3, 0, 2, 0, 2, 0, 4, 4, 4, 0,
            2, 8, 0, 0, 4, 4, 4, 0, 2, 5, 2, 0, 0, 5, 0, 0,
            2, 6, 2, 0, 3, 3, 3, 0, 2, 2, 2, 0, 4, 4, 4, 0,
            2, 5, 0, 0, 4, 4, 4, 0, 2, 4, 2, 0, 4, 4, 4, 0,
            2, 6, 0, 0, 3, 3, 5, 0, 2, 2, 2, 0, 4, 4, 6, 0,
            2, 5, 0, 0, 0, 4, 6, 0, 2, 4, 0, 0, 0, 4, 7, 0,
            2, 6, 0, 0, 3, 3, 5, 0, 2, 2, 2, 0, 4, 4, 6, 0,
            2, 5, 0, 0, 0, 4, 6, 0, 2, 4, 0, 0, 0, 4, 7, 0,
        };

        private ushort _pc;

        public M6502()
        {
            Memory = new PrgRom();
            Opcodes = CreateOpcodes();
        }

        public int CyclesRemaining
        {
            get;
            private set;
        }

        public int ElapsedTicks
        {
            get;
            private set;
        }

        public int Status
        {
            get;
            private set;
        }

        public PrgRom Memory
        {
            get;
        }

        public int PC
        {
            get
            {
                return _pc;
            }

            set
            {
                _pc = (ushort)value;
            }
        }

        public byte A
        {
            get;
            set;
        }

        int INesProcessor.A
        {
            get
            {
                return A;
            }

            set
            {
                A = (byte)value;
            }
        }

        public byte X
        {
            get;
            set;
        }

        int INesProcessor.X
        {
            get
            {
                return X;
            }

            set
            {
                X = (byte)value;
            }
        }

        public byte Y
        {
            get;
            set;
        }

        int INesProcessor.Y
        {
            get
            {
                return Y;
            }

            set
            {
                Y = (byte)value;
            }
        }

        public byte S
        {
            get;
            set;
        }

        int INesProcessor.S
        {
            get
            {
                return S;
            }

            set
            {
                S = (byte)value;
            }
        }

        public byte P
        {
            get;
            set;
        }

        int INesProcessor.P
        {
            get
            {
                return P;
            }

            set
            {
                P = (byte)value;
            }
        }

        public ProcessorState Flags
        {
            get
            {
                return (ProcessorState)P;
            }

            set
            {
                P = (byte)value;
            }
        }

        public bool Carry
        {
            get
            {
                return (Flags & ProcessorState.Carry) != 0;
            }

            set
            {
                if (value)
                {
                    Flags |= ProcessorState.Carry;
                }
                else
                {
                    Flags &= ~ProcessorState.Carry;
                }
            }
        }

        public bool Zero
        {
            get
            {
                return (Flags & ProcessorState.Zero) != 0;
            }

            set
            {
                if (value)
                {
                    Flags |= ProcessorState.Zero;
                }
                else
                {
                    Flags &= ~ProcessorState.Zero;
                }
            }
        }

        public bool Interrupt
        {
            get
            {
                return (Flags & ProcessorState.Irq) != 0;
            }

            set
            {
                if (value)
                {
                    Flags |= ProcessorState.Zero;
                }
                else
                {
                    Flags &= ~ProcessorState.Zero;
                }
            }
        }

        public bool Decimal
        {
            get
            {
                return (Flags & ProcessorState.Decimal) != 0;
            }

            set
            {
                if (value)
                {
                    Flags |= ProcessorState.Decimal;
                }
                else
                {
                    Flags &= ~ProcessorState.Decimal;
                }
            }
        }

        public bool Break
        {
            get
            {
                return (Flags & ProcessorState.Brk) != 0;
            }

            set
            {
                if (value)
                {
                    Flags |= ProcessorState.Brk;
                }
                else
                {
                    Flags &= ~ProcessorState.Brk;
                }
            }
        }

        public bool Overflow
        {
            get
            {
                return (Flags & ProcessorState.Overflow) != 0;
            }

            set
            {
                if (value)
                {
                    Flags |= ProcessorState.Overflow;
                }
                else
                {
                    Flags &= ~ProcessorState.Overflow;
                }
            }
        }

        public bool Negative
        {
            get
            {
                return (Flags & ProcessorState.Negative) != 0;
            }

            set
            {
                if (value)
                {
                    Flags |= ProcessorState.Negative;
                }
                else
                {
                    Flags &= ~ProcessorState.Negative;
                }
            }
        }

        public bool IrqPending
        {
            get;
            private set;
        }

        private Opcode[] Opcodes
        {
            get;
        }

        public byte this[int index]
        {
            get
            {
                return Memory[index];
            }

            set
            {
                Memory[index] = value;
            }
        }

        public void AddCycles(int cycles)
        {
            ElapsedTicks += cycles;
            CyclesRemaining -= cycles;
        }

        public int ReadImmediate8(int address)
        {
            return Memory[address];
        }

        public void WriteImmediate8(int address, int value)
        {
            Memory[address] = (byte)value;
        }

        public int ReadImmediate16(int address)
        {
            return
                ReadImmediate8(address) |
                (ReadImmediate8(address + 1) << 8);
        }

        void INesProcessor.WriteImmediate16(int address, int value)
        {
            throw new NotSupportedException();
        }

        public int ZeroPage(int address)
        {
            return ReadImmediate8((byte)address);
        }

        public int ZeroPageX(int address)
        {
            return (byte)(ReadImmediate8(address) + X);
        }

        public int ZeroPageY(int address)
        {
            return (byte)(ReadImmediate8(address) + Y);
        }

        public int Absolute(int address)
        {
            return ReadImmediate16(address);
        }

        public int AbsoluteX(int address)
        {
            return AddWithPageCross(ReadImmediate16(address), X);
        }

        public int AbsoluteY(int address)
        {
            return AddWithPageCross(ReadImmediate16(address), Y);
        }

        public int IndirectX(int address)
        {
            var zpx0 = ZeroPageX(address);
            var zpx1 = ZeroPageX(address + 1);
            var index =
                ReadImmediate8(zpx0) |
                (ReadImmediate8(zpx1) << 8);

            return Absolute(index);
        }

        public int IndirectY(int address)
        {
            var index = ReadImmediate8(address);
            return AbsoluteY(index);
        }

        public void Push(int value)
        {
            this[0x100 | S--] = (byte)value;
        }

        public void PushWord(int value)
        {
            Push((byte)(value >> 8));
            Push((byte)value);
        }

        public void PushState()
        {
            PushWord(PC);
            Push(P);
        }

        public byte Pull()
        {
            var value = this[0x100 | ++S];
            return value;
        }

        int INesProcessor.Pull()
        {
            return Pull();
        }

        public int PullWord()
        {
            var low = Pull();
            var high = Pull();
            var result = low | (high << 8);
            return result;
        }

        public void PullState()
        {
            P = Pull();
            PC = PullWord();
        }

        public void Rti()
        {
            PullState();
            if (!Interrupt && IrqPending)
            {
                Irq();
            }
        }

        public void Reset()
        {
            X = 0;
            Y = 0;
            S = 0;
            A = 0;
            P = 0x22;
            IrqPending = false;

            PC = ReadImmediate16(ResetVector);
            Status = StatusOk;
        }

        public void Nmi()
        {
            PushState();
            ClearInterrupt();
            PC = ReadImmediate16(NmiVector);
        }

        public void Irq()
        {
            if (Interrupt)
            {
                IrqPending = true;
                return;
            }

            PushState();
            ClearInterrupt();

            PC = ReadImmediate16(IrqVector);
            IrqPending = false;
        }

        public void Brk()
        {
            PushState();
            PC = ReadImmediate16(IrqVector);
            Break = true;
        }

        public int Execute(int cycles)
        {
            CyclesRemaining = cycles;
            while (CyclesRemaining > 0)
            {
                NextInstruction();
                if (Status != StatusOk)
                {
                    return Status;
                }
            }

            return StatusOk;
        }

        public void NextInstruction()
        {
            var instruction = Memory[PC];
            var opcode = Opcodes[instruction];
            if (opcode is null)
            {
                Status = PC;
                return;
            }

            var cycles = CycleCost[instruction];
            AddCycles(cycles);

            PC += OpcodeSizes[instruction] + 1;
            opcode();
            return;
        }

        private void ClearInterrupt()
        {
            P &= 0xEF;
            P |= 0x24;
        }

        private int AddWithPageCross(int value, int index)
        {
            var result = (ushort)value + (byte)index;

            // We lose one cycle if we cross a page boundary.
            if ((result & 0xFF00) != (value & 0xFF00))
            {
                CyclesRemaining--;
                ElapsedTicks++;
            }

            return (ushort)result;
        }

        private Opcode[] CreateOpcodes()
        {
            var read = new ReadHelper(this);
            var write = new WriteHelper(this);
            var op = new OpcodeHelper(this, read, write);
            var opcodes = new Opcode[0x100];

            opcodes[0x00] = Brk;
            opcodes[0x01] = op.Ora(read.IndirectX);
            opcodes[0x05] = op.Ora(read.ZeroPage);
            opcodes[0x06] = op.Asl(read.ZeroPage, write.ZeroPage);
            opcodes[0x08] = op.Push(read.P);
            opcodes[0x09] = op.Ora(read.Immediate);
            opcodes[0x0A] = op.Asl(read.A, write.A);
            opcodes[0x0D] = op.Ora(read.Absolute);
            opcodes[0x0E] = op.Asl(read.Absolute, write.Absolute);

            opcodes[0x10] = op.Branch(!Negative);
            opcodes[0x11] = op.Ora(read.IndirectY);
            opcodes[0x15] = op.Ora(read.ZeroPageX);
            opcodes[0x16] = op.Asl(read.ZeroPageX, write.ZeroPageX);
            opcodes[0x18] = op.Clc;
            opcodes[0x19] = op.Ora(read.AbsoluteY);
            opcodes[0x1D] = op.Ora(read.AbsoluteX);
            opcodes[0x1E] = op.Asl(read.AbsoluteX, write.AbsoluteX);

            opcodes[0x20] = op.Jsr(read.Immediate16);
            opcodes[0x21] = op.And(read.IndirectX);
            opcodes[0x24] = op.Bit(read.ZeroPage);
            opcodes[0x25] = op.And(read.ZeroPage);
            opcodes[0x26] = op.Rol(read.ZeroPage, write.ZeroPage);
            opcodes[0x28] = op.Pull(write.P);
            opcodes[0x29] = op.And(read.Immediate);
            opcodes[0x2A] = op.Rol(read.A, write.A);
            opcodes[0x2C] = op.Bit(read.Absolute);
            opcodes[0x2D] = op.And(read.Absolute);
            opcodes[0x2E] = op.Rol(read.Absolute, write.Absolute);

            opcodes[0x30] = op.Branch(Negative);
            opcodes[0x31] = op.And(read.IndirectY);
            opcodes[0x35] = op.And(read.ZeroPageX);
            opcodes[0x36] = op.Rol(read.ZeroPageX, write.ZeroPageX);
            opcodes[0x38] = op.Sec;
            opcodes[0x39] = op.And(read.AbsoluteY);
            opcodes[0x3D] = op.And(read.AbsoluteX);
            opcodes[0x3E] = op.Rol(read.AbsoluteX, write.AbsoluteX);

            opcodes[0x40] = Rti;
            opcodes[0x41] = op.Eor(read.IndirectX);
            opcodes[0x45] = op.Eor(read.ZeroPage);
            opcodes[0x46] = op.Lsr(read.ZeroPage, write.ZeroPage);
            opcodes[0x48] = op.Push(read.A);
            opcodes[0x49] = op.Eor(read.Immediate);
            opcodes[0x4A] = op.Lsr(read.A, write.A);
            opcodes[0x4C] = op.JmpAbsolute;
            opcodes[0x4D] = op.Eor(read.Absolute);
            opcodes[0x4E] = op.Lsr(read.Absolute, write.Absolute);

            opcodes[0x50] = op.Branch(!Overflow);
            opcodes[0x51] = op.Eor(read.IndirectY);
            opcodes[0x55] = op.Eor(read.ZeroPageX);
            opcodes[0x56] = op.Lsr(read.ZeroPageX, write.ZeroPageX);
            opcodes[0x58] = op.Cli;
            opcodes[0x59] = op.Eor(read.AbsoluteY);
            opcodes[0x5D] = op.Eor(read.AbsoluteX);
            opcodes[0x5E] = op.Lsr(read.AbsoluteX, write.AbsoluteX);

            opcodes[0x60] = op.Rts;
            opcodes[0x61] = op.Adc(read.IndirectX);
            opcodes[0x65] = op.Adc(read.ZeroPage);
            opcodes[0x66] = op.Ror(read.ZeroPage, write.ZeroPage);
            opcodes[0x68] = op.Pull(write.A);
            opcodes[0x69] = op.Adc(read.Immediate);
            opcodes[0x6A] = op.Ror(read.A, write.A);
            opcodes[0x6C] = op.JmpIndirect;
            opcodes[0x6D] = op.Adc(read.Absolute);
            opcodes[0x6E] = op.Ror(read.Absolute, write.Absolute);

            opcodes[0x70] = op.Branch(Overflow);
            opcodes[0x71] = op.Adc(read.IndirectY);
            opcodes[0x75] = op.Adc(read.ZeroPageX);
            opcodes[0x76] = op.Ror(read.ZeroPageX, write.ZeroPageX);
            opcodes[0x78] = op.Sei;
            opcodes[0x79] = op.Adc(read.AbsoluteY);
            opcodes[0x7D] = op.Adc(read.AbsoluteX);
            opcodes[0x7E] = op.Ror(read.AbsoluteX, write.AbsoluteX);

            opcodes[0x81] = op.Sta(write.IndirectX);
            opcodes[0x84] = op.Sty(write.ZeroPage);
            opcodes[0x85] = op.Sta(write.ZeroPage);
            opcodes[0x86] = op.Stx(write.ZeroPage);
            opcodes[0x88] = op.Dec(read.Y, write.Y);
            opcodes[0x8A] = op.Txa;
            opcodes[0x8C] = op.Sty(write.Absolute);
            opcodes[0x8D] = op.Sta(write.Absolute);
            opcodes[0x8E] = op.Stx(write.Absolute);

            opcodes[0x90] = op.Branch(!Carry);
            opcodes[0x91] = op.Sta(write.IndirectY);
            opcodes[0x94] = op.Sty(write.ZeroPageX);
            opcodes[0x95] = op.Sta(write.ZeroPageX);
            opcodes[0x96] = op.Stx(write.ZeroPageY);
            opcodes[0x98] = op.Tya;
            opcodes[0x99] = op.Sta(write.AbsoluteY);
            opcodes[0x9A] = op.Txs;
            opcodes[0x9D] = op.Sta(write.AbsoluteX);

            opcodes[0xA0] = op.Ldy(read.Immediate);
            opcodes[0xA1] = op.Lda(read.IndirectX);
            opcodes[0xA2] = op.Ldx(read.Immediate);
            opcodes[0xA4] = op.Ldy(read.ZeroPage);
            opcodes[0xA5] = op.Lda(read.ZeroPage);
            opcodes[0xA6] = op.Ldx(read.ZeroPage);
            opcodes[0xA8] = op.Tay;
            opcodes[0xA9] = op.Lda(read.Immediate);
            opcodes[0xAA] = op.Tax;
            opcodes[0xAC] = op.Ldy(read.Absolute);
            opcodes[0xAD] = op.Lda(read.Absolute);
            opcodes[0xAE] = op.Ldx(read.Absolute);

            opcodes[0xB0] = op.Branch(Carry);
            opcodes[0xB1] = op.Lda(read.IndirectY);
            opcodes[0xB4] = op.Ldy(read.ZeroPageX);
            opcodes[0xB5] = op.Lda(read.ZeroPageX);
            opcodes[0xB6] = op.Ldx(read.ZeroPageY);
            opcodes[0xB8] = op.Clv;
            opcodes[0xB9] = op.Lda(read.AbsoluteY);
            opcodes[0xBA] = op.Tsx;
            opcodes[0xBC] = op.Ldy(read.AbsoluteX);
            opcodes[0xBD] = op.Lda(read.AbsoluteX);
            opcodes[0xBE] = op.Ldx(read.AbsoluteY);

            opcodes[0xC0] = op.Cpy(read.Immediate);
            opcodes[0xC1] = op.Cmp(read.IndirectX);
            opcodes[0xC4] = op.Cpy(read.ZeroPage);
            opcodes[0xC5] = op.Cmp(read.ZeroPage);
            opcodes[0xC6] = op.Dec(read.ZeroPage, write.ZeroPage);
            opcodes[0xC8] = op.Inc(read.Y, write.Y);
            opcodes[0xC9] = op.Cmp(read.Immediate);
            opcodes[0xCA] = op.Dec(read.X, write.X);
            opcodes[0xCC] = op.Cpy(read.Absolute);
            opcodes[0xCD] = op.Cmp(read.Absolute);
            opcodes[0xCE] = op.Dec(read.Absolute, write.Absolute);

            opcodes[0xD0] = op.Branch(!Zero);
            opcodes[0xD1] = op.Cmp(read.IndirectY);
            opcodes[0xD5] = op.Cmp(read.ZeroPageX);
            opcodes[0xD6] = op.Dec(read.ZeroPageX, write.ZeroPageX);
            opcodes[0xD8] = op.Cld;
            opcodes[0xD9] = op.Cmp(read.AbsoluteY);
            opcodes[0xDD] = op.Cmp(read.AbsoluteX);
            opcodes[0xDE] = op.Dec(read.AbsoluteX, write.AbsoluteX);

            opcodes[0xE0] = op.Cpx(read.Immediate);
            opcodes[0xE1] = op.Sbc(read.IndirectX);
            opcodes[0xE4] = op.Cpx(read.ZeroPage);
            opcodes[0xE5] = op.Sbc(read.ZeroPage);
            opcodes[0xE6] = op.Inc(read.ZeroPage, write.ZeroPage);
            opcodes[0xE8] = op.Inc(read.X, write.X);
            opcodes[0xE9] = op.Sbc(read.Immediate);
            opcodes[0xEA] = op.Nop;
            opcodes[0xEC] = op.Cpx(read.Absolute);
            opcodes[0xED] = op.Sbc(read.Absolute);
            opcodes[0xEE] = op.Inc(read.Absolute, write.Absolute);

            opcodes[0xF0] = op.Branch(Zero);
            opcodes[0xF1] = op.Sbc(read.IndirectY);
            opcodes[0xF5] = op.Sbc(read.ZeroPageX);
            opcodes[0xF6] = op.Inc(read.ZeroPageX, write.ZeroPageX);
            opcodes[0xF8] = op.Sed;
            opcodes[0xF9] = op.Sbc(read.AbsoluteY);
            opcodes[0xFD] = op.Sbc(read.AbsoluteX);
            opcodes[0xFE] = op.Inc(read.AbsoluteX, write.AbsoluteX);

            return opcodes;
        }
    }
}
