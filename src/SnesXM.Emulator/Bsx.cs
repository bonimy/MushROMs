using System;
using System.Globalization;
using System.IO;
using System.Text;
using Helper;

namespace SnesXM.Emulator
{
    public sealed partial class Bsx : IBsx, IDisposable
    {
        private const int BiosSize = 0x100000;
        private const int DefaultFlashSize = 0x100000;
        private const int FlashSizeMask = DefaultFlashSize - 1;
        private const int PsRamSize = 0x80000;
        private const int MmcLength = 0x10;
        private const int PpuLength = 0x20;
        private const int OutputLength = 0x20;

        private const int PpuBase = 0x2180;

        private IEmulator Emulator
        {
            get;
            set;
        }
        private ISettings Settings;
        private IDisplay Display;
        private IMemoryMap Memory => Emulator.MemoryMap;
        private IMultiCart Multi;
        private Pointer<byte>[] Map => Memory.Map;
        private bool[] BlockIsRam => Memory.BlockIsRam;
        private bool[] BlockIsRom => Memory.BlockIsRom;
        private Pointer<byte> Ram => Memory.Ram;
        private Pointer<byte> Sram => Memory.Sram;
        private Pointer<byte> PsRam => Memory.BsRam;
        private Pointer<byte> BiosRom => Memory.BiosRom;

        private static readonly byte[] Flashcard = new byte[20] // FIXME
        {
            0x4D, 0x00, 0x50, 0x00,	// vendor id
	        0x00, 0x00,				// ?
	        0x1A, 0x00,				// 2MB Flash (1MB = 0x2A)
	        0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] Initial2192 = new byte[0x20]
        {
            00, 00, 00, 00, 00,		// unknown
	        01, 01, 00, 00, 00,
            00,						// seconds (?)
	        00,						// minutes
	        00,						// hours
	        10, 10, 10, 10, 10,		// unknown
	        10, 10, 10, 10, 10,		// dummy
	        00, 00, 00, 00, 00, 00, 00, 00, 00
        };

        private bool FlashMode
        {
            get;
            set;
        }
        private int FlashSize
        {
            get;
            set;
        }

        public Pointer<byte> MapRom
        {
            get;
            private set;
        }
        private Pointer<byte> FlashRom
        {
            get;
            set;
        }

        private bool Dirty1
        {
            get;
            set;
        }
        private bool Dirty2
        {
            get;
            set;
        }
        private bool Bootup
        {
            get;
            set;
        }
        private bool FlashEnable
        {
            get;
            set;
        }
        private bool WriteEnable
        {
            get;
            set;
        }
        private bool ReadEnable
        {
            get;
            set;
        }
        private int FlashCommand
        {
            get;
            set;
        }
        private int OldWrite
        {
            get;
            set;
        }
        private int NewWrite
        {
            get;
            set;
        }
        private int OutIndex
        {
            get;
            set;
        }
        private byte[] Output
        {
            get;
            set;
        }
        private byte[] RelativePpu
        {
            get;
            set;
        }
        private Pointer<byte> RealPpu
        {
            get;
            set;
        }
        private byte[] Mmc
        {
            get;
            set;
        }
        private byte[] PreviousMmc
        {
            get;
            set;
        }
        private byte[] Test2192
        {
            get;
            set;
        }

        private bool FlashCsr
        {
            get;
            set;
        }
        private bool FlashGsr
        {
            get;
            set;
        }
        private bool FlashBsr
        {
            get;
            set;
        }
        private bool FlashCommandDone
        {
            get;
            set;
        }

        private Stream SatStream1
        {
            get;
            set;
        }

        private Stream SatStream2
        {
            get;
            set;
        }

        private bool SatPfLatch1Enable
        {
            get;
            set;
        }
        private bool SatDtLatch1Enable
        {
            get;
            set;
        }
        private bool SatPfLatch2Enable
        {
            get;
            set;
        }
        private bool SatDtLatch2Enable
        {
            get;
            set;
        }

        private bool SatStream1Loaded
        {
            get;
            set;
        }
        private bool SatStream1First
        {
            get;
            set;
        }
        private int SatStream1Count
        {
            get;
            set;
        }

        private bool SatStream2Loaded
        {
            get;
            set;
        }
        private bool SatStream2First
        {
            get;
            set;
        }
        private int SatStream2Count
        {
            get;
            set;
        }

