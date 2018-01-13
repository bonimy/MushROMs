// <copyright file="Range2D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Diagnostics;

namespace Helper
{
    [DebuggerDisplay("H = {Horizontal}, V = {Vertical}")]
    public struct Range2D
    {
        public static readonly Range2D Empty = new Range2D();

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int Area
        {
            get
            {
                return Width * Height;
            }
        }

        public bool IsInFirstQuadrantInclusive
        {
            get
            {
                return Width >= 0 && Height >= 0;
            }
        }

        public bool IsInFirstQuadrantExclusive
        {
            get
            {
                return Width > 0 && Height > 0;
            }
        }

        public Range2D(int value) : this(value, value)
        {
        }

        public Range2D(int horizontal, int vertical)
        {
            Width = horizontal;
            Height = vertical;
        }

        public bool Contains(Position2D position)
        {
            return
                position.X >= 0 && position.X < Width &&
                position.Y >= 0 && position.Y < Height;
        }

        public Range2D Add(Range2D value)
        {
            return this + value;
        }

        public Range2D Subtract(Range2D value)
        {
            return this - value;
        }

        public Range2D Negate()
        {
            return -this;
        }

        public Range2D Multiply(Range2D value)
        {
            return this * value;
        }

        public Range2D Divide(Range2D value)
        {
            return this / value;
        }

        public static Range2D TopLeft(Range2D value1, Range2D value2)
        {
            return new Range2D(
                Math.Min(value1.Width, value2.Width),
                Math.Min(value1.Height, value2.Height));
        }

        public static Range2D TopRight(Range2D value1, Range2D value2)
        {
            return new Range2D(
                Math.Max(value1.Width, value2.Width),
                Math.Min(value1.Height, value2.Height));
        }

        public static Range2D BottomLeft(Range2D value1, Range2D value2)
        {
            return new Range2D(
                Math.Min(value1.Width, value2.Width),
                Math.Max(value1.Height, value2.Height));
        }

        public static Range2D BottomRight(Range2D value1, Range2D value2)
        {
            return new Range2D(
                Math.Max(value1.Width, value2.Width),
                Math.Max(value1.Height, value2.Height));
        }

        public override bool Equals(object obj)
        {
            if (obj is Range2D value)
            {
                return value == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Width ^ Height;
        }

        public override string ToString()
        {
            return SR.GetString("H={0}, V={1}", Width, Height);
        }

        public static bool operator ==(Range2D left, Range2D right)
        {
            return
                left.Width == right.Width &&
                left.Height == right.Height;
        }

        public static bool operator !=(Range2D left, Range2D right)
        {
            return !(left == right);
        }

        public static Range2D operator +(Range2D left, Range2D right)
        {
            return new Range2D(
                left.Width + right.Width,
                left.Height + right.Height);
        }

        public static Range2D operator -(Range2D left, Range2D right)
        {
            return new Range2D(
                left.Width - right.Width,
                left.Height - right.Height);
        }

        public static Range2D operator -(Range2D right)
        {
            return new Range2D(-right.Width, -right.Height);
        }

        public static Range2D operator *(Range2D left, Range2D right)
        {
            return new Range2D(
                left.Width * right.Width,
                left.Height * right.Height);
        }

        public static Range2D operator /(Range2D left, Range2D right)
        {
            return new Range2D(
                left.Width / right.Width,
                left.Height / right.Height);
        }

        public static implicit operator Range2D(int range)
        {
            return new Range2D(range, range);
        }
    }
}
