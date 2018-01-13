using System;
using Helper;

namespace SnesXM.Emulator
{
    public class Cart : ICart
    {
        public int Size
        {
            get;
            set;
        }
        public int SramSize
        {
            get;
            set;
        }
        public int SramMask
        {
            get;
            set;
        }
        public int Offset
        {
            get;
            set;
        }
        public Pointer<byte> Sram
        {
            get;
            set;
        }
        public string FileName
        {
            get;
            set;
        }

        public Cart()
        {
            FileName = String.Empty;
        }

        public void Initialize()
        {
            Size =
            SramSize =
            SramMask =
            Offset = 0;
            Sram = null;
            FileName = String.Empty;
        }
    }
}
