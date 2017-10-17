using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Helper;

namespace SnesXM.Emulator
{
    public class MemoryMap// : IMemoryMap
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

        public const int RamStartAddress = 0x7E0000;
        public const int RamMaxSize = 0x20000;

        public const int SramStartAddress = 0x700000;
        public const int SramMaxSize = 0x20000;

        public const int VramMaxSize = 0x10000;

        public IEmulator Emulator { get; private set; }
        private IMessageLog MessageLog => Emulator.MessageLog;
        private IDisplay Display => Emulator.Display;
        private IMultiCart MultiCart => Emulator.MultiCart;
        private ISettings Settings => Emulator.Settings;
        private IInternalPpu InternalPpu => Emulator.InternalPpu;
        private ISuperFx SuperFx => Emulator.SuperFx;
        private IPort Port => Emulator.Port;
        private ICpu Cpu => Emulator.Cpu;
        private IGfx Gfx => Emulator.Gfx;
        private IPixelFormatter PixelFormatter => Gfx.PixelFormatter;
        private ICheats Cheats => Emulator.Cheats;

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

        private Pointer<byte> Ram
        {
            get;
            set;
        }

        private Pointer<byte> Rom
        {
            get;
            set;
        }

        private Pointer<byte> Sram
        {
            get;
            set;
        }

        private Pointer<byte> Vram
        {
            get;
            set;
        }

        private Pointer<byte> FillRam
        {
            get;
            set;
        }

        private Pointer<byte> BwRam
        {
            get;
            set;
        }

        private Pointer<byte> C4Ram
        {
            get;
            set;
        }

        private Pointer<byte> Obc1Ram
        {
            get;
            set;
        }

        private Pointer<byte> BsRam
        {
            get;
            set;
        }

        private Pointer<byte> BiosRom
        {
            get;
            set;
        }

        private Pointer<byte>[] Map
        {
            get;
            set;
        }

        private Pointer<byte>[] WriteMap
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

        private string LastRomFileName
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

            Map = new Pointer<byte>[NumberOfBlocks];
            WriteMap = new Pointer<byte>[NumberOfBlocks];
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
            Ram = new Pointer<byte>(0x20000);
            Sram = new Pointer<byte>(0x20000);
            Vram = new Pointer<byte>(0x10000);
            Rom = new Pointer<byte>(SafeMaxRomSize);

            InternalPpu.Initialize();

            FillRam = new Pointer<byte>(Rom);

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

        public int HeaderRemove(int size, Pointer<byte> buf)
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

        private static bool IsAllAscii(Pointer<byte> data, int size)
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

        private int FileLoader(Pointer<byte> buffer, string path, int maxSize)
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
                    var ptr = new Pointer<byte>(buffer);

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

        private bool LoadRomMemory(Pointer<byte> source, int sourceSize)
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
            Settings.DisplayColor = PixelFormatter.BuildPixel(31, 31, 31);
            Port.SetUiColor(255, 255, 255);

            CalculatedSize = 0;
            ExtendedFormat = ExtendedFormat.Nope;

            int hiScore = ScoreHiRom(false);
            int loScore = ScoreLoRom(false);

            if (HeaderCount == 0 && !Settings.ForceNoHeader &&
                ((hiScore > loScore && ScoreHiRom(true) > hiScore) ||
                (hiScore <= loScore && ScoreLoRom(true) > loScore)))
            {
                romFillSize -= 0x200;
                {
                    var rom = Rom.GetArray();
                    var index = Rom.Offset;
                    Array.Copy(rom, index + SmcHeaderSize, rom, index, romFillSize);
                }
                MessageLog.Message(MessageType.Info, SR.InfoTryForceNoHeader);

                hiScore = ScoreHiRom(false);
                loScore = ScoreLoRom(false);
            }

            CalculatedSize = romFillSize & ~0x1FFF;

            if (CalculatedSize > 0x400000 &&
                (Rom[0x7fd5] + (Rom[0x7fd6] << 8)) != 0x3423 && // exclude SA-1
                (Rom[0x7fd5] + (Rom[0x7fd6] << 8)) != 0x3523 &&
                (Rom[0x7fd5] + (Rom[0x7fd6] << 8)) != 0x4332 && // exclude S-DD1
                (Rom[0x7fd5] + (Rom[0x7fd6] << 8)) != 0x4532 &&
                (Rom[0xffd5] + (Rom[0xffd6] << 8)) != 0xF93a && // exclude SPC7110
                (Rom[0xffd5] + (Rom[0xffd6] << 8)) != 0xF53a)
            {
                ExtendedFormat = ExtendedFormat.Yeah;
            }

