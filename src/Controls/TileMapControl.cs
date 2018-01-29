// <copyright file="TileMapControl.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Helper;
using MushROMs;

namespace Controls
{
    public abstract partial class TileMapControl : DesignControl, ITileMap
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal TileMapResizeMode TileMapResizeMode
        {
            get;
            set;
        }

        protected TileMapControl()
        {
            ViewSize = new Range2D(8, 8);
            TileSize = new Range2D(8, 8);
            ZoomSize = new Range2D(1, 1);
        }

        protected override void SetBoundsCore(
            int x,
            int y,
            int width,
            int height,
            BoundsSpecified specified)
        {
            var padding = BorderPadding;
            var clientSize = new Size(
                width - padding.Horizontal,
                height - padding.Vertical);

            var size = new Size(
                clientSize.Width + padding.Horizontal,
                clientSize.Height + padding.Vertical);

            base.SetBoundsCore(x, y, size.Width, size.Height, specified);
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

        private void SetClientSizeFromTileMap()
        {
            if ((Size)TileMapSize == ClientSize ||
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

        public void DrawViewTilePath(
            GraphicsPath path,
            Position2D tile,
            Padding padding)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            path.Reset();

            var dot = tile * CellSize;
            var rectangle = new Rectangle(
                dot.X + padding.Left,
                dot.Y + padding.Top,
                CellSize.Width - 1 - padding.Horizontal,
                CellSize.Height - 1 - padding.Vertical);

            path.AddRectangle(rectangle);
        }

        public abstract void GenerateSelectionPath(
            GraphicsPath path,
            ISelection<int> selection);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            GetActiveTileFromMouse(e);

            base.OnMouseMove(e);
        }

        protected virtual void GetActiveTileFromMouse(MouseEventArgs e)
        {
            if (CellSize == Range2D.Empty)
            {
                return;
            }

            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (!ClientRectangle.Contains(e.Location))
            {
                return;
            }

            if (!MouseHovering)
            {
                ActiveViewTile = (Position2D)e.Location / CellSize;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            GetActiveTileFromKeys(e);

            base.OnKeyDown(e);
        }

        protected virtual void GetActiveTileFromKeys(KeyEventArgs e)
        {
            if (e is null)
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
    }
}
