// <copyright file="ReadHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes.Emulator
{
    using System;

    internal class ReadHelper
    {
        public ReadHelper(INesProcessor processor)
        {
            Processor = processor ??
                throw new ArgumentNullException(nameof(processor));
        }

        public ReadValueCallback A
        {
            get
            {
                return () => Processor.A;
            }
        }

        public ReadValueCallback X
        {
            get
            {
                return () => Processor.X;
            }
        }

        public ReadValueCallback Y
        {
            get
            {
                return () => Processor.Y;
            }
        }

        public ReadValueCallback S
        {
            get
            {
                return () => Processor.S;
            }
        }

        public ReadValueCallback P
        {
            get
            {
                return () => Processor.P;
            }
        }

        public ReadValueCallback Immediate
        {
            get
            {
                return () => Processor.ReadImmediate8(PC++);
            }
        }

        public ReadValueCallback Immediate16
        {
            get
            {
                return () =>
                {
                    var value = Processor.ReadImmediate16(PC);
                    PC += 2;
                    return value;
                };
            }
        }

        public ReadValueCallback ZeroPage
        {
            get
            {
                return ReadByte(Processor.ZeroPage);
            }
        }

        public ReadValueCallback ZeroPageX
        {
            get
            {
                return ReadByte(Processor.ZeroPageX);
            }
        }

        public ReadValueCallback ZeroPageY
        {
            get
            {
                return ReadByte(Processor.ZeroPageY);
            }
        }

        public ReadValueCallback Absolute
        {
            get
            {
                return ReadWord(Processor.Absolute);
            }
        }

        public ReadValueCallback AbsoluteX
        {
            get
            {
                return ReadWord(Processor.AbsoluteX);
            }
        }

        public ReadValueCallback AbsoluteY
        {
            get
            {
                return ReadWord(Processor.AbsoluteY);
            }
        }

        public ReadValueCallback IndirectX
        {
            get
            {
                return ReadByte(Processor.IndirectX);
            }
        }

        public ReadValueCallback IndirectY
        {
            get
            {
                return ReadByte(Processor.IndirectY);
            }
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

        private ReadValueCallback ReadByte(Func<int, int> read)
        {
            return () =>
            {
                var address = read(PC - 1);
                return Processor.ReadImmediate8(address);
            };
        }

        private ReadValueCallback ReadWord(Func<int, int> read)
        {
            return () =>
            {
                var address = read(PC - 2);
                return Processor.ReadImmediate8(address);
            };
        }
    }
}
