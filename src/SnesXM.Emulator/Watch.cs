using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public struct Watch
    {
        public bool On
        {
            get;
            private set;
        }
        public int Size
        {
            get;
            private set;
        }
        public int Format
        {
            get;
            private set;
        }
        public int Address
        {
            get;
            private set;
        }
        public string Buffer
        {
            get;
            private set;
        }
        public string Description
        {
            get;
            private set;
        }
    }
}
