// <copyright file="PaletteData.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using Helper;
using Helper.PixelFormats;

namespace Snes
{
    public delegate Color15BppBgr PaletteColorMethod(Color15BppBgr color);

    public class PaletteData : IReadOnlyList<Color15BppBgr>
    {
        private Color15BppBgr[] Colors
        {
            get;
        }

        public IDataSelection Selection
        {
            get;
        }

        public int Count
        {
            get
            {
                return Colors.Length;
            }
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

        internal PaletteData(Color15BppBgr[] colors, IDataSelection selection)
        {
            Colors = colors ??
                throw new ArgumentNullException(nameof(colors));

            Selection = selection ??
                throw new ArgumentNullException(nameof(selection));
        }

        public void Clear()
        {
            Clear(Color15BppBgr.Empty);
        }

        public void Clear(Color15BppBgr color)
        {
            AlterTiles(x => color);
        }

        public void Invert()
        {
            AlterTiles(x => x ^ 0x7FFF);
        }

        public void Blend(BlendMode blendMode, ColorF bottom)
        {
            AlterTiles(blend);

            Color15BppBgr blend(Color15BppBgr color)
            {
                var colorF = (ColorF)color;
                var result = colorF.BlendWith(bottom, blendMode);
                return (Color15BppBgr)result;
            }
        }

        public void AlterTiles(PaletteColorMethod method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            for (var i = Count; --i >= 0;)
            {
                this[i] = method(this[i]);
            }
        }

        public IEnumerator<Color15BppBgr> GetEnumerator()
        {
            return Colors.GetEnumerator() as IEnumerator<Color15BppBgr>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
