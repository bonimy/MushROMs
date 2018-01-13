// <copyright file="CheckerPattern.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Drawing;
using Helper;

namespace Controls
{
    public struct CheckerPattern
    {
        public Color Color1
        {
            get;
        }

        public Color Color2
        {
            get;
        }

        public Size Size
        {
            get;
        }

        public bool IsEmpty
        {
            get
            {
                return Size.IsEmpty;
            }
        }

        public CheckerPattern(Color color1, Color color2, Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(size),
                    SR.ErrorUpperBoundExclusive(nameof(size), size, Size.Empty));
            }

            Color1 = color1;
            Color2 = color2;
            Size = size;
        }
    }
}
