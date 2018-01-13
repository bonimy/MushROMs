using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public struct Vma
    {
        public bool High
        {
            get;
            set;
        }
        public int Increment
        {
            get;
            set;
        }
        public int Address
        {
            get;
            set;
        }
        public int Mask1
        {
            get;
            set;
        }
        public int FullGraphicCount
        {
            get;
            set;
        }
        public int Shift
        {
            get;
            set;
        }
    }
}
