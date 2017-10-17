using System;
using Helper;

namespace SnesXM.Emulator.Apu
{
    public delegate void SpcSnapshotMethod();

    public partial class SpcDsp
    {
        public const int VoiceCount       = 8;
        public const int RegisterCount    = 0x80;
        private const int StateSize       = 640;
        private const int ExtraSize       = 16;
        private const int EchoHistorySize = 8;
        private const int BrrBufferSize   = 12;
        private const int BrrBlockSize    = 9;

        public const int MVolLeftIndex  = 0x0C;
        public const int MVolRightIndex = 0x1C;
        public const int EVolLeftIndex  = 0x2C;
        public const int EVolRightIndex = 0x3C;
        public const int KOnIndex       = 0x4C;
        public const int KOffIndex      = 0x5C;
        public const int FlgIndex       = 0x6C;
        public const int EndXIndex      = 0x7C;
        public const int EfbIndex       = 0x0D;
        public const int PmonIndex      = 0x2D;
        public const int NonIndex       = 0x3D;
        public const int EonIndex       = 0x4D;
        public const int DirIndex       = 0x5D;
        public const int EsaIndex       = 0x6D;
        public const int EdlIndex       = 0x7D;
        public const int FirIndex       = 0x0F;  // 8 coefficients at 0x0F, 0x1F ... 0x7F

        /// <summary>
        /// A 16-bit mask for which voice (8 voices; 2 channels), determining
        /// whether that voice is enabled.
        /// </summary>
        private int StereoSwitch
        {
            get;
            set;
        }
        private bool TakeSpcSnapshot
        {
            get;
            set;
        }
        private SpcSnapshotMethod SpcSnapshotMethod
        {
            get;
            set;
        }

        private State M;

        public int SampleCount => M.SampleCount;

        public SpcDsp(byte[] ram)
        {
            M = new State(this, ram);

            MuteVoices(0);
            DisableSurround(false);
            SetOutput(null, 0);
            Reset();

            StereoSwitch = 0xFFFF;
            TakeSpcSnapshot = false;
            SpcSnapshotMethod = null;
        }

        public void Reset()
        {
            M.Reset();
        }

        public void MuteVoices(int mask)
        {
            M.MuteVoices(mask);
        }

        private void CopyState()
        {
            throw new NotImplementedException();
        }

        private bool CheckKon()
        {
            var old = M.KOnCheck;
            M.KOnCheck = false;
            return old;
        }

        private void DumpSpcSnapshot()
        {
            TakeSpcSnapshot = true;
        }

        private void CallSnapshot()
        {
            if (TakeSpcSnapshot)
            {
                TakeSpcSnapshot = false;
                SpcSnapshotMethod?.Invoke();
            }
        }

        private void DisableSurround(bool value)
        {
            // Not supported
        }

        private void SetOutput(Pointer<short> output, int size)
        {
            M.SetOutput(output, size);
        }

        private static short ClampInt16(int value)
        {
            if (value < Int16.MinValue)
                return Int16.MinValue;

            if (value > Int16.MaxValue)
                return Int16.MaxValue;

            return (short)value;
        }

        private static readonly byte[] InitialRegisters = new byte[RegisterCount]
        {
0x45,0x8B,0x5A,0x9A,0xE4,0x82,0x1B,0x78,0x00,0x00,0xAA,0x96,0x89,0x0E,0xE0,0x80,
0x2A,0x49,0x3D,0xBA,0x14,0xA0,0xAC,0xC5,0x00,0x00,0x51,0xBB,0x9C,0x4E,0x7B,0xFF,
0xF4,0xFD,0x57,0x32,0x37,0xD9,0x42,0x22,0x00,0x00,0x5B,0x3C,0x9F,0x1B,0x87,0x9A,
0x6F,0x27,0xAF,0x7B,0xE5,0x68,0x0A,0xD9,0x00,0x00,0x9A,0xC5,0x9C,0x4E,0x7B,0xFF,
0xEA,0x21,0x78,0x4F,0xDD,0xED,0x24,0x14,0x00,0x00,0x77,0xB1,0xD1,0x36,0xC1,0x67,
0x52,0x57,0x46,0x3D,0x59,0xF4,0x87,0xA4,0x00,0x00,0x7E,0x44,0x00,0x4E,0x7B,0xFF,
0x75,0xF5,0x06,0x97,0x10,0xC3,0x24,0xBB,0x00,0x00,0x7B,0x7A,0xE0,0x60,0x12,0x0F,
0xF7,0x74,0x1C,0xE5,0x39,0x3D,0x73,0xC1,0x00,0x00,0x7A,0xB3,0xFF,0x4E,0x7B,0xFF
        };

