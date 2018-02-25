// <copyright file="OpcodeHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes.Emulator
{
    using System;

    internal class OpcodeHelper
    {
        public OpcodeHelper(
            INesProcessor processor,
            ReadHelper read,
            WriteHelper write)
        {
            Processor = processor ??
                throw new ArgumentNullException(nameof(processor));

            Read = read ??
                throw new ArgumentNullException(nameof(read));

            Write = write ??
                throw new ArgumentNullException(nameof(write));
        }

        private INesProcessor Processor
        {
            get;
        }

        private int PC
        {
            get
            {
                return Processor.PC;
            }

            set
            {
                Processor.PC = value;
            }
        }

        private ReadHelper Read
        {
            get;
        }

        private WriteHelper Write
        {
            get;
        }

        public Opcode Branch(bool test)
        {
            return () =>
            {
                if (!test)
                {
                    PC++;
                    return;
                }

                Processor.AddCycles(1);

                var result = PC + (sbyte)Processor[PC] + 1;
                if ((result & 0x100) != (PC & 0x100))
                {
                    Processor.AddCycles(2);
                }

                PC = result;
            };
        }

        public void Clc()
        {
            Processor.Carry = false;
        }

        public void Cld()
        {
            Processor.Decimal = false;
        }

        public void Cli()
        {
            Processor.Decimal = false;
        }

        public void Clv()
        {
            Processor.Overflow = false;
        }

        public void Sec()
        {
            Processor.Carry = true;
        }

        public void Sed()
        {
            Processor.Decimal = true;
        }

        public void Sei()
        {
            Processor.Interrupt = true;
        }

        public void Sev()
        {
            Processor.Overflow = true;
        }

        public Opcode Push(ReadValueCallback read)
        {
            return () =>
            {
                var value = read();
                Processor.Push(value);
            };
        }

        public Opcode Pull(WriteValueCallback write)
        {
            return () =>
            {
                var value = Processor.Pull();
                write(value);
            };
        }

        public Opcode LoadToRegister(
            ReadValueCallback readValue,
            WriteValueCallback writeToRegister)
        {
            return () =>
            {
                var value = readValue();
                writeToRegister((byte)value);
                WriteNZFlags(value);
            };
        }

        public Opcode Lda(ReadValueCallback readValue)
        {
            return LoadToRegister(readValue, Write.A);
        }

        public Opcode Ldx(ReadValueCallback readValue)
        {
            return LoadToRegister(readValue, Write.X);
        }

        public Opcode Ldy(ReadValueCallback readValue)
        {
            return LoadToRegister(readValue, Write.Y);
        }

        public Opcode ModifyA(
            ReadValueCallback readValue,
            Func<int, int, int> binaryOperation)
        {
            return () =>
            {
                var a = Read.A();
                var value = readValue();
                var result = binaryOperation(a, value);
                Write.A(result);
                WriteNZFlags(result);
            };
        }

        public Opcode Adc(ReadValueCallback readValue)
        {
            return ModifyA(readValue, Adc);
        }

        public Opcode Sbc(ReadValueCallback readValue)
        {
            return ModifyA(readValue, Sbc);
        }

        public Opcode Ora(ReadValueCallback readValue)
        {
            return ModifyA(readValue, (x, y) => x | y);
        }

        public Opcode And(ReadValueCallback readValue)
        {
            return ModifyA(readValue, (x, y) => x & y);
        }

        public Opcode Eor(ReadValueCallback readValue)
        {
            return ModifyA(readValue, (x, y) => x ^ y);
        }

        public Opcode Bit(ReadValueCallback readValue)
        {
            return () =>
            {
                var value = readValue();
                var a = Read.A();
                Processor.Zero = (byte)(value & a) == 0;

                var v = value & (int)ProcessorState.Overflow;
                Processor.Overflow = v != 0;

                var n = value & (int)ProcessorState.Negative;
                Processor.Negative = n != 0;
            };
        }

        public Opcode Compare(
            ReadValueCallback readFirstValue,
            ReadValueCallback readSecondValue)
        {
            return () =>
            {
                var x = (byte)readFirstValue();
                var y = (byte)readSecondValue();
                var z = x.CompareTo(y);

                Processor.Carry = z >= 0;
                Processor.Zero = z == 0;
                Processor.Negative = z < 0;
            };
        }

        public Opcode Cmp(ReadValueCallback readValue)
        {
            return Compare(Read.A, readValue);
        }

        public Opcode Cpx(ReadValueCallback readValue)
        {
            return Compare(Read.X, readValue);
        }

        public Opcode Cpy(ReadValueCallback readValue)
        {
            return Compare(Read.Y, readValue);
        }

        public Opcode StoreFromRegister(
            ReadValueCallback readRegister,
            WriteValueCallback writeValue)
        {
            return () =>
            {
                var value = readRegister();
                writeValue(value);
            };
        }

        public Opcode Sta(WriteValueCallback writeValue)
        {
            return StoreFromRegister(Read.A, writeValue);
        }

        public Opcode Stx(WriteValueCallback writeValue)
        {
            return StoreFromRegister(Read.X, writeValue);
        }

        public Opcode Sty(WriteValueCallback writeValue)
        {
            return StoreFromRegister(Read.Y, writeValue);
        }

        public Opcode Inc(
            ReadValueCallback readValue,
            WriteValueCallback writeValue)
        {
            return ModifyValue(readValue, writeValue, value => value + 1);
        }

        public Opcode Dec(
            ReadValueCallback readValue,
            WriteValueCallback writeValue)
        {
            return ModifyValue(readValue, writeValue, value => value - 1);
        }

        public Opcode Asl(
            ReadValueCallback readValue,
            WriteValueCallback writeValue)
        {
            return ModifyValue(readValue, writeValue, Asl);
        }

        public Opcode Lsr(
            ReadValueCallback readValue,
            WriteValueCallback writeValue)
        {
            return ModifyValue(readValue, writeValue, Lsr);
        }

        public Opcode Rol(
            ReadValueCallback readValue,
            WriteValueCallback writeValue)
        {
            return ModifyValue(readValue, writeValue, Rol);
        }

        public Opcode Ror(
            ReadValueCallback readValue,
            WriteValueCallback writeValue)
        {
            return ModifyValue(readValue, writeValue, Ror);
        }

        public Opcode ModifyValue(
            ReadValueCallback readValue,
            WriteValueCallback writeValue,
            Func<int, int> modify)
        {
            return () =>
            {
                var value = readValue();
                var result = modify(value);

                writeValue(result);
                WriteNZFlags(result);
            };
        }

        public Opcode Jmp(ReadValueCallback readValue)
        {
            return () =>
            {
                var value = readValue();
                PC = value;
            };
        }

        public void JmpAbsolute()
        {
            Jmp(Read.Immediate16);
        }

        public void JmpIndirect()
        {
            Jmp(ReadIndirect);

            int ReadIndirect()
            {
                var address = Processor.ReadImmediate16(PC);
                var value = Processor.ReadImmediate16(address);
                return value;
            }
        }

        public Opcode Jsr(ReadValueCallback readValue)
        {
            return () =>
            {
                Processor.PushWord(PC + 1);
                Jmp(readValue);
            };
        }

        public void Rts()
        {
            PC = Processor.PullWord() + 1;
        }

        public void Nop()
        {
            // Do nothing.
        }

        public void Tax()
        {
            Lda(Read.X);
        }

        public void Tay()
        {
            Lda(Read.Y);
        }

        public void Txa()
        {
            Ldx(Read.A);
        }

        public void Txy()
        {
            Ldx(Read.Y);
        }

        public void Tya()
        {
            Ldy(Read.A);
        }

        public void Tyx()
        {
            Ldy(Read.X);
        }

        public void Txs()
        {
            Processor.S = Processor.X;
        }

        public void Tsa()
        {
            LoadToRegister(Read.A, Write.S);
        }

        public void Tsx()
        {
            LoadToRegister(Read.X, Write.S);
        }

        public void Tsy()
        {
            LoadToRegister(Read.Y, Write.S);
        }

        private int Adc(int left, int right)
        {
            var c = Processor.P & 1;
            var result = left + right + c;

            var sign1 = left & 0x80;
            var sign2 = right & 0x80;
            var sign3 = result & 0x80;
            var overflow = (sign1 == sign2) && (sign1 != sign3);

            Processor.Carry = result > Byte.MaxValue;
            Processor.Overflow = overflow;

            return result;
        }

        private int Sbc(int left, int right)
        {
            var c = Processor.P & 1;
            var result = left - right - (c ^ 1);

            var sign1 = left & 0x80;
            var sign2 = -right & 0x80;
            var sign3 = result & 0x80;
            var overflow = (sign1 == sign2) && (sign1 != sign3);

            Processor.Carry = result <= Byte.MaxValue;
            Processor.Overflow = overflow;

            return result;
        }

        private int Asl(int value)
        {
            var result = value << 1;
            Processor.Carry = result > Byte.MaxValue;
            return (byte)result;
        }

        private int Lsr(int value)
        {
            var result = value << 1;
            Processor.Carry = (value & 1) != 0;
            return (byte)result;
        }

        private int Rol(int value)
        {
            var result = value << 1;
            var bit = Processor.Carry ? 1 : 0;
            Processor.Carry = result > Byte.MaxValue;
            result |= bit;
            return (byte)result;
        }

        private int Ror(int value)
        {
            var result = value >> 1;
            var bit = Processor.Carry ? 0x80 : 0;
            Processor.Carry = (value & 1) != 0;
            result |= bit;
            return (byte)result;
        }

        private void WriteNZFlags(int value)
        {
            Processor.Zero = (byte)value == 0;
            Processor.Negative = (sbyte)value < 0;
        }
    }
}
