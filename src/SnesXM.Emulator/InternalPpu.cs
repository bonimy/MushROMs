using Helper.PixelFormats;

namespace SnesXM.Emulator
{
    public class InternalPpu
    {
        private ClipData[][] Clip { get; set; }

        private bool ColorsChanged { get; set; }

        private bool OBJChanged { get; set; }

        private bool DirectColourMapsNeedRebuild { get; set; }

        internal byte[][] TileCache { get; set; }

        internal byte[][] TileCached { get; set; }

        private ushort VRAMReadBuffer { get; set; }

        private bool Interlace { get; set; }

        private bool InterlaceOBJ { get; set; }

        private bool PseudoHires { get; set; }

        private bool DoubleWidthPixels { get; set; }

        private bool DoubleHeightPixels { get; set; }

        private int CurrentLine { get; set; }

        private int PreviousLine { get; set; }

        private byte[] XB { get; set; }

        private int[] Red { get; set; }

        private int[] Green { get; set; }

        private int[] Blue { get; set; }

        private Color15BppBgr[] Colors { get; set; }

        private ushort[] ScreenColors { get; set; }

        private byte MaxBrightness { get; set; }

        private bool RenderThisFrame { get; set; }

        private int RenderedScreenWidth { get; set; }

        private int RenderedScreenHeight { get; set; }

        private int FrameCount { get; set; }

        private int RenderedFramesCount { get; set; }

        private int DisplayedRenderedFrameCount { get; set; }

        private int TotalEmulatedFrames { get; set; }

        private int SkippedFrames { get; set; }

        private int FrameSkip { get; set; }

        public InternalPpu()
        {
            Clip = new ClipData[2][];
            for (int i = 0; i < 2; i++)
                Clip[i] = new ClipData[6];

            TileCache = new byte[7][];
            TileCached = new byte[7][];

            Red = new int[Ppu.CgDataSize];
            Green = new int[Ppu.CgDataSize];
            Blue = new int[Ppu.CgDataSize];
            Colors = new Color15BppBgr[Ppu.CgDataSize];
        }

        public Color15BppBgr[] GetColors() => Colors;
    }
}
