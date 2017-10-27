// <copyright file="CheckerPattern.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Drawing;
using Helper;

namespace MushROMs.Controls
{
    public class CheckerPattern
    {
        public Color Color1
        {
            get;
            set;
        }

        public Color Color2
        {
            get;
            set;
        }

        private Size _size;

        public Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (value.Width <= 0 || value.Height <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        SR.ErrorUpperBoundExclusive(nameof(value), value, Size.Empty));
                }

                _size = value;
            }
        }

        public CheckerPattern(Color color1, Color color2, Size size)
        {
            Color1 = color1;
            Color2 = color2;
            Size = size;
        }
    }
}
