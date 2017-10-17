using System;
using Helper;

namespace SnesXM
{
    public interface IMemoryMap
    {
        Pointer<byte> NsrtHeader { get; }

        int HeaderCount { get; set; }

        Pointer<byte> Ram { get; }
        Pointer<byte> Rom { get; }
        Pointer<byte> Sram { get; }
        Pointer<byte> Vram { get; }
        Pointer<byte> FillRam { get; }
        Pointer<byte> BwRam { get; }
        Pointer<byte> C4Ram { get; }
        Pointer<byte> Obc1Ram { get; }
        Pointer<byte> BsRam { get; }
        Pointer<byte> BiosRom { get; }

        Pointer<byte>[] Map { get; }
        Pointer<byte>[] WriteMap { get; }
        bool[] BlockIsRam { get; }
        bool[] BlockIsRom { get; }

        byte ExtendedFormat { get; set; }

        string RomFilename { get; set; }
        string RomName { get; set; }
        string RawRomNamw { get; set; }
        string RomId { get; set; }
        int CompanyId { get; set; }
        byte RomRegion { get; set; }
        byte RomSpeed { get; set; }
        byte RomType { get; set; }
        byte RomSize { get; set; }
        int RomChecksum { get; set; }
        int RomComplementChecksum { get; set; }
        int RomCrc32 { get; set; }
        int RomFramesPerSecond { get; set; }

        bool HiRom { get; set; }
        bool LoRom { get; set; }
        byte SramSize { get; set; }
        int SramMask { get; set; }
        int CalculatedSize { get; set; }
        int CalculatedChecksum { get; set; }

        event EventHandler PostRomInit;

        void MapWriteProtectRom();
    }
}
