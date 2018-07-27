// <copyright file="Obj16Tile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using static Helper.StringHelper;

    public struct Obj16Tile : IEquatable<Obj16Tile>
    {
        public const int NumberOfTiles = 4;

        public const int TopLeftIndex = 0;
        public const int BottomLeftIndex = 1;
        public const int TopRightIndex = 2;
        public const int BottomRightIndex = 3;

        public const int SizeOf = NumberOfTiles * ObjTile.SizeOf;

        public Obj16Tile(
            ObjTile topLeft,
            ObjTile topRight,
            ObjTile bottomLeft,
            ObjTile bottomRight)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
        }

        public ObjTile TopLeft
        {
            get;
            set;
        }

        public ObjTile TopRight
        {
            get;
            set;
        }

        public ObjTile BottomLeft
        {
            get;
            set;
        }

        public ObjTile BottomRight
        {
            get;
            set;
        }

        public ObjTile this[int index]
        {
            get
            {
                switch (index)
                {
                    case TopLeftIndex:
                        return TopLeft;

                    case BottomLeftIndex:
                        return TopRight;

                    case TopRightIndex:
                        return BottomLeft;

                    case BottomRightIndex:
                        return BottomRight;

                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case TopLeftIndex:
                        TopLeft = value;
                        return;

                    case BottomLeftIndex:
                        TopRight = value;
                        return;

                    case TopRightIndex:
                        BottomLeft = value;
                        return;

                    case BottomRightIndex:
                        BottomRight = value;
                        return;

                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(index));
                }
            }
        }

        public static bool operator ==(Obj16Tile left, Obj16Tile right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Obj16Tile left, Obj16Tile right)
        {
            return !(left == right);
        }

        public static int GetXCoordinate(int index)
        {
            return index / 2;
        }

        public static int GetYCoordinate(int index)
        {
            return index % 2;
        }

        public Obj16Tile FlipX()
        {
            return new Obj16Tile(
                BottomLeft.FlipX(),
                BottomRight.FlipX(),
                TopLeft.FlipX(),
                TopRight.FlipX());
        }

        public Obj16Tile FlipY()
        {
            return new Obj16Tile(
                TopRight.FlipY(),
                TopLeft.FlipY(),
                BottomRight.FlipY(),
                BottomLeft.FlipY());
        }

        public bool Equals(Obj16Tile other)
        {
            return
                TopLeft.Equals(other.TopLeft) &&
                TopRight.Equals(other.TopRight) &&
                BottomLeft.Equals(other.BottomLeft) &&
                BottomRight.Equals(other.BottomRight);
        }

        public override bool Equals(object obj)
        {
            if (obj is Obj16Tile tile)
            {
                return Equals(tile);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (TopLeft.Value | (TopRight.Value << 0x10)) ^
                (BottomLeft.Value | (BottomRight.Value << 0x10));
        }

        public override string ToString()
        {
            return GetString(
                "{0}-{1}-{2}-{3}",
                TopLeft,
                TopRight,
                BottomLeft,
                BottomRight);
        }
    }
}
