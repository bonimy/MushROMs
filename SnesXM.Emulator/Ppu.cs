using System;
using Helper.PixelFormats;

namespace SnesXM.Emulator
{
    public class Ppu : IPpu
    {
        public const int FirstVisibleLine = 1;

        public const int Tile2Bit = 0;
        public const int Tile4Bit = 1;
        public const int Tile8Bit = 2;
        public const int Tile2BitEven = 3;
        public const int Tile2BitOdd = 4;
        public const int Tile4BitEven = 5;
        public const int Tile4BitOdd = 6;
        public const int TileBitFormatSize = 7;

        public const int Max2BitTiles = 0x1000;
        public const int Max4BitTiles = 0x800;
        public const int Max8BitTiles = 0x400;

        public const int MaxClipSize = 6;
        public const int BgCount = 4;
        public const int CgDataSize = 0x100;
        public const int MaxSprites = 0x80;

        private IEmulator Emulator
        {
            get;
            set;
        }
        private IControls Controls => Emulator.Controls;
        private IInternalPpu InternalPpu => Emulator.InternalPpu;
        ITimings Timings => Emulator.Timings;

        private Vma _vma;

        public Vma Vma
        {
            get => _vma;
            set => _vma = value;
        }

        public int Wram
        {
            get;
            set;
        }

        internal Bg[] Bg
        {
            get;
            private set;
        }

        public BgMode BgMode
        {
            get;
            set;
        }
        public int Bg3Priority
        {
            get;
            set;
        }

        public bool CgFlip
        {
            get;
            set;
        }
        public int CgFlipRead
        {
            get;
            set;
        }
        public int CgAdd
        {
            get;
            set;
        }
        internal Color15BppBgr555[] CgData
        {
            get;
            private set;
        }

        internal Obj[] Obj
        {
            get;
            private set;
        }
        public bool ObjThroughMain
        {
            get;
            set;
        }
        public bool ObjThroughSub
        {
            get;
            set;
        }
        public bool ObjAddition
        {
            get;
            set;
        }
        public int ObjNameBase
        {
            get;
            set;
        }
        public int ObjNameSelect
        {
            get;
            set;
        }
        public int ObjSizeSelect
        {
            get;
            set;
        }

        public int OamAddress
        {
            get;
            set;
        }
        public int SavedOamAddress
        {
            get;
            set;
        }
        public int OamPriorityRotation
        {
            get;
            set;
        }
        public int OamFlip
        {
            get;
            set;
        }
        public int OamReadFlip
        {
            get;
            set;
        }
        public int OamTileAddress
        {
            get;
            set;
        }
        public int OamWriteRegister
        {
            get;
            set;
        }
        internal int[] OamData
        {
            get;
            private set;
        }

        public int FirstSprite
        {
            get;
            set;
        }
        public int LastSprite
        {
            get;
            set;
        }
        public int RangeTimeOver
        {
            get;
            set;
        }

        public bool HTimerEnabled
        {
            get;
            set;
        }
        public bool VTimerEnabled
        {
            get;
            set;
        }
        public int HTimerPosition
        {
            get;
            set;
        }
        public int VTimerPosition
        {
            get;
            set;
        }
        public int IrqHBeamPos
        {
            get;
            set;
        }
        public int IrqVBeamPos
        {
            get;
            set;
        }

        public bool HBeamFlip
        {
            get;
            set;
        }
        public bool VBeamFlip
        {
            get;
            set;
        }
        public int HBeamPosLatched
        {
            get;
            set;
        }
        public int VBeamPosLatched
        {
            get;
            set;
        }
        public int GunHLatch
        {
            get;
            set;
        }
        public int GunVLatch
        {
            get;
            set;
        }
        public int HVBeamCounterLatched
        {
            get;
            set;
        }

        public bool Mode7HFlip
        {
            get;
            set;
        }
        public bool Mode7VFlip
        {
            get;
            set;
        }
        public int Mode7Repeat
        {
            get;
            set;
        }
        public int MatrixA
        {
            get;
            set;
        }
        public int MatrixB
        {
            get;
            set;
        }
        public int MatrixC
        {
            get;
            set;
        }
        public int MatrixD
        {
            get;
            set;
        }
        public int CenterX
        {
            get;
            set;
        }
        public int CenterY
        {
            get;
            set;
        }
        public int Mode7HOffset
        {
            get;
            set;
        }
        public int Mode7VOffset
        {
            get;
            set;
        }

        public int Mosaic
        {
            get;
            set;
        }
        public int MosaicStart
        {
            get;
            set;
        }
        internal bool[] BgMosaic
        {
            get;
            private set;
        }

        public int Window1Left
        {
            get;
            set;
        }
        public int Window1Right
        {
            get;
            set;
        }
        public int Window2Left
        {
            get;
            set;
        }
        public int Window2Right
        {
            get;
            set;
        }
        public bool RecomputeWindows
        {
            get;
            set;
        }
        internal byte[] ClipCounts
        {
            get;
            private set;
        }
        internal ClipLogic[] ClipWindowOverlapLogic
        {
            get;
            private set;
        }
        internal bool[] ClipWindow1Enable
        {
            get;
            private set;
        }
        internal bool[] ClipWindow2Enable
        {
            get;
            private set;
        }
        internal bool[] ClipWindow1Inside
        {
            get;
            private set;
        }
        internal bool[] ClipWindow2Inside
        {
            get;
            set;
        }

