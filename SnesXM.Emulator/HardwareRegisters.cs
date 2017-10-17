using System;

namespace SnesXM.Emulator
{
    public class HardwareRegisters
    {
        //               Register                      Address    Name         Style   Access  Timing
        public const int ScreenDisplay               = 0x2100; // INIDISP      single  write   any time
        public const int ObjectSizeAndCharacterSize  = 0x2101; // OBSEL        single  write   RegisterTiming.FBlankVBlank;
        public const int OamAddressLow               = 0x2102; // OAMADDL      single  write   RegisterTiming.FBlankVBlank;
        public const int OamAddressHigh              = 0x2103; // OAMADDH      single  write   RegisterTiming.FBlankVBlank;
        public const int OamDataWrite                = 0x2104; // OAMDATA      single  write   RegisterTiming.FBlankVBlank;
        public const int BgModeandCharacterSize      = 0x2105; // BGMODE       single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int Mosaic                      = 0x2106; // MOSAIC       single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int BgTilemapAddressBg1         = 0x2107; // BG1SC        single  write   RegisterTiming.FBlankVBlank;
        public const int BgTilemapAddressBg2         = 0x2108; // BG2SC        single  write   RegisterTiming.FBlankVBlank;
        public const int BgTilemapAddressBg3         = 0x2109; // BG3SC        single  write   RegisterTiming.FBlankVBlank;
        public const int BgTilemapAddressBg4         = 0x210A; // BG3SC        single  write   RegisterTiming.FBlankVBlank;
        public const int BgCharacterAddressBg1And2   = 0x210B; // BG12NBA      single  write   RegisterTiming.FBlankVBlank;
        public const int BgCharacterAddressBg3And4   = 0x210C; // BG34NBA      single  write   RegisterTiming.FBlankVBlank;
        public const int BgScrollHBg1                = 0x210D; // BG1HOFS      dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int BgScrollVBg1                = 0x210E; // BG1VOFS      dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int BgScrollHBg2                = 0x210F; // BG2HOFS      dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int BgScrollVBg2                = 0x2110; // BG2VOFS      dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int BgScrollHBg3                = 0x2111; // BG3HOFS      dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int BgScrollVBg3                = 0x2112; // BG3VOFS      dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int BgScrollHBg4                = 0x2113; // BG4HOFS      dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int BgScrollVBg4                = 0x2114; // BG4VOFS      dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int VideoPortControl            = 0x2115; // VMAIN        single  write   RegisterTiming.FBlankVBlank;
        public const int VramAddressLow              = 0x2116; // VMADDL       single  write   RegisterTiming.FBlankVBlank;
        public const int VramAddressHigh             = 0x2117; // VMADDH       single  write   RegisterTiming.FBlankVBlank;
        public const int VramDataWriteLow            = 0x2118; // VMDATAL      single  write   RegisterTiming.FBlankVBlank;
        public const int VramDataWriteHigh           = 0x2119; // VMDATAH      single  write   RegisterTiming.FBlankVBlank;
        public const int Mode7Settings               = 0x211A; // M7SEL        single  write   RegisterTiming.FBlankVBlank;
        public const int Mode7MatrixA                = 0x211B; // M7A          dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int Mode7MatrixB                = 0x211C; // M7B          dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int Mode7MatrixC                = 0x211D; // M7C          dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int Mode7MatrixD                = 0x211E; // M7D          dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int Mode7MatrixX                = 0x211F; // M7X          dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int Mode7MatrixY                = 0x2120; // M7Y          dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int CgRamAddress                = 0x2121; // CGADD        single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int CgRamDataWrite              = 0x2122; // CGDATA       dual    write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowMaskSettingsBg1And2   = 0x2123; // W12SEL       single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowMaskSettingsBg3And4   = 0x2124; // W34SEL       single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowMaskSettingsObj       = 0x2125; // WOBJSEL      single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowPositionWH0           = 0x2126; // WH0          single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowPositionWH1           = 0x2127; // WH1          single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowPositionWH2           = 0x2128; // WH2          single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowPositionWH3           = 0x2129; // WH3          single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowMaskLogicregistersBg  = 0x212A; // WBGLOG       single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int WindowMaskLogicregistersObj = 0x212B; // WOBJLOG      single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int MainScreenDestination       = 0x212C; // TM           single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int SubScreenDestination        = 0x212D; // TS           single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int MainWindowMaskDestination   = 0x212E; // TMW          single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int SubWindowMaskDestination    = 0x212F; // TSW          single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int ColorMathSettings           = 0x2130; // CGWSEL       single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int ColorMathDesignation        = 0x2131; // CGADSUB      single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int ColorMathData               = 0x2132; // COLDATA      single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int ScreenModeSelect            = 0x2133; // SETINI       single  write   RegisterTiming.FBlankVBlankHBlank;
        public const int MultiplicationResultLow     = 0x2134; // MPYL         single  read    RegisterTiming.FBlankVBlankHBlank;
        public const int MultiplicationResultMid     = 0x2135; // MPYM         single  read    RegisterTiming.FBlankVBlankHBlank;
        public const int MultiplicationResultHigh    = 0x2136; // MPYH         single  read    RegisterTiming.FBlankVBlankHBlank;
        public const int SoftwareLatch               = 0x2137; // SLHV         single          any time
        public const int OamDataRead                 = 0x2138; // OAMDATAREAD  dual    read    RegisterTiming.FBlankVBlank;
        public const int VramDataReadLow             = 0x2139; // VMDATALREAD  single  read    RegisterTiming.FBlankVBlank;
        public const int VramDataReadHigh            = 0x213A; // VMDATAHREAD  single  read    RegisterTiming.FBlankVBlank;
        public const int CgRamDataRead               = 0x213B; // CGDATAREAD   dual    read    RegisterTiming.FBlankVBlank;
        public const int ScanlineLocationHorizontal  = 0x213C; // OPHCT        dual    read    any time
        public const int ScanlineLocationVertical    = 0x213D; // OPVCT        dual    read    any time
        public const int PpuStatus77                 = 0x213E; // STAT77       single  read    any time
        public const int PpuStatus78                 = 0x213F; // STAT78       single  read    any time
        public const int ApuIO0                      = 0x2140; // APUIO0       single  both    any time
        public const int ApuIO1                      = 0x2141; // APUIO1       single  both    any time
        public const int ApuIO2                      = 0x2142; // APUIO2       single  both    any time
        public const int ApuIO3                      = 0x2143; // APUIO3       single  both    any time
        public const int WramData                    = 0x2180; // WMDATA       single  both    any time
        public const int WramAddressLow              = 0x2181; // WMADDL       single  write   any time
        public const int WramAddressMid              = 0x2182; // WMADDM       single  write   any time
        public const int WramAddressHigh             = 0x2183; // WMADDH       single  write   any time

