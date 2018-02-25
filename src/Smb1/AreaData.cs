// <copyright file="AreaData.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Smb1
{
    using System;
    using System.Collections.Generic;
    using Helper;
    using Nes;
    using static Helper.ThrowHelper;

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

        public AreaData(
            AreaObjectData areaObjectData,
            AreaSpriteData areaSpriteData,
            int areaNumber,
            AreaType areaType)
        {
            AreaObjectData = areaObjectData ??
                throw new ArgumentNullException(nameof(areaObjectData));

            AreaSpriteData = areaSpriteData ??
                throw new ArgumentNullException(nameof(areaSpriteData));

            AreaNumber = areaNumber;
            AreaType = AreaType;
        }

        public AreaObjectData AreaObjectData
        {
            get;
        }

        public AreaSpriteData AreaSpriteData
        {
            get;
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

        public static byte GetAreaNumber(PrgRom rom, int world, int level)
        {
            if (rom is null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            if (world < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(world),
                    world);
            }

            if (level < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(level),
                    level);
            }

            var worldIndex = rom[LevelsPerWorldPointer + world];
            var levelIndex = worldIndex + AreaListPointer;

            return (byte)(rom[levelIndex + level] & 0x7F);
        }

        public static AreaData FromRomData(PrgRom rom, int areaNumber)
        {
            if (rom is null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            if (areaNumber < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(areaNumber),
                    areaNumber);
            }

            var areaType = (AreaType)((areaNumber >> 5) & 3);
            var reducedMapNumer = areaNumber & 0x1F;
            var objectIndex =
                reducedMapNumer +
                rom[AreaTypeObjectOffsetPointer + (int)areaType];

            var objectAddress =
                rom[ObjectAddressLowBytePointer + objectIndex] |
                (rom[ObjectAddressHighBytePointer + objectIndex] << 8);

            var areaObjectData = new AreaObjectData(rom, objectAddress);

            var spriteIndex =
                reducedMapNumer +
                rom[AreaTypeSpriteOffsetPointer + (int)areaType];

            var spriteAddress =
                rom[SpriteAddressLowBytePointer + spriteIndex] |
                (rom[SpriteAddressHighBytePointer + spriteIndex] << 8);

            var areaSpriteData = new AreaSpriteData(rom, spriteAddress);

            return new AreaData(areaObjectData, areaSpriteData, areaNumber, areaType);
        }

        public static AreaData[] GetAllAreas(PrgRom src)
        {
            var areas = new AreaData[NumberOfAreas];

            var areaPointers = AreaTypeObjectOffsetPointer;
            var ws = src[areaPointers + (int)AreaType.Water];
            var gs = src[areaPointers + (int)AreaType.Grassland];
            var us = src[areaPointers + (int)AreaType.Underground];
            var cs = src[areaPointers + (int)AreaType.Castle];
            var list = new List<AreaIndex>()
            {
                new AreaIndex(ws, AreaType.Water),
                new AreaIndex(gs, AreaType.Grassland),
                new AreaIndex(us, AreaType.Underground),
                new AreaIndex(cs, AreaType.Castle),
                new AreaIndex(Int32.MaxValue, (AreaType)(-1))
            };

            list.Sort((x, y) => x.Index - y.Index);

            for (var i = 0; i < NumberOfAreas; i++)
            {
                var areaNumber = i;
                for (var j = 0; j < 4; j++)
                {
                    if (i >= list[j].Index && i < list[j + 1].Index)
                    {
                        areaNumber -= list[j].Index;
                        areaNumber |= (int)list[j].AreaType << 5;
                        break;
                    }
                }

                areas[i] = FromRomData(src, areaNumber);
            }

            return areas;
        }

        public override string ToString()
        {
            return SR.GetString("Area{0:X2}", AreaNumber);
        }

        private struct AreaIndex
        {
            public AreaIndex(int index, AreaType areaType)
            {
                Index = index;
                AreaType = areaType;
            }

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

            public override string ToString()
            {
                return SR.GetString("0x{0:X2}: {1}", Index, AreaType);
            }
        }
    }
}
