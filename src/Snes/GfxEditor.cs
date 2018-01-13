// <copyright file="GfxEditor.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Helper;
using Helper.PixelFormats;

namespace Snes
{
    public class GfxEditor
    {
        /// <summary>
        /// Gets or sets the raw, unformatted GFX data.
        /// </summary>
        /// <remarks>
        /// This array contains just the data with no information about formatting. It can be converted to any format (1BPP, 2BPP NES, 4BPP SNES, 8BPP Mode7, etc.). The idea is that whenever we need to get a selection of data (e.g. the range of tiles to draw in the GUI, or a selection of tiles to -for example- rotate 90 degrees), we pass the selection info, and return an array of formatted GFX data.
        /// </remarks>
        private byte[] Data
        {
            get;
            set;
        }

        private UndoFactory UndoFactory
        {
            get;
            set;
        }

        public GfxEditor(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));

            UndoFactory = new UndoFactory();
        }

        public Gfx CreateGfxFromSelection(IGfxSelection selection)
        {
            if (selection == null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            var tiles = new GfxTile[selection.Count];
            var format = selection.GraphicsFormat;
            var size = GfxTile.BytesPerTile(format);

            for (var i = selection.Count; --i >= 0;)
            {
                var index = selection[i];
                var address = selection.StartAddress + (index * size);

                tiles[i] = new GfxTile(Data, address, format);
            }

            return new Gfx(tiles);
        }

        public void WriteGfxData(Gfx gfx, IGfxSelection selection)
        {
            var size = GfxTile.BytesPerTile(selection.GraphicsFormat);

            for (var i = selection.Count; --i >= 0;)
            {
                var index = selection[i];
                if (index >= gfx.Count || index < 0)
                {
                    continue;
                }

                var address = selection.StartAddress + (index * size);
                if (address >= Data.Length || address < 0)
                {
                    continue;
                }

                var tile = gfx[index].ToFormattedData(selection.GraphicsFormat);

                Array.Copy(tile, 0, Data, address, Math.Min(tile.Length, Data.Length - address));
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

            if (selection == null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (colors == null)
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

            var darkScale = selection.Selection is GateSelection1D ? 2 : 1;

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
    }
}
