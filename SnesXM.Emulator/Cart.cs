using System;
using Helper;

namespace SnesXM.Emulator
{
    public class Cart : ICart
    {
        public int CartSize
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
        public int CartOffset
        {
            get;
            set;
        }
        public SharedArray<byte> Sram
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
            CartSize =
            SramSize =
            SramMask =
            CartOffset = 0;
            Sram = null;
            FileName = String.Empty;
        }
    }
}
