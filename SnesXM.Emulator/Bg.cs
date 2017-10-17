namespace SnesXM.Emulator
{
    public struct Bg
    {
        public static readonly Bg Empty = new Bg();

        public int ScBase
        {
            get;
            set;
        }
        public int HOffset
        {
            get;
            set;
        }
        public int VOffset
        {
            get;
            set;
        }
        public int BgSize
        {
            get;
            set;
        }
        public int NameBase
        {
            get;
            set;
        }
        public int ScSize
        {
            get;
            set;
        }
    }
}
