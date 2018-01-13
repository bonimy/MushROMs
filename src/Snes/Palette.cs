// <copyright file="Palette.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Threading.Tasks;
using Helper;
using Helper.PixelFormats;

namespace Snes
{
    public delegate Color32BppArgb PaletteColorMethod(Color32BppArgb color);

    public abstract class Palette
    {
        public abstract Color32BppArgb this[int index]
        {
            get;
            set;
        }

        public abstract int Count
        {
            get;
        }

        public void Clear(Selection1D selection)
        {
            Clear(selection, Color32BppArgb.Empty);
        }

        public void Clear(Selection1D selection, Color32BppArgb color)
        {
            AlterTiles(selection, x => color);
        }

        public void Invert(Selection1D selection)
        {
            AlterTiles(selection, x => x ^ 0xFFFFFF);
        }

        public void Blend(Selection1D selection, BlendMode blendMode, ColorF bottom)
        {
            AlterTiles(selection, blend);

            Color32BppArgb blend(Color32BppArgb color)
            {
                var colorF = (ColorF)color;
                var result = colorF.BlendWith(bottom, blendMode);
                return result;
            }
        }

        public void AlterTiles(Selection1D selection, PaletteColorMethod method)
        {
            if (selection == null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            foreach (var i in selection)
            {
                this[i] = method(this[i]);
            }
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

            if (selection == null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (zero + span > Count)
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

            unsafe
            {
                // Cast the scan0 pointer as a fundamental pixel type.
                var pixels = (Color32BppArgb*)scan0;

                Parallel.For(
                    0,
                    Count,
                    i =>
                {
                    var index = i + zero;

                    var color = this[index];

                    color.Alpha = Byte.MaxValue;

                    // Darken regions that are not in the selection
                    if (selection != null && !selection.Contains(i + zero))
                    {
                        color.Red >>= darkShift;
                        color.Green >>= darkShift;
                        color.Blue >>= darkShift;
                    }

                    // Get destination pointer address.
                    var dest = pixels +
                        ((i % view.Width) * zoom.Height) +
                        ((i / view.Width) * zoomRow);

                    // Draw the tile.
                    for (var h = zoom.Height; --h >= 0; dest += window.Width)
                    {
                        for (var w = zoom.Width; --w >= 0;)
                        {
                            dest[w] = color;
                        }
                    }
                });
            }
        }
    }
}
