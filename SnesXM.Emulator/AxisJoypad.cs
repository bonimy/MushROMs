namespace SnesXM.Emulator
{
    public struct AxisJoypad
    {
        public int Port
        {
            get;
            set;
        }
        public bool Invert
        {
            get;
            set;
        }
        public int Axis
        {
            get;
            set;
        }
        public int Threshold
        {
            get;
            set;
        }
    }
}
