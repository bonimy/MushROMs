namespace SnesXM.Emulator
{
    public interface IDisplay
    {
        string GetDirectory(DirectoryType directoryType);

        string GetFilename(string ext, DirectoryType directoryType);
    }
}
