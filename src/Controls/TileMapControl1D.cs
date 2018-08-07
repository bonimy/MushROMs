// <copyright file="TileMapControl1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MushROMs.TileMaps;
using static Helper.ThrowHelper;

namespace Controls
{
    public class TileMapControl1D : TileMapControl
    {
        private int _gridSize;
        private int _zeroTile;

        public int ZeroTile
        {
            get
            {
                return _zeroTile;
            }

            set
            {
                if (ZeroTile == value)
                {
                    return;
                }

                _zeroTile = value;
                OnZeroIndexChanged(EventArgs.Empty);
            }
        }

        public int GridSize
        {
            get
            {
                return _gridSize;
            }

            set
            {
                if (GridSize == value)
                {
                    return;
                }

                if (value < 0)
                {
                    throw ValueNotGreaterThanEqualTo(
                        nameof(value),
                        value);
                }

                _gridSize = value;
                OnGridLengthChanged(EventArgs.Empty);
            }
        }

        public Point ZeroPosition
        {
            get
            {
                var zero = Point.Empty;
                if (HorizontalScrollBar != null)
                {
                    zero.X = HorizontalScrollBar.Value;
                }

                if (VerticalScrollBar != null)
                {
                    zero.Y = VerticalScrollBar.Value;
                }

                return zero;
            }
        }

        public bool GridContainsTile(int tile)
        {
            return tile >= 0 && tile < GridSize;
        }

        public override void GenerateSelectionPath(
            GraphicsPath path,
            ISelection1D selection)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            path.Reset();

            for (var y = ViewHeight; --y >= 0;)
            {
                for (var x = ViewWidth; --x >= 0;)
                {
                    var index = GetGridTile(new Point(x, y));
                    if (selection.Contains(index) && GridContainsTile(index))
                    {
                        var edges = new Point[]
                        {
                            new Point(x - 1, y),
                            new Point(x, y - 1),
                            new Point(x + 1, y),
                            new Point(x, y + 1)
                        };
                        var clips = new int[]
                        {
                            x * CellWidth,
                            y * CellHeight,
                            ((x + 1) * CellWidth) - 1,
                            ((y + 1) * CellHeight) - 1
                        };
                        var corners = new Point[4];
                        corners[0] = new Point(clips[0], clips[1]);
                        corners[1] = new Point(clips[2], clips[1]);
                        corners[2] = new Point(clips[2], clips[3]);
                        corners[3] = new Point(clips[0], clips[3]);

                        for (var i = edges.Length; --i >= 0;)
                        {
                            var index2 = GetGridTile(edges[i]);
                            if (!selection.Contains(index2) || !GridContainsTile(index2))
                            {
                                path.StartFigure();
                                path.AddLine(corners[((i - 1) & 3)], corners[i]);
                            }
                        }
                    }
                }
            }
        }

        protected override void ResetHorizontalScrollBar()
        {
            if (HorizontalScrollBar is null)
            {
                return;
            }

            if (HorizontalScrollBar.Enabled = ViewWidth > 1)
            {
                HorizontalScrollBar.SmallChange = 1;
                HorizontalScrollBar.LargeChange = ViewWidth - 1;
                HorizontalScrollBar.Minimum = 0;
                HorizontalScrollBar.Maximum = ((ViewWidth - 1) * 2) - 1;
                HorizontalScrollBar.Value = ZeroTile % ViewWidth;
            }
        }

        protected override void ResetVerticalScrollBar()
        {
            if (VerticalScrollBar is null)
            {
                return;
            }

            if (ViewWidth == 0)
            {
                return;
            }

            var rows = GridSize / ViewWidth;
            var enabled = rows > ViewHeight;

            if (enabled)
            {
                VerticalScrollBar.Enabled = true;
                VerticalScrollBar.Minimum = 0;
                VerticalScrollBar.Maximum = rows - 1;
                VerticalScrollBar.SmallChange = 1;
                VerticalScrollBar.LargeChange = ViewHeight;

                var value = ZeroTile / ViewWidth;
                if (rows <= value + ViewHeight)
                {
                    value = rows - ViewHeight;
                }

                VerticalScrollBar.Value = value;
            }
            else
            {
                VerticalScrollBar.Value = 0;
                VerticalScrollBar.Enabled = false;
            }
        }

        protected override void AdjustScrollBarPositions()
        {
            HorizontalScrollBar.Value = ZeroTile % ViewWidth;
            VerticalScrollBar.Value = ZeroTile / ViewWidth;
        }

        protected override void ScrollTileMapHorizontal(int value)
        {
            var zeroY = ZeroTile / ViewWidth;
            ZeroTile = value + (zeroY * ViewHeight);
        }

        protected override void ScrollTileMapVertical(int value)
        {
            var zeroX = ZeroTile % ViewWidth;
            ZeroTile = (value * ViewWidth) + zeroX;
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

        public static int GetViewTileX(int gridTile, int viewWidth, int zeroIndex)
        {
            if (viewWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(viewWidth),
                    viewWidth);
            }

            return (gridTile - zeroIndex) % viewWidth;
        }

        public static int GetViewTileY(int gridTile, int viewWidth, int zeroIndex)
        {
            if (viewWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(viewWidth),
                    viewWidth);
            }

            return (gridTile - zeroIndex) / viewWidth;
        }

        public static Point GetViewTile(
            int gridTile,
            int viewWidth,
            int zeroIndex)
        {
            return new Point(
                GetViewTileX(gridTile, viewWidth, zeroIndex),
                GetViewTileY(gridTile, viewWidth, zeroIndex));
        }

        public int GetGridTile(Point viewTile)
        {
            return GetGridTile(viewTile, ViewWidth, ZeroTile);
        }

        public int GetGridTile(int viewTileX, int viewTileY)
        {
            return GetGridTile(viewTileX, viewTileY, ViewWidth, ZeroTile);
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
            return GetGridTile(viewTile.X, viewTile.Y, viewWidth, zeroIndex);
        }

        public static int GetGridTile(
            int viewTileX,
            int viewTileY,
            int viewWidth,
            int zeroIndex)
        {
            if (viewWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(viewWidth),
                    viewWidth);
            }

            return (viewTileY * viewWidth) + viewTileX + zeroIndex;
        }
    }
}
