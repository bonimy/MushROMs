using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Helper;

namespace SnesXM.Emulator
{
    public class MemoryMap
    {
        public const int BlockSize = 0x1000;
        public const int NumberOfBlocks = 0x1000000 / BlockSize;
        public const int Shift = 12;
        public const int Mask = BlockSize - 1;
        public const int MaxRomSize = 0x800000;
        private const int SmcHeaderSize = 0x200;
        private const int SafeMaxRomSize = MaxRomSize + SmcHeaderSize + 0x8000;
        public const int RomNameLength = 23;
        private const int NsrtHeaderSize = 0x20;

        public IEmulator Emulator { get; private set; }
        private IMessageLog MessageLog => Emulator.MessageLog;
        private IMultiCart MultiCart => Emulator.MultiCart;
        private ISettings Settings => Emulator.Settings;
        private IInternalPpu InternalPpu => Emulator.InternalPpu;
        private ISuperFx SuperFx => Emulator.SuperFx;

        private byte[] NsrtHeader
        {
            get;
            set;
        }

        private int HeaderCount
        {
            get;
            set;
        }

        private SharedArray<byte> Ram
        {
            get;
            set;
        }

        private SharedArray<byte> Rom
        {
            get;
            set;
        }

        private SharedArray<byte> Sram
        {
            get;
            set;
        }

        private SharedArray<byte> Vram
        {
            get;
            set;
        }

        private SharedArray<byte> FillRam
        {
            get;
            set;
        }

        private SharedArray<byte> BwRam
        {
            get;
            set;
        }

        private SharedArray<byte> C4Ram
        {
            get;
            set;
        }

        private SharedArray<byte> Obc1Ram
        {
            get;
            set;
        }

        private SharedArray<byte> BsRam
        {
            get;
            set;
        }

        private SharedArray<byte> BiosRom
        {
            get;
            set;
        }

        private SharedArray<byte>[] Map
        {
            get;
            set;
        }

        private SharedArray<byte>[] WriteMap
        {
            get;
            set;
        }

        private bool[] BlockIsRam
        {
            get;
            set;
        }

        private bool[] BlockIsRom
        {
            get;
            set;
        }

        private ExtendedFormat ExtendedFormat
        {
            get;
            set;
        }

        private string RomFileName
        {
            get;
            set;
        }

        private string RomName
        {
            get;
            set;
        }

        private string RawRomName
        {
            get;
            set;
        }

        private string RomId
        {
            get;
            set;
        }

        private int CompanyId
        {
            get;
            set;
        }

        private byte RomRegion
        {
            get;
            set;
        }

        private byte RomSpeed
        {
            get;
            set;
        }

        private byte RomType
        {
            get;
            set;
        }

        private byte RomSize
        {
            get;
            set;
        }

        private int RomChecksum
        {
            get;
            set;
        }

        private int RomComplementChecksum
        {
            get;
            set;
        }

        private int RomCrc32
        {
            get; set;
        }

        private int RomFramesPerSecond
        {
            get;
            set;
        }

        private bool HiRom
        {
            get;
            set;
        }

        private bool LoRom
        {
            get;
            set;
        }

        private byte SramSize
        {
            get;
            set;
        }

        private int SramMask
        {
            get;
            set;
        }

        private int CalculatedSize
        {
            get;
            set;
        }

        private int CalculatedChecksum
        {
            get;
            set;
        }

        private ExtensionDictionary<FileFormat> FileFormatDictionary
        {
            get;
            set;
        }

        private event EventHandler PostRomInitFunc;

        public MemoryMap(IEmulator emulator)
        {
            Emulator = emulator ?? throw new ArgumentNullException(nameof(emulator));

            NsrtHeader = new byte[NsrtHeaderSize];

            Map = new SharedArray<byte>[NumberOfBlocks];
            WriteMap = new SharedArray<byte>[NumberOfBlocks];
            BlockIsRam = new bool[NumberOfBlocks];
            BlockIsRom = new bool[NumberOfBlocks];

            FileFormatDictionary = new ExtensionDictionary<FileFormat>()
            {
                { ".zip", FileFormat.Zip },
                { ".msu1", FileFormat.Zip },
                { ".jma", FileFormat.Jma }
            };
        }

