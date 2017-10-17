namespace SnesXM.Emulator
{
    public struct AxisPointer
    {
        public int Port
        {
            get;
            set;
        }
        public int Speed
        {
            get;
            set;
        }
        public bool Invert
        {
            get;
            set;
        }
        public bool IsVertical
        {
            get;
            set;
        }
    }
}
