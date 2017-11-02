// <copyright file="GfxEditor.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                var address = selection.StartAddress + index * size;

                tiles[i] = new GfxTile(Data, address, format);
            }

            return new Gfx(tiles);
        }

        public void WriteGfxData(Gfx gfx, IGfxSelection selection)
        {
            var count = Math.Min(gfx.Count, selection.Count);

            for (var i = count; --i >= 0;)
            {
                var tile = selection[i];
                var address = selection.StartAddress + tile * size;
            }
        }

        public void DrawPixelData(
            IntPtr scan0,
            int length,
            int startAddress,
            GraphicsFormat graphicsFormat,
            Range2D view,
            Range2D zoom,
            Selection1D selection,
            IList<Color32BppArgb> colors,
            int colorStartIndex)
        {
            if (scan0 == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(scan0));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if ((uint)startAddress >= (uint)Data.Length)
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

            if (colorStartIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(colorStartIndex));
            }

            var window = view * zoom;
            if (window.Area * Color32BppArgb.SizeOf > length)
            {
                throw new ArgumentException();
            }

            var unitSize = GfxTile.BytesPerTile(graphicsFormat);
            var area = view.Area;

            var totalLength = Math.Min(unitSize * area, Data.Length - startAddress);
            var totalTiles = totalLength / unitSize;

            var darkScale = selection is GateSelection1D ? 2 : 1;
        }
    }
}
