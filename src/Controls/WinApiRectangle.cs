// <copyright file="WinApiRectangle.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using static Helper.StringHelper;

    /// <summary>
    /// A rectangle structure whose data layout is consistent with the RECTANGLE struct used in the Windows API.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WinApiRectangle : IEquatable<WinApiRectangle>
    {
        public static readonly WinApiRectangle Empty = default;

        public int Left
        {
            get;
            set;
        }

        public int Top
        {
            get;
            set;
        }

        public int Right
        {
            get;
            set;
        }

        public int Bottom
        {
            get;
            set;
        }

        public int X
        {
            get
            {
                return Left;
            }

            set
            {
                Left = value;
            }
        }

        public int Y
        {
            get
            {
                return Top;
            }

            set
            {
                Top = value;
            }
        }

        public Point Location
        {
            get
            {
                return new Point(X, Y);
            }

            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public int Width
        {
            get
            {
                return Right - Left;
            }

            set
            {
                Right = value + Left;
            }
        }

        public int Height
        {
            get
            {
                return Bottom - Top;
            }

            set
            {
                Bottom = value + Top;
            }
        }

        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }

            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public WinApiRectangle(
            int left,
            int top,
            int right,
            int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public bool Equals(WinApiRectangle other)
        {
            return
                Left.Equals(other.Left) &&
                Top.Equals(other.Top) &&
                Right.Equals(other.Right) &&
                Bottom.Equals(other.Bottom);
        }

        public override bool Equals(object obj)
        {
            if (obj is WinApiRectangle other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            return GetString(
                "{{L:{0},T:{1},R:{2},B:{3}}}",
                Left,
                Top,
                Right,
                Bottom);
        }

        public static implicit operator WinApiRectangle(
            Rectangle rectangle)
        {
            return new WinApiRectangle(
                rectangle.Left,
                rectangle.Top,
                rectangle.Right,
                rectangle.Bottom);
        }

        public static implicit operator Rectangle(
            WinApiRectangle rectangle)
        {
            return Rectangle.FromLTRB(
                rectangle.Left,
                rectangle.Top,
                rectangle.Right,
                rectangle.Bottom);
        }

        public static bool operator ==(
            WinApiRectangle left,
            WinApiRectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            WinApiRectangle left,
            WinApiRectangle right)
        {
            return !(left == right);
        }
    }
}
