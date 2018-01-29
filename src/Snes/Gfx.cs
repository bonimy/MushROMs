// <copyright file="Gfx.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Helper;
using Helper.PixelFormats;

namespace Snes
{
    public class Gfx : IReadOnlyList<byte>
    {
        private byte[] Data
        {
            get;
            set;
        }

        public int Count
        {
            get
            {
                return Data.Length;
            }
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

        public Gfx(int size, GraphicsFormat graphicsFormat) :
            this(new byte[size * GfxTile.BytesPerTile(graphicsFormat)])
        {
        }

        public Gfx(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public GfxData CreateGfxFromSelection(IGfxSelection selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            var tiles = new GfxTile[selection.Count];
            var format = selection.GraphicsFormat;
            for (var i = selection.Count; --i >= 0;)
            {
                tiles[i] = new GfxTile(Data, selection[i], format);
            }

            return new GfxData(tiles, selection);
        }

        public void WriteGfxData(GfxData gfxData)
        {
            if (gfxData is null)
            {
                throw new ArgumentNullException(nameof(gfxData));
            }

            var selection = gfxData.Selection;
            var size = gfxData.BytesPerTile;
            var format = gfxData.GraphicsFormat;
            for (var i = selection.Count; --i >= 0;)
            {
                var address = selection[i];
                var inRange = address + size <= Data.Length && address >= 0;
                if (!inRange)
                {
                    continue;
                }

                var tile = gfxData[i].ToFormattedData(format);
                var length = Math.Min(tile.Length, Data.Length - address);
                Array.Copy(tile, 0, Data, address, length);
            }
        }

        public void DrawPixelData(
            IntPtr scan0,
            int length,
            int startAddress,
            GraphicsFormat graphicsFormat,
            Range2D view,
            Range2D zoom,
            IGfxSelection selection,
            IReadOnlyList<Color32BppArgb> colors)
        {
            if (scan0 == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(scan0));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if ((uint)startAddress > (uint)Data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (!Enum.IsDefined(typeof(GraphicsFormat), graphicsFormat))
            {
                throw new InvalidEnumArgumentException(
                    nameof(graphicsFormat),
                    (int)graphicsFormat,
                    typeof(GraphicsFormat));
            }

            if (!view.IsInFirstQuadrantExclusive)
            {
                throw new ArgumentException();
            }

            if (!zoom.IsInFirstQuadrantExclusive)
            {
                throw new ArgumentException();
            }

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (colors is null)
            {
                throw new ArgumentNullException(nameof(colors));
            }

            var window = view * zoom;
            if (window.Area * Color32BppArgb.SizeOf > length)
            {
                throw new ArgumentException();
            }

            var numColors = GfxTile.ColorsPerPixel(graphicsFormat);
            if (numColors >= colors.Count)
            {
                throw new ArgumentException();
            }

            var cell = zoom * view;
            var plane = (window.Width * zoom.Height) - cell.Width;
            var dot = (window.Width * zoom.Height) - zoom.Width;

            var unitSize = GfxTile.BytesPerTile(graphicsFormat);
            var area = view.Area;

            var totalLength = Math.Min(unitSize * area, Data.Length - startAddress);
            var totalTiles = totalLength / unitSize;

            unsafe
            {
                var pixels = (Color32BppArgb*)scan0;

                Parallel.For(
                    0,
                    totalTiles,
                    i =>
                {
                    var tile = new GfxTile(Data, i + startAddress, graphicsFormat);
                    var dots = (byte*)&tile;

                    var dest = pixels +
                    ((i % view.Width) * cell.Width) +
                    ((i / view.Width) * cell.Height);

                    // Lets hope the compiler is smart enough to unroll the outer loops.
                    for (var y = GfxTile.PlanesPerTile; --y >= 0; dest += plane)
                    {
                        for (var x = GfxTile.DotsPerPlane; --x >= 0; dest -= dot, dots++)
                        {
                            var color = colors[dots[0]];

                            for (var j = zoom.Height; --j >= 0; dest += window.Width)
                            {
                                for (var k = zoom.Width; --k >= 0;)
                                {
                                    dest[k] = color;
                                }
                            }
                        }
                    }
                });
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