            // if both vectors are invalid, it's type 1 interleaved LoROM
            if (ExtendedFormat == ExtendedFormat.Nope &&
                ((Rom[0x7ffc] + (Rom[0x7ffd] << 8)) < 0x8000) &&
                ((Rom[0xfffc] + (Rom[0xfffd] << 8)) < 0x8000))
            {
                if (!Settings.ForceInterleaved && !Settings.ForceNotInterleaved)
                    DeinterleaveType1(romFillSize, Rom);
            }

            hiScore = ScoreHiRom(false);
            loScore = ScoreLoRom(false);

            var romHeader = new Pointer<byte>(Rom);

            if (ExtendedFormat != ExtendedFormat.Nope)
            {
                var swappedHiRom = ScoreHiRom(false, 0x400000);
                var swappedLoRom = ScoreLoRom(false, 0x400000);

                if (Math.Max(swappedHiRom, swappedLoRom) >= Math.Max(loScore, hiScore))
                {
                    ExtendedFormat = ExtendedFormat.BigFirst;
                    hiScore = swappedHiRom;
                    loScore = swappedLoRom;
                    romHeader += 0x400000;
                }
                else
                    ExtendedFormat = ExtendedFormat.SmallFirst;
            }

            var tales = false;
            var interleaved =
                Settings.ForceInterleaved ||
                Settings.ForceInterleaved2 ||
                Settings.ForceInterleavedGd24;

            if (Settings.ForceLoRom || (!Settings.ForceHiRom && loScore >= hiScore))
            {
                LoRom = true;
                HiRom = false;

                // ignore map type byte if not 0x2x or 0x3x
                if ((romHeader[0x7fd5] & 0xf0) == 0x20 || (romHeader[0x7fd5] & 0xf0) == 0x30)
                {
                    switch (romHeader[0x7fd5] & 0xf)
                    {
                    case 1:
                        interleaved = true;
                        break;

                    case 5:
                        interleaved = true;
                        tales = true;
                        break;
                    }
                }
            }
            else
            {
                LoRom = false;
                HiRom = true;

                if ((romHeader[0xffd5] & 0xf0) == 0x20 || (romHeader[0xffd5] & 0xf0) == 0x30)
                {
                    switch (romHeader[0xffd5] & 0xf)
                    {
                    case 0:
                    case 3:
                        interleaved = true;
                        break;
                    }
                }
            }

            // this two games fail to be detected
            if (!Settings.ForceHiRom && !Settings.ForceLoRom)
            {
                var name = Encoding.ASCII.GetString(Rom.GetArray(), Rom.Offset, RomNameLength);

                if (name.StartsWith("YUYU NO QUIZ DE GO!GO!") ||
                    name.StartsWith("BATMAN--REVENGE JOKER"))
                {
                    LoRom = true;
                    HiRom = false;
                    interleaved = false;
                    tales = false;
                }
            }

            if (!Settings.ForceNotInterleaved && interleaved)
            {
                MessageLog.Message(MessageType.Info, SR.InfoConevrtInterleavedRom);

                if (tales)
                {
                    if (ExtendedFormat == ExtendedFormat.BigFirst)
                    {
                        DeinterleaveType1(0x400000, Rom);
                        DeinterleaveType1(CalculatedSize - 0x400000, Rom + 0x400000);
                    }
                    else
                    {
                        DeinterleaveType1(CalculatedSize - 0x400000, Rom);
                        DeinterleaveType1(0x400000, Rom + CalculatedSize - 0x400000);
                    }

                    LoRom = false;
                    HiRom = true;
                }
                else
                if (Settings.ForceInterleavedGd24 && CalculatedSize == 0x300000)
                {
                    LoRom ^= HiRom;
                    HiRom ^= LoRom;
                    LoRom ^= HiRom;
                    DeinterleaveGd24(CalculatedSize, Rom);
                }
                else
                if (Settings.ForceInterleaved2)
                {
                    DeinterleaveType2(CalculatedSize, Rom);
                }
                else
                {
                    LoRom ^= HiRom;
                    HiRom ^= LoRom;
                    LoRom ^= HiRom;
                    DeinterleaveType1(CalculatedSize, Rom);
                }

                hiScore = ScoreHiRom(false);
                loScore = ScoreLoRom(false);

                if ((HiRom && (loScore >= hiScore || hiScore < 0)) ||
                    (LoRom && (hiScore > loScore || loScore < 0)))
                {
                    MessageLog.Message(MessageType.Info, SR.InfoRomTypeLied);
                    Settings.ForceNotInterleaved = true;
                    Settings.ForceInterleaved = false;
                    return false;
                }
            }

