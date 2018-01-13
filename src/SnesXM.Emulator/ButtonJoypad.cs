namespace SnesXM.Emulator
{
    public struct ButtonJoypad
    {
        public int PadNumber
        {
            get;
            set;
        }

        public bool Toggle
        {
            get;
            set;
        }

        public bool Turbo
        {
            get;
            set;
        }

        public bool Sticky
        {
            get;
            set;
        }

        public int Buttons
        {
            get;
            set;
        }
    }
}
