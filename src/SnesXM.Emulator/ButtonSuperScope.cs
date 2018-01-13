using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public struct ButtonSuperScope
    {
        public bool Fire
        {
            get;
            set;
        }
        public bool Cursor
        {
            get;
            set;
        }
        public bool Turbo
        {
            get;
            set;
        }
        public bool Pause
        {
            get;
            set;
        }
        public bool AimOffscreen
        {
            get;
            set;
        }
    }
}