        public int this[int address]
        {
            get
            {
                var bank = (byte)(address >> 0x10);
                var offset = (ushort)address;
                var value = 0;

                // MMC
                if (bank >= 0x01 && bank <= 0x0E)
                    return Mmc[bank];

                // Flash IO
                if (bank < 0xC0)
                    return value;

                // default: read-through mode
                value = GetBypassFlashIO(address);

                // note: may be more registers, purposes unknown
                switch (offset)
                {
                case 0x0002:
                case 0x8002:
                    if (FlashBsr)
                        value = 0xC0; // Page Status Register
                    break;

                case 0x0004:
                case 0x8004:
                    if (FlashGsr)
                        value = 0x82; // Global Status Register
                    break;

                case 0x5555:
                    if (FlashEnable)
                        value = 0x80; // ???
                    break;

                case 0xFF00:
                case 0xFF02:
                case 0xFF04:
                case 0xFF06:
                case 0xFF08:
                case 0xFF0A:
                case 0xFF0C:
                case 0xFF0E:
                case 0xFF10:
                case 0xFF12:
                    // return flash vendor information
                    if (ReadEnable)
                        value = Flashcard[offset - 0xFF00];
                    break;
                }

                if (FlashCsr)
                {
                    value = 0x80; // Compatible Status Register
                    FlashCsr = false;
                }

                return value;
            }
            set
            {
                var bank = (byte)(address >> 0x10);
                var offset = (ushort)address;
                var result = (byte)value;

                // MMC
                if (bank >= 0x01 && bank <= 0x0E)
                {
                    Mmc[bank] = result;
                    if (bank == 0x0E)
                        MapAll();
                }

                // Flash IO
                if (bank < 0xC0)
                    return;

                // Write to Flash
                if (WriteEnable)
                {
                    SetBypassFlashIO(address, result);
                    WriteEnable = false;
                    return;
                }

                // Flash Command Handling
                if (Mmc[0x0C] == 0)
                    return;

                //Memory Pack Type 1 & 3 & 4
                FlashCommand <<= 8;
                FlashCommand |= result;

                switch (result)
                {
                case 0x00:
                case 0xFF:
                    //Reset to normal
                    FlashEnable = false;
                    FlashBsr = false;
                    FlashCsr = false;
                    FlashGsr = false;
                    ReadEnable = false;
                    WriteEnable = false;
                    FlashCommandDone = true;
                    break;

                case 0x10:
                case 0x40:
                    // Write byte
                    FlashEnable = false;
                    FlashBsr = false;
                    FlashCsr = true;
                    FlashGsr = false;
                    ReadEnable = false;
                    WriteEnable = true;
                    FlashCommandDone = true;
                    break;

                case 0x50:
                    // Clear Status Register
                    FlashEnable = false;
                    FlashBsr = false;
                    FlashCsr = false;
                    FlashGsr = false;
                    FlashCommandDone = true;
                    break;

                case 0x70:
                    // Read CSR
                    FlashEnable = false;
                    FlashBsr = false;
                    FlashCsr = true;
                    FlashGsr = false;
                    ReadEnable = false;
                    WriteEnable = false;
                    FlashCommandDone = true;
                    break;

                case 0x71:
                    // Read Extended Status Registers (Page and Global)
                    FlashEnable = false;
                    FlashBsr = true;
                    FlashCsr = false;
                    FlashGsr = true;
                    ReadEnable = false;
                    WriteEnable = false;
                    FlashCommandDone = true;
                    break;

                case 0x75:
                    // Show Page Buffer / Vendor Info
                    FlashCsr = false;
                    ReadEnable = true;
                    FlashCommandDone = true;
                    break;

                case 0xD0:
                    // DO COMMAND
                    switch ((ushort)FlashCommand)
                    {
                    case 0x20D0: //Block Erase
                        var dest = (Mmc[0x02] != 0) ?
                            (address & 0x0F0000) : ((address & 0x1E0000) >> 1);

                        for (var x = 0; x < 0x10000; x++)
                            MapRom[dest + x] = Byte.MaxValue;
                        break;

                    case 0xA7D0: //Chip Erase (ONLY IN TYPE 1 AND 4)
                        if ((Flashcard[6] & 0xF0) == 0x10 || (Flashcard[6] & 0xF0) == 0x40)
                        {
                            for (var x = 0; x < DefaultFlashSize; x++)
                            {
                                MapRom[x] = 0xFF;
                            }
                        }
                        break;

                    case 0x38D0: //Flashcart Reset
                        break;
                    }
                    break;
                }
            }
        }

