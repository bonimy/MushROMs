using System;
using Helper;

namespace SnesXM.Emulator
{
    partial class Bsx
    {
        private class PpuIndexer : IIndexer<int>
        {
            private Bsx Bsx
            {
                get;
                set;
            }

            public int this[int index]
            {
                get => Bsx.GetPpu((short)index);
                set => Bsx.SetPpu((byte)value, (short)index);
            }

            public PpuIndexer(Bsx bsx)
            {
                Bsx = bsx ?? throw new ArgumentNullException(nameof(bsx));
            }
        }

        private byte GetPpu(short address)
        {
            // known read registers
            switch (address)
            {
            //Stream 1
            // Logical Channel 1 + Data Structure (R/W)
            case 0x2188:
                return RealPpu[0x2188];

            // Logical Channel 2 (R/W) [6bit]
            case 0x2189:
                return RealPpu[0x2189];

            // Prefix Count (R)
            case 0x218A:
                if (!SatPfLatch1Enable || !SatDtLatch1Enable)
                {
                    return 0;
                }

                if (RealPpu[0x2188] == 0 && RealPpu[0x2189] == 0)
                {
                    return 1;
                }

                if (RealPpu[0x218A] <= 0)
                {
                    SatStream1Count++;
                    SetStream1(SatStream1Count - 1);
                }

                if (!SatStream1Loaded && (SatStream1Count - 1) > 0)
                {
                    SatStream1Count = 1;
                    SetStream1(SatStream1Count - 1);
                }

                if (SatStream1Loaded)
                    return RealPpu[0x218A];
                else
                    return 0;

            // Prefix Latch (R/W)
            case 0x218B:
                if (SatPfLatch1Enable)
                {
                    if (RealPpu[0x2188] == 0 && RealPpu[0x2189] == 0)
                    {
                        RealPpu[0x218B] = 0x90;
                    }

                    if (SatStream1Loaded)
                    {
                        var temp = (byte)0;
                        if (SatStream1First)
                        {
                            // First packet
                            temp |= 0x10;
                            SatStream1First = false;
                        }

                        RealPpu[0x218A]--;

                        if (RealPpu[0x218A] == 0)
                        {
                            //Last packet
                            temp |= 0x80;
                        }

                        RealPpu[0x218B] = temp;
                    }

                    RealPpu[0x218D] |= RealPpu[0x218B];
                    return RealPpu[0x218B];
                }
                else
                {
                    return 0;
                }

            // Data Latch (R/W)
            case 0x218C:
                if (SatDtLatch1Enable)
                {
                    if (RealPpu[0x2188] == 0 && RealPpu[0x2189] == 0)
                    {
                        RealPpu[0x218C] = GetRtc();
                    }
                    else if (SatStream1Loaded)
                    {
                        RealPpu[0x218C] = (byte)SatStream1.ReadByte();
                    }
                    return RealPpu[0x218C];
                }
                else
                {
                    return 0;
                }

            // OR gate (R)
            case 0x218D:
                {
                    var result = RealPpu[0x218D];
                    RealPpu[0x218D] = 0;

                    return result;
                }
            //Stream 2
            // Logical Channel 1 + Data Structure (R/W)
            case 0x218E:
                return RealPpu[0x218E];

            // Logical Channel 2 (R/W) [6bit]
            case 0x218F:
                return RealPpu[0x218F];

            // Prefix Count (R)
            case 0x2190:
                if (!SatPfLatch2Enable || !SatDtLatch2Enable)
                {
                    return 0;
                }

                if (RealPpu[0x218E] == 0 && RealPpu[0x218F] == 0)
                {
                    return 1;
                }

                if (RealPpu[0x2190] <= 0)
                {
                    SatStream2Count++;
                    SetStream2(SatStream2Count - 1);
                }

                if (!SatStream2Loaded && (SatStream2Count - 1) > 0)
                {
                    SatStream2Count = 1;
                    SetStream2(SatStream2Count - 1);
                }

                if (SatStream2Loaded)
                    return RealPpu[0x2190];
                else
                    return 0;

            // Prefix Latch (R/W)
            case 0x2191:
                if (SatPfLatch2Enable)
                {
                    if (RealPpu[0x218E] == 0 && RealPpu[0x218F] == 0)
                    {
                        RealPpu[0x2191] = 0x90;
                    }

                    if (SatStream2Loaded)
                    {
                        var temp = (byte)0;
                        if (SatStream2First)
                        {
                            // First packet
                            temp |= 0x10;
                            SatStream2First = false;
                        }

                        RealPpu[0x2190]--;

                        if (RealPpu[0x2190] == 0)
                        {
                            //Last packet
                            temp |= 0x80;
                        }

                        RealPpu[0x2191] = temp;
                    }

                    RealPpu[0x2193] |= RealPpu[0x2191];
                    return RealPpu[0x2191];
                }
                else
                {
                    return 0;
                }

            // Data Latch (R/W)
            case 0x2192:
                if (SatDtLatch2Enable)
                {
                    if (RealPpu[0x218E] == 0 && RealPpu[0x218F] == 0)
                    {
                        RealPpu[0x2192] = GetRtc();
                    }
                    else if (SatStream2Loaded)
                    {
                        RealPpu[0x2192] = (byte)SatStream2.ReadByte();
                    }
                    return RealPpu[0x2192];
                }
                else
                {
                    return 0;
                }

            // OR gate (R)
            case 0x2193:
                {
                    var result = RealPpu[0x2193];
                    RealPpu[0x2193] = 0;
                    return result;
                }
            //Other
            // Satellaview LED / Stream Enable (R/W) [4bit]
            case 0x2194:
                return RealPpu[0x2194];

            // Unknown
            case 0x2195:
                return RealPpu[0x2195];

            // Satellaview Status (R)
            case 0x2196:
                return RealPpu[0x2196];

            // Soundlink Settings (R/W)
            case 0x2197:
                return RealPpu[0x2197];

            // Serial I/O - Serial Number (R/W)
            case 0x2198:
                return RealPpu[0x2198];

            // Serial I/O - Unknown (R/W)
            case 0x2199:
                return RealPpu[0x2199];

            default:
                return (byte)Emulator.OpenBus;
            }
        }

