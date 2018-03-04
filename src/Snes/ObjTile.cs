// <copyright file="ObjTile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using Helper;

    public struct ObjTile : IEquatable<ObjTile>
    {
        public const int SizeOf = sizeof(ushort);

        private const int TileIndexMask = 0x3FF;
        private const int PaletteNumberBitShift = 10;
        private const int PaletteNumberMask = 7;
        private const int PriorityBitShift = 13;
        private const int PriorityMask = 1;
        private const int FlipBitShift = 14;
        private const int FlipMask = 3;

        private ushort value;

        private ObjTile(int value)
        {
            this.value = (ushort)value;
        }

        public int Value
        {
            get { return value; }
            set { this = new ObjTile(value); }
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
                if (value != LayerPriority.Priority0)
                {
                    Value |= PriorityMask << PriorityBitShift;
                }
                else
                {
                    Value &= ~PriorityMask << PriorityBitShift;
                }
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

        public bool XFlipped
        {
            get
            {
                return (TileFlipMode & TileFlipMode.FlipHorizontal) != 0;
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
            return left.Value == right.Value;
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

        public bool Equals(ObjTile obj)
        {
            return Value.Equals(obj.Value);
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
            return SR.GetString(Value, "X4");
        }
    }
}
