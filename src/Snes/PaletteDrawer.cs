// <copyright file="PaletteDrawer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using System.Threading.Tasks;
    using Helper.PixelFormat;
    using MushROMs;

    public class PaletteDrawer
    {
        public PaletteDrawer(Palette palette)
        {
            Palette = palette ??
                throw new ArgumentNullException(nameof(palette));
        }

        public Palette Palette
        {
            get;
        }

        private unsafe Color32BppArgb* Scan0
        {
            get;
            set;
        }

        private int StartAddress
        {
            get;
            set;
        }

        private int StartIndex
        {
            get;
            set;
        }

        private int Offset
        {
            get;
            set;
        }

        private int ViewWidth
        {
            get;
            set;
        }

        private int ViewHeight
        {
            get;
            set;
        }

        private int ZoomWidth
        {
            get;
            set;
        }

        private int ZoomHeight
        {
            get;
            set;
        }

        private int Width
        {
            get;
            set;
        }

        private int Height
        {
            get;
            set;
        }

        private ISelection1D Selection
        {
            get;
            set;
        }

        public void DrawPalette(
            IntPtr scan0,
            int length,
            int startAddress,
            int viewWidth,
            int viewHeight,
            int zoomWidth,
            int zoomHeight,
            ISelection1D selection)
        {
            if (scan0 == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(scan0));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            unsafe
            {
                Scan0 = (Color32BppArgb*)scan0;
            }

            Selection = selection;
            ViewWidth = viewWidth;
            ViewHeight = viewHeight;
            ZoomWidth = zoomWidth;
            ZoomHeight = zoomHeight;
            Width = viewWidth * zoomWidth;
            Height = viewHeight * zoomHeight;

            var area = Width * Height;
            if (area * Color32BppArgb.SizeOf > length)
            {
                throw new ArgumentException();
            }

            StartAddress = startAddress;
            StartIndex = StartAddress / Color15BppBgr.SizeOf;
            Offset = StartAddress % Color15BppBgr.SizeOf;

            var span = GetSpan();
            Parallel.For(0, span, DrawSquare);

            int GetSpan()
            {
                var usableDataSize = Palette.Count - Offset;
                var usableColorSize =
                    usableDataSize / Color15BppBgr.SizeOf;

                var viewTileSize = viewWidth * viewHeight;
                var gridSize = usableColorSize - StartIndex;

                return Math.Min(viewTileSize, gridSize);
            }
        }

        private unsafe void DrawSquare(int index)
        {
            var address = StartAddress + (index * Color15BppBgr.SizeOf);
            var tile = address / Color15BppBgr.SizeOf;

            Color32BppArgb color = new Color15BppBgr(
                Palette[address],
                Palette[address + 1]);

            /*

            // Darken regions that are not in the selection
            if (Selection != null && !Selection.Contains(tile))
            {
                color.Red >>= darkShift;
                color.Green >>= darkShift;
                color.Blue >>= darkShift;
            }
            */

            // Get destination pointer address.
            var x = index % ViewWidth;
            var y = index / ViewWidth;
            var dest = Scan0 +
                (x * ZoomWidth) +
                (y * Width * ZoomHeight);

            // Draw the tile.
            DrawSquare(dest, color);
        }

        private unsafe void DrawSquare(
            Color32BppArgb* dest,
            Color32BppArgb color)
        {
            color.Alpha = Byte.MaxValue;
            for (var h = ZoomHeight; --h >= 0; dest += Width)
            {
                DrawLine();
            }

            void DrawLine()
            {
                for (var w = ZoomWidth; --w >= 0;)
                {
                    dest[w] = color;
                }
            }
        }
    }
}
