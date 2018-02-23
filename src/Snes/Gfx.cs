// <copyright file="Gfx.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading.Tasks;
    using Helper.PixelFormat;

    public class Gfx : IReadOnlyList<byte>
    {
        public Gfx(int size, GraphicsFormat graphicsFormat)
            : this(new byte[size * GfxTile.BytesPerTile(graphicsFormat)])
        {
        }

        public Gfx(byte[] data)
            : this(
                  data ?? throw new ArgumentNullException(nameof(data)),
                  0,
                  data.Length)
        {
        }

        public Gfx(byte[] data, int startIndex, int size)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var result = new byte[size];
            Array.Copy(data, startIndex, result, 0, size);
            Data = result;
        }

        public int Count
        {
            get
            {
                return Data.Length;
            }
        }

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

        public GfxData CreateGfxFromSelection(
            IGfxSelection selection)
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
                var inRange = address + size <= Count && address >= 0;
                if (!inRange)
                {
                    continue;
                }

                var tile = gfxData[i].ToFormattedData(format);
                var length = Math.Min(tile.Length, Count - address);
                Array.Copy(tile, 0, Data, address, length);
            }
        }

        public void DrawPixelData(
            IntPtr scan0,
            int length,
            int startAddress,
            GraphicsFormat graphicsFormat,
            Size view,
            Size zoom,
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

            if ((uint)startAddress > (uint)Count)
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

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (colors is null)
            {
                throw new ArgumentNullException(nameof(colors));
            }

            var width = view.Width * zoom.Width;
            var height = view.Height * zoom.Height;
            var window = new Size(width, height);
            if (width * height * Color32BppArgb.SizeOf > length)
            {
                throw new ArgumentException();
            }

            var numColors = GfxTile.ColorsPerPixel(graphicsFormat);
            if (numColors >= colors.Count)
            {
                throw new ArgumentException();
            }

            var cellWidth = zoom.Width * view.Width;
            var cellHeight = zoom.Height * view.Height;
            var plane = (window.Width * zoom.Height) - cellWidth;
            var dot = (window.Width * zoom.Height) - zoom.Width;

            var unitSize = GfxTile.BytesPerTile(graphicsFormat);
            var area = view.Width * view.Height;

            var totalLength = Math.Min(unitSize * area, Count - startAddress);
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
                    ((i % view.Width) * cellWidth) +
                    ((i / view.Width) * cellHeight);

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

        public void CopyTo(byte[] array, int arrayIndex)
        {
            Data.CopyTo(array, arrayIndex);
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
