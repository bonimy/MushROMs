using System;
using System.IO;

namespace SnesXM.Emulator
{
    public class Display : IDisplay
    {
        public const int Width = 0x100;
        public const int Height = 0xE0;
        public const int ExtendedHeight = 0xEF;

        public const int MaxWidth = 2 * Width;
        public const int MaxHeight = 2 * ExtendedHeight;

        public string GetDirectory(DirectoryType directoryType)
        {
            var startDirectory = AppContext.BaseDirectory;
            var result = startDirectory;

            switch (directoryType)
            {
            case DirectoryType.Default:
            case DirectoryType.Home:
                result = startDirectory;
                break;


            }

            if (Path.IsPathRooted(result))
            {
                result = startDirectory;
            }

            return result;
        }

        public string GetFilename(string ext, DirectoryType directoryType)
        {
            throw new NotImplementedException();
        }
    }
}