        private IIndexer<int> FlashIO { get; }
        public IIndexer<int> Ppu { get; }

        public Bsx(IEmulator emulator)
        {
            Emulator = emulator ?? throw new ArgumentNullException(nameof(emulator));

            Output = new byte[OutputLength];
            RelativePpu = new byte[PpuLength];
            RealPpu = (Pointer<byte>)RelativePpu - PpuBase;
            Mmc = new byte[MmcLength];
            PreviousMmc = new byte[MmcLength];
            Test2192 = new byte[0x20];

            FlashIO = new FlashIOIndexer(this);
            Ppu = new PpuIndexer(this);
        }

        public void Initialize()
        {
            Settings.Bs = false;

            if (IsBsxBios(Memory.Rom, Memory.CalculatedSize))
            {
                // BS-X itself

                Settings.Bs = true;
                Settings.BsxItself = true;

                Memory.LoRom = true;
                Memory.HiRom = false;



                FlashMode = false;
                FlashSize = DefaultFlashSize;

                Bootup = true;
            }
            else
            {
                Settings.BsxItself = false;

                var r1 = IsBsx(Memory.Rom + 0x7FC0) == 1;
                var r2 = IsBsx(Memory.Rom + 0xFFC0) == 1;
                Settings.Bs = r1 || r2;

                if (Settings.Bs)
                {
                    // BS games

                    Memory.LoRom = r1;
                    Memory.HiRom = r2;

                    var header = Memory.Rom + (r1 ? 0x7FC0 : 0xFFC0);

                    FlashMode = (header[0x18] & 0xEF) != 0x20;
                    FlashSize = DefaultFlashSize;

                    Bootup = Settings.BsxBootup;

                    if (!LoadBios() && !IsBsxBios(BiosRom, BiosSize))
                    {
                        Bootup = false;
                        BiosRom.Clear(BiosSize);
                    }
                }
            }

            if (Settings.Bs)
            {
                MapRom = null;
                FlashRom = Memory.Rom;
                Emulator.SramInitialValue = 0x00;
            }
        }

        public void Reset()
        {
            if (Settings.BsxItself)
                Memory.Rom.Clear(FlashSize);

            Array.Clear(RelativePpu, 0, PpuLength);
            Array.Clear(Mmc, 0, MmcLength);
            Array.Clear(PreviousMmc, 0, MmcLength);

            Dirty1 = false;
            Dirty2 = false;
            FlashEnable = false;
            WriteEnable = false;
            ReadEnable = false;
            FlashCommand = 0;
            OldWrite = 0;
            NewWrite = 0;

            OutIndex = 0;
            Array.Clear(Output, 0, OutputLength);

            // starting from the bios
            Mmc[0x02] = Mmc[0x03] = Mmc[0x05] = Mmc[0x06] = 0x80;
            Mmc[0x09] = Mmc[0x0B] = 0x80;

            Mmc[0x07] = Mmc[0x08] = 0x80;
            Mmc[0x0E] = 0x80;

            // stream reset
            SatPfLatch1Enable = SatDtLatch1Enable = false;
            SatPfLatch2Enable = SatDtLatch2Enable = false;

            SatStream1Loaded = SatStream2Loaded = false;
            SatStream1First = SatStream2First = false;
            SatStream1Count = SatStream2Count = 0;

            ReleaseSatStream1();
            ReleaseSatStream2();

            if (Settings.Bs)
                MapAll();
        }

        public void PostLoadState()
        {
            var temp = new byte[MmcLength];

            var pd1 = Dirty1;
            var pd2 = Dirty2;
            Array.Copy(Mmc, temp, MmcLength);

            Array.Copy(PreviousMmc, Mmc, MmcLength);
            MapAll();

            Array.Copy(temp, Mmc, MmcLength);
            Dirty1 = pd1;
            Dirty2 = pd2;
        }

        private bool LoadBios()
        {
            var dir = Display.GetDirectory(DirectoryType.Bios);
            dir += Path.PathSeparator;
            var path = dir + "BS-X.bin";

            if (!File.Exists(path))
                path = dir + "BS-X.bios";

            if (!File.Exists(path))
                return false;

            byte[] data;

            try
            {
                data = File.ReadAllBytes(path);
            }
            catch (Exception)
            {
                return false;
            }

            BiosRom.Copy(data, Math.Min(data.Length, BiosSize));

            return true;
        }