        internal static readonly short[] Guass = new short[0x200]
        {
   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,
   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   2,   2,   2,   2,   2,
   2,   2,   3,   3,   3,   3,   3,   4,   4,   4,   4,   4,   5,   5,   5,   5,
   6,   6,   6,   6,   7,   7,   7,   8,   8,   8,   9,   9,   9,  10,  10,  10,
  11,  11,  11,  12,  12,  13,  13,  14,  14,  15,  15,  15,  16,  16,  17,  17,
  18,  19,  19,  20,  20,  21,  21,  22,  23,  23,  24,  24,  25,  26,  27,  27,
  28,  29,  29,  30,  31,  32,  32,  33,  34,  35,  36,  36,  37,  38,  39,  40,
  41,  42,  43,  44,  45,  46,  47,  48,  49,  50,  51,  52,  53,  54,  55,  56,
  58,  59,  60,  61,  62,  64,  65,  66,  67,  69,  70,  71,  73,  74,  76,  77,
  78,  80,  81,  83,  84,  86,  87,  89,  90,  92,  94,  95,  97,  99, 100, 102,
 104, 106, 107, 109, 111, 113, 115, 117, 118, 120, 122, 124, 126, 128, 130, 132,
 134, 137, 139, 141, 143, 145, 147, 150, 152, 154, 156, 159, 161, 163, 166, 168,
 171, 173, 175, 178, 180, 183, 186, 188, 191, 193, 196, 199, 201, 204, 207, 210,
 212, 215, 218, 221, 224, 227, 230, 233, 236, 239, 242, 245, 248, 251, 254, 257,
 260, 263, 267, 270, 273, 276, 280, 283, 286, 290, 293, 297, 300, 304, 307, 311,
 314, 318, 321, 325, 328, 332, 336, 339, 343, 347, 351, 354, 358, 362, 366, 370,
 374, 378, 381, 385, 389, 393, 397, 401, 405, 410, 414, 418, 422, 426, 430, 434,
 439, 443, 447, 451, 456, 460, 464, 469, 473, 477, 482, 486, 491, 495, 499, 504,
 508, 513, 517, 522, 527, 531, 536, 540, 545, 550, 554, 559, 563, 568, 573, 577,
 582, 587, 592, 596, 601, 606, 611, 615, 620, 625, 630, 635, 640, 644, 649, 654,
 659, 664, 669, 674, 678, 683, 688, 693, 698, 703, 708, 713, 718, 723, 728, 732,
 737, 742, 747, 752, 757, 762, 767, 772, 777, 782, 787, 792, 797, 802, 806, 811,
 816, 821, 826, 831, 836, 841, 846, 851, 855, 860, 865, 870, 875, 880, 884, 889,
 894, 899, 904, 908, 913, 918, 923, 927, 932, 937, 941, 946, 951, 955, 960, 965,
 969, 974, 978, 983, 988, 992, 997,1001,1005,1010,1014,1019,1023,1027,1032,1036,
1040,1045,1049,1053,1057,1061,1066,1070,1074,1078,1082,1086,1090,1094,1098,1102,
1106,1109,1113,1117,1121,1125,1128,1132,1136,1139,1143,1146,1150,1153,1157,1160,
1164,1167,1170,1174,1177,1180,1183,1186,1190,1193,1196,1199,1202,1205,1207,1210,
1213,1216,1219,1221,1224,1227,1229,1232,1234,1237,1239,1241,1244,1246,1248,1251,
1253,1255,1257,1259,1261,1263,1265,1267,1269,1270,1272,1274,1275,1277,1279,1280,
1282,1283,1284,1286,1287,1288,1290,1291,1292,1293,1294,1295,1296,1297,1297,1298,
1299,1300,1300,1301,1302,1302,1303,1303,1303,1304,1304,1304,1304,1304,1305,1305,
        };

        private const int SimpleCounterRange = 0x800 * 5 * 3;

        private static readonly int[] CounterRates = new int[0x20]
        {
            SimpleCounterRange + 1, // never fires
                    4 << 9, 3 << 9,
            5 << 8, 4 << 9, 3 << 8,
            5 << 7, 4 << 7, 3 << 7,
            5 << 6, 4 << 6, 3 << 6,
            5 << 5, 4 << 5, 3 << 5,
            5 << 4, 4 << 4, 3 << 4,
            5 << 3, 4 << 3, 3 << 3,
            5 << 2, 4 << 2, 3 << 2,
            5 << 1, 4 << 1, 3 << 1,
            5 << 0, 4 << 0, 3 << 0,
            2, 1
        };

        private static readonly int[] CounterOffsets = new int[0x20]
        {
              1, 0, 1040,
            536, 0, 1040,
            536, 0, 1040,
            536, 0, 1040,
            536, 0, 1040,
            536, 0, 1040,
            536, 0, 1040,
            536, 0, 1040,
            536, 0, 1040,
            536, 0, 1040,
                 0,
                 0
        };
    }
}