        private void SetPpu(byte value, short address)
        {
            // known write registers
            switch (address)
            {
            //Stream 1
            // Logical Channel 1 + Data Structure (R/W)
            case 0x2188:
                if (RealPpu[0x2188 ] == value)
                {
                    SatStream1Count = 0;
                }
                RealPpu[0x2188 ] = value;
                break;

            // Logical Channel 2 (R/W) [6bit]
            case 0x2189:
                if (RealPpu[0x2188 ] == (value & 0x3F))
                {
                    SatStream1Count = 0;
                }
                RealPpu[0x2189 ] = (byte)(value & 0x3F);
                break;

            // Prefix Latch (R/W)
            case 0x218B:
                SatPfLatch1Enable = (value != 0);
                break;

            // Data Latch (R/W)
            case 0x218C:
                if (RealPpu[0x2188 ] == 0 && RealPpu[0x2189 ] == 0)
                {
                    OutIndex = 0;
                }
                SatDtLatch1Enable = (value != 0);
                break;

            //Stream 2
            // Logical Channel 1 + Data Structure (R/W)
            case 0x218E:
                if (RealPpu[0x218E ] == value)
                {
                    SatStream2Count = 0;
                }
                RealPpu[0x218E ] = value;
                break;

            // Logical Channel 2 (R/W) [6bit]
            case 0x218F:
                if (RealPpu[0x218F ] == (value & 0x3F))
                {
                    SatStream2Count = 0;
                }
                RealPpu[0x218F ] = (byte)(value & 0x3F);
                break;

            // Prefix Latch (R/W)
            case 0x2191:
                SatPfLatch2Enable = (value != 0);
                break;

            // Data Latch (R/W)
            case 0x2192:
                if (RealPpu[0x218E ] == 0 && RealPpu[0x218F ] == 0)
                {
                    OutIndex = 0;
                }
                SatDtLatch2Enable = (value != 0);
                break;

            //Other
            // Satellaview LED / Stream Enable (R/W) [4bit]
            case 0x2194:
                RealPpu[0x2194 ] = (byte)(value & 0x0F);
                break;

            // Soundlink Settings (R/W)
            case 0x2197:
                RealPpu[0x2197 ] = value;
                break;
            }
        }
    }
}
