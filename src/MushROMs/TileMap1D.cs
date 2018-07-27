// <copyright file="TileMap1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;
    using System.Drawing;
    using static Helper.ThrowHelper;
    using static System.Math;

    public class TileMap1D : TileMap
    {
        private int gridSize;
        private int zeroTile;
        private int activeGridTile;

        public int GridSize
        {
            get
            {
                return gridSize;
            }

            set
            {
                if (GridSize == value)
                {
                    return;
                }

                AssertGreaterThanEqualToZero(value);
                gridSize = value;
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        public int ZeroTile
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

        public int VisibleGridSpan
        {
            get
            {
                var area = ViewWidth * ViewHeight;
                return Min(GridSize - ZeroTile, area);
            }
        }

        public int ActiveGridTile
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

        public static int GetViewTileX(int gridTile, int viewWidth)
        {
            return GetViewTileX(gridTile, viewWidth, 0);
        }

        public static int GetViewTileY(int gridTile, int viewWidth)
        {
            return GetViewTileY(gridTile, viewWidth, 0);
        }

        public static Point GetViewTile(int gridTile, int viewWidth)
        {
            return GetViewTile(gridTile, viewWidth, 0);
        }

        public static int GetViewTileX(
            int gridTile,
            int viewWidth,
            int zeroIndex)
        {
            AssertViewWidth(viewWidth);
            return (gridTile - zeroIndex) % viewWidth;
        }

        public static int GetViewTileY(
            int gridTile,
            int viewWidth,
            int zeroIndex)
        {
            AssertViewWidth(viewWidth);
            return (gridTile - zeroIndex) / viewWidth;
        }

        public static Point GetViewTile(
            int gridTile,
            int viewWidth,
            int zeroIndex)
        {
            var x = GetViewTileX(gridTile, viewWidth, zeroIndex);
            var y = GetViewTileY(gridTile, viewWidth, zeroIndex);
            return new Point(x, y);
        }

        public static int GetGridTile(Point viewTile, int viewWidth)
        {
            return GetGridTile(viewTile, viewWidth, 0);
        }

        public static int GetGridTile(
            int viewTileX,
            int viewTileY,
            int viewWidth)
        {
            return GetGridTile(viewTileX, viewTileY, viewWidth, 0);
        }

        public static int GetGridTile(
            Point viewTile,
            int viewWidth,
            int zeroIndex)
        {
            return GetGridTile(
                viewTile.X,
                viewTile.Y,
                viewWidth,
                zeroIndex);
        }

        public static int GetGridTile(
            int viewTileX,
            int viewTileY,
            int viewWidth,
            int zeroIndex)
        {
            AssertViewWidth(viewWidth);
            return (viewTileY * viewWidth) + viewTileX + zeroIndex;
        }

        public bool TileIsInGrid(int tile)
        {
            return tile >= 0 && tile < GridSize;
        }

        public int GetViewTileX(int gridTile)
        {
            return GetViewTileX(gridTile, ViewWidth, ZeroTile);
        }

        public int GetViewTileY(int gridTile)
        {
            return GetViewTileY(gridTile, ViewWidth, ZeroTile);
        }

        public Point GetViewTile(int gridTile)
        {
            return GetViewTile(gridTile, ViewWidth, ZeroTile);
        }

        public int GetGridTile(Point viewTile)
        {
            return GetGridTile(viewTile, ViewWidth, ZeroTile);
        }

        public int GetGridTile(int viewTileX, int viewTileY)
        {
            return GetGridTile(
                viewTileX,
                viewTileY,
                ViewWidth,
                ZeroTile);
        }

        private static void AssertViewWidth(int viewWidth)
        {
            if (viewWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(viewWidth),
                    viewWidth);
            }
        }
    }
}
