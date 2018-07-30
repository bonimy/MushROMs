// <copyright file="TileMapControl.TileMap.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    partial class TileMapControl
    {
        private Size _viewSize;
        private Size _tileSize;
        private Size _zoomSize;

        public event EventHandler ViewSizeChanged;

        public event EventHandler CellSizeChanged;

        public event EventHandler GridLengthChanged;

        public event EventHandler ZeroIndexChanged;

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The size, in tiles, of the view area")]
        public Size ViewSize
        {
            get
            {
                return _viewSize;
            }

            set
            {
                if (ViewSize == value)
                {
                    return;
                }

                _viewSize = value;
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int ViewWidth
        {
            get
            {
                return _viewSize.Width;
            }

            set
            {
                if (ViewWidth == value)
                {
                    return;
                }

                _viewSize.Width = value;
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int ViewHeight
        {
            get
            {
                return _viewSize.Height;
            }

            set
            {
                if (ViewHeight == value)
                {
                    return;
                }

                _viewSize.Height = value;
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The unit size of a single tile")]
        public Size TileSize
        {
            get
            {
                return _tileSize;
            }

            set
            {
                if (TileSize == value)
                {
                    return;
                }

                _tileSize = value;
                OnCellSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int TileWidth
        {
            get
            {
                return _tileSize.Width;
            }

            set
            {
                if (TileWidth == value)
                {
                    return;
                }

                _tileSize.Width = value;
                OnCellSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int TileHeight
        {
            get
            {
                return _tileSize.Height;
            }

            set
            {
                if (TileHeight == value)
                {
                    return;
                }

                _tileSize.Height = value;
                OnCellSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The size, in pixels, of the unit size.")]
        public Size ZoomSize
        {
            get
            {
                return _zoomSize;
            }

            set
            {
                if (ZoomSize == value)
                {
                    return;
                }

                _zoomSize = value;
                OnCellSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int ZoomWidth
        {
            get
            {
                return _zoomSize.Width;
            }

            set
            {
                if (ZoomWidth == value)
                {
                    return;
                }

                _zoomSize.Width = value;
                OnCellSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int ZoomHeight
        {
            get
            {
                return _zoomSize.Height;
            }

            set
            {
                if (ZoomHeight == value)
                {
                    return;
                }

                _zoomSize.Height = value;
                OnCellSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The size, in pixels, of a single tile.")]
        public Size CellSize
        {
            get
            {
                return new Size(CellWidth, CellHeight);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int CellWidth
        {
            get
            {
                return TileWidth * ZoomWidth;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int CellHeight
        {
            get
            {
                return TileHeight * ZoomHeight;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public Size TileMapSize
        {
            get
            {
                return new Size(TileMapWidth, TileMapHeight);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int TileMapWidth
        {
            get
            {
                return CellWidth * ViewWidth;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int TileMapHeight
        {
            get
            {
                return CellHeight * ViewHeight;
            }
        }

        protected virtual void OnViewSizeChanged(EventArgs e)
        {
            SetClientSizeFromTileMap();
            ResetScrollBars();
            ViewSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnCellSizeChanged(EventArgs e)
        {
            SetClientSizeFromTileMap();
            CellSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnGridLengthChanged(EventArgs e)
        {
            ResetScrollBars();
            GridLengthChanged?.Invoke(this, e);
        }

        protected virtual void OnZeroIndexChanged(EventArgs e)
        {
            AdjustScrollBarPositions();
            ZeroIndexChanged?.Invoke(this, e);
        }
    }
}
