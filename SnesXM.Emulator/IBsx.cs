using Helper;

namespace SnesXM.Emulator
{
    public interface IBsx
    {
        int this[int index] { get; set; }
        IIndexer<int> Ppu { get; }

        void Initialize();
        void Reset();
        void PostLoadState();
    }
}
