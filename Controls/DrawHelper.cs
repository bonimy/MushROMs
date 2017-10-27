// <copyright file="DrawHelper.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System.Drawing;
using Helper.PixelFormats;

namespace Controls
{
    public static class DrawHelper
    {
        public static Color ColorFromColor32BppArgb(Color32BppArgb color)
        {
            return Color.FromArgb(
                color.Alpha,
                color.Red,
                color.Green,
                color.Blue);
        }

        public static Color32BppArgb Color32BppArgbFromColor(Color color)
        {
            return new Color32BppArgb(color.A, color.R, color.G, color.B);
        }
    }
}