        public static string GetName(int address)
        {
            switch (address)
            {
            case ScreenDisplay:               return "INIDISP";
            case ObjectSizeAndCharacterSize:  return "OBSEL";
            case OamAddressLow:               return "OAMADDL";
            case OamAddressHigh:              return "OAMADDH";
            case OamDataWrite:                return "OAMDATA";
            case BgModeandCharacterSize:      return "BGMODE";
            case Mosaic:                      return "MOSAIC";
            case BgTilemapAddressBg1:         return "BG1SC";
            case BgTilemapAddressBg2:         return "BG2SC";
            case BgTilemapAddressBg3:         return "BG3SC";
            case BgTilemapAddressBg4:         return "BG3SC";
            case BgCharacterAddressBg1And2:   return "BG12NBA";
            case BgCharacterAddressBg3And4:   return "BG34NBA";
            case BgScrollHBg1:                return "BG1HOFS";
            case BgScrollVBg1:                return "BG1VOFS";
            case BgScrollHBg2:                return "BG2HOFS";
            case BgScrollVBg2:                return "BG2VOFS";
            case BgScrollHBg3:                return "BG3HOFS";
            case BgScrollVBg3:                return "BG3VOFS";
            case BgScrollHBg4:                return "BG4HOFS";
            case BgScrollVBg4:                return "BG4VOFS";
            case VideoPortControl:            return "VMAIN";
            case VramAddressLow:              return "VMADDL";
            case VramAddressHigh:             return "VMADDH";
            case VramDataWriteLow:            return "VMDATAL";
            case VramDataWriteHigh:           return "VMDATAH";
            case Mode7Settings:               return "M7SEL";
            case Mode7MatrixA:                return "M7A";
            case Mode7MatrixB:                return "M7B";
            case Mode7MatrixC:                return "M7C";
            case Mode7MatrixD:                return "M7D";
            case Mode7MatrixX:                return "M7X";
            case Mode7MatrixY:                return "M7Y";
            case CgRamAddress:                return "CGADD";
            case CgRamDataWrite:              return "CGDATA";
            case WindowMaskSettingsBg1And2:   return "W12SEL";
            case WindowMaskSettingsBg3And4:   return "W34SEL";
            case WindowMaskSettingsObj:       return "WOBJSEL";
            case WindowPositionWH0:           return "WH0";
            case WindowPositionWH1:           return "WH1";
            case WindowPositionWH2:           return "WH2";
            case WindowPositionWH3:           return "WH3";
            case WindowMaskLogicregistersBg:  return "WBGLOG";
            case WindowMaskLogicregistersObj: return "WOBJLOG";
            case MainScreenDestination:       return "TM";
            case SubScreenDestination:        return "TS";
            case MainWindowMaskDestination:   return "TMW";
            case SubWindowMaskDestination:    return "TSW";
            case ColorMathSettings:           return "CGWSEL";
            case ColorMathDesignation:        return "CGADSUB";
            case ColorMathData:               return "COLDATA";
            case ScreenModeSelect:            return "SETINI";
            case MultiplicationResultLow:     return "MPYL";
            case MultiplicationResultMid:     return "MPYM";
            case MultiplicationResultHigh:    return "MPYH";
            case SoftwareLatch:               return "SLHV";
            case OamDataRead:                 return "OAMDATAREAD";
            case VramDataReadLow:             return "VMDATALREAD";
            case VramDataReadHigh:            return "VMDATAHREAD";
            case CgRamDataRead:               return "CGDATAREAD";
            case ScanlineLocationHorizontal:  return "OPHCT";
            case ScanlineLocationVertical:    return "OPVCT";
            case PpuStatus77:                 return "STAT77";
            case PpuStatus78:                 return "STAT78";
            case ApuIO0:                      return "APUIO0";
            case ApuIO1:                      return "APUIO1";
            case ApuIO2:                      return "APUIO2";
            case ApuIO3:                      return "APUIO3";
            case WramData:                    return "WMDATA";
            case WramAddressLow:              return "WMADDL";
            case WramAddressMid:              return "WMADDM";
            case WramAddressHigh:             return "WMADDH";
            default:                          return String.Empty;
            }
        }

