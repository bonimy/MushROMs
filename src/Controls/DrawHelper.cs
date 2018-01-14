// <copyright file="DrawHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System.Drawing;
using Helper;
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

        public static Point ToPoint(this Position2D position)
        {
            return new Point(position.X, position.Y);
        }

        public static Position2D ToPosition2D(this Point point)
        {
            return new Position2D(point.X, point.Y);
        }

        public static Range2D ToRange2D(this Size size)
        {
            return new Range2D(size.Width, size.Height);
        }

        public static Size ToSize(this Range2D range)
        {
            return new Size(range.Width, range.Height);
        }
    }
}
