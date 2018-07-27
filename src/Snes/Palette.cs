// <copyright file="Palette.cs" company="Public Domain">
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
    using Helper.PixelFormat;
    using MushROMs;

    public class Palette : IReadOnlyList<byte>
    {
        public Palette(int size)
            : this(new byte[size * Color15BppBgr.SizeOf])
        {
        }

        public Palette(byte[] data)
            : this(
                data ?? throw new ArgumentNullException(nameof(data)),
                0,
                data.Length)
        {
        }

        public Palette(byte[] data, int startIndex, int size)
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
                return Data.Count;
            }
        }

        private IList<byte> Data
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

        public PaletteData CreatePaletteFromSelection(
            ISelection1D selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            var colors = new Color15BppBgr[selection.Count];
            for (var i = selection.Count; --i >= 0;)
            {
                var startIndex = selection[i] * Color15BppBgr.SizeOf;
                colors[i] = new Color15BppBgr(
                    this[startIndex + Color15BppBgr.LowIndex],
                    this[startIndex + Color15BppBgr.HighIndex]);
            }

            return new PaletteData(colors, selection);
        }

        public void WritePaletteData(PaletteData paletteData)
        {
            if (paletteData is null)
            {
                throw new ArgumentNullException(nameof(paletteData));
            }

            var selection = paletteData.Selection;
            for (var i = selection.Count; --i >= 0;)
            {
                var startIndex = selection[i] * Color15BppBgr.SizeOf;
                var lowIndex = startIndex + Color15BppBgr.LowIndex;
                var highIndex = startIndex + Color15BppBgr.HighIndex;

                this[lowIndex] = paletteData[i].Low;
                this[highIndex] = paletteData[i].High;
            }
        }

        public int IndexOf(byte item)
        {
            return Data.IndexOf(item);
        }

        public bool Contains(byte item)
        {
            return Data.Contains(item);
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            Data.CopyTo(array, arrayIndex);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
