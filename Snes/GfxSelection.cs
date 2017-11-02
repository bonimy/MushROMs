using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Helper;

namespace Snes
{
    public class GfxSelection : IGfxSelection
    {
        public int StartAddress
        {
            get;
        }

        private Selection1D Selection
        {
            get;
        }

        public GraphicsFormat GraphicsFormat
        {
            get;
            private set;
        }

        public int Count
        {
            get
            {
                return Selection.Count;
            }
        }

        public int this[int index]
        {
            get
            {
                return Selection[index];
            }
        }

        public GfxSelection(Selection1D selection, int startAddress, GraphicsFormat graphicsFormat)
        {
            if (!Enum.IsDefined(typeof(GraphicsFormat), graphicsFormat))
            {
                throw new InvalidEnumArgumentException(
                    nameof(graphicsFormat),
                    (int)graphicsFormat,
                    typeof(GraphicsFormat));
            }

            Selection = selection ?? throw new ArgumentNullException(nameof(selection));
            StartAddress = startAddress;
            GraphicsFormat = graphicsFormat;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return Selection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
