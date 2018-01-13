// <copyright file="GfxSelection.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

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

        public ISelection<int> Selection
        {
            get;
        }

        public GraphicsFormat GraphicsFormat
        {
            get;
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

        public GfxSelection(ISelection<int> selection, int startAddress, GraphicsFormat graphicsFormat)
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
