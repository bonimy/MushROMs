// <copyright file="INesProcessor.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes
{
    public interface INesProcessor
    {
        int PC
        {
            get;
            set;
        }

        int A
        {
            get;
            set;
        }

        int X
        {
            get;
            set;
        }

        int Y
        {
            get;
            set;
        }

        int S
        {
            get;
            set;
        }

        int P
        {
            get;
            set;
        }

        bool Carry
        {
            get;
            set;
        }

        bool Zero
        {
            get;
            set;
        }

        bool Interrupt
        {
            get;
            set;
        }

        bool Decimal
        {
            get;
            set;
        }

        bool Break
        {
            get;
            set;
        }

        bool Overflow
        {
            get;
            set;
        }

        bool Negative
        {
            get;
            set;
        }

        byte this[int index]
        {
            get;
            set;
        }

        void AddCycles(int cycles);

        int ReadImmediate8(int address);

        void WriteImmediate8(int address, int value);

        int ReadImmediate16(int address);

        void WriteImmediate16(int address, int value);

        int ZeroPage(int address);

        int ZeroPageX(int address);

        int ZeroPageY(int address);

        int Absolute(int address);

        int AbsoluteX(int address);

        int AbsoluteY(int address);

        int IndirectX(int address);

        int IndirectY(int address);

        void Push(int value);

        void PushWord(int value);

        void PushState();

        int Pull();

        int PullWord();

        void PullState();

        void Reset();

        int Execute(int cycles);

        void NextInstruction();
    }
}
