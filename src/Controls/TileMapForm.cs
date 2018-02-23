// <copyright file="TileMapForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Controls
{
    public class TileMapForm : DesignForm
    {
        private TileMapControl _mainTileMapControl;

        public TileMapControl TileMapControl
        {
            get
            {
                return _mainTileMapControl;
            }

            set
            {
                if (TileMapControl == value)
                {
                    return;
                }

                _mainTileMapControl = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private Padding MainTileMapPadding
        {
            get;
            set;
        }

        protected virtual Size MinimumTileSize
        {
            get
            {
                return new Size(1, 1);
            }
        }

        protected virtual Size MaximumTileSize
        {
            get
            {
                return Size.Empty;
            }
        }

        /// <summary>
        /// Do not call this method during this class's constructor. Add an event to <see cref="Form.Load"/> and call it there.
        /// </summary>
        public void AnchorFormToTileMap()
        {
            if (TileMapControl == null)
            {
                MainTileMapPadding = Padding.Empty;
                return;
            }

            var form = WinApiMethods.GetWindowRectangle(this);
            var child = WinApiMethods.GetWindowRectangle(TileMapControl);
            var client = WinApiMethods.DeflateRectangle(
                child,
                TileMapControl.BorderPadding);

            MainTileMapPadding = WinApiMethods.GetPadding(form, client);
        }

        protected virtual void SetSizeFromTileMapControl()
        {
            if (TileMapControl.TileMapResizeMode == TileMapResizeMode.ControlResize)
            {
                return;
            }

            if (TileMapControl.TileMapResizeMode == TileMapResizeMode.None)
            {
                TileMapControl.TileMapResizeMode = TileMapResizeMode.ControlResize;
            }

            var window = WinApiMethods.InflateSize(
                TileMapControl.ClientSize, MainTileMapPadding);
            window = SizeFromTileMap(window);
            Size = window;

            if (TileMapControl.TileMapResizeMode == TileMapResizeMode.ControlResize)
            {
                TileMapControl.TileMapResizeMode = TileMapResizeMode.None;
            }
        }

        private Rectangle GetTileMapRectangle(Size window)
        {
            return GetTileMapRectangle(new Rectangle(Point.Empty, window));
        }

        private Rectangle GetTileMapRectangle(Rectangle window)
        {
            // Edge case for empty rectangles
            if (window.Size == Size.Empty)
            {
                return Rectangle.Empty;
            }

            if (TileMapControl.CellSize.IsEmpty)
            {
                return window;
            }

            // Remove the control padding from rectangle.
            var tilemap = WinApiMethods.DeflateRectangle(
                window,
                MainTileMapPadding);

            var client = WinApiMethods.DeflateRectangle(
                WinApiMethods.GetWindowRectangle(TileMapControl),
                TileMapControl.BorderPadding);

            // Gets the residual width that is not included in the tilemap size.
            var residualWidth = tilemap.Width % TileMapControl.CellWidth;
            var residualHeight = tilemap.Height % TileMapControl.CellHeight;

            // Get the current dimensions of the window
            var parent = WinApiMethods.GetWindowRectangle(this);

            // Remove residual area.
            tilemap.Width -= residualWidth;
            tilemap.Height -= residualHeight;

            // Left or top adjust the client if sizing on those borders.
            if (window.Left != parent.Left && window.Right == parent.Right)
            {
                if (tilemap.Width >= TileMapControl.CellWidth)
                {
                    tilemap.X += residualWidth;
                }
                else
                {
                    tilemap.X = client.X;
                }
            }

            if (window.Top != parent.Top && window.Bottom == parent.Bottom)
            {
                if (tilemap.Height >= TileMapControl.CellHeight)
                {
                    tilemap.Y += residualHeight;
                }
                else
                {
                    tilemap.Y = client.Y;
                }
            }

            // Ensure non-negative values.
            if (tilemap.Width <= 0)
            {
                tilemap.Width = TileMapControl.CellWidth;
            }

            if (tilemap.Height <= 0)
            {
                tilemap.Height = TileMapControl.CellHeight;
            }

            return tilemap;
        }

        private Size GetBoundTileSize(Size window)
        {
            var cellW = TileMapControl.CellWidth;
            var cellH = TileMapControl.CellHeight;

            // Defines the possible minimum and maximum tile sizes.
            var min = new List<Size>();
            var max = new List<Size>();

            // Min/Max tile size according to the form min/max size.
            min.Add(GetTileMapRectangle(MinimumSize).Size);
            max.Add(GetTileMapRectangle(MaximumSize).Size);

            // Min/Max tile size according to system-defined min/max size.
            min.Add(GetTileMapRectangle(SystemInformation.MinimumWindowSize).Size);
            max.Add(GetTileMapRectangle(SystemInformation.PrimaryMonitorMaximizedWindowSize).Size);

            // Min/Max tile size according to the derived value.
            var tileMin = MinimumTileSize;
            var tileMax = MaximumTileSize;

            // The dimensions need to be on a pixel scale
            tileMin.Width *= cellW;
            tileMin.Height *= cellH;
            tileMax.Width *= cellW;
            tileMax.Height *= cellH;

            min.Add(tileMin);
            max.Add(tileMax);

            // Edge case to prevent zero size
            min.Add(new Size(cellW, cellH));

            // Restrict the lower bound of the tilemap.
            foreach (var size in min)
            {
                window.Width = Math.Max(window.Width, size.Width);
                window.Height = Math.Max(window.Height, size.Height);
            }

            // Restrict upper bounds
            foreach (var size in max)
            {
                if (size.Width > 0)
                {
                    window.Width = Math.Min(window.Width, size.Width);
                }

                if (size.Height > 0)
                {
                    window.Height = Math.Min(window.Height, size.Height);
                }
            }

            return window;
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            if (TileMapControl.TileMapResizeMode == TileMapResizeMode.None)
            {
                TileMapControl.TileMapResizeMode = TileMapResizeMode.FormResize;
            }

            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            if (TileMapControl.TileMapResizeMode == TileMapResizeMode.FormResize)
            {
                TileMapControl.TileMapResizeMode = TileMapResizeMode.None;
            }
        }

        protected override void OnAdjustWindowBounds(RectangleEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            e.Rectangle = RectangleFromTileMap(e.Rectangle);
        }

        protected override void OnAdjustWindowSize(RectangleEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            e.Size = SizeFromTileMap(e.Size);
        }

        private Rectangle RectangleFromTileMap(Rectangle window)
        {
            if (TileMapControl is null ||
                MainTileMapPadding == Padding.Empty ||
                TileMapControl.CellSize.IsEmpty)
            {
                return window;
            }

            var tilemap = GetTileMapRectangle(window);
            tilemap.Size = GetBoundTileSize(tilemap.Size);

            // Set new tile size
            if (TileMapControl.TileMapResizeMode != TileMapResizeMode.TileMapCellResize)
            {
                TileMapControl.ViewSize = new Size(
                    tilemap.Width / TileMapControl.CellWidth,
                    tilemap.Height / TileMapControl.CellHeight);
            }

            // Adjust window size to bind to tilemap.
            return WinApiMethods.InflateRectangle(tilemap, MainTileMapPadding);
        }

        private Size SizeFromTileMap(Size window)
        {
            if (TileMapControl is null ||
                MainTileMapPadding == Padding.Empty ||
                TileMapControl.CellSize.IsEmpty)
            {
                return window;
            }

            var tilemap = GetTileMapRectangle(window).Size;
            tilemap = GetBoundTileSize(tilemap);

            // Set new tile size
            if (TileMapControl.TileMapResizeMode == TileMapResizeMode.TileMapCellResize)
            {
                TileMapControl.ViewSize = new Size(
                    tilemap.Width / TileMapControl.CellWidth,
                    tilemap.Height / TileMapControl.CellHeight);
            }

            // Return inflated window size.
            return WinApiMethods.InflateSize(tilemap, MainTileMapPadding);
        }
    }
}
