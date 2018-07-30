// <copyright file="TileMapForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using static System.Math;
    using static WinApiMethods;
    using ResizeMode = TileMapResizeMode;

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
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
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

        private ResizeMode ResizeMode
        {
            get
            {
                return TileMapControl.TileMapResizeMode;
            }

            set
            {
                TileMapControl.TileMapResizeMode = value;
            }
        }

        private bool IsEmptyTileMap
        {
            get
            {
                return TileMapControl is null ||
                MainTileMapPadding == Padding.Empty ||
                TileMapControl.CellSize.IsEmpty;
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

            var form = GetWindowRectangle(this);
            var child = GetWindowRectangle(
                TileMapControl);

            var client = DeflateRectangle(
                child,
                TileMapControl.BorderPadding);

            MainTileMapPadding = GetPadding(form, client);
        }

        protected virtual void SetSizeFromTileMapControl()
        {
            if (ResizeMode == ResizeMode.ControlResize)
            {
                return;
            }

            if (ResizeMode == ResizeMode.None)
            {
                ResizeMode = ResizeMode.ControlResize;
            }

            var window = InflateSize(
                TileMapControl.ClientSize,
                MainTileMapPadding);

            window = SizeFromTileMap(window);
            Size = window;

            if (ResizeMode == ResizeMode.ControlResize)
            {
                ResizeMode = ResizeMode.None;
            }
        }

        private Rectangle GetTileMapRectangle(Size window)
        {
            return GetTileMapRectangle(
                new Rectangle(Point.Empty, window));
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
            var tilemap = DeflateRectangle(
                window,
                MainTileMapPadding);

            var client = DeflateRectangle(
                GetWindowRectangle(TileMapControl),
                TileMapControl.BorderPadding);

            // Gets the residual width that is not included in the tilemap size.
            var width = tilemap.Width;
            var height = tilemap.Height;

            var cellWidth = TileMapControl.CellWidth;
            var cellHeight = TileMapControl.CellHeight;

            var residualWidth = width % cellWidth;
            var residualHeight = height % cellHeight;

            // Get the current dimensions of the window
            var parent = GetWindowRectangle(this);

            // Remove residual area.
            tilemap.Width -= residualWidth;
            tilemap.Height -= residualHeight;

            // Left or top adjust the client if sizing on those borders.
            var leftAligned = window.Left == parent.Left;
            var rightAligned = window.Right == parent.Right;
            if (!leftAligned && rightAligned)
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

            var topAligned = window.Top == parent.Top;
            var bottomAligned = window.Bottom == parent.Bottom;
            if (!topAligned && bottomAligned)
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
            var minSystemTileMap = GetTileMapRectangle(
                SystemInformation.MinimumWindowSize);

            var maxSystemTileMap = GetTileMapRectangle(
                SystemInformation.PrimaryMonitorMaximizedWindowSize);

            min.Add(minSystemTileMap.Size);
            max.Add(maxSystemTileMap.Size);

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
                window.Width = Max(window.Width, size.Width);
                window.Height = Max(window.Height, size.Height);
            }

            // Restrict upper bounds
            foreach (var size in max)
            {
                if (size.Width > 0)
                {
                    window.Width = Min(window.Width, size.Width);
                }

                if (size.Height > 0)
                {
                    window.Height = Min(window.Height, size.Height);
                }
            }

            return window;
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            if (ResizeMode == ResizeMode.None)
            {
                ResizeMode = ResizeMode.FormResize;
            }

            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            if (ResizeMode == ResizeMode.FormResize)
            {
                ResizeMode = ResizeMode.None;
            }
        }

        protected override void OnAdjustWindowBounds(
            RectangleEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            e.Rectangle = RectangleFromTileMap(e.Rectangle);
        }

        protected override void OnAdjustWindowSize(
            RectangleEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            e.Size = SizeFromTileMap(e.Size);
        }

        private Rectangle RectangleFromTileMap(Rectangle window)
        {
            if (IsEmptyTileMap)
            {
                return window;
            }

            var tilemap = GetTileMapRectangle(window);
            tilemap.Size = GetBoundTileSize(tilemap.Size);

            // Set new tile size
            if (ResizeMode != ResizeMode.TileMapCellResize)
            {
                TileMapControl.ViewSize = new Size(
                    tilemap.Width / TileMapControl.CellWidth,
                    tilemap.Height / TileMapControl.CellHeight);
            }

            // Adjust window size to bind to tilemap.
            return InflateRectangle(tilemap, MainTileMapPadding);
        }

        private Size SizeFromTileMap(Size window)
        {
            if (IsEmptyTileMap)
            {
                return window;
            }

            var tilemap = GetTileMapRectangle(window).Size;
            tilemap = GetBoundTileSize(tilemap);

            // Set new tile size
            if (ResizeMode == ResizeMode.TileMapCellResize)
            {
                TileMapControl.ViewSize = new Size(
                    tilemap.Width / TileMapControl.CellWidth,
                    tilemap.Height / TileMapControl.CellHeight);
            }

            // Return inflated window size.
            return InflateSize(tilemap, MainTileMapPadding);
        }
    }
}
