// <copyright file="WinApiRectangle.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Helper;

namespace Controls
{
    /// <summary>
    /// A rectangle structure whose data layout is consistent with the RECTANGLE struct used in the Windows API.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WinAPIRectangle : IEquatable<WinAPIRectangle>
    {
        public static readonly WinAPIRectangle Empty = new WinAPIRectangle();

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

        public WinAPIRectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public bool Equals(WinAPIRectangle obj)
        {
            return
                Left.Equals(obj.Left) &&
                Top.Equals(obj.Top) &&
                Right.Equals(obj.Right) &&
                Bottom.Equals(obj.Bottom);
        }

        public override bool Equals(object obj)
        {
            if (obj is WinAPIRectangle value)
            {
                return Equals(value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(nameof(Left));
            sb.Append(": ");
            sb.Append(SR.GetString(Left));
            sb.Append(", ");
            sb.Append(nameof(Top));
            sb.Append(": ");
            sb.Append(SR.GetString(Top));
            sb.Append(", ");
            sb.Append(nameof(Right));
            sb.Append(": ");
            sb.Append(SR.GetString(Right));
            sb.Append(", ");
            sb.Append(nameof(Bottom));
            sb.Append(": ");
            sb.Append(SR.GetString(Bottom));
            sb.Append('}');
            return sb.ToString();
        }

        public static implicit operator WinAPIRectangle(Rectangle rectangle)
        {
            return new WinAPIRectangle(
                rectangle.Left,
                rectangle.Top,
                rectangle.Right,
                rectangle.Bottom);
        }

        public static implicit operator Rectangle(WinAPIRectangle rectangle)
        {
            return Rectangle.FromLTRB(
                rectangle.Left,
                rectangle.Top,
                rectangle.Right,
                rectangle.Bottom);
        }

        public static bool operator ==(
            WinAPIRectangle left,
            WinAPIRectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            WinAPIRectangle left,
            WinAPIRectangle right)
        {
            return !(left == right);
        }
    }
}
