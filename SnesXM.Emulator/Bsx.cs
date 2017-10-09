using System;
using System.IO;

namespace SnesXM.Emulator
{
    public class Bsx
    {
        private const int BiosSize = 0x100000;
        private const int FlashSize = 0x100000;
        private const int FlashSizeMask = FlashSize - 1;
        private const int PsRamSize = 0x80000;

        private static readonly byte[] Flashcard = new byte[20]
        {
            0x4D, 0x00, 0x50, 0x00,	// vendor id
	        0x00, 0x00,				// ?
	        0x1A, 0x00,				// 2MB Flash (1MB = 0x2A)
	        0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static bool FlashMode
        {
            get;
            set;
        }

        private static byte[] MapRom
        {
            get;
            set;
        }

        private static byte[] FlashRom
        {
            get;
            set;
        }

        private bool FlashBsr
        {
            get;
            set;
        }

        private bool FlashGsr
        {
            get;
            set;
        }

        private bool FlashCsr
        {
            get;
            set;
        }

        private bool FlashEnable
        {
            get;
            set;
        }

        private bool ReadEnable
        {
            get;
            set;
        }

        private bool WriteEnable
        {
            get;
            set;
        }

        private bool FlashCommandDone
        {
            get;
            set;
        }

        private byte[] Mmc
        {
            get;
            set;
        }

        private uint FlashCommand
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

        public byte this[int address]
        {
            get
            {
                byte bank = (byte)(address >> 0x10);
                ushort offset = (ushort)address;
                byte value = 0;

                // MMC
                if (bank >= 0x01 && bank <= 0x0E)
                    return Mmc[bank];

                // Flash IO
                if (bank >= 0xC0)
                {
                    value = GetBypassFlashIO(address);

                    switch (offset)
                    {
                    case 0x0002:
                    case 0x8002:
                        if (FlashBsr)
                            value = 0xC0;
                        break;

                    case 0x0004:
                    case 0x8004:
                        if (FlashGsr)
                            value = 0x82;
                        break;

                    case 0x5555:
                        if (FlashEnable)
                            value = 0x80;
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
                        if (ReadEnable)
                            value = Flashcard[offset - 0xFF00];
                        break;
                    }

                    if (FlashCsr)
                    {
                        value = 0x80;
                        FlashCsr = false;
                    }
                }

                return value;
            }
            set
            {
                byte bank = (byte)(address >> 0x10);
                ushort offset = (ushort)address;

                // MMC
                if (bank >= 0x01 && bank <= 0x0E)
                {
                    Mmc[bank] = value;
                    if (bank == 0x0E)
                        Map();
                }

                // Flash IO
                if (bank >= 0xC0)
                {
                    // Write to Flash
                    if (WriteEnable)
                    {
                        SetBypassFlashIO(address, value);
                        WriteEnable = false;
                        return;
                    }

                    // Flash Command Handling
                    if (Mmc[0x0C] != 0)
                    {
                        FlashCommand <<= 8;
                        FlashCommand |= value;

                        switch (value)
                        {
                        case 0x00:
                        case 0xFF:
                            //Reset to normal
                            FlashEnable = false;
                            FlashBsr = false;
                            FlashCsr = false;
                            FlashGsr = false;
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
                            switch (FlashCommand & 0xFFFF)
                            {
                            case 0x20D0: //Block Erase
                                for (uint x = 0; x < 0x10000; x++)
                                {
                                    //BSX_Set_Bypass_FlashIO(((address & 0xFF0000) + x), 0xFF);
                                    if (Mmc[0x02] != 0)
                                        MapRom[(address & 0x0F0000) + x] = 0xFF;
                                    else
                                        MapRom[((address & 0x1E0000) >> 1) + x] = 0xFF;
                                }
                                break;

                            case 0xA7D0: //Chip Erase (ONLY IN TYPE 1 AND 4)
                                if ((Flashcard[6] & 0xF0) == 0x10 || (Flashcard[6] & 0xF0) == 0x40)
                                {
                                    for (uint x = 0; x < FlashSize; x++)
                                    {
                                        //BSX_Set_Bypass_FlashIO(x, 0xFF);
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

                    switch (offset)
                    {
                    case 0x0002:
                    case 0x8002:
                        if (FlashBsr)
                            value = 0xC0;
                        break;

                    case 0x0004:
                    case 0x8004:
                        if (FlashGsr)
                            value = 0x82;
                        break;

                    case 0x5555:
                        if (FlashEnable)
                            value = 0x80;
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
                        if (ReadEnable)
                            value = Flashcard[offset - 0xFF00];
                        break;
                    }

                    if (FlashCsr)
                    {
                        value = 0x80;
                        FlashCsr = false;
                    }
                }
            }
        }

        public Bsx()
        {
            Mmc = new byte[0x10];
        }

        private void SetStream1(byte count)
        {
            throw new NotImplementedException();
        }

        private void SetStream2(byte count)
        {
            throw new NotImplementedException();
        }

        private byte GetBypassFlashIO(int offset)
        {
            throw new NotImplementedException();
        }

        private void SetBypassFlashIO(int offseet, byte value)
        {
            throw new NotImplementedException();
        }

        private void Map()
        {
            throw new NotImplementedException();
        }
    }
}