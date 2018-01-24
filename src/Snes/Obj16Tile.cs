// <copyright file="Obj16Tile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using Helper;

namespace Snes
{
    public struct Obj16Tile : IEquatable<Obj16Tile>
    {
        public const int NumberOfTiles = 4;

        public const int TopLeftIndex = 0;
        public const int BottomLeftIndex = 1;
        public const int TopRightIndex = 2;
        public const int BottomRightIndex = 3;

        public const int SizeOf = NumberOfTiles * ObjTile.SizeOf;

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
                        throw new ArgumentOutOfRangeException(nameof(index));
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
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

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

        public static int GetXCoordinate(int index)
        {
            return index / 2;
        }

        public static int GetYCoordinate(int index)
        {
            return index % 2;
        }

        public bool Equals(Obj16Tile obj)
        {
            return this == obj;
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
            return SR.GetString(
                "{0}-{1}-{2}-{3}",
                TopLeft.Value.ToString("X4"),
                TopRight.Value.ToString("X4"),
                BottomLeft.Value.ToString("X4"),
                BottomRight.Value.ToString("X4"));
        }

        public static bool operator ==(Obj16Tile left, Obj16Tile right)
        {
            return
                left.TopLeft == right.TopLeft &&
                left.TopRight == right.TopRight &&
                left.BottomLeft == right.BottomLeft &&
                left.BottomRight == right.BottomRight;
        }

        public static bool operator !=(Obj16Tile left, Obj16Tile right)
        {
            return !(left == right);
        }
    }
}