        public RegisterAccess GetMemoryAccess(int address)
        {
            switch (address)
            {
            case ScreenDisplay               : return RegisterAccess.Write;
            case ObjectSizeAndCharacterSize  : return RegisterAccess.Write;
            case OamAddressLow               : return RegisterAccess.Write;
            case OamAddressHigh              : return RegisterAccess.Write;
            case OamDataWrite                : return RegisterAccess.Write;
            case BgModeandCharacterSize      : return RegisterAccess.Write;
            case Mosaic                      : return RegisterAccess.Write;
            case BgTilemapAddressBg1         : return RegisterAccess.Write;
            case BgTilemapAddressBg2         : return RegisterAccess.Write;
            case BgTilemapAddressBg3         : return RegisterAccess.Write;
            case BgTilemapAddressBg4         : return RegisterAccess.Write;
            case BgCharacterAddressBg1And2   : return RegisterAccess.Write;
            case BgCharacterAddressBg3And4   : return RegisterAccess.Write;
            case BgScrollHBg1                : return RegisterAccess.Write;
            case BgScrollVBg1                : return RegisterAccess.Write;
            case BgScrollHBg2                : return RegisterAccess.Write;
            case BgScrollVBg2                : return RegisterAccess.Write;
            case BgScrollHBg3                : return RegisterAccess.Write;
            case BgScrollVBg3                : return RegisterAccess.Write;
            case BgScrollHBg4                : return RegisterAccess.Write;
            case BgScrollVBg4                : return RegisterAccess.Write;
            case VideoPortControl            : return RegisterAccess.Write;
            case VramAddressLow              : return RegisterAccess.Write;
            case VramAddressHigh             : return RegisterAccess.Write;
            case VramDataWriteLow            : return RegisterAccess.Write;
            case VramDataWriteHigh           : return RegisterAccess.Write;
            case Mode7Settings               : return RegisterAccess.Write;
            case Mode7MatrixA                : return RegisterAccess.Write;
            case Mode7MatrixB                : return RegisterAccess.Write;
            case Mode7MatrixC                : return RegisterAccess.Write;
            case Mode7MatrixD                : return RegisterAccess.Write;
            case Mode7MatrixX                : return RegisterAccess.Write;
            case Mode7MatrixY                : return RegisterAccess.Write;
            case CgRamAddress                : return RegisterAccess.Write;
            case CgRamDataWrite              : return RegisterAccess.Write;
            case WindowMaskSettingsBg1And2   : return RegisterAccess.Write;
            case WindowMaskSettingsBg3And4   : return RegisterAccess.Write;
            case WindowMaskSettingsObj       : return RegisterAccess.Write;
            case WindowPositionWH0           : return RegisterAccess.Write;
            case WindowPositionWH1           : return RegisterAccess.Write;
            case WindowPositionWH2           : return RegisterAccess.Write;
            case WindowPositionWH3           : return RegisterAccess.Write;
            case WindowMaskLogicregistersBg  : return RegisterAccess.Write;
            case WindowMaskLogicregistersObj : return RegisterAccess.Write;
            case MainScreenDestination       : return RegisterAccess.Write;
            case SubScreenDestination        : return RegisterAccess.Write;
            case MainWindowMaskDestination   : return RegisterAccess.Write;
            case SubWindowMaskDestination    : return RegisterAccess.Write;
            case ColorMathSettings           : return RegisterAccess.Write;
            case ColorMathDesignation        : return RegisterAccess.Write;
            case ColorMathData               : return RegisterAccess.Write;
            case ScreenModeSelect            : return RegisterAccess.Write;
            case MultiplicationResultLow     : return RegisterAccess.Read;
            case MultiplicationResultMid     : return RegisterAccess.Read;
            case MultiplicationResultHigh    : return RegisterAccess.Read;
            case SoftwareLatch               : return RegisterAccess.None;
            case OamDataRead                 : return RegisterAccess.Read;
            case VramDataReadLow             : return RegisterAccess.Read;
            case VramDataReadHigh            : return RegisterAccess.Read;
            case CgRamDataRead               : return RegisterAccess.Read;
            case ScanlineLocationHorizontal  : return RegisterAccess.Read;
            case ScanlineLocationVertical    : return RegisterAccess.Read;
            case PpuStatus77                 : return RegisterAccess.Read;
            case PpuStatus78                 : return RegisterAccess.Read;
            case ApuIO0                      : return RegisterAccess.ReadAndWrite;
            case ApuIO1                      : return RegisterAccess.ReadAndWrite;
            case ApuIO2                      : return RegisterAccess.ReadAndWrite;
            case ApuIO3                      : return RegisterAccess.ReadAndWrite;
            case WramData                    : return RegisterAccess.ReadAndWrite;
            case WramAddressLow              : return RegisterAccess.Write;
            case WramAddressMid              : return RegisterAccess.Write;
            case WramAddressHigh             : return RegisterAccess.Write;
            default                          : return RegisterAccess.None;
            }
        }

