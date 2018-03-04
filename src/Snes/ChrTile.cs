﻿// <copyright file="ChrTile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using Helper;

    public struct ChrTile : IEquatable<ChrTile>
    {
        public const int Size = sizeof(ushort);

        private const int TileIndexMask = 0x1FF;
        private const int PaletteNumberBitShift = 9;
        private const int PaletteNumberMask = 7;
        private const int PriorityBitShift = 12;
        private const int PriorityMask = 3;
        private const int FlipBitShift = 14;
        private const int FlipMask = 3;

        private ushort value;

        private ChrTile(int value)
        {
            this.value = (ushort)value;
        }

        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this = new ChrTile(value);
            }
        }

        public int TileIndex
        {
            get
            {
                return Value & TileIndexMask;
            }

            set
            {
                Value &= ~TileIndexMask;
                Value |= value & TileIndexMask;
            }
        }

        public int PaletteNumber
        {
            get
            {
                return (Value >> PaletteNumberBitShift) & PaletteNumberMask;
            }

            set
            {
                Value &= ~(PaletteNumberMask << PaletteNumberBitShift);
                Value |= (value & PaletteNumberMask) << PaletteNumberBitShift;
            }
        }

        public LayerPriority Priority
        {
            get
            {
                return (LayerPriority)((Value >> PriorityBitShift) & PriorityMask);
            }

            set
            {
                Value &= ~(PriorityMask << PriorityBitShift);
                Value |= ((int)value & PriorityMask) << PriorityBitShift;
            }
        }

        public TileFlipMode TileFlipMode
        {
            get
            {
                return (TileFlipMode)((Value >> FlipBitShift) & FlipMask);
            }

            set
            {
                Value &= ~(FlipMask << FlipBitShift);
                Value |= ((int)value & FlipMask) << FlipBitShift;
            }
        }

        public static implicit operator int(ChrTile tile)
        {
            return tile.Value;
        }

        public static implicit operator ChrTile(int value)
        {
            return new ChrTile(value);
        }

        public static bool operator ==(ChrTile left, ChrTile right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ChrTile left, ChrTile right)
        {
            return !(left == right);
        }

        public bool Equals(ChrTile obj)
        {
            return Value.Equals(obj.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is ChrTile tile)
            {
                return Equals(tile);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return SR.GetString(Value, "X4");
        }
    }
}