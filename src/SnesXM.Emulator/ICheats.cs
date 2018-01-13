namespace SnesXM.Emulator
{
    public interface ICheats
    {
        bool LoadCheatFile(string path);

        void InitializeCheatData();
        void ApplyCheats();
    }
}
