using System;
using System.Collections.Generic;
using System.Text;
using Helper.PixelFormats;

namespace SnesXM.Emulator
{
    public delegate int BuildPixel(int red, int green, int blue);
    public delegate Color24BppRgb DecomposePixel(int pixel);
    public delegate void DrawBackdrop(int offset, int left, int right);
    public delegate void DrawTile(int tile, int offset, int startLine, int lineCount);
    public delegate void DrawClippedTile(int tile, int offset, int startPixel, int width, int startLine, int lineCount);
    public delegate void DrawMosaicPixel(int tile, int offset, int startLine, int startPixel, int width, int lineCount);
    public delegate void DrawMode7Bg(int left, int right, int depth);

    public delegate void DisplayString(string text, int linesFromBottom, int pixelsFromLeft, bool allowWrap);

    public class Gfx
    {
        public PixelFormat PixelFormat
        {
            get;
            private set;
        }

        private BuildPixel BuildPixel
        {
            get;
            set;
        }
        private BuildPixel BuildPixel2
        {
            get;
            set;
        }
        private DecomposePixel DecomposePixel
        {
            get;
            set;
        }

        private DrawBackdrop DrawBackdropMath
        {
            get;
            set;
        }
        private DrawBackdrop DrawBackdropNoMath
        {
            get;
            set;
        }
        private DrawTile DrawTileMath
        {
            get;
            set;
        }
        private DrawTile DrawTileNoMath
        {
            get;
            set;
        }
        private DrawClippedTile DrawClippedTileMath
        {
            get;
            set;
        }
        private DrawClippedTile DrawClippedTileNoMath
        {
            get;
            set;
        }
        private DrawMosaicPixel DrawMosaicPixelMath
        {
            get;
            set;
        }
        private DrawMosaicPixel DrawMosaicPixelNoMath
        {
            get;
            set;
        }
        private DrawMode7Bg DrawMode7Bg1Math
        {
            get;
            set;
        }
        private DrawMode7Bg DrawMode7Bg1NoMath
        {
            get;
            set;
        }
        private DrawMode7Bg DrawMode7Bg2Math
        {
            get;
            set;
        }
        private DrawMode7Bg DrawMode7Bg2NoMath
        {
            get;
            set;
        }
    }
}