        public void Initialize()
        {
            Ram = new SharedArray<byte>(0x20000);
            Sram = new SharedArray<byte>(0x20000);
            Vram = new SharedArray<byte>(0x10000);
            Rom = new SharedArray<byte>(SafeMaxRomSize);

            InternalPpu.Initialize();

            FillRam = new SharedArray<byte>(Rom);

            Rom += 0x8000;

            C4Ram = Rom + 0x400000 + 8192 * 8;
            Obc1Ram = Rom + 0x400000;
            BiosRom = Rom + 0x300000;
            BsRam = Rom + 0x400000;

            SuperFx.Initialize();

            PostRomInitFunc = null;
        }

        public int ScoreHiRom(bool skipSmcHeader)
        {
            return ScoreHiRom(skipSmcHeader, 0);
        }
        public int ScoreHiRom(bool skipSmcHeader, int offset)
        {
            var buf = Rom + 0xFF00 + offset + (skipSmcHeader ? SmcHeaderSize : 0);
            int score = 0;


            if ((buf[0xD5] & 1) != 0)
                score += 2;

            if (buf[0xD5] == 23)
                score -= 2;

            if (buf[0xD4] == 0x20)
                score += 2;

            if ((buf[0xDC] + (buf[0xDD] << 8)) + (buf[0xDE] + (buf[0xDf] << 8)) == 0xFFFF)
            {
                score += 2;
                if (buf[0xDE] + (buf[0xDF] << 8) != 0)
                    score++;
            }

            if (buf[0xDA] == 0x33)
                score += 2;

            if ((buf[0xD5] & 0x0F) < 4)
                score += 2;

            if ((buf[0xFD] & 0x80) == 0)
                score -= 6;

            if (buf[0xFC] + (buf[0xFD] << 8) > 0xFFB0)
                score -= 2;

            if (CalculatedSize > 0x400 * 0x400 * 3)
                score += 4;

            if ((1 << (buf[0xD7] - 7)) > 0x30)
                score -= 1;

            if (!IsAllAscii(buf + 0xB0, 6))
                score -= 1;

            if (!IsAllAscii(buf + 0xC0, RomNameLength))
                score -= 1;

            return score;
        }

        public int ScoreLoRom(bool skipSmcHeader)
        {
            return ScoreLoRom(skipSmcHeader, 0);
        }
        public int ScoreLoRom(bool skipSmcHeader, int offset)
        {
            var buf = Rom + 0x7F00 + offset + (skipSmcHeader ? SmcHeaderSize : 0);
            int score = 0;

            if ((buf[0xD5] & 1) == 0)
                score += 3;

            if (buf[0xD5] == 23)
                score += 2;

            if ((buf[0xDC] + (buf[0xDD] << 8)) + (buf[0xDE] + (buf[0xDf] << 8)) == 0xFFFF)
            {
                score += 2;
                if (buf[0xDE] + (buf[0xDF] << 8) != 0)
                    score++;
            }

            if (buf[0xDA] == 0x33)
                score += 2;

            if ((buf[0xD5] & 0x0F) < 4)
                score += 2;

            if ((buf[0xFD] & 0x80) == 0)
                score -= 6;

            if (buf[0xFC] + (buf[0xFD] << 8) > 0xFFB0)
                score -= 2;

            if (CalculatedSize <= 0x400 * 0x400 * 0x10)
                score += 2;

            if ((1 << (buf[0xD7] - 7)) > 0x30)
                score -= 1;

            if (!IsAllAscii(buf + 0xB0, 6))
                score -= 1;

            if (!IsAllAscii(buf + 0xC0, RomNameLength))
                score -= 1;

            return score;
        }

