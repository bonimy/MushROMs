// <copyright file="Position2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Helper
{
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    [TypeConverter(typeof(Position2DConverter))]
    public struct Position2D
    {
        public static readonly Position2D Empty = new Position2D();

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public Position2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position2D Add(Position2D value)
        {
            return this + value;
        }

        public Position2D Subtract(Position2D value)
        {
            return this - value;
        }

        public Position2D Negate()
        {
            return -this;
        }

        public Position2D Multiply(Range2D value)
        {
            return this * value;
        }

        public Position2D Divide(Range2D value)
        {
            return this / value;
        }

        public static Position2D TopLeft(Position2D left, Position2D right)
        {
            return new Position2D(
                Math.Min(left.X, right.X),
                Math.Min(left.Y, right.Y));
        }

        public static Position2D TopRight(Position2D left, Position2D right)
        {
            return new Position2D(
                Math.Max(left.X, right.X),
                Math.Min(left.Y, right.Y));
        }

        public static Position2D BottomLeft(Position2D left, Position2D right)
        {
            return new Position2D(
                Math.Min(left.X, right.X),
                Math.Max(left.Y, right.Y));
        }

        public static Position2D BottomRight(Position2D left, Position2D right)
        {
            return new Position2D(
                Math.Max(left.X, right.X),
                Math.Max(left.Y, right.Y));
        }

        public override bool Equals(object obj)
        {
            if (obj is Position2D value)
            {
                return value == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override string ToString()
        {
            return SR.GetString("X={0}, Y={1}", X, Y);
        }

        public static bool operator ==(Position2D left, Position2D right)
        {
            return
                left.X == right.X &&
                left.Y == right.Y;
        }

        public static bool operator !=(Position2D left, Position2D right)
        {
            return !(left == right);
        }

        public static implicit operator Range2D(Position2D position)
        {
            return new Range2D(position.X, position.Y);
        }

        public static implicit operator Position2D(Range2D range)
        {
            return new Position2D(range.Width, range.Height);
        }

        public static Position2D operator +(Position2D left, Range2D right)
        {
            return new Position2D(left.X + right.Width, left.Y + right.Height);
        }

        public static Position2D operator -(Position2D left, Position2D right)
        {
            return new Position2D(left.X - right.X, left.Y - right.Y);
        }

        public static Position2D operator -(Position2D right)
        {
            return new Position2D(-right.X, -right.Y);
        }

        public static Position2D operator *(Position2D left, Range2D right)
        {
            return new Position2D(left.X * right.Width, left.Y * right.Height);
        }

        public static Position2D operator /(Position2D left, Range2D right)
        {
            var position = new Position2D(
                left.X / right.Width,
                left.Y / right.Height);
            /*
            if (left.X < 0)
                position.X--;
            if (left.Y < 0)
                position.Y--;
                */
            return position;
        }

        public static implicit operator Point(Position2D value)
        {
            return new Point(value.X, value.Y);
        }

        public static implicit operator Position2D(Point value)
        {
            return new Position2D(value.X, value.Y);
        }
    }
}
