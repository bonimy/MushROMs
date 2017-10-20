﻿using System;
using System.Collections.Generic;
using Helper;
using MushROMs.SMB1;
using NesAreaData = MushROMs.SMB1.AreaData;
using static MushROMs.NES.AddressConverter;
using static MushROMs.SNES.AddressConverter;

namespace MushROMs.SMAS.SMB1
{
    public class AreaData
    {
        public const int NumberOfWorlds = 8;
        public const int LevelsPerWorldPointer = 0x04C11C;
        public const int AreaListPointer = 0x04C124;
        public const int ObjectAddressLowBytePointer = 0x04C194;
        public const int ObjectAddressHighBytePointer = 0x04C1B6;
        public const int SpriteAddressLowBytePointer = 0x04C14C;
        public const int SpriteAddressHighBytePointer = 0x04C16E;
        public const int AreaTypeSpriteOffsetPointer = 0x04C148;
        public const int AreaTypeObjectOffsetPointer = 0x04C190;
        public const int NumberOfAreas = ObjectAddressHighBytePointer - ObjectAddressLowBytePointer;

        public AreaObjectData AreaObjectData
        {
            get;
            private set;
        }

        public AreaHeader AreaHeader
        {
            get => AreaObjectData.AreaHeader;
            set => AreaObjectData.AreaHeader = value;
        }

        public AreaSpriteData AreaSpriteData
        {
            get;
            private set;
        }

        public int AreaNumber
        {
            get;
            set;
        }
        public AreaType AreaType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the size, in bytes, of the <see cref="AreaObjectData"/> and
        /// <see cref="AreaSpriteData"/> of this <see cref="AreaData"/>.
        /// </summary>
        public int DataSize => AreaObjectData.DataSize + AreaSpriteData.DataSize;

        public AreaData(
            AreaObjectData areaObjectData,
            AreaSpriteData areaSpriteData,
            int areaNumber,
            AreaType areaType)
        {
            AreaObjectData = areaObjectData ?? throw new ArgumentNullException(nameof(areaObjectData));
            AreaSpriteData = areaSpriteData ?? throw new ArgumentNullException(nameof(areaSpriteData));

            AreaNumber = areaNumber;
            AreaType = AreaType;
        }

        public static byte GetAreaNumber(byte[] rom, int world, int level)
        {
            if (rom == null)
                throw new ArgumentNullException(nameof(rom));
            if (world < 0)
                throw new ArgumentNullException(nameof(world));
            if (level < 0)
                throw new ArgumentNullException(nameof(level));

            var areas = MemoryToLoRom(AreaListPointer);
            var worldOffsetPointer = MemoryToLoRom(LevelsPerWorldPointer);
            int worldOffset = rom[worldOffsetPointer + world];
            worldOffset += MemoryToLoRom(AreaListPointer);

            return (byte)(rom[worldOffset + level] & 0x7F);
        }


        public static AreaData FromRomData(byte[] rom, int areaNumber)
        {
            if (rom == null)
                throw new ArgumentNullException(nameof(rom));
            if (areaNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(areaNumber));

            var lo = MemoryToLoRom(ObjectAddressLowBytePointer);
            var hi = MemoryToLoRom(ObjectAddressHighBytePointer);
            var areas = MemoryToLoRom(AreaTypeObjectOffsetPointer);

            var areaType = (AreaType)((areaNumber >> 5) & 3);
            var reducedMapNumer = areaNumber & 0x1F;
            var index = reducedMapNumer + rom[areas + (int)areaType];
            var address = rom[lo + index] | (rom[hi + index] << 8);
            address |= 0x40000;
            address = MemoryToLoRom(address);

            var areaObjectData = new AreaObjectData(rom, address);

            lo = MemoryToLoRom(SpriteAddressLowBytePointer);
            hi = MemoryToLoRom(SpriteAddressHighBytePointer);
            areas = MemoryToLoRom(AreaTypeSpriteOffsetPointer);

            index = reducedMapNumer + rom[areas + (int)areaType];

            address = rom[lo + index] | (rom[hi + index] << 8);
            address |= 0x40000;
            address = MemoryToLoRom(address);

            var areaSpriteData = new AreaSpriteData(rom, address);

            return new AreaData(areaObjectData, areaSpriteData, areaNumber, areaType);
        }

        public AreaData(NesAreaData nes)
        {
            if (nes == null)
                throw new ArgumentNullException(nameof(nes));

            AreaNumber = nes.AreaNumber;
            AreaType = nes.AreaType;
            AreaObjectData = new AreaObjectData(nes.AreaObjectData, nes.AreaType);
            AreaSpriteData = new AreaSpriteData(nes.AreaSpriteData);
        }

        private int GetCeilingSize(TerrainMode terrain)
        {
            switch (terrain)
            {
            case TerrainMode.None:
            case TerrainMode.Ceiling0Floor2:
                return 0;
            case TerrainMode.Ceiling1Floor2:
            case TerrainMode.Ceiling1Floor5:
            case TerrainMode.Ceiling1Floor6:
            case TerrainMode.Ceiling1Floor0:
            case TerrainMode.Ceiling1Floor9:
            case TerrainMode.Ceiling1Middle5Floor2:
            case TerrainMode.Ceiling1Middle4Floor2:
                return 1;
            case TerrainMode.Ceiling3Floor2:
            case TerrainMode.Ceiling3Floor5:
                return 3;
            case TerrainMode.Ceiling4Floor2:
            case TerrainMode.Ceiling4Floor5:
            case TerrainMode.Ceiling4Floor6:
                return 4;
            case TerrainMode.Ceiling8Floor2:
                return 8;
            case TerrainMode.Solid:
                return 0x10;
            default:
                return -1;
            }
        }

