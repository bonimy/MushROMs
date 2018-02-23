// <copyright file="EditorFormHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MushROMs;

namespace Controls.Editors
{
    public abstract class EditorFormHelper : IEditorFormHelper
    {
        public event EventHandler ActiveViewTileChanged;

        public TileMapForm Form
        {
            get;
        }

        private TileMapControl TileMapControl
        {
            get
            {
                return Form.TileMapControl;
            }
        }

        public IEditor Editor
        {
            get;
        }

        private Point _activeViewTile;

        public Point ActiveViewTile
        {
            get
            {
                return _activeViewTile;
            }

            set
            {
                if (_activeViewTile == value)
                {
                    return;
                }

                _activeViewTile = value;
                OnActiveViewTileChanged(EventArgs.Empty);
            }
        }

        protected EditorFormHelper(TileMapForm form, IEditor editor)
        {
            Form = form ??
                throw new ArgumentNullException(nameof(form));

            Editor = editor ??
                throw new ArgumentNullException(nameof(editor));

            var tileMapControl = Form.TileMapControl;
            tileMapControl.KeyDown += TileMapControl_KeyDown;
            tileMapControl.MouseMove += TileMapControl_MouseMove;
            tileMapControl.DrawViewTile += DrawViewTile;
        }

        protected virtual void OnActiveViewTileChanged(EventArgs e)
        {
            ActiveViewTileChanged?.Invoke(this, e);
            Form.TileMapControl.Invalidate();
        }

        protected virtual void DrawActiveViewTile(
            TileMapControl tileMapControl,
            Graphics g)
        {
            if (tileMapControl is null)
            {
                throw new ArgumentNullException(nameof(tileMapControl));
            }

            if (g is null)
            {
                throw new ArgumentNullException(nameof(g));
            }

            var tile = ActiveViewTile;
            var padding = new Padding(2);
            var cellSize = tileMapControl.CellSize;
            var x = tile.X * cellSize.Width;
            var y = tile.Y * cellSize.Height;

            x += padding.Left;
            y += padding.Top;
            var width = cellSize.Width - padding.Horizontal;
            var height = cellSize.Height - padding.Vertical;
            var rectangle = new Rectangle(x, y, width - 1, height - 1);

            using (var path = new GraphicsPath())
            {
                path.AddRectangle(rectangle);

                g.DrawPath(Pens.Black, path);
            }
        }

        private void DrawViewTile(object sender, PaintEventArgs e)
        {
            DrawActiveViewTile(sender as TileMapControl, e.Graphics);
        }

        protected virtual void SetActiveViewTileFromPixel(
            TileMapControl tileMapControl,
            Point location)
        {
            if (tileMapControl is null)
            {
                throw new ArgumentNullException(nameof(tileMapControl));
            }

            var clientRectangle = tileMapControl.ClientRectangle;
            if (!clientRectangle.Contains(location))
            {
                return;
            }

            if (!tileMapControl.MouseHovering)
            {
                var cellSize = tileMapControl.CellSize;
                var x = location.X / cellSize.Width;
                var y = location.Y / cellSize.Height;
                ActiveViewTile = new Point(x, y);
            }
        }

        private void TileMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            SetActiveViewTileFromPixel(
                sender as TileMapControl,
                e.Location);
        }

        protected virtual void SetActiveViewTileFromKey(
            TileMapControl tileMapControl,
            Keys keys)
        {
            var active = ActiveViewTile;
            switch (keys)
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

            ActiveViewTile = active;
        }

        private void TileMapControl_KeyDown(object sender, KeyEventArgs e)
        {
            SetActiveViewTileFromKey(
                sender as TileMapControl,
                e.KeyCode);
        }
    }
}