        private static bool IsBsxBios(Pointer<byte> rom, int size)
        {
            return size == BiosSize && Encoding.ASCII.GetString(
                rom.GetArray(), rom.Offset + 0x7FC0, 21) == "Satellaview BS-X     ";
        }

        private void ReleaseSatStream1()
        {
            if (SatStream1 != null)
            {
                SatStream1.Dispose();
                SatStream1 = null;
            }
        }
        private void ReleaseSatStream2()
        {
            if (SatStream2 != null)
            {
                SatStream2.Dispose();
                SatStream2 = null;
            }
        }

        private void SetStream1(int count)
        {
            ReleaseSatStream1();

            var dir = Display.GetDirectory(DirectoryType.Sat);
            dir += Path.PathSeparator;

            var name = String.Format(CultureInfo.InvariantCulture, "BS{0:X4}-{1}.bin",
                RealPpu[0x2188] | (RealPpu[0x2189] << 8), (byte)count);

            var path = dir + name;

            if (!File.Exists(path))
            {
                SatStream1Loaded = false;
                return;
            }

            try
            {
                SatStream1 = File.OpenRead(path);
            }
            catch (Exception)
            {
                SatStream1Loaded = false;
                return;
            }

            var queueSize = SatStream1.Length / 22f;
            RealPpu[0x218A] = (byte)(Math.Ceiling(queueSize));
            RealPpu[0x218D] = 0;
            SatStream1First = true;
            SatStream1Loaded = true;
        }

        private void SetStream2(int count)
        {
            ReleaseSatStream2();

            var dir = Display.GetDirectory(DirectoryType.Sat);
            dir += Path.PathSeparator;

            var name = String.Format(CultureInfo.InvariantCulture, "BS{0:X4}-{1}.bin",
                RealPpu[0x218E] | (RealPpu[0x218F] << 8), (byte)count);

            var path = dir + name;

            if (!File.Exists(path))
            {
                SatStream2Loaded = false;
                return;
            }

            try
            {
                SatStream2 = File.OpenRead(path);
            }
            catch (Exception)
            {
                SatStream2Loaded = false;
                return;
            }

            var queueSize = SatStream2.Length / 22f;
            RealPpu[0x2190] = (byte)(Math.Ceiling(queueSize));
            RealPpu[0x2193] = 0;
            SatStream2First = true;
            SatStream2Loaded = true;
        }

        private byte GetRtc()
        {
            var time = DateTime.Now;

            Test2192[0] = 0x00;
            Test2192[1] = 0x00;
            Test2192[2] = 0x00;
            Test2192[3] = 0x00;
            Test2192[4] = 0x10;
            Test2192[5] = 0x01;
            Test2192[6] = 0x01;
            Test2192[7] = 0x00;
            Test2192[8] = 0x00;
            Test2192[9] = 0x00;
            Test2192[10] = (byte)time.Second;
            Test2192[11] = (byte)time.Minute;
            Test2192[12] = (byte)time.Hour;
            Test2192[13] = (byte)(time.DayOfWeek + 1);
            Test2192[14] = (byte)time.Day;
            Test2192[15] = (byte)time.Month;
            Test2192[16] = (byte)(time.Year + 1900);
            Test2192[17] = (byte)((time.Year + 1900) >> 8);

            var result = Test2192[OutIndex++];

            if (OutIndex > 22)
                OutIndex = 0;

            return result;
        }

        private void MapAll()
        {
            Array.Copy(Mmc, PreviousMmc, MmcLength);

            MapRom = new Pointer<byte>(FlashRom);
            FlashSize = DefaultFlashSize;

            MapSnes();

            if (Mmc[0x02] != 0)
                MapHiRom();
            else
                MapLoRom();

            MapFlashIO();
            MapPsRam();
            MapSram();
            MapRam();

            MapBios();
            MapMmc();

            // Monitor new register changes
            Dirty1 = false;
            Dirty2 = false;

            Memory.MapWriteProtectRom();
        }

