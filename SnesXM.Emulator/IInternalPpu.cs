using Helper.PixelFormats;

namespace SnesXM.Emulator
{
    public interface IInternalPpu
    {
        bool Interlace { get; }

        Color15BppBgr555[] GetColors();

        void Initialize();
    }
}