            if (ExtendedFormat == ExtendedFormat.SmallFirst)
                tales = true;

            if (tales)
            {
                var tmp = new byte[CalculatedSize - 0x400000];
                var array = Rom.GetArray();
                var offset = Rom.Offset;
                var remainder = CalculatedSize - 0x400000;

                MessageLog.Message(MessageType.Info, SR.InfoFixingSwappedExHiRom);
                Array.Copy(array, offset, tmp, 0, remainder);
                Array.Copy(array, offset + remainder, array, offset, 0x400000);
                Array.Copy(tmp, 0, array, offset + 0x400000, remainder);
            }

            LastRomFileName = RomFileName;

            Emulator.Uniracers = false;
            Emulator.SramInitialValue = 0x60;

            Cheats.LoadCheatFile(Display.GetFilename(".cht", DirectoryType.Cheat));

            InitializeRom();

            Cheats.InitializeCheatData();
            Cheats.ApplyCheats();

            Cpu.Reset();

            return true;
        }

        public void InitializeRom()
        {
            Settings.SuperFx =
            Settings.Sa1 =
            Settings.C4 =
            Settings.Sdd1 =
            Settings.Spc7110 =
            Settings.Spc7110Rtc =
            Settings.Obc1 =
            Settings.Srtc =
            Settings.Bs =
            Settings.Msu1 = false;

            Settings.Seta =
            Settings.Dsp = 0;

            if (false)
                throw new NotImplementedException("SuperFx");

            CompanyId = -1;
            RomId = String.Empty;

            var romHeader = Rom + 0x7FB0;
            if (ExtendedFormat == ExtendedFormat.BigFirst)
                romHeader += 0x400000;
            if (HiRom)
                romHeader += 0x8000;

            if (false)
                throw new NotImplementedException("InitBsx()");

            ParseSnesHeader(romHeader);

            //// Detect and initialize chips
            //// detection codes are compatible with NSRT

            // DSP1/2/3/4
            if (RomType == 0x03)
            {
                if (RomSpeed == 0x30)
                    Settings.Dsp = 4; // DSP4
                else
                    Settings.Dsp = 1; // DSP1
            }
            else if (RomType == 0x05)
            {
                if (RomSpeed == 0x20)
                    Settings.Dsp = 2; // DSP2
                else
                if (RomSpeed == 0x30 && romHeader[0x2a] == 0xB2)
                    Settings.Dsp = 3; // DSP3
                else
                    Settings.Dsp = 1; // DSP1
            }

            switch (Settings.Dsp)
            {
            case 1:
            case 2:
            case 3:
            case 4:
                throw new NotImplementedException();

            default:
                // SetDsp = null;
                break;
            }

            var identifier = ((RomType & 0xFF) << 8) + (RomSpeed & 0xFF);

            switch (identifier)
            {
            // SRTC
            case 0x5535:
                Settings.Srtc = true;
                throw new NotImplementedException();
            //S9xInitSRTC();
            //break;

            // SPC7110
            case 0xF93A:
                Settings.Spc7110Rtc = true;
                Settings.Spc7110 = true;
                throw new NotImplementedException();
            //S9xInitSPC7110();
            //break;

            case 0xF53A:
                Settings.Spc7110 = true;
                throw new NotImplementedException();
            //S9xInitSPC7110();
            //break;

            // OBC1
            case 0x2530:
                Settings.Obc1 = true;
                break;

            // SA1
            case 0x3423:
            case 0x3523:
                Settings.Sa1 = true;
                break;

            // SuperFX
            case 0x1320:
            case 0x1420:
            case 0x1520:
            case 0x1A20:
                Settings.SuperFx = true;
                //S9xInitSuperFX();
                if (Rom[0x7FDA] == 0x33)
                    SramSize = Rom[0x7FBD];
                else
                    SramSize = 5;
                throw new NotImplementedException();

            // SDD1
            case 0x4332:
            case 0x4532:
                Settings.Sdd1 = true;
                break;

            // ST018
            case 0xF530:
                /*
                Settings.SETA = ST_018;
                SetSETA = NULL;
                GetSETA = NULL;
                SRAMSize = 2;
                SNESGameFixes.SRAMInitialValue = 0x00;
                break;
                */
                throw new NotImplementedException();

            // ST010/011
            case 0xF630:
                /*
                if (Rom[0x7FD7] == 0x09)
                {
                    //Settings.SETA = ST_011;
                    //SetSETA = &S9xSetST011;
                    //GetSETA = &S9xGetST011;
                }
                else
                {
                    Settings.SETA = ST_010;
                    SetSETA = &S9xSetST010;
                    GetSETA = &S9xGetST010;
                }
                SRAMSize = 2;
                SNESGameFixes.SRAMInitialValue = 0x00;
                break;
                */
                throw new NotImplementedException();

            // C4
            case 0xF320:
                Settings.C4 = true;
                break;
            }

            if (false)
                throw new NotImplementedException("Msu1");
        }

        private static uint CaclulateCrc32(byte[] data, uint crc32)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            for (int i = 0; i < data.Length; i++)
                crc32 = ((crc32 >> 8) & 0x00FFFFFF) ^ Crc32Table[(crc32 ^ data[i]) & 0xFF];

            return ~crc32;
        }

        public static string GetString(Pointer<byte> data, int index, int length)
        {
            var s = new char[length];

            for (int i = 0; i < length; i++)
            {
                s[i] = (char)data[index + i];
            }

            return new string(s);
        }

        private string GetSafeName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var s = new char[name.Length];
            for (int i = s.Length; --i >= 0;)
            {
                if (s[i] >= 32 && s[i] < 127)
                    s[i] = name[i];
                else
                    s[i] = '_';
            }

            return new string(s);
        }

        private string GetSafeNameJis(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var s = new char[name.Length];
            for (int i = s.Length; --i >= 0;)
            {
                // ASCII characters
                if (s[i] >= 32 && s[i] < 127)
                    s[i] = name[i];

                // JIS x201 Katakana
                else if (RomRegion == 0 && s[i] >= 0xA0 && s[i] < 0xE0)
                    s[i] = name[i];
                else
                    s[i] = '_';
            }

            return new string(s);
        }

        private void ParseSnesHeader(Pointer<byte> romHeader)
        {
            bool bs = Settings.Bs & !Settings.BsxItself;

            RomName = GetString(romHeader, 0x10, RomNameLength - 1);
            if (bs)
                throw new NotImplementedException("BSX");

            if (bs)
            {
                throw new NotImplementedException("BSX");
            }
            else
                RomSize = romHeader[0x27];

            if (bs)
            {
                throw new NotImplementedException("BSX");
            }
            else
            {
                SramSize = romHeader[0x28];
                RomSpeed = romHeader[0x25];
                RomType = romHeader[0x26];
                RomRegion = romHeader[0x29];
            }

            RomChecksum = romHeader[0x2E] | (romHeader[0x2F] << 8);
            RomComplementChecksum = romHeader[0x2C] | (romHeader[0x2D] << 8);

            RomId = GetString(romHeader, 2, 4);

            if (romHeader[0x2A] != 0x33)
            {
                CompanyId = ((romHeader[0x2A] >> 4) & 0x0F) * 36 +
                    (romHeader[0x2A] & 0x0F);
            }
            else if (Char.IsLetterOrDigit((char)romHeader[0x00]) &&
                Char.IsLetterOrDigit((char)romHeader[0x01]))
            {
                int c0 = Char.ToUpper((char)romHeader[0x00]);
                int c1 = Char.ToUpper((char)romHeader[0x01]);

                c0 = (c0 > '9') ? c0 - '7' : c0 - '0';
                c1 = (c1 > '9') ? c1 - '7' : c1 - '0';
                CompanyId = c0 * 36 + c1;
            }
        }

        private void DeinterleaveType1(int size, Pointer<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            Settings.DisplayColor = PixelFormatter.BuildPixel(0, 31, 0);
            Emulator.Port.SetUiColor(0, 255, 0);

            var blocks = new byte[0x100];
            int nblocks = size >> 0x10;

            for (int i = 0; i < nblocks; i++)
            {
                blocks[i * 2] = (byte)(i + nblocks);
                blocks[i * 2 + 1] = (byte)i;
            }

            var array = data.GetArray();
            var offset = data.Offset;

            var temp = new byte[0x8000];
            for (int i = 0; i < nblocks * 2; i++)
            {
                for (int j = i; j < nblocks * 2; j++)
                {
                    if (blocks[j] == i)
                    {
                        Array.Copy(array, offset + blocks[j] * 0x8000, temp, 0, 0x8000);
                        Array.Copy(array, offset + blocks[i] * 0x8000, array, offset + blocks[j] * 0x8000, 0x8000);
                        Array.Copy(temp, 0, array, offset + blocks[i] * 0x8000, 0x8000);

                        byte b = blocks[j];
                        blocks[j] = blocks[i];
                        blocks[i] = b;
                        break;
                    }
                }
            }
        }

        private void DeinterleaveType2(int size, Pointer<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // For odd super FX images
            Settings.DisplayColor = PixelFormatter.BuildPixel(31, 14, 0);
            Emulator.Port.SetUiColor(255, 119, 25);

            var blocks = new byte[0x100];
            int nblocks = size >> 0x10;
            int step = 0x40;

            while (nblocks <= step)
                step >>= 1;
            nblocks = step;

            for (int i = 0; i < nblocks * 2; i++)
            {
                blocks[i] = (byte)((i & ~0x0F) | ((i & 3) << 2) | ((i & 0x0C) >> 2));
            }

            var array = data.GetArray();
            var offset = data.Offset;

            var temp = new byte[0x10000];
            for (int i = 0; i < nblocks * 2; i++)
            {
                for (int j = i; j < nblocks * 2; j++)
                {
                    if (blocks[j] == i)
                    {
                        Array.Copy(array, offset + blocks[j] * 0x10000, temp, 0, 0x10000);
                        Array.Copy(array, offset + blocks[i] * 0x10000, array, offset + blocks[j] * 0x10000, 0x10000);
                        Array.Copy(temp, 0, array, offset + blocks[i] * 0x10000, 0x10000);

                        byte b = blocks[j];
                        blocks[j] = blocks[i];
                        blocks[i] = b;
                        break;
                    }
                }
            }
        }

        private void DeinterleaveGd24(int size, Pointer<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // For 24Mb images dumped with Game Doctor.
            if (size != 0x300000)
                return;

            Settings.DisplayColor = Emulator.Gfx.PixelFormatter.BuildPixel(0, 31, 31);
            Emulator.Port.SetUiColor(0, 255, 255);

            var array = data.GetArray();
            var offset = data.Offset;

            var temp = new byte[0x80000];
            Array.Copy(array, offset + 0x180000, temp, 0, 0x80000);
            Array.Copy(array, offset + 0x200000, array, offset + 0x180000, 0x80000);
            Array.Copy(array, offset + 0x280000, array, offset + 0x200000, 0x80000);
            Array.Copy(temp, 0, array, offset + 0x280000, 0x80000);

            DeinterleaveType1(size, data);
        }

        private static readonly uint[] Crc32Table = new uint[0x100]
        {
            0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419, 0x706af48f,
            0xe963a535, 0x9e6495a3, 0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988,
            0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91, 0x1db71064, 0x6ab020f2,
            0xf3b97148, 0x84be41de, 0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7,
            0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9,
            0xfa0f3d63, 0x8d080df5, 0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172,
            0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b, 0x35b5a8fa, 0x42b2986c,
            0xdbbbc9d6, 0xacbcf940, 0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
            0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423,
            0xcfba9599, 0xb8bda50f, 0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924,
            0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d, 0x76dc4190, 0x01db7106,
            0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433,
            0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x086d3d2d,
            0x91646c97, 0xe6635c01, 0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e,
            0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457, 0x65b0d9c6, 0x12b7e950,
            0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
            0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7,
            0xa4d1c46d, 0xd3d6f4fb, 0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0,
            0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9, 0x5005713c, 0x270241aa,
            0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
            0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81,
            0xb7bd5c3b, 0xc0ba6cad, 0xedb88320, 0x9abfb3b6, 0x03b6e20c, 0x74b1d29a,
            0xead54739, 0x9dd277af, 0x04db2615, 0x73dc1683, 0xe3630b12, 0x94643b84,
            0x0d6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1,
            0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb,
            0x196c3671, 0x6e6b06e7, 0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc,
            0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5, 0xd6d6a3e8, 0xa1d1937e,
            0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b,
            0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55,
            0x316e8eef, 0x4669be79, 0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236,
            0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 0xc5ba3bbe, 0xb2bd0b28,
            0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
            0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x026d930a, 0x9c0906a9, 0xeb0e363f,
            0x72076785, 0x05005713, 0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38,
            0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21, 0x86d3d2d4, 0xf1d4e242,
            0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777,
            0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69,
            0x616bffd3, 0x166ccf45, 0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2,
            0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc,
            0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
            0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693,
            0x54de5729, 0x23d967bf, 0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94,
            0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d
        };
    }
}