        public RegisterAccessStyle GetRegisterAccessStyle(int address)
        {
            switch (address)
            {
            case ScreenDisplay               : return RegisterAccessStyle.Single;
            case ObjectSizeAndCharacterSize  : return RegisterAccessStyle.Single;
            case OamAddressLow               : return RegisterAccessStyle.Single;
            case OamAddressHigh              : return RegisterAccessStyle.Single;
            case OamDataWrite                : return RegisterAccessStyle.Single;
            case BgModeandCharacterSize      : return RegisterAccessStyle.Single;
            case Mosaic                      : return RegisterAccessStyle.Single;
            case BgTilemapAddressBg1         : return RegisterAccessStyle.Single;
            case BgTilemapAddressBg2         : return RegisterAccessStyle.Single;
            case BgTilemapAddressBg3         : return RegisterAccessStyle.Single;
            case BgTilemapAddressBg4         : return RegisterAccessStyle.Single;
            case BgCharacterAddressBg1And2   : return RegisterAccessStyle.Single;
            case BgCharacterAddressBg3And4   : return RegisterAccessStyle.Single;
            case BgScrollHBg1                : return RegisterAccessStyle.Dual;
            case BgScrollVBg1                : return RegisterAccessStyle.Dual;
            case BgScrollHBg2                : return RegisterAccessStyle.Dual;
            case BgScrollVBg2                : return RegisterAccessStyle.Dual;
            case BgScrollHBg3                : return RegisterAccessStyle.Dual;
            case BgScrollVBg3                : return RegisterAccessStyle.Dual;
            case BgScrollHBg4                : return RegisterAccessStyle.Dual;
            case BgScrollVBg4                : return RegisterAccessStyle.Dual;
            case VideoPortControl            : return RegisterAccessStyle.Single;
            case VramAddressLow              : return RegisterAccessStyle.Single;
            case VramAddressHigh             : return RegisterAccessStyle.Single;
            case VramDataWriteLow            : return RegisterAccessStyle.Single;
            case VramDataWriteHigh           : return RegisterAccessStyle.Single;
            case Mode7Settings               : return RegisterAccessStyle.Single;
            case Mode7MatrixA                : return RegisterAccessStyle.Dual;
            case Mode7MatrixB                : return RegisterAccessStyle.Dual;
            case Mode7MatrixC                : return RegisterAccessStyle.Dual;
            case Mode7MatrixD                : return RegisterAccessStyle.Dual;
            case Mode7MatrixX                : return RegisterAccessStyle.Dual;
            case Mode7MatrixY                : return RegisterAccessStyle.Dual;
            case CgRamAddress                : return RegisterAccessStyle.Single;
            case CgRamDataWrite              : return RegisterAccessStyle.Dual;
            case WindowMaskSettingsBg1And2   : return RegisterAccessStyle.Single;
            case WindowMaskSettingsBg3And4   : return RegisterAccessStyle.Single;
            case WindowMaskSettingsObj       : return RegisterAccessStyle.Single;
            case WindowPositionWH0           : return RegisterAccessStyle.Single;
            case WindowPositionWH1           : return RegisterAccessStyle.Single;
            case WindowPositionWH2           : return RegisterAccessStyle.Single;
            case WindowPositionWH3           : return RegisterAccessStyle.Single;
            case WindowMaskLogicregistersBg  : return RegisterAccessStyle.Single;
            case WindowMaskLogicregistersObj : return RegisterAccessStyle.Single;
            case MainScreenDestination       : return RegisterAccessStyle.Single;
            case SubScreenDestination        : return RegisterAccessStyle.Single;
            case MainWindowMaskDestination   : return RegisterAccessStyle.Single;
            case SubWindowMaskDestination    : return RegisterAccessStyle.Single;
            case ColorMathSettings           : return RegisterAccessStyle.Single;
            case ColorMathDesignation        : return RegisterAccessStyle.Single;
            case ColorMathData               : return RegisterAccessStyle.Single;
            case ScreenModeSelect            : return RegisterAccessStyle.Single;
            case MultiplicationResultLow     : return RegisterAccessStyle.Single;
            case MultiplicationResultMid     : return RegisterAccessStyle.Single;
            case MultiplicationResultHigh    : return RegisterAccessStyle.Single;
            case SoftwareLatch               : return RegisterAccessStyle.Single;
            case OamDataRead                 : return RegisterAccessStyle.Dual;
            case VramDataReadLow             : return RegisterAccessStyle.Single;
            case VramDataReadHigh            : return RegisterAccessStyle.Single;
            case CgRamDataRead               : return RegisterAccessStyle.Dual;
            case ScanlineLocationHorizontal  : return RegisterAccessStyle.Dual;
            case ScanlineLocationVertical    : return RegisterAccessStyle.Dual;
            case PpuStatus77                 : return RegisterAccessStyle.Single;
            case PpuStatus78                 : return RegisterAccessStyle.Single;
            case ApuIO0                      : return RegisterAccessStyle.Single;
            case ApuIO1                      : return RegisterAccessStyle.Single;
            case ApuIO2                      : return RegisterAccessStyle.Single;
            case ApuIO3                      : return RegisterAccessStyle.Single;
            case WramData                    : return RegisterAccessStyle.Single;
            case WramAddressLow              : return RegisterAccessStyle.Single;
            case WramAddressMid              : return RegisterAccessStyle.Single;
            case WramAddressHigh             : return RegisterAccessStyle.Single;
            default                          : return RegisterAccessStyle.Single;
            }
        }

