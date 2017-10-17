using System;
using Helper;

namespace SnesXM.Emulator
{
    public class CheatData
    {
        public const int MaxCheats = 150;

        private Cheat[] Cheats
        {
            get;
            set;
        }

        public int Size
        {
            get;
            private set;
        }

        private byte[] CwRam
        {
            get;
            set;
        }
        private byte[] CsRam
        {
            get;
            set;
        }
        private byte[] CiRam
        {
            get;
            set;
        }
        private Pointer<byte> Ram
        {
            get;
            set;
        }
        private Pointer<byte> FillRam
        {
            get;
            set;
        }
        private Pointer<byte> Sram
        {
            get;
            set;
        }
        private uint[] AllBits
        {
            get;
            set;
        }
        private byte[] WatchRam
        {
            get;
            set;
        }

        private Watch[] Watches
        {
            get;
            set;
        }

        public CheatData()
        {
            Cheats = new Cheat[MaxCheats];

            CwRam = new byte[0x20000];
            CsRam = new byte[0x10000];
            CiRam = new byte[0x2000];

            AllBits = new uint[0x32000 >> 5];
            WatchRam = new byte[0x32000];

            Watches = new Watch[0x10];
        }

        private static bool IsHexDigit(char c)
        {
            return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
        }

        private bool IsAllHex(string text, int index, int length)
        {
            for (var i = index + length; --i >= 0;)
                if (!IsHexDigit(text[i]))
                    return false;
            return true;
        }

        public void StartCheatSearch()
        {
            Ram.CopyTo(CwRam, 0x20000);
            Sram.CopyTo(CsRam, 0x10000);
            FillRam.CopyTo(0x3000, CiRam, 0x2000);
            for (var i = AllBits.Length; --i >= 0;)
                AllBits[i] = 0xFFFFFFFF;
        }

        public void SearchForChange(CheatComparisonMode cmp, CheatDataSize size, bool signed, bool update)
        {
            var l = (int)size;

            if (signed)
            {
                for (var i = 0; i < 0x20000 - l; i++)
                {

                }
            }

            throw new NotImplementedException();
        }
    }
}
