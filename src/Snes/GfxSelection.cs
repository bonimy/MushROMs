// <copyright file="GfxSelection.cs" company="Public Domain">
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
    using MushROMs;

    public sealed class GfxSelection : IGfxSelection
    {
        public GfxSelection(
            ISelection1D selection,
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

        public int StartAddress
        {
            get;
        }

        public int StartIndex
        {
            get
            {
                return Selection.StartIndex;
            }
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

        private ISelection1D Selection
        {
            get;
        }

        public int this[int index]
        {
            get
            {
                return StartAddress + (Selection[index] * BytesPerTile);
            }
        }

        public bool Contains(int index)
        {
            return Selection.Contains(index);
        }

        public GfxSelection Copy()
        {
            return Move(StartAddress);
        }

        public GfxSelection Move(int startAddress)
        {
            return new GfxSelection(
                Selection,
                startAddress,
                GraphicsFormat);
        }

        ISelection1D ISelection1D.Copy()
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