        public static RegisterTiming GetRegisterTiming(int address)
        {
            switch (address)
            {
            case ScreenDisplay               : return  RegisterTiming.AnyTime;
            case ObjectSizeAndCharacterSize  : return  RegisterTiming.FBlankVBlank;
            case OamAddressLow               : return  RegisterTiming.FBlankVBlank;
            case OamAddressHigh              : return  RegisterTiming.FBlankVBlank;
            case OamDataWrite                : return  RegisterTiming.FBlankVBlank;
            case BgModeandCharacterSize      : return  RegisterTiming.FBlankVBlankHBlank;
            case Mosaic                      : return  RegisterTiming.FBlankVBlankHBlank;
            case BgTilemapAddressBg1         : return  RegisterTiming.FBlankVBlank;
            case BgTilemapAddressBg2         : return  RegisterTiming.FBlankVBlank;
            case BgTilemapAddressBg3         : return  RegisterTiming.FBlankVBlank;
            case BgTilemapAddressBg4         : return  RegisterTiming.FBlankVBlank;
            case BgCharacterAddressBg1And2   : return  RegisterTiming.FBlankVBlank;
            case BgCharacterAddressBg3And4   : return  RegisterTiming.FBlankVBlank;
            case BgScrollHBg1                : return  RegisterTiming.FBlankVBlankHBlank;
            case BgScrollVBg1                : return  RegisterTiming.FBlankVBlankHBlank;
            case BgScrollHBg2                : return  RegisterTiming.FBlankVBlankHBlank;
            case BgScrollVBg2                : return  RegisterTiming.FBlankVBlankHBlank;
            case BgScrollHBg3                : return  RegisterTiming.FBlankVBlankHBlank;
            case BgScrollVBg3                : return  RegisterTiming.FBlankVBlankHBlank;
            case BgScrollHBg4                : return  RegisterTiming.FBlankVBlankHBlank;
            case BgScrollVBg4                : return  RegisterTiming.FBlankVBlankHBlank;
            case VideoPortControl            : return  RegisterTiming.FBlankVBlank;
            case VramAddressLow              : return  RegisterTiming.FBlankVBlank;
            case VramAddressHigh             : return  RegisterTiming.FBlankVBlank;
            case VramDataWriteLow            : return  RegisterTiming.FBlankVBlank;
            case VramDataWriteHigh           : return  RegisterTiming.FBlankVBlank;
            case Mode7Settings               : return  RegisterTiming.FBlankVBlank;
            case Mode7MatrixA                : return  RegisterTiming.FBlankVBlankHBlank;
            case Mode7MatrixB                : return  RegisterTiming.FBlankVBlankHBlank;
            case Mode7MatrixC                : return  RegisterTiming.FBlankVBlankHBlank;
            case Mode7MatrixD                : return  RegisterTiming.FBlankVBlankHBlank;
            case Mode7MatrixX                : return  RegisterTiming.FBlankVBlankHBlank;
            case Mode7MatrixY                : return  RegisterTiming.FBlankVBlankHBlank;
            case CgRamAddress                : return  RegisterTiming.FBlankVBlankHBlank;
            case CgRamDataWrite              : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowMaskSettingsBg1And2   : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowMaskSettingsBg3And4   : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowMaskSettingsObj       : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowPositionWH0           : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowPositionWH1           : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowPositionWH2           : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowPositionWH3           : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowMaskLogicregistersBg  : return  RegisterTiming.FBlankVBlankHBlank;
            case WindowMaskLogicregistersObj : return  RegisterTiming.FBlankVBlankHBlank;
            case MainScreenDestination       : return  RegisterTiming.FBlankVBlankHBlank;
            case SubScreenDestination        : return  RegisterTiming.FBlankVBlankHBlank;
            case MainWindowMaskDestination   : return  RegisterTiming.FBlankVBlankHBlank;
            case SubWindowMaskDestination    : return  RegisterTiming.FBlankVBlankHBlank;
            case ColorMathSettings           : return  RegisterTiming.FBlankVBlankHBlank;
            case ColorMathDesignation        : return  RegisterTiming.FBlankVBlankHBlank;
            case ColorMathData               : return  RegisterTiming.FBlankVBlankHBlank;
            case ScreenModeSelect            : return  RegisterTiming.FBlankVBlankHBlank;
            case MultiplicationResultLow     : return  RegisterTiming.FBlankVBlankHBlank;
            case MultiplicationResultMid     : return  RegisterTiming.FBlankVBlankHBlank;
            case MultiplicationResultHigh    : return  RegisterTiming.FBlankVBlankHBlank;
            case SoftwareLatch               : return  RegisterTiming.AnyTime;
            case OamDataRead                 : return  RegisterTiming.FBlankVBlank;
            case VramDataReadLow             : return  RegisterTiming.FBlankVBlank;
            case VramDataReadHigh            : return  RegisterTiming.FBlankVBlank;
            case CgRamDataRead               : return  RegisterTiming.FBlankVBlank;
            case ScanlineLocationHorizontal  : return  RegisterTiming.AnyTime;
            case ScanlineLocationVertical    : return  RegisterTiming.AnyTime;
            case PpuStatus77                 : return  RegisterTiming.AnyTime;
            case PpuStatus78                 : return  RegisterTiming.AnyTime;
            case ApuIO0                      : return  RegisterTiming.AnyTime;
            case ApuIO1                      : return  RegisterTiming.AnyTime;
            case ApuIO2                      : return  RegisterTiming.AnyTime;
            case ApuIO3                      : return  RegisterTiming.AnyTime;
            case WramData                    : return  RegisterTiming.AnyTime;
            case WramAddressLow              : return  RegisterTiming.AnyTime;
            case WramAddressMid              : return  RegisterTiming.AnyTime;
            case WramAddressHigh             : return  RegisterTiming.AnyTime;
            default                          : return RegisterTiming.None;
            }
        }

        [Flags]
        public enum RegisterAccess
        {
            None = 0,
            Read = 1 << 0,
            Write = 1 << 1,
            ReadAndWrite = Read | Write
        }

        public enum RegisterAccessStyle
        {
            Single,
            Dual
        }

        [Flags]
        public enum RegisterTiming
        {
            None = 0,
            FBlank = 1 << 0,
            VBlank = 1 << 1,
            HBlank = 1 << 2,
            FBlankVBlank = FBlank | VBlank,
            FBlankVBlankHBlank = FBlank | VBlank | HBlank,
            AnyTime = -1
        }
    }
}