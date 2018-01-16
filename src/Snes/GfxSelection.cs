// <copyright file="GfxSelection.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using MushROMs;

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

        public int BytesPerTile
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
                return StartAddress + (Selection[index] * BytesPerTile);
            }
        }

        public GfxSelection(
            ISelection<int> selection,
            int startAddress,
            GraphicsFormat graphicsFormat)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (!Enum.IsDefined(typeof(GraphicsFormat), graphicsFormat))
            {
                throw new InvalidEnumArgumentException(
                    nameof(graphicsFormat),
                    (int)graphicsFormat,
                    typeof(GraphicsFormat));
            }

            Selection = selection.Copy();
            StartAddress = startAddress;
            GraphicsFormat = graphicsFormat;
            BytesPerTile = GfxTile.BytesPerTile(GraphicsFormat);
        }

        public GfxSelection Copy()
        {
            return Move(StartAddress);
        }

        public GfxSelection Move(int startAddress)
        {
            return new GfxSelection(Selection, startAddress, GraphicsFormat);
        }

        IDataSelection IDataSelection.Copy()
        {
            return Copy();
        }

        IGfxSelection IGfxSelection.Move(int startAddress)
        {
            return Move(startAddress);
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