        private void MapSnes()
        {
            // Banks 00->3F and 80->BF
            for (var c = 0; c < 0x400; c += 0x10)
            {
                Map[c + 0] = Map[c + 0x800] = Ram;
                Map[c + 1] = Map[c + 0x801] = Ram;
                BlockIsRam[c + 0] = BlockIsRam[c + 0x800] = true;
                BlockIsRam[c + 1] = BlockIsRam[c + 0x801] = true;

                // We will need a better way to manage this in the future.
                Map[c + 2] = Map[c + 0x802] = new byte[(int)MapType.Ppu];
                Map[c + 3] = Map[c + 0x803] = new byte[(int)MapType.Ppu];
                Map[c + 4] = Map[c + 0x804] = new byte[(int)MapType.Cpu];
                Map[c + 5] = Map[c + 0x805] = new byte[(int)MapType.Cpu];
                Map[c + 6] = Map[c + 0x806] = new byte[(int)MapType.None];
                Map[c + 7] = Map[c + 0x807] = new byte[(int)MapType.None];
            }
        }

        private void MapLoRom()
        {
            // Banks 00->3F and 80->BF
            for (var c = 0; c < 0x400; c += 0x10)
            {
                for (var i = c; i < c + 0x10; i++)
                {
                    Map[i] = Map[i + 0x800] = MapRom + ((c << 11) % FlashSize) - 0x8000;
                    BlockIsRam[i] = BlockIsRam[i + 0x800] = WriteEnable;
                    BlockIsRom[i] = BlockIsRom[i + 0x800] = !WriteEnable;
                }
            }

            // Banks 40->7F and C0->FF
            for (var c = 0; c < 0x400; c += 16)
            {
                for (var i = c; i < c + 8; i++)
                    Map[i + 0x400] = Map[i + 0xC00] = MapRom + (c << 11) % FlashSize;

                for (var i = c + 8; i < c + 16; i++)
                    Map[i + 0x400] = Map[i + 0xC00] = MapRom + ((c << 11) % FlashSize) - 0x8000;

                for (var i = c; i < c + 16; i++)
                {
                    BlockIsRam[i + 0x400] = BlockIsRam[i + 0xC00] = WriteEnable;
                    BlockIsRom[i + 0x400] = BlockIsRom[i + 0xC00] = !WriteEnable;
                }
            }
        }

        private void MapHiRom()
        {
            // Banks 00->3F and 80->BF
            for (var c = 0; c < 0x400; c += 0x10)
            {
                for (var i = c + 8; i < c + 0x10; i++)
                {
                    Map[i] = Map[i + 0x800] = MapRom + (c << 12) % FlashSize;
                    BlockIsRam[i] = BlockIsRam[i + 0x800] = WriteEnable;
                    BlockIsRom[i] = BlockIsRom[i + 0x800] = !WriteEnable;
                }
            }

            // Banks 40->7F and C0->FF
            for (var c = 0; c < 0x400; c += 16)
            {
                for (var i = c; i < c + 16; i++)
                {
                    Map[i + 0x400] = Map[i + 0xC00] = MapRom + (c << 12) % FlashSize;
                    BlockIsRam[i + 0x400] = BlockIsRam[i + 0xC00] = WriteEnable;
                    BlockIsRom[i + 0x400] = BlockIsRom[i + 0xC00] = !WriteEnable;
                }
            }
        }

        private void MapMmc()
        {
            // Banks 01->0E:5000-5FFF
            for (var c = 0x010; c < 0x0F0; c += 16)
            {
                Map[c + 5] = new byte[(int)MapType.Bsx];
                BlockIsRam[c + 5] = BlockIsRom[c + 5] = false;
            }
        }

        private void MapFlashIO()
        {
            if (Mmc[0x0C] != 0 || Mmc[0x0D] != 0)
            {
                // Bank C0:0000, 2AAA, 5555, FF00-FF1F
                for (var c = 0; c < 0x10; c++)
                {
                    Map[c + 0xC00] = new byte[(int)MapType.Bsx];
                    BlockIsRam[c + 0xC00] = true;
                    BlockIsRom[c + 0xC00] = false;
                }
            }
        }

        private void MapSram()
        {
            // Banks 10->17:5000-5FFF
            for (var c = 0x100; c < 0x180; c += 16)
            {
                Map[c + 5] = Sram + ((c & 0x70) << 8) - 0x5000;
                BlockIsRam[c + 5] = true;
                BlockIsRom[c + 5] = false;
            }
        }

