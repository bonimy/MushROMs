using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Helper;

namespace Controls
{
    partial class TileMapControl
    {
        private Range2D _viewSize;
        private Range2D _tileSize;
        private Range2D _zoomSize;

        private Position2D _activeViewTile;

        public event EventHandler ViewSizeChanged;

        public event EventHandler CellSizeChanged;

        public event EventHandler ActiveViewTileChanged;

        public event EventHandler GridLengthChanged;

        public event EventHandler ZeroIndexChanged;

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The size, in tiles, of the view area")]
        public Range2D ViewSize
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
                OnViewSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                OnViewSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                OnViewSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The unit size of a single tile")]
        public Range2D TileSize
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
                OnCellSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                OnCellSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                OnCellSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The size, in pixels, of the unit size.")]
        public Range2D ZoomSize
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
                OnCellSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                OnCellSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                OnCellSizeChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The size, in pixels, of a single tile.")]
        public Range2D CellSize
        {
            get
            {
                return TileSize * ZoomSize;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellWidth
        {
            get
            {
                return TileWidth * ZoomWidth;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellHeight
        {
            get
            {
                return TileHeight * ZoomHeight;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Range2D TileMapSize
        {
            get
            {
                return CellSize * ViewSize;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TileMapWidth
        {
            get
            {
                return CellWidth * ViewWidth;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TileMapHeight
        {
            get
            {
                return CellHeight * ViewHeight;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Position2D ActiveViewTile
        {
            get
            {
                return _activeViewTile;
            }

            set
            {
                if (ActiveViewTile == value)
                {
                    return;
                }

                _activeViewTile = value;
                OnActiveViewTileChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ActiveViewX
        {
            get
            {
                return _activeViewTile.X;
            }

            set
            {
                if (ActiveViewX == value)
                {
                    return;
                }

                _activeViewTile.X = value;
                OnActiveViewTileChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ActiveViewY
        {
            get
            {
                return _activeViewTile.Y;
            }

            set
            {
                if (ActiveViewY == value)
                {
                    return;
                }

                _activeViewTile.Y = value;
                OnActiveViewTileChanged(this, EventArgs.Empty);
            }
        }

        protected virtual void OnViewSizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
            ResetScrollBars();
            ViewSizeChanged?.Invoke(sender, e);
        }

        protected virtual void OnCellSizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
            CellSizeChanged?.Invoke(sender, e);
        }

        protected virtual void OnActiveViewTileChanged(object sender, EventArgs e)
        {
            ActiveViewTileChanged?.Invoke(sender, e);
        }

        protected virtual void OnGridLengthChanged(object sender, EventArgs e)
        {
            ResetScrollBars();
            GridLengthChanged?.Invoke(sender, e);
        }

        protected virtual void OnZeroIndexChanged(object sender, EventArgs e)
        {
            AdjustScrollBarPositions();
            ZeroIndexChanged?.Invoke(sender, e);
        }
    }
}
