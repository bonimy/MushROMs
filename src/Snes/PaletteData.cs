// <copyright file="PaletteData.cs" company="Public Domain">
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
    using Helper;
    using Helper.PixelFormat;
    using MushROMs.TileMaps;

    public class PaletteData : IReadOnlyList<Color15BppBgr>
    {
        internal PaletteData(
            Color15BppBgr[] colors,
            ISelection1D selection)
        {
            Colors = colors ??
                throw new ArgumentNullException(nameof(colors));

            Selection = selection ??
                throw new ArgumentNullException(nameof(selection));
        }

        private PaletteData(PaletteData paletteData)
        {
            if (paletteData is null)
            {
                throw new ArgumentNullException(nameof(paletteData));
            }

            var result = new Color15BppBgr[paletteData.Count];
            paletteData.CopyTo(result, 0);
            Colors = result;

            Selection = paletteData.Selection.Copy();
        }

        public ISelection1D Selection
        {
            get;
        }

        public int Count
        {
            get
            {
                return Colors.Count;
            }
        }

        private IList<Color15BppBgr> Colors
        {
            get;
        }

        public Color15BppBgr this[int index]
        {
            get
            {
                return Colors[index];
            }

            set
            {
                Colors[index] = value;
            }
        }

        public PaletteData Copy()
        {
            return new PaletteData(this);
        }

        public void Empty()
        {
            Empty(Color15BppBgr.Empty);
        }

        public void Empty(Color15BppBgr color)
        {
            AlterColors(x => color);
        }

        public void InvertColors()
        {
            AlterColors(x => (Color15BppBgr)(x ^ 0x7FFF));
        }

        public void Blend(BlendMode blendMode, ColorF bottom)
        {
            AlterColors(blend);

            Color15BppBgr blend(Color15BppBgr color)
            {
                var colorF = (ColorF)color;
                var result = ColorF.Blend(colorF, bottom, blendMode);
                return (Color15BppBgr)result;
            }
        }

        public void Colorize(ColorF amount)
        {
            AlterColors(colorize);

            Color15BppBgr colorize(Color15BppBgr color)
            {
                var result = ColorF.HueBlend(color, amount);
                return (Color15BppBgr)result;
            }
        }

        public void AlterColors(
            Func<Color15BppBgr, Color15BppBgr> alterColor)
        {
            if (alterColor is null)
            {
                throw new ArgumentNullException(nameof(alterColor));
            }

            for (var i = Count; --i >= 0;)
            {
                this[i] = alterColor(this[i]);
            }
        }

        public void CopyTo(Color15BppBgr[] array, int arrayIndex)
        {
            Colors.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Color15BppBgr> GetEnumerator()
        {
            return Colors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
