// <copyright file="FormControlTileMapHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MushROMs.TileMaps;

namespace Controls
{
    public abstract class FormControlTileMapHelper
    {
        public TileMap TileMap
        {
            get;
        }

        public DesignForm Form
        {
            get;
        }

        public DesignControl Control
        {
            get;
        }

        public ScrollBar VScrollBar
        {
            get;
        }

        public ScrollBar HScrollBar
        {
            get;
        }

        private Padding FormControlPadding
        {
            get;
            set;
        }

        private ResizeMode ResizeOrigin
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

        protected FormControlTileMapHelper(
            TileMap tileMap,
            DesignForm form,
            DesignControl control,
            ScrollBar vScrollBar,
            ScrollBar hScrollBar)
        {
            TileMap = tileMap ??
                throw new ArgumentNullException(nameof(tileMap));

            Form = form ??
                throw new ArgumentNullException(nameof(form));

            Control = control ??
                throw new ArgumentNullException(nameof(control));

            // These can be null.
            if (vScrollBar != null)
            {
                VScrollBar = vScrollBar;
                VScrollBar.Scroll += VScrollBar_Scroll;
                VScrollBar.ValueChanged += VScrollBar_ValueChanged;
            }

            if (hScrollBar != null)
            {
                HScrollBar = hScrollBar;
                HScrollBar.Scroll += HScrollBar_Scroll;
                HScrollBar.ValueChanged += HScrollBar_ValueChanged;
            }

            TileMap.TileSizeChanged += TileMap_CellSizeChanged;
            TileMap.ZoomSizeChanged += TileMap_CellSizeChanged;
            TileMap.ViewSizeChanged += TileMap_ViewSizeChanged;
            TileMap.GridSizeChanged += TileMap_GridSizeChanged;
            TileMap.ZeroTileChanged += TileMap_ZeroTileChanged;

            Form.AdjustWindowSize += Form_AdjustWindowSize;
            Form.AdjustWindowBounds += Form_AdjustWindowBounds;
            Form.ResizeBegin += Form_ResizeBegin;
            Form.ResizeEnd += Form_ResizeEnd;

            Control.ClientSizeChanged += Control_ClientSizeChanged;

            ResetFormControlPadding();
        }

        private void ResetFormControlPadding()
        {
            FormControlPadding = GetFormControlPadding();

            Padding GetFormControlPadding()
            {
                var window = WinApiMethods.GetWindowRectangle(Form);
                var child = WinApiMethods.GetWindowRectangle(Control);
                var client = WinApiMethods.DeflateRectangle(
                    child,
                    Control.BorderPadding);

                return WinApiMethods.GetPadding(window, client);
            }
        }

        private void SetFormSizeFromControl()
        {
            // If we're already doing a control resize, don't do again.
            if (ResizeOrigin == ResizeMode.Control)
            {
                return;
            }

            // Set the resize origin as the control.
            if (ResizeOrigin == ResizeMode.None)
            {
                ResizeOrigin = ResizeMode.Control;
            }

            // Get the new window size by adding the new control size to the padding of the original form size and control size.
            var window = WinApiMethods.InflateSize(
                Control.ClientSize,
                FormControlPadding);

            var tileMap = SizeFromTileMap(window);
            Form.Size = tileMap;

            // Reset the resize origin once we've moved out of it.
            if (ResizeOrigin == ResizeMode.Control)
            {
                ResizeOrigin = ResizeMode.None;
            }
        }

        private Size SizeFromTileMap(Size window)
        {
            var tilemap = GetTileMapSize(window);
            tilemap = GetBoundTileSize(tilemap);

            // Set new tile size
            if (ResizeOrigin == ResizeMode.TileMap)
            {
                TileMap.ViewSize = new Size(
                    tilemap.Width / TileMap.CellWidth,
                    tilemap.Height / TileMap.CellHeight);
            }

            // Return inflated window size.
            return WinApiMethods.InflateSize(tilemap, FormControlPadding);
        }

        private Size GetBoundTileSize(Size window)
        {
            var cellW = TileMap.CellWidth;
            var cellH = TileMap.CellHeight;

            // Defines the possible minimum and maximum tile sizes.
            var min = new List<Size>();
            var max = new List<Size>();

            // Min/Max tile size according to the form min/max size.
            min.Add(GetTileMapSize(Form.MinimumSize));
            max.Add(GetTileMapSize(Form.MaximumSize));

            // Min/Max tile size according to system-defined min/max size.
            min.Add(GetTileMapSize(SystemInformation.MinimumWindowSize));
            max.Add(GetTileMapSize(SystemInformation.PrimaryMonitorMaximizedWindowSize));

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

        private Size GetTileMapSize(Size window)
        {
            var windowRectangle = new Rectangle(Point.Empty, window);
            var tileMapRectangle = GetTileMapRectangle(windowRectangle);
            return tileMapRectangle.Size;
        }

        private Rectangle GetTileMapRectangle(Rectangle window)
        {
            // Remove the control padding from rectangle.
            var tilemap = WinApiMethods.DeflateRectangle(
                window,
                FormControlPadding);

            var client = WinApiMethods.DeflateRectangle(
                WinApiMethods.GetWindowRectangle(Control),
                Control.BorderPadding);

            // Gets the residual width that is not included in the tilemap size.
            var residualWidth = tilemap.Width % TileMap.CellWidth;
            var residualHeight = tilemap.Height % TileMap.CellHeight;

            // Get the current dimensions of the window
            var parent = WinApiMethods.GetWindowRectangle(Form);

            // Remove residual area.
            tilemap.Width -= residualWidth;
            tilemap.Height -= residualHeight;

            // Left or top adjust the client if sizing on those borders.
            if (window.Left != parent.Left && window.Right == parent.Right)
            {
                if (tilemap.Width >= TileMap.CellWidth)
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
                if (tilemap.Height >= TileMap.CellHeight)
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
                tilemap.Width = TileMap.CellWidth;
            }

            if (tilemap.Height <= 0)
            {
                tilemap.Height = TileMap.CellHeight;
            }

            return tilemap;
        }

        private void Control_ClientSizeChanged(object sender, EventArgs e)
        {
            SetFormSizeFromControl();
        }

        private void Form_ResizeEnd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form_ResizeBegin(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form_AdjustWindowSize(object sender, RectangleEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form_AdjustWindowBounds(object sender, RectangleEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TileMap_ZeroTileChanged(object sender, EventArgs e)
        {
            AdjustScrollBarPositions();
        }

        private void TileMap_GridSizeChanged(object sender, EventArgs e)
        {
            ResetScrollBars();
        }

        private void TileMap_ViewSizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
            ResetScrollBars();
        }

        private void TileMap_CellSizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
        }

        private void SetClientSizeFromTileMap()
        {
            Control.ClientSize = TileMap.Size;
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

        private void HScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapHorizontal(HScrollBar.Value);
            Control.Invalidate();
        }

        private void HScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            ScrollTileMapHorizontal(e.NewValue);
            Control.Invalidate();
        }

        private void VScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapVertical(VScrollBar.Value);
            Control.Invalidate();
        }

        private void VScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            ScrollTileMapVertical(e.NewValue);
            Control.Invalidate();
        }

        private enum ResizeMode
        {
            None,
            TileMap,
            Control,
            Form
        }
    }
}