        public int HeaderRemove(int size, SharedArray<byte> buf)
        {
            if (buf == null)
                throw new ArgumentNullException(nameof(buf));

            int calcSize = size & ~0x1FFF;

            if ((size - calcSize != SmcHeaderSize || Settings.ForceNoHeader) && !Settings.ForceHeader)
                return size;

            var nsrtHead = buf + 0x1D0;

            var nsrt = Encoding.ASCII.GetString(
                nsrtHead.GetArray(), nsrtHead.Offset + 24, 4);

            if (nsrt != "NSRT")
                return end();

            if (nsrtHead[28] != 22)
                return end();

            {
                var sum = 0;
                for (int i = 0; i < NsrtHeaderSize; i++)
                    sum += nsrtHead[i];
                if ((byte)sum != nsrtHead[30])
                    return end();
            }

            if (nsrtHead[30] + nsrtHead[31] != 0xFF)
                return end();

            if ((nsrtHead[0] & 0x0F) > 0x0D)
                return end();

            if ((nsrtHead[0] >> 4) > 3)
                return end();

            if ((nsrtHead[0]) >> 4 == 0)
                return end();

            Array.Copy(nsrtHead.GetArray(), nsrtHead.Offset, NsrtHeader, 0, NsrtHeaderSize);

            return end();

            int end()
            {
                var array = buf.GetArray();
                var offset = buf.Offset;
                Array.Copy(array, offset + SmcHeaderSize, array, offset, calcSize);
                HeaderCount++;
                size -= SmcHeaderSize;
                return size;
            }
        }

        private static bool IsAllAscii(SharedArray<byte> data, int size)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            for (int i = 0; i < size; i++)
            {
                if (data[i] < 32 || data[i] > 126)
                    return false;
            }

            return true;
        }

        private int FileLoader(SharedArray<byte> buffer, string path, int maxSize)
        {
            var totalSize = 0;

            Array.Clear(NsrtHeader, 0, NsrtHeaderSize);
            HeaderCount = 0;

            var format = FileFormat.Default;
            FileFormatDictionary.TryGetValue(path, out format);

            switch (format)
            {
            case FileFormat.Zip:
                throw new NotImplementedException();
            case FileFormat.Jma:
                throw new NotImplementedException();

            case FileFormat.Default:
            default:
                using (var fp = new FileStream(path, FileMode.Open))
                {
                    RomFileName = path;

                    int size = 0;
                    bool more = false;
                    var ptr = new SharedArray<byte>(buffer);

                    do
                    {
                        var array = buffer.GetArray();
                        var offset = buffer.Offset;

                        size = fp.Read(array, offset, maxSize + SmcHeaderSize - (ptr - buffer));

                        size = HeaderRemove(size, ptr);
                        totalSize += size;
                        ptr += size;

                        var ext = Path.GetExtension(path);

                        if (ptr - buffer < maxSize + SmcHeaderSize)
                        {
                            throw new NotImplementedException();
                        }
                    } while (more);
                }
                break;
            }

            switch (HeaderCount)
            {
            case 0:
                MessageLog.Message(MessageType.Info, SR.InfoNoRomHeader);
                break;
            case 1:
                MessageLog.Message(MessageType.Info, SR.InfoRomHeaderIgnored);
                break;
            default:
                MessageLog.Message(MessageType.Info, SR.InfoRomHeaderMultipleIgnored);
                break;
            }

            return totalSize;
        }

        private bool LoadRomMemory(SharedArray<byte> source, int sourceSize)
        {
            if (source == null || sourceSize > MaxRomSize)
                return false;


            do
            {
                var dest = Rom.GetArray();
                var destIndex = Rom.Offset;
                var src = source.GetArray();
                var srcIndex = source.Offset;

                Array.Clear(dest, destIndex, MaxRomSize);
                MultiCart.Initialize();
                Array.Copy(src, srcIndex, dest, destIndex, sourceSize);
            }
            while (!LoadRomInt(sourceSize));

            return true;
        }

        private bool LoadRom(string path)
        {
            if (String.IsNullOrEmpty(path))
                return false;

            int totalFileSize;

            do
            {
                var dest = Rom.GetArray();
                var destIndex = Rom.Offset;

                Array.Clear(dest, destIndex, MaxRomSize);
                MultiCart.Initialize();
                totalFileSize = FileLoader(Rom, path, MaxRomSize);

                if (totalFileSize == 0)
                    return false;

                if (!Settings.NoPatch)
                    throw new NotImplementedException();
            }
            while (!LoadRomInt(totalFileSize));

            return true;
        }

        private bool LoadRomInt(int romFillSize)
        {
            Settings.DisplayColor = Por
        }
    }
}
