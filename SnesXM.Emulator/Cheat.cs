using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public class Cheat
    {
        public int Address
        {
            get;
            set;
        }

        public byte Value
        {
            get;
            set;
        }

        public byte SavedValue
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public bool Saved
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
