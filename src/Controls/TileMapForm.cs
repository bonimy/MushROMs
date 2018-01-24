// <copyright file="TileMapForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Helper;
using MushROMs;

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
            get { return new Size(1, 1); }
        }

        protected virtual Size MaximumTileSize
        {
            get { return Size.Empty; }
        }

        public void AnchorFormToTileMap()
        {
            if (TileMapControl != null)
            {
                var form = WinAPIMethods.GetWindowRectangle(this);
                var child = WinAPIMethods.GetWindowRectangle(TileMapControl);
                var client = WinAPIMethods.DeflateRectangle(child, TileMapControl.BorderPadding);
                MainTileMapPadding = WinAPIMethods.GetPadding(form, client);
            }
            else
            {
                MainTileMapPadding = Padding.Empty;
            }
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

            var client = ClientSize;
            var window = WinAPIMethods.InflateSize(
                TileMapControl.ClientSize, MainTileMapPadding);
            window = AdjustSize(window);
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

            if (TileMapControl.CellSize == Range2D.Empty)
            {
                return window;
            }

            // Remove the control padding from rectangle.
            var tilemap = WinAPIMethods.DeflateRectangle(
                window,
                MainTileMapPadding);

            var client = WinAPIMethods.DeflateRectangle(
                WinAPIMethods.GetWindowRectangle(TileMapControl),
                TileMapControl.BorderPadding);

            // Gets the residual width that is not included in the tilemap size.
            var residualWidth = tilemap.Width % TileMapControl.CellWidth;
            var residualHeight = tilemap.Height % TileMapControl.CellHeight;

            // Get the current dimensions of the window
            var parent = WinAPIMethods.GetWindowRectangle(this);

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
            /*
            if (TileMapControl.TileMapResizeMode == TileMapResizeMode.None)
            {
                TileMapControl.TileMapResizeMode = TileMapResizeMode.FormResize;
            }
            */
            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            /*
            if (TileMapControl.TileMapResizeMode == TileMapResizeMode.FormResize)
            {
                TileMapControl.TileMapResizeMode = TileMapResizeMode.None;
            }
            */
        }

        protected override Rectangle AdjustSizingRectangle(Rectangle window)
        {
            if (TileMapControl is null ||
                MainTileMapPadding == Padding.Empty ||
                TileMapControl.CellSize == Range2D.Empty)
            {
                return base.AdjustSizingRectangle(window);
            }

            var tilemap = GetTileMapRectangle(window);
            tilemap.Size = GetBoundTileSize(tilemap.Size);

            // Set new tile size
            if (TileMapControl.TileMapResizeMode != TileMapResizeMode.TileMapCellResize)
            {
                TileMapControl.ViewSize = new Range2D(
                    tilemap.Width / TileMapControl.CellWidth,
                    tilemap.Height / TileMapControl.CellHeight);
            }

            // Adjust window size to bind to tilemap.
            return WinAPIMethods.InflateRectangle(tilemap, MainTileMapPadding);
        }

        protected override Size AdjustSize(Size window)
        {
            if (TileMapControl is null ||
                MainTileMapPadding == Padding.Empty ||
                TileMapControl.CellSize == Range2D.Empty)
            {
                return base.AdjustSize(window);
            }

            var tilemap = GetTileMapRectangle(window).Size;
            tilemap = GetBoundTileSize(tilemap);

            // Set new tile size
            if (TileMapControl.TileMapResizeMode != TileMapResizeMode.TileMapCellResize)
            {
                TileMapControl.ViewSize = new Range2D(
                    tilemap.Width / TileMapControl.CellWidth,
                    tilemap.Height / TileMapControl.CellHeight);
            }

            // Return inflated window size.
            return WinAPIMethods.InflateSize(tilemap, MainTileMapPadding);
        }
    }
}