        private struct AreaIndex
        {
            public int Index;
            public AreaType AreaType;

            public AreaIndex(int index, AreaType areaType)
            {
                Index = index;
                AreaType = areaType;
            }
        }

        public static AreaData[] GetAllAreas(byte[] src)
        {
            var areas = new AreaData[NumberOfAreas];

            var areaPointers = MemoryToLoRom(AreaTypeObjectOffsetPointer);
            var ws = src[areaPointers + (int)AreaType.Water];
            var gs = src[areaPointers + (int)AreaType.Grassland];
            var us = src[areaPointers + (int)AreaType.Underground];
            var cs = src[areaPointers + (int)AreaType.Castle];
            var list = new List<AreaIndex>(new AreaIndex[] {
                new AreaIndex(ws, AreaType.Water),
                new AreaIndex(gs, AreaType.Grassland),
                new AreaIndex(us, AreaType.Underground),
                new AreaIndex(cs, AreaType.Castle),
                new AreaIndex(Int32.MaxValue, (AreaType)(-1))});
            list.Sort((x, y) => x.Index - y.Index);

            for (var i = 0; i < NumberOfAreas; i++)
            {
                var map = i;
                for (var j = 0; j < 4; j++)
                {
                    if (i >= list[j].Index && i < list[j + 1].Index)
                    {
                        map -= list[j].Index;
                        map |= (int)list[j].AreaType << 5;
                        break;
                    }
                }

                areas[i] = FromRomData(src, map);
            }

            return areas;
        }

        public override string ToString() => SR.GetString("Area{0:X2}", AreaNumber);

        public static bool WriteAllLevels(byte[] ptr, byte[] src)
        {
            if (ptr == null)
                throw new ArgumentNullException(nameof(ptr));

            var first = MemoryToLoRom(0x04C1D8);
            var last = MemoryToLoRom(0x04D800);
            var range = last - first;

            var nlevels = NesAreaData.GetAllAreas(src);
            var slevels = new AreaData[NumberOfAreas];

            var size = 0;
            for (var i = 0; i < NumberOfAreas; i++)
            {
                slevels[i] = new AreaData(nlevels[i]);
                size += slevels[i].DataSize;
            }

            if (size >= range)
                return false;

            var msrc = NesToPc(NesAreaData.AreaListPointer);
            var mdest = MemoryToLoRom(AreaListPointer);
            for (var i = 0; i < NumberOfAreas; i++)
                ptr[mdest + i] = src[msrc + i];

            var wsrc = NesToPc(NesAreaData.LevelsPerWorldPointer);
            var wdest = MemoryToLoRom(LevelsPerWorldPointer);
            for (var i = 0; i < 8; i++)
                ptr[wdest + i] = src[wsrc + i];

            var atesrc = NesToPc(NesAreaData.AreaTypeSpriteOffsetPointer);
            var atedest = MemoryToLoRom(AreaTypeSpriteOffsetPointer);
            for (var i = 0; i < 4; i++)
                ptr[atedest + i] = src[atesrc + i];

            var atlsrc = NesToPc(MushROMs.SMB1.AreaData.AreaTypeObjectOffsetPointer);
            var atldest = MemoryToLoRom(AreaTypeObjectOffsetPointer);
            for (var i = 0; i < 4; i++)
                ptr[atldest + i] = src[atlsrc + i];

            var lldest = MemoryToLoRom(ObjectAddressLowBytePointer);
            var hldest = MemoryToLoRom(ObjectAddressHighBytePointer);

            var ledest = MemoryToLoRom(SpriteAddressLowBytePointer);
            var hedest = MemoryToLoRom(SpriteAddressHighBytePointer);

            var address = first;
            for (var i = 0; i < NumberOfAreas; i++)
            {
                var level = slevels[i];
                var map = level.AreaNumber;
                var type = (int)level.AreaType;
                var reduced = map & 0x1F;
                var index = reduced + ptr[atldest + type];

                var snes = MemoryToLoRom(address);
                ptr[lldest + index] = (byte)snes;
                ptr[hldest + index] = (byte)(snes >> 8);

                ptr[address++] = level.AreaHeader.Value1;
                ptr[address++] = level.AreaHeader.Value2;
                for (var j = 0; j < level.AreaObjectData.AreaObjectCount; j++)
                {
                    var obj = level.AreaObjectData[j];
                    ptr[address++] = obj.Value1;
                    ptr[address++] = obj.Value2;
                    if (obj.Size == 3)
                        ptr[address++] = obj.Value3;
                }
                ptr[address++] = 0xFD;

                index = reduced + ptr[atedest + type];
                snes = MemoryToLoRom(address);
                ptr[ledest + index] = (byte)snes;
                ptr[hedest + index] = (byte)(snes >> 8);

                for (var j = 0; j < level.AreaSpriteData.AreaSpriteCount; j++)
                {
                    var obj = level.AreaSpriteData[j];
                    ptr[address++] = obj.Value1;
                    ptr[address++] = obj.Value2;
                    if (obj.Size == 3)
                        ptr[address++] = obj.Value3;
                }
                ptr[address++] = 0xFF;
            }

            return true;
        }
    }
}
