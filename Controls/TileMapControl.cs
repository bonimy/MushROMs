// <copyright file="TileMapControl.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Helper;

namespace Controls
{
    public abstract class TileMapControl : DesignControl, ITileMap
    {
        private ScrollBar _verticalScrollBar;
        private ScrollBar _heightScrollBar;

        private Range2D _viewSize;
        private Range2D _tileSize;
        private Range2D _zoomSize;

        private Position2D _activeViewTile;

        public event EventHandler ViewSizeChanged;

        public event EventHandler CellSizeChanged;

        public event EventHandler ActiveViewTileChanged;

        public event EventHandler GridLengthChanged;

        public event EventHandler ZeroIndexChanged;

        public Range2D ViewSize
        {
            get
            {
                return _viewSize;
            }

            set
            {
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
                _viewSize.Height = value;
                OnViewSizeChanged(this, EventArgs.Empty);
            }
        }

        public Range2D TileSize
        {
            get
            {
                return _tileSize;
            }

            set
            {
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
                _tileSize.Height = value;
                OnCellSizeChanged(this, EventArgs.Empty);
            }
        }

        public Range2D ZoomSize
        {
            get
            {
                return _zoomSize;
            }

            set
            {
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
                _zoomSize.Height = value;
                OnCellSizeChanged(this, EventArgs.Empty);
            }
        }

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

        public Position2D ActiveViewTile
        {
            get
            {
                return _activeViewTile;
            }

            set
            {
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
                _activeViewTile.Y = value;
                OnActiveViewTileChanged(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal TileMapResizeMode TileMapResizeMode
        {
            get;
            set;
        }

        public ScrollBar VerticalScrollBar
        {
            get
            {
                return _verticalScrollBar;
            }

            set
            {
                if (VerticalScrollBar == value)
                {
                    return;
                }

                if (VerticalScrollBar != null)
                {
                    VerticalScrollBar.Scroll -= VerticalScrollBar_Scroll;
                    VerticalScrollBar.ValueChanged -= VerticalScrollBar_ValueChanged;
                }

                _verticalScrollBar = value;

                if (VerticalScrollBar != null)
                {
                    VerticalScrollBar.Scroll += VerticalScrollBar_Scroll;
                    VerticalScrollBar.ValueChanged += VerticalScrollBar_ValueChanged;
                }

                ResetVerticalScrollBar();
            }
        }

        public ScrollBar HorizontalScrollBar
        {
            get
            {
                return _heightScrollBar;
            }

            set
            {
                if (HorizontalScrollBar == value)
                {
                    return;
                }

                if (HorizontalScrollBar != null)
                {
                    HorizontalScrollBar.Scroll -= HorizontalScrollBar_Scroll;
                    HorizontalScrollBar.ValueChanged -= HorizontalScrollBar_ValueChanged;
                }

                _heightScrollBar = value;

                if (HorizontalScrollBar != null)
                {
                    HorizontalScrollBar.Scroll += HorizontalScrollBar_Scroll;
                    HorizontalScrollBar.ValueChanged += HorizontalScrollBar_ValueChanged;
                }

                ResetHorizontalScrollBar();
            }
        }

        protected override void SetClientSizeCore(int x, int y)
        {
            if (TileMapResizeMode == TileMapResizeMode.ControlResize)
            {
                return;
            }

            if (TileMapResizeMode == TileMapResizeMode.None)
            {
                TileMapResizeMode = TileMapResizeMode.ControlResize;
            }

            base.SetClientSizeCore(x, y);

            if (TileMapResizeMode == TileMapResizeMode.ControlResize)
            {
                TileMapResizeMode = TileMapResizeMode.None;
            }
        }

        public void ResetScrollBars()
        {
            ResetVerticalScrollBar();
            ResetHorizontalScrollBar();
        }

        protected abstract void ResetHorizontalScrollBar();

        protected abstract void ResetVerticalScrollBar();

        protected abstract void AdjustScrollBarPositions();

        protected abstract void ScrollTileMapVertical(int value);

        protected abstract void ScrollTileMapHorizontal(int value);

        private void SetClientSizeFromTileMap()
        {
            if (TileMapSize.ToSize() == ClientSize ||
                TileMapResizeMode == TileMapResizeMode.TileMapCellResize)
            {
                return;
            }

            if (TileMapResizeMode == TileMapResizeMode.None)
            {
                TileMapResizeMode = TileMapResizeMode.TileMapCellResize;
            }

            SetClientSizeCore(TileMapSize.Width, TileMapSize.Height);

            if (TileMapResizeMode == TileMapResizeMode.TileMapCellResize)
            {
                TileMapResizeMode = TileMapResizeMode.None;
            }
        }

        public void DrawViewTilePath(GraphicsPath path, Position2D tile, Padding padding)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            path.Reset();

            var dot = tile * CellSize;
            path.AddRectangle(new Rectangle(
                dot.X + padding.Left,
                dot.Y + padding.Top,
                CellSize.Width - 1 - padding.Horizontal,
                CellSize.Height - 1 - padding.Vertical));
        }

        public abstract void GenerateSelectionPath(GraphicsPath path);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            GetActiveTileFromMouse(e);

            base.OnMouseMove(e);
        }

        protected virtual void GetActiveTileFromMouse(MouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (!ClientRectangle.Contains(e.Location))
            {
                return;
            }

            if (!MouseHovering)
            {
                ActiveViewTile = e.Location.ToPosition2D() / CellSize;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            GetActiveTileFromKeys(e);

            base.OnKeyDown(e);
        }

        protected virtual void GetActiveTileFromKeys(KeyEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var active = ActiveViewTile;
            switch (e.KeyCode)
            {
            case Keys.Left:
            active.X--;
            break;

            case Keys.Right:
            active.X++;
            break;

            case Keys.Up:
            active.Y--;
            break;

            case Keys.Down:
            active.Y++;
            break;
            }

            if (ActiveViewTile != active)
            {
                ActiveViewTile = active;
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

        private void HorizontalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ScrollTileMapHorizontal(e.NewValue);
            }
        }

        private void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ScrollTileMapVertical(e.NewValue);
            }
        }

        private void VerticalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapVertical(VerticalScrollBar.Value);
        }

        private void HorizontalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapHorizontal(HorizontalScrollBar.Value);
        }
    }
}
