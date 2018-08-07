// <copyright file="TileMap2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.TileMaps
{
    using System;
    using System.Drawing;

    public class TileMap2D : TileMap
    {
        private Size gridSize;
        private Point zeroTile;
        private Point activeGridTile;

        public Size GridSize
        {
            get
            {
                return gridSize;
            }

            set
            {
                SetGridWidthInternal(value.Width);
                SetGridHeightInternal(value.Height);
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        public int GridWidth
        {
            get
            {
                return GridSize.Width;
            }

            set
            {
                SetGridWidthInternal(value);
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        public int GridHeight
        {
            get
            {
                return GridSize.Height;
            }

            set
            {
                SetGridHeightInternal(value);
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        public Point ZeroTile
        {
            get
            {
                return zeroTile;
            }

            set
            {
                if (ZeroTile == value)
                {
                    return;
                }

                zeroTile = value;
                OnZeroTileChanged(EventArgs.Empty);
            }
        }

        public int ZeroTileX
        {
            get
            {
                return ZeroTile.X;
            }

            set
            {
                if (ZeroTileX == value)
                {
                    return;
                }

                zeroTile.X = value;
                OnZeroTileChanged(EventArgs.Empty);
            }
        }

        public int ZeroTileY
        {
            get
            {
                return ZeroTile.Y;
            }

            set
            {
                if (ZeroTileY == value)
                {
                    return;
                }

                zeroTile.Y = value;
                OnZeroTileChanged(EventArgs.Empty);
            }
        }

        public Point ActiveGridTile
        {
            get
            {
                return activeGridTile;
            }

            set
            {
                if (ActiveGridTile == value)
                {
                    return;
                }

                activeGridTile = value;
                OnActiveGridTileChanged(EventArgs.Empty);
            }
        }

        public override Point ActiveViewTile
        {
            get
            {
                return GetViewTile(ActiveGridTile);
            }

            set
            {
                ActiveGridTile = GetGridTile(value);
            }
        }

        public static int GetGridTileX(int viewTileX, int zeroTileX)
        {
            return viewTileX + zeroTileX;
        }

        public static int GetGridTileY(int viewTileY, int zeroTileY)
        {
            return viewTileY + zeroTileY;
        }

        public static Point GetGridTile(Point viewTile, Point zeroTile)
        {
            var x = GetGridTileX(viewTile.X, zeroTile.X);
            var y = GetGridTileY(viewTile.Y, zeroTile.Y);
            return new Point(x, y);
        }

        public static int GetViewTileX(int gridTileX, int zeroTileX)
        {
            return gridTileX - zeroTileX;
        }

        public static int GetViewTileY(int gridTileY, int zeroTileY)
        {
            return gridTileY - zeroTileY;
        }

        public static Point GetViewTile(Point gridTile, Point zeroTile)
        {
            var x = GetViewTileX(gridTile.X, zeroTile.X);
            var y = GetViewTileY(gridTile.Y, zeroTile.Y);
            return new Point(x, y);
        }

        public bool TileIsInGrid(Point tile)
        {
            var rectangle = new Rectangle(Point.Empty, GridSize);
            return rectangle.Contains(tile);
        }

        public int GetGridTileX(int viewTileX)
        {
            return GetGridTileX(viewTileX, ZeroTileX);
        }

        public int GetGridTileY(int viewTileY)
        {
            return GetGridTileY(viewTileY, ZeroTileY);
        }

        public Point GetGridTile(Point viewTile)
        {
            return GetGridTile(viewTile, ZeroTile);
        }

        public int GetViewTileX(int gridTileX)
        {
            return GetViewTileX(gridTileX, ZeroTileX);
        }

        public int GetViewTileY(int gridTileY)
        {
            return GetViewTileY(gridTileY, ZeroTileY);
        }

        public Point GetViewTile(Point gridTile)
        {
            return GetViewTile(gridTile, ZeroTile);
        }

        private void SetGridWidthInternal(int value)
        {
            if (GridWidth == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            gridSize.Width = value;
        }

        private void SetGridHeightInternal(int value)
        {
            if (GridHeight == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            gridSize.Height = value;
        }
    }
}
