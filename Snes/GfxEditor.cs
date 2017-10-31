// <copyright file="GfxEditor.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using Helper;

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
            var size = GfxTile.DataSizePerTile(format);

            for (var i = selection.Count; --i >= 0;)
            {
                var tile = selection[i];
                var address = selection.StartAddress + tile * size;

                tiles[i] = new GfxTile(Data, address, format);
            }

            return new Gfx(tiles);
        }
    }
}
