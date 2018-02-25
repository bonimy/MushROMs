// <copyright file="WriteHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes.Emulator
{
    using System;

    internal class WriteHelper
    {
        public WriteHelper(INesProcessor processor)
        {
            Processor = processor ??
                throw new ArgumentNullException(nameof(processor));
        }

        public WriteValueCallback A
        {
            get
            {
                return value => Processor.A = value;
            }
        }

        public WriteValueCallback X
        {
            get
            {
                return value => Processor.X = value;
            }
        }

        public WriteValueCallback Y
        {
            get
            {
                return value => Processor.Y = value;
            }
        }

        public WriteValueCallback S
        {
            get
            {
                return value => Processor.S = value;
            }
        }

        public WriteValueCallback P
        {
            get
            {
                return value => Processor.P = value;
            }
        }

        public WriteValueCallback Immediate8
        {
            get
            {
                return value => Processor.WriteImmediate8(PC++, value);
            }
        }

        public WriteValueCallback ZeroPage
        {
            get
            {
                return WriteByte(Processor.ZeroPage);
            }
        }

        public WriteValueCallback ZeroPageX
        {
            get
            {
                return WriteByte(Processor.ZeroPageX);
            }
        }

        public WriteValueCallback ZeroPageY
        {
            get
            {
                return WriteByte(Processor.ZeroPageY);
            }
        }

        public WriteValueCallback Absolute
        {
            get
            {
                return WriteWord(Processor.Absolute);
            }
        }

        public WriteValueCallback AbsoluteX
        {
            get
            {
                return WriteWord(Processor.AbsoluteX);
            }
        }

        public WriteValueCallback AbsoluteY
        {
            get
            {
                return WriteWord(Processor.AbsoluteY);
            }
        }

        public WriteValueCallback IndirectX
        {
            get
            {
                return WriteByte(Processor.IndirectX);
            }
        }

        public WriteValueCallback IndirectY
        {
            get
            {
                return WriteByte(Processor.IndirectY);
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

        private WriteValueCallback WriteByte(Func<int, int> read)
        {
            return value =>
            {
                var address = read(PC - 1);
                Processor.WriteImmediate8(address, value);
            };
        }

        private WriteValueCallback WriteWord(Func<int, int> read)
        {
            return value =>
            {
                var address = read(PC - 2);
                Processor.WriteImmediate8(address, value);
            };
        }
    }
}
