namespace SnesXM.Emulator
{
    public struct Obj
    {
        public static readonly Obj Empty = new Obj();

        int HPos
        {
            get;
            set;
        }
        int VPos
        {
            get;
            set;
        }
        int HFlip
        {
            get;
            set;
        }
        int VFlip
        {
            get;
            set;
        }
        int Name
        {
            get;
            set;
        }
        int Priority
        {
            get;
            set;
        }
        int Palette
        {
            get;
            set;
        }
        int Size
        {
            get;
            set;
        }
    }
}
