// <copyright file="Palette.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helper;
using Helper.PixelFormats;
using MushROMs;

namespace Snes
{
    public class Palette : IReadOnlyList<byte>
    {
        private byte[] Data
        {
            get;
        }

        public byte this[int index]
        {
            get
            {
                return Data[index];
            }

            set
            {
                Data[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return Data.Length;
            }
        }

        public Palette(int size) :
            this(new byte[size * Color15BppBgr.SizeOf])
        {
        }

        public Palette(byte[] data) : this(
            data ?? throw new ArgumentNullException(nameof(data)),
            0,
            data.Length)
        {
        }

        public Palette(byte[] data, int startIndex, int size)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var result = new byte[size];
            Array.Copy(data, startIndex, result, 0, size);
            Data = result;
        }

        public void DrawDataAsTileMap(IntPtr scan0, int length, int zero, int span, Range2D view, Range2D zoom, Selection1D selection)
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

            if (zero + span > Data.Length)
            {
                throw new ArgumentException();
            }

            var window = view * zoom;
            var zoomRow = view.Height * window.Width;

            if (window.Area * Color32BppArgb.SizeOf > length)
            {
                throw new ArgumentException();
            }

            // This determines how much we darken the non-selected region.
            var darkShift = selection is GateSelection1D ? 2 : 1;

            Parallel.For(0, Data.Length, DrawSquare);

            void DrawSquare(int i)
            {
                this.DrawSquare(
                    scan0,
                    i,
                    zero,
                    view.Width,
                    zoom.Width,
                    zoom.Height,
                    zoomRow,
                    darkShift,
                    window.Width,
                    selection);
            }
        }

        private void DrawSquare(
            IntPtr scan0,
            int index,
            int startIndex,
            int viewWidth,
            int zoomWidth,
            int zoomHeight,
            int zoomRow,
            int darkShift,
            int windowWidth,
            Selection1D selection)
        {
            var address = index + startIndex;

            Color32BppArgb color = new Color15BppBgr(
                Data[address],
                Data[address + 1]);

            // Darken regions that are not in the selection
            if (selection != null && !selection.Contains(index + startIndex))
            {
                color.Red >>= darkShift;
                color.Green >>= darkShift;
                color.Blue >>= darkShift;
            }

            // Get destination pointer address.
            var dest = scan0 +
                ((index % viewWidth) * zoomHeight) +
                ((index / viewWidth) * zoomRow);

            // Draw the tile.
            DrawSquare(dest, color, zoomWidth, zoomHeight, windowWidth);
        }

        private static void DrawSquare(
            IntPtr dest,
            Color32BppArgb color,
            int zoomWidth,
            int zoomHeight,
            int windowWidth)
        {
            unsafe
            {
                var pixels = (Color32BppArgb*)dest;
                color.Alpha = Byte.MaxValue;
                for (var h = zoomHeight; --h >= 0; pixels += windowWidth)
                {
                    for (var w = zoomWidth; --w >= 0;)
                    {
                        pixels[w] = color;
                    }
                }
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return Data.GetEnumerator() as IEnumerator<byte>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
