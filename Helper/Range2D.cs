using System;
using System.Diagnostics;

namespace Helper
{
    [DebuggerDisplay("H = {Horizontal}, V = {Vertical}")]
    public struct Range2D
    {
        public static readonly Range2D Empty = new Range2D();

        public int Horizontal
        {
            get;
            private set;
        }
        public int Vertical
        {
            get;
            private set;
        }

        public int Area => Horizontal * Vertical;
        public Range2D(int value) : this(value, value)
        {
        }
        public Range2D(int horizontal, int vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public bool Contains(Position2D position)
        {
            return position.X >= 0 && position.X < Horizontal &&
                position.Y >= 0 && position.Y < Vertical;
        }

        public Range2D Add(Range2D value)      => this + value;
        public Range2D Subtract(Range2D value) => this - value;
        public Range2D Negate()                => -this;
        public Range2D Multiply(Range2D value) => this * value;
        public Range2D Divide(Range2D value)   => this / value;

        public static Range2D TopLeft(Range2D value1, Range2D value2)
        {
            return new Range2D(Math.Min(value1.Horizontal, value2.Horizontal),
                Math.Min(value1.Vertical, value2.Vertical));
        }
        public static Range2D TopRight(Range2D value1, Range2D value2)
        {
            return new Range2D(Math.Max(value1.Horizontal, value2.Horizontal),
                Math.Min(value1.Vertical, value2.Vertical));
        }
        public static Range2D BottomLeft(Range2D value1, Range2D value2)
        {
            return new Range2D(Math.Min(value1.Horizontal, value2.Horizontal),
                Math.Max(value1.Vertical, value2.Vertical));
        }
        public static Range2D BottomRight(Range2D value1, Range2D value2)
        {
            return new Range2D(Math.Max(value1.Horizontal, value2.Horizontal),
                Math.Max(value1.Vertical, value2.Vertical));
        }

        public override bool Equals(object obj)
        {
            if (obj is Range2D value)
                return value == this;

            return false;
        }
        public override int GetHashCode() => Horizontal ^ Vertical;

        public override string ToString() =>
            SR.GetString("H={0}, V={1}", Horizontal, Vertical);

        public static bool operator ==(Range2D left, Range2D right)
        {
            return left.Horizontal == right.Horizontal &&
                left.Vertical == right.Vertical;
        }
        public static bool operator !=(Range2D left, Range2D right)
        {
            return !(left == right);
        }

        public static Range2D operator +(Range2D left, Range2D right)
        {
            return new Range2D(left.Horizontal + right.Horizontal, left.Vertical + right.Vertical);
        }
        public static Range2D operator -(Range2D left, Range2D right)
        {
            return new Range2D(left.Horizontal - right.Horizontal, left.Vertical - right.Vertical);
        }
        public static Range2D operator -(Range2D right)
        {
            return new Range2D(-right.Horizontal, -right.Vertical);
        }
        public static Range2D operator *(Range2D left, Range2D right)
        {
            return new Range2D(left.Horizontal * right.Horizontal, left.Vertical * right.Vertical);
        }
        public static Range2D operator /(Range2D left, Range2D right)
        {
            return new Range2D(left.Horizontal / right.Horizontal, left.Vertical / right.Vertical);
        }

        public static implicit operator Range2D(int range)
        {
            return new Range2D(range, range);
        }
    }
}