        public bool ForcedBlanking
        {
            get;
            set;
        }

        public Color15BppBgr555 FixedColor
        {
            get;
            set;
        }
        public int Brightness
        {
            get;
            set;
        }
        public int ScreenHeight
        {
            get;
            set;
        }

        public bool Need16x8Multiply
        {
            get;
            set;
        }
        public int BgNOffsetValue
        {
            get;
            set;
        }
        public int Mode7Value
        {
            get;
            set;
        }

        public int Hdma
        {
            get;
            set;
        }
        public int HdmaEnded
        {
            get;
            set;
        }

        public int OpenBus1
        {
            get;
            set;
        }
        public int OpenBus2
        {
            get;
            set;
        }

        public Ppu(IEmulator emulator)
        {
            Emulator = emulator ?? throw new NullReferenceException(nameof(emulator));

            Bg = new Bg[BgCount];
            BgMosaic = new bool[BgCount];
            CgData = new Color15BppBgr555[CgDataSize];
            Obj = new Obj[MaxSprites];
            OamData = new int[0x200 + 0x20];

            ClipCounts = new byte[MaxClipSize];
            ClipWindowOverlapLogic = new ClipLogic[MaxClipSize];
            ClipWindow1Enable = new bool[MaxClipSize];
            ClipWindow2Enable = new bool[MaxClipSize];
            ClipWindow1Inside = new bool[MaxClipSize];
            ClipWindow2Inside = new bool[MaxClipSize];
        }

        public void Reset()
        {
            SoftReset();
            Controls.Reset();
            Mode7HOffset =
            Mode7VOffset =
            Mode7Value = 0;
        }

        public void SoftReset()
        {
            Controls.SoftReset();

            _vma.High = false;
            _vma.Increment = 1;
            _vma.Address = 0;
            _vma.FullGraphicCount = 0;
            _vma.Shift = 9;

            Wram = 0;

            for (var i = 0; i < BgCount; i++)
            {
                Bg[i] = SnesXM.Emulator.Bg.Empty;
            }

            BgMode = BgMode.Mode0;
            Bg3Priority = 0;

            CgFlip = false;
            CgFlipRead = 0;
            CgAdd = 0;

            var colors = InternalPpu.GetColors();

            for (var i = 0; i < CgDataSize; i++)
            {
                var r = (i & (7 << 0)) << 2;
                var g = (i & (7 << 3)) << 2;
                var b = (i & (2 << 6)) << 3; // I don't know why this is different (snes9x ppu.cpp:L1870)

                colors[i] = new Color15BppBgr555(r, g, b);
                CgData[i] = colors[i];
            }

            for (var i = 0; i < MaxSprites; i++)
            {
                Obj[i] = SnesXM.Emulator.Obj.Empty;
            }

            ObjThroughMain = false;
            ObjThroughSub = false;
            ObjAddition = false;
            ObjNameBase = 0;
            ObjNameSelect = 0;
            ObjSizeSelect = 0;

            OamAddress = 0;
            SavedOamAddress = 0;
            OamPriorityRotation = 0;
            OamFlip = 0;
            OamReadFlip = 0;
            OamTileAddress = 0;
            OamWriteRegister = 0;
            Array.Clear(OamData, 0, OamData.Length);

            FirstSprite = 0;
            LastSprite = MaxSprites - 1;
            RangeTimeOver = 0;

            HTimerEnabled = false;
            VTimerEnabled = false;
            HTimerPosition = Timings.HMax + 1;
            VTimerPosition = Timings.VMax + 1;
            IrqHBeamPos = 0x1FF;
            IrqVBeamPos = 0x1FF;

            HBeamFlip = false;
            VBeamFlip = false;
            HBeamPosLatched = 0;
            VBeamPosLatched = 0;
            GunHLatch = 0;
            GunVLatch = 1000;
            HVBeamCounterLatched = 0;

            Mode7HFlip = false;
            Mode7VFlip = false;
            Mode7Repeat = 0;
            MatrixA =
            MatrixB =
            MatrixC =
            MatrixD = 0;
            CenterX =
            CenterY = 0;

            Mosaic = 0;
            Array.Clear(BgMosaic, 0, BgCount);

            Window1Left = 1;
            Window1Right = 0;
            Window2Left = 1;
            Window2Right = 0;
            RecomputeWindows = true;

            for (var i = 0; i < MaxClipSize; i++)
            {
                ClipCounts[i] = 0;
                ClipWindowOverlapLogic[i] = ClipLogic.Or;
                ClipWindow1Enable[i] = false;
                ClipWindow2Enable[i] = false;
                ClipWindow1Inside[i] = true;
                ClipWindow2Inside[i] = true;
            }

            ForcedBlanking = true;

            FixedColor = Color15BppBgr555.Empty;
            Brightness = 0;
            ScreenHeight = Display.Height;

            Need16x8Multiply = false;
            BgNOffsetValue = 0;

            Hdma = 0;
            HdmaEnded = 0;

            OpenBus1 = 0;
            OpenBus2 = 0;

            throw new NotImplementedException();
        }
    }
}
