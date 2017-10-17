using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public class ButtonController
    {
        public ButtonJoypad Joypad
        {
            get;
            set;
        }

        public ButtonMouse Mouse
        {
            get;
            set;
        }

        public ButtonSuperScope SuperScope
        {
            get;
            set;
        }

        public ButtonPointer Pointer
        {
            get;
            set;
        }

        public ButtonJustifier Justifier
        {
            get;
            set;
        }

        public int MultiIdx
        {
            get;
            set;
        }

        public int Command
        {
            get;
            set;
        }
    }
}
