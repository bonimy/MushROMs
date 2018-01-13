namespace SnesXM.Emulator
{
    public class Registers
    {
        private byte _db;
        private Pair16 _p;
        private Pair16 _a;
        private Pair16 _d;
        private Pair16 _s;
        private Pair16 _x;
        private Pair16 _y;
        private Pair32 _pc;

        public int Db
        {
            get => _db;
            set => _db = (byte)value;
        }

        public Pair16 P
        {
            get => _p;
            set => _p = value;
        }
        public int PL
        {
            get => _p.Low;
            set => _p.Low = value;
        }
        public int PH
        {
            get => _p.High;
            set => _p.High = value;
        }

        public Pair16 A
        {
            get => _a;
            set => _a = value;
        }
        public int AL
        {
            get => _a.Low;
            set => _a.Low = value;
        }
        public int AH
        {
            get => _a.High;
            set => _a.High = value;
        }

        public Pair16 D
        {
            get => _d;
            set => _d = value;
        }
        public int DL
        {
            get => _d.Low;
            set => _d.Low = value;
        }
        public int DH
        {
            get => _d.High;
            set => _d.High = value;
        }

        public Pair16 S
        {
            get => _s;
            set => _s = value;
        }
        public int SL
        {
            get => _s.Low;
            set => _s.Low = value;
        }
        public int SH
        {
            get => _s.High;
            set => _s.High = value;
        }

        public Pair16 X
        {
            get => _x;
            set => _x = value;
        }
        public int XL
        {
            get => _x.Low;
            set => _x.Low = value;
        }
        public int XH
        {
            get => _x.High;
            set => _x.High = value;
        }

        public Pair16 Y
        {
            get => _y;
            set => _y = value;
        }
        public int YL
        {
            get => _y.Low;
            set => _y.Low = value;
        }
        public int YH
        {
            get => _y.High;
            set => _y.High = value;
        }

        public Pair32 Pc
        {
            get => _pc;
            set => _pc = value;
        }
        public int PcL
        {
            get => _pc.Low;
            set => _pc.Low = value;
        }
        public int PcH
        {
            get => _pc.High;
            set => _pc.High = value;
        }
        public int PcW
        {
            get => _pc.Word;
            set => _pc.Word = value;
        }
        public int PcB
        {
            get => _pc.Bank;
            set => _pc.Bank = value;
        }
    }
}
