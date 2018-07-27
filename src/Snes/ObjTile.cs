// <copyright file="ObjTile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using static Helper.StringHelper;

    public struct ObjTile : IEquatable<ObjTile>
    {
        public const int SizeOf = sizeof(ushort);

        private const int TileIndexMask = 0x3FF;
        private const int PaletteOffset = 10;
        private const int PaletteMask = 7;
        private const int PriorityOffset = 13;
        private const int PriorityMask = 1;
        private const int FlipOffset = 14;
        private const int FlipMask = 3;

        private ushort value;

        private ObjTile(int value)
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

        public int PaletteNumber
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
                if (value != LayerPriority.Priority0)
                {
                    Value |= PriorityMask << PriorityOffset;
                }
                else
                {
                    Value &= ~PriorityMask << PriorityOffset;
                }
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

        public bool XFlipped
        {
            get
            {
                return
                    (TileFlipMode & TileFlipMode.FlipHorizontal) != 0;
            }

            set
            {
                if (value)
                {
                    TileFlipMode &= ~TileFlipMode.FlipHorizontal;
                }
                else
                {
                    TileFlipMode |= TileFlipMode.FlipHorizontal;
                }
            }
        }

        public bool YFlipped
        {
            get
            {
                return (TileFlipMode & TileFlipMode.FlipVeritcal) != 0;
            }

            set
            {
                if (value)
                {
                    TileFlipMode &= ~TileFlipMode.FlipVeritcal;
                }
                else
                {
                    TileFlipMode |= TileFlipMode.FlipVeritcal;
                }
            }
        }

        public static implicit operator int(ObjTile tile)
        {
            return tile.Value;
        }

        public static implicit operator ObjTile(int value)
        {
            return new ObjTile(value);
        }

        public static bool operator ==(ObjTile left, ObjTile right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ObjTile left, ObjTile right)
        {
            return left.Value != right.Value;
        }

        public ObjTile FlipX()
        {
            var tile = this;
            tile.XFlipped ^= true;
            return tile;
        }

        public ObjTile FlipY()
        {
            var tile = this;
            tile.YFlipped ^= true;
            return tile;
        }

        public bool Equals(ObjTile other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjTile tile)
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
