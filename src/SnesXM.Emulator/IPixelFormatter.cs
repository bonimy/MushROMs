namespace SnesXM.Emulator
{
    public interface IPixelFormatter
    {
        PixelFormat PixelFormat { get; }

        int SpareRgbBitMask { get; }

        int MaxRed { get; }
        int MaxGreen { get; }
        int MaxBlue { get; }
        int RedLowBitMask { get; }
        int GreenLowBitMask { get; }
        int BlueLowBitMask { get; }
        int RedHighBitMask { get; }
        int GreenHighBitMask { get; }
        int BlueHighBitMask { get; }
        int FirstColorMask { get; }
        int SecondColorMask { get; }
        int ThirdColorMask { get; }
        int AlphaBitsMask { get; }

        int GreenHiBit { get; }
        int RgbLowBitsMask { get; }
        int RgbHighBitsMask { get; }
        int RgbHighBitsMaskX2 { get; }
        int RgbRemoveLowBitsMask { get; }
        int FirstThirdColorMask { get; }
        int TwoLowBitsMask { get; }
        int HighBitsShiftedTwoMask { get; }

        int BuildPixel(int r, int g, int b);

        int BuildPixel2(int r, int g, int b);

        //(int r, int g, int b) DecomposePixel(int pixel);
    }
}
