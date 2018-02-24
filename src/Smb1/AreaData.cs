﻿// <copyright file="AreaData.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Collections.Generic;
using Helper;
using MushROMs.NES;
using static MushROMs.NES.AddressConverter;

namespace MushROMs.SMB1
{
    public class AreaData
    {
        public const int NumberOfWorlds = 8;
        public const int LevelsPerWorldPointer = 0x9CB4;
        public const int AreaListPointer = 0x9CBC;
        public const int ObjectAddressLowBytePointer = 0x9D2C;
        public const int ObjectAddressHighBytePointer = 0x9D4E;
        public const int SpriteAddressLowBytePointer = 0x9CE4;
        public const int SpriteAddressHighBytePointer = 0x9D06;
        public const int AreaTypeSpriteOffsetPointer = 0x9CE0;
        public const int AreaTypeObjectOffsetPointer = 0x9D28;
        public const int NumberOfAreas = ObjectAddressHighBytePointer - ObjectAddressLowBytePointer;

        public AreaObjectData AreaObjectData
        {
            get;
            private set;
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
        public int DataSize
        {
            get
            {
                return AreaObjectData.DataSize + AreaSpriteData.DataSize;
            }
        }

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
            {
                throw new ArgumentNullException(nameof(rom));
            }

            if (world < 0)
            {
                throw new ArgumentNullException(nameof(world));
            }

            if (level < 0)
            {
                throw new ArgumentNullException(nameof(level));
            }

            var areas = AddressConverter.NesToPc(AreaListPointer);
            var worldOffsetPointer = AddressConverter.NesToPc(LevelsPerWorldPointer);
            int worldOffset = rom[worldOffsetPointer + world];
            worldOffset += AddressConverter.NesToPc(AreaListPointer);

            return (byte)(rom[worldOffset + level] & 0x7F);
        }

        public static AreaData FromRomData(byte[] rom, int areaNumber)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            if (areaNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(areaNumber));
            }

            var lo = NesToPc(ObjectAddressLowBytePointer);
            var hi = NesToPc(ObjectAddressHighBytePointer);
            var areas = NesToPc(AreaTypeObjectOffsetPointer);

            var areaType = (AreaType)((areaNumber >> 5) & 3);
            var reducedMapNumer = areaNumber & 0x1F;
            var index = reducedMapNumer + rom[areas + (int)areaType];
            var address = rom[lo + index] | (rom[hi + index] << 8);
            address = NesToPc(address);

            var areaObjectData = new AreaObjectData(rom, address);

            lo = NesToPc(SpriteAddressLowBytePointer);
            hi = NesToPc(SpriteAddressHighBytePointer);
            areas = NesToPc(AreaTypeSpriteOffsetPointer);

            index = reducedMapNumer + rom[areas + (int)areaType];

            address = rom[lo + index] | (rom[hi + index] << 8);
            address = NesToPc(address);

            var areaSpriteData = new AreaSpriteData(rom, address);

            return new AreaData(areaObjectData, areaSpriteData, areaNumber, areaType);
        }

        public static AreaData[] GetAllAreas(byte[] src)
        {
            var areas = new AreaData[NumberOfAreas];

            var areaPointers = NesToPc(AreaTypeObjectOffsetPointer);
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

        public override string ToString()
        {
            return SR.GetString("Area{0:X2}", AreaNumber);
        }

        private struct AreaIndex
        {
            public int Index
            {
                get;
                set;
            }

            public AreaType AreaType
            {
                get;
                set;
            }

            public AreaIndex(int index, AreaType areaType)
            {
                Index = index;
                AreaType = areaType;
            }

            public override string ToString()
            {
                return SR.GetString("0x{0:X2}: {1}", Index, AreaType);
            }
        }
    }
}
