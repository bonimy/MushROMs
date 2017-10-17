using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public class InputCommand
    {
        public int InputType
        {
            get;
            set;
        }
        public int MultiPress
        {
            get;
            set;
        }
        public bool ButtonNrpt
        {
            get;
            set;
        }

        public ButtonController ButtonController
        {
            get;
            private set;
        }

        public AxisController AxisController
        {
            get;
            private set;
        }

        public bool AimMouse0
        {
            get;
            set;
        }
        public bool AimMouse1
        {
            get;
            set;
        }
        public bool AimScope
        {
            get;
            set;
        }
        public bool AimJustifier0
        {
            get;
            set;
        }
        public bool AimJustifier1
        {
            get;
            set;
        }

        private byte[] Port
        {
            get;
            set;
        }
    }
}