        private void MapPsRamMirrorSub(int bank)
        {
            bank <<= 4;

            if (Mmc[0x02] != 0)
            {
                for (var c = 0; c < 0x100; c += 16)
                {
                    for (var i = c; i < c + 16; i++)
                    {
                        Map[i + bank] = PsRam + (c << 12) % PsRamSize;
                        BlockIsRam[i + bank] = true;
                        BlockIsRom[i + bank] = false;
                    }
                }
            }
            else
            {
                for (var c = 0; c < 0x100; c += 16)
                {
                    if ((bank & 0x7F) >= 0x40)
                    {
                        for (var i = c; i < c + 8; i++)
                            Map[i + bank] = PsRam + (c << 11) % PsRamSize;

                        for (var i = c; i < c + 8; i++)
                        {
                            BlockIsRam[i + bank] = true;
                            BlockIsRom[i + bank] = false;
                        }
                    }

                    for (var i = c + 8; i < c + 16; i++)
                        Map[i + bank] = PsRam + ((c << 11) % PsRamSize) - 0x8000;

                    for (var i = c + 8; i < c + 16; i++)
                    {
                        BlockIsRam[i + bank] = true;
                        BlockIsRom[i + bank] = false;
                    }
                }
            }
        }

        private void MapPsRam()
        {
            if (Mmc[0x02] == 0)
            {
                //LoROM Mode
                if (Mmc[0x05] == 0 && Mmc[0x06] == 0)
                {
                    //Map PSRAM to 00-0F/80-8F
                    if (Mmc[0x03] != 0)
                        MapPsRamMirrorSub(0x00);

                    if (Mmc[0x04] != 0)
                        MapPsRamMirrorSub(0x80);
                }
                else if (Mmc[0x05] != 0 && Mmc[0x06] == 0)
                {
                    //Map PSRAM to 20-2F/A0-AF
                    if (Mmc[0x03] != 0)
                        MapPsRamMirrorSub(0x20);

                    if (Mmc[0x04] != 0)
                        MapPsRamMirrorSub(0xA0);
                }
                else if (Mmc[0x05] == 0 && Mmc[0x06] != 0)
                {
                    //Map PSRAM to 40-4F/C0-CF
                    if (Mmc[0x03] != 0)
                        MapPsRamMirrorSub(0x40);

                    if (Mmc[0x04] != 0)
                        MapPsRamMirrorSub(0xC0);
                }
                else
                {
                    //Map PSRAM to 60-6F/E0-EF
                    if (Mmc[0x03] != 0)
                        MapPsRamMirrorSub(0x60);

                    if (Mmc[0x04] != 0)
                        MapPsRamMirrorSub(0xE0);
                }

                //Map PSRAM to 70-7D/F0-FF
                if (Mmc[0x03] != 0)
                    MapPsRamMirrorSub(0x70);

                if (Mmc[0x04] != 0)
                    MapPsRamMirrorSub(0xF0);
            }
            else
            {
                //HiROM Mode
                if (Mmc[0x05] == 0 && Mmc[0x06] == 0)
                {
                    //Map PSRAM to 40-47/C0-C7
                    if (Mmc[0x03] != 0)
                        MapPsRamMirrorSub(0x40);

                    if (Mmc[0x04] != 0)
                        MapPsRamMirrorSub(0xC0);

                }
                else if (Mmc[0x05] != 0 && Mmc[0x06] == 0)
                {
                    //Map PSRAM to 50-57/D0-D7
                    if (Mmc[0x03] != 0)
                        MapPsRamMirrorSub(0x50);

                    if (Mmc[0x04] != 0)
                        MapPsRamMirrorSub(0xD0);
                }
                else if (Mmc[0x05] == 0 && Mmc[0x06] != 0)
                {
                    //Map PSRAM to 60-67/E0-E7
                    if (Mmc[0x03] != 0)
                        MapPsRamMirrorSub(0x60);

                    if (Mmc[0x04] != 0)
                        MapPsRamMirrorSub(0xE0);
                }
                else
                {
                    //Map PSRAM to 70-77/F0-F7
                    if (Mmc[0x03] != 0)
                        MapPsRamMirrorSub(0x70);

                    if (Mmc[0x04] != 0)
                        MapPsRamMirrorSub(0xF0);
                }

                if (Mmc[0x03] != 0)
                {
                    //Map PSRAM to 20->3F:6000-7FFF
                    for (var c = 0x200; c < 0x400; c += 16)
                    {
                        Map[c + 6] = PsRam + ((c & 0x70) << 12) % PsRamSize;
                        Map[c + 7] = PsRam + ((c & 0x70) << 12) % PsRamSize;
                        BlockIsRam[c + 6] = true;
                        BlockIsRam[c + 7] = true;
                        BlockIsRom[c + 6] = false;
                        BlockIsRom[c + 7] = false;
                    }
                }

                if (Mmc[0x04] != 0)
                {
                    //Map PSRAM to A0->BF:6000-7FFF
                    for (var c = 0xA00; c < 0xC00; c += 16)
                    {
                        Map[c + 6] = PsRam + ((c & 0x70) << 12) % PsRamSize;
                        Map[c + 7] = PsRam + ((c & 0x70) << 12) % PsRamSize;
                        BlockIsRam[c + 6] = true;
                        BlockIsRam[c + 7] = true;
                        BlockIsRom[c + 6] = false;
                        BlockIsRom[c + 7] = false;
                    }
                }
            }
        }

