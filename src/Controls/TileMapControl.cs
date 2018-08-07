// <copyright file="TileMapControl.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MushROMs.TileMaps;

namespace Controls
{
    public abstract partial class TileMapControl : DesignControl, ITileMap
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public event PaintEventHandler DrawViewTile;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public event PaintEventHandler DrawSelection;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal TileMapResizeMode TileMapResizeMode
        {
            get;
            set;
        }

        protected TileMapControl()
        {
            ViewSize = new Size(8, 8);
            TileSize = new Size(8, 8);
            ZoomSize = new Size(1, 1);
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
            if ((Size)TileMapSize == ClientSize)
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
            Point tile,
            Padding padding)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            path.Reset();

            var x = tile.X * CellWidth;
            var y = tile.Y * CellHeight;
            var rectangle = new Rectangle(
                x + padding.Left,
                y + padding.Top,
                CellSize.Width - 1 - padding.Horizontal,
                CellSize.Height - 1 - padding.Vertical);

            path.AddRectangle(rectangle);
        }

        public abstract void GenerateSelectionPath(
            GraphicsPath path,
            ISelection1D selection);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnDrawViewTile(e);
            OnDrawSelection(e);
        }

        protected virtual void OnDrawViewTile(PaintEventArgs e)
        {
            DrawViewTile?.Invoke(this, e);
        }

        protected virtual void OnDrawSelection(PaintEventArgs e)
        {
            DrawSelection?.Invoke(this, e);
        }
    }
}
