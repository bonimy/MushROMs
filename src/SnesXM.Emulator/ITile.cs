namespace SnesXM.Emulator
{
    public interface ITile
    {
        void Initialize();
        void SlecetTileRenderers(BgMode bgMode, bool sub, bool obj);
        void SelectTileConverter(int depth, bool hiRes, bool sub, bool mosaic);
    }
}