        private void MapBios()
        {
            // Banks 00->1F:8000-FFFF
            if (Mmc[0x07] != 0)
            {
                for (var c = 0; c < 0x200; c += 16)
                {
                    for (var i = c + 8; i < c + 16; i++)
                    {
                        Map[i] = BiosRom + ((c << 11) % BiosSize) - 0x8000;
                        BlockIsRam[i] = false;
                        BlockIsRom[i] = true;
                    }
                }
            }

            // Banks 80->9F:8000-FFFF
            if (Mmc[0x08] != 0)
            {
                for (var c = 0; c < 0x200; c += 16)
                {
                    for (var i = c + 8; i < c + 16; i++)
                    {
                        Map[i + 0x800] = BiosRom + ((c << 11) % BiosSize) - 0x8000;
                        BlockIsRam[i + 0x800] = false;
                        BlockIsRom[i + 0x800] = true;
                    }
                }
            }
        }

        private void MapRam()
        {
            // Banks 7E->7F
            for (var c = 0; c < 16; c++)
            {
                Map[c + 0x7E0] = Ram;
                Map[c + 0x7F0] = Ram + 0x10000;
                BlockIsRam[c + 0x7E0] = true;
                BlockIsRam[c + 0x7F0] = true;
                BlockIsRom[c + 0x7E0] = false;
                BlockIsRom[c + 0x7F0] = false;
            }
        }

        private void MapDirty()
        {
            // Banks 00->1F and 80->9F:8000-FFFF
            if (Mmc[0x02] != 0)
            {
                for (var c = 0; c < 0x200; c += 16)
                {
                    for (var i = c + 8; i < c + 16; i++)
                    {
                        Map[i] = Map[i + 0x800] = MapRom + (c << 12) % FlashSize;
                        BlockIsRam[i] = BlockIsRam[i + 0x800] = WriteEnable;
                        BlockIsRom[i] = BlockIsRom[i + 0x800] = !WriteEnable;
                    }
                }
            }
            else
            {
                for (var c = 0; c < 0x200; c += 16)
                {
                    for (var i = c + 8; i < c + 16; i++)
                    {
                        Map[i] = Map[i + 0x800] = MapRom + ((c << 11) % FlashSize) - 0x8000;
                        BlockIsRam[i] = BlockIsRam[i + 0x800] = WriteEnable;
                        BlockIsRom[i] = BlockIsRom[i + 0x800] = !WriteEnable;
                    }
                }
            }
        }

        private static bool IsValidNormalBank(int bank)
        {
            switch ((byte)bank)
            {
            case 0x20:
            case 0x21:
            case 0x30:
            case 0x31:
                return true;
            default:
                return false;
            }
        }

        private static int IsBsx(Pointer<byte> p)
        {
            if ((p[26] == 0x33 || p[26] == 0xFF) &&
                (p[21] != 0 || (p[21] & 0x83) == 0x80) &&
                IsValidNormalBank(p[24]))
            {
                var m = p[22];

                if (m == 0 && p[23] == 0)
                    return 2;

                if ((m == 0xFF && p[23] == 0xFF) || ((m & 0x0F) == 0 && ((m >> 4) - 1 < 12)))
                    return 1;
            }

            return 0;
        }

        public void Dispose()
        {
            ReleaseSatStream1();
            ReleaseSatStream2();
        }
    }
}