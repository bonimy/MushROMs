// <copyright file="TileMap.cs" company="Public Domain">
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

    public abstract class TileMap
    {
        public static readonly Size FallbackTileSize = new Size(8, 8);
        public static readonly Size FallbackZoomSize = new Size(2, 2);
        public static readonly Size FallbackViewSize = new Size(16, 8);

        private Size tileSize;
        private Size zoomSize;
        private Size viewSize;

        protected TileMap()
        {
            tileSize = FallbackTileSize;
            zoomSize = FallbackZoomSize;
            viewSize = FallbackViewSize;
        }

        public event EventHandler GridSizeChanged;

        public event EventHandler ZeroTileChanged;

        public event EventHandler ActiveGridTileChanged;

        public event EventHandler TileSizeChanged;

        public event EventHandler ZoomSizeChanged;

        public event EventHandler ViewSizeChanged;

        public abstract Point ActiveViewTile
        {
            get;
            set;
        }

        public Size TileSize
        {
            get
            {
                return tileSize;
            }

            set
            {
                SetTileWidthInternal(value.Width);
                SetTileHeightInternal(value.Height);
                OnTileSizeChanged(EventArgs.Empty);
            }
        }

        public int TileWidth
        {
            get
            {
                return TileSize.Width;
            }

            set
            {
                SetTileWidthInternal(value);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        public int TileHeight
        {
            get
            {
                return TileSize.Height;
            }

            set
            {
                SetTileHeightInternal(value);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        public Size ZoomSize
        {
            get
            {
                return zoomSize;
            }

            set
            {
                SetZoomWidthInternal(value.Width);
                SetZoomHeightInternal(value.Height);
                OnZoomSizeChanged(EventArgs.Empty);
            }
        }

        public int ZoomWidth
        {
            get
            {
                return ZoomSize.Width;
            }

            set
            {
                SetZoomWidthInternal(value);
                OnZoomSizeChanged(EventArgs.Empty);
            }
        }

        public int ZoomHeight
        {
            get
            {
                return ZoomSize.Height;
            }

            set
            {
                SetZoomHeightInternal(value);
                OnZoomSizeChanged(EventArgs.Empty);
            }
        }

        public Size ViewSize
        {
            get
            {
                return viewSize;
            }

            set
            {
                SetViewWidthInternal(value.Width);
                SetViewHeightInternal(value.Height);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        public int ViewWidth
        {
            get
            {
                return ViewSize.Width;
            }

            set
            {
                SetViewWidthInternal(value);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        public int ViewHeight
        {
            get
            {
                return ViewSize.Height;
            }

            set
            {
                SetViewHeightInternal(value);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        public Size CellSize
        {
            get
            {
                return new Size(CellWidth, CellHeight);
            }
        }

        public int CellWidth
        {
            get
            {
                return TileWidth * ZoomWidth;
            }
        }

        public int CellHeight
        {
            get
            {
                return TileHeight * ZoomHeight;
            }
        }

        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
        }

        public int Width
        {
            get
            {
                return CellWidth * ViewWidth;
            }
        }

        public int Height
        {
            get
            {
                return CellHeight * ViewHeight;
            }
        }

        public Point GetViewTileFromScreenDot(Point dot, bool zoom)
        {
            var size = zoom ? CellSize : TileSize;
            var x = dot.X / size.Width;
            var y = dot.Y / size.Height;

            return new Point(x, y);
        }

        public Point GetScreenDotFromViewTile(Point tile, bool zoom)
        {
            var size = zoom ? CellSize : TileSize;
            var x = tile.X * size.Width;
            var y = tile.Y * size.Height;

            return new Point(x, y);
        }

        public bool ViewTileIsInViewRegion(Point viewTile)
        {
            var rectangle = new Rectangle(Point.Empty, ViewSize);
            return rectangle.Contains(viewTile);
        }

        protected virtual void OnTileSizeChanged(EventArgs e)
        {
            TileSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnZoomSizeChanged(EventArgs e)
        {
            ZoomSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnViewSizeChanged(EventArgs e)
        {
            ViewSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnGridSizeChanged(EventArgs e)
        {
            GridSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnZeroTileChanged(EventArgs e)
        {
            ZeroTileChanged?.Invoke(this, e);
        }

        protected virtual void OnActiveGridTileChanged(EventArgs e)
        {
            ActiveGridTileChanged?.Invoke(this, e);
        }

        private protected void AssertGreaterThanEqualToZero(int value)
        {
            if (value < 0)
            {
                throw ValueNotGreaterThan(
                    nameof(value),
                    value);
            }
        }

        private protected void AssertGreaterThanZero(int value)
        {
            if (value <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(value),
                    value);
            }
        }

        private void SetTileWidthInternal(int value)
        {
            if (TileWidth == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            tileSize.Width = value;
        }

        private void SetTileHeightInternal(int value)
        {
            if (TileHeight == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            tileSize.Height = value;
        }

        private void SetZoomWidthInternal(int value)
        {
            if (ZoomWidth == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            zoomSize.Width = value;
        }

        private void SetZoomHeightInternal(int value)
        {
            if (ZoomHeight == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            zoomSize.Height = value;
        }

        private void SetViewWidthInternal(int value)
        {
            if (ViewWidth == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            viewSize.Width = value;
        }

        private void SetViewHeightInternal(int value)
        {
            if (ViewHeight == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            viewSize.Height = value;
        }
    }
}
