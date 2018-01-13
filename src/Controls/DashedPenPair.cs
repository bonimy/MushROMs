// <copyright file="DashedPenPair.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Helper;

namespace Controls
{
    public struct DashedPenPair
    {
        public Color Color1
        {
            get;
        }

        public Color Color2
        {
            get;
        }

        public int Length1
        {
            get;
        }

        public int Length2
        {
            get;
        }

        public DashedPenPair(Color color1, Color color2, int length1, int length2)
        {
            if (length1 <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(length1),
                    SR.ErrorUpperBoundExclusive(nameof(length1), length1, 0));
            }

            if (length2 <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(length2),
                    SR.ErrorUpperBoundExclusive(nameof(length2), length2, 0));
            }

            Color1 = color1;
            Color2 = color2;

            Length1 = length1;
            Length2 = length2;
        }

        public void SetPenProperties(Pen pen1, Pen pen2)
        {
            if (pen1 != null)
            {
                pen1.Color = Color1;
                pen1.DashStyle = DashStyle.Custom;
                pen1.DashPattern = new float[] { Length1, Length2 };
                pen1.DashOffset = 0;
            }

            if (pen2 != null)
            {
                pen2.Color = Color2;
                pen2.DashStyle = DashStyle.Custom;
                pen2.DashPattern = new float[] { Length2, Length1 };
                pen2.DashOffset = Length1;
            }
        }
    }
}
