// <copyright file="ChrTile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using static Helper.StringHelper;

    public struct ChrTile : IEquatable<ChrTile>
    {
        public const int Size = sizeof(ushort);

        private const int TileIndexMask = 0x1FF;
        private const int PaletteOffset = 9;
        private const int PaletteMask = 7;
        private const int PriorityOffset = 12;
        private const int PriorityMask = 3;
        private const int FlipOffset = 14;
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
                this.value = (ushort)value;
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

        public int PaletteIndex
        {
            get
            {
                return (Value >> PaletteOffset) & PaletteMask;
            }

            set
            {
                Value &= ~(PaletteMask << PaletteOffset);
                Value |= (value & PaletteMask) << PaletteOffset;
            }
        }

        public LayerPriority Priority
        {
            get
            {
                return (LayerPriority)(
                    (Value >> PriorityOffset) & PriorityMask);
            }

            set
            {
                Value &= ~(PriorityMask << PriorityOffset);
                Value |= ((int)value & PriorityMask) << PriorityOffset;
            }
        }

        public TileFlipMode TileFlipMode
        {
            get
            {
                return (TileFlipMode)((Value >> FlipOffset) & FlipMask);
            }

            set
            {
                Value &= ~(FlipMask << FlipOffset);
                Value |= ((int)value & FlipMask) << FlipOffset;
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
            return GetString(Value, "X4");
        }
    }
}
