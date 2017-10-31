using System;
using System.Collections.Generic;
using System.Text;
using Helper;

namespace Snes
{
    public class GfxEditor
    {
        /// <summary>
        /// The raw, unformatted GFX data.
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
        }

        private interface IGfxSelection
        {
            int StartIndex
            {
                get;
            }

            GraphicsFormat GraphicsFormat
            {
                get;
            }
        }
    }
}
