// <copyright file="SubstringPointer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Diagnostics;
using System.Text;

namespace Helper
{
    [DebuggerDisplay("Start = {Start}, Length = {Length}")]
    public struct SubstringPointer
    {
        public const int EndOfString = -1;

        public static readonly SubstringPointer Empty = new SubstringPointer();

        public int Start
        {
            get;
            private set;
        }

        public int End
        {
            get;
            private set;
        }

        public int Length
        {
            get;
            private set;
        }

        public SubstringPointer(int start, int end)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(start),
                    SR.ErrorLowerBoundExclusive(nameof(start), start, 0));
            }

            if (end < start && end != EndOfString)
            {
                throw new ArgumentException(
                    SR.ErrorSubstringPointerLength(nameof(end), end), nameof(end));
            }

            Start = start;
            End = end;
            Length = end == EndOfString ? EndOfString : (end - start);
        }

        public static SubstringPointer FromStartAndLength(int start, int length)
        {
            if (length == EndOfString)
            {
                return new SubstringPointer(start, EndOfString);
            }

            if (length < 0)
            {
                throw new ArgumentException(
                    SR.ErrorSubstringPointerLength(nameof(length), length), nameof(length));
            }

            return new SubstringPointer(start, start + length);
        }

        public static SubstringPointer FromLengthAndEnd(int length, int end)
        {
            if (length == EndOfString)
            {
                return new SubstringPointer(0, end);
            }

            if (length < 0)
            {
                throw new ArgumentException(
                    SR.ErrorSubstringPointerLength(nameof(length), length), nameof(length));
            }

            if (end == EndOfString)
            {
                var result = Empty;
                result.Start = EndOfString;
                result.Length = length;
                result.End = EndOfString;

                return result;
            }

            if (end < length)
            {
                throw new ArgumentException(
                    SR.ErrorSubstringPointerLength(nameof(end), end), nameof(end));
            }

            return new SubstringPointer(end - length, end);
        }

        public string GetSubstring(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (Length == EndOfString)
            {
                return value.Substring(Start);
            }

            if (End > value.Length)
            {
                throw new ArgumentException(
                    SR.ErrorStringSubstringSize(nameof(value), Start, Length), nameof(value));
            }

            if (Start == EndOfString)
            {
                if (End == EndOfString)
                {
                    return value.Substring(value.Length - Length);
                }

                return value.Substring(End - Length);
            }

            return value.Substring(Start, Length);
        }

        public static bool operator ==(SubstringPointer left, SubstringPointer right)
        {
            return left.Start == right.Start &&
                left.End == right.End;
        }

        public static bool operator !=(SubstringPointer left, SubstringPointer right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is SubstringPointer value)
            {
                return value == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Start ^ End;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append(nameof(Start));
            sb.Append(": ");
            sb.Append(Start);
            sb.Append(", ");
            sb.Append(nameof(Length));
            sb.Append(": ");
            if (Length == EndOfString)
            {
                sb.Append("End of String");
            }
            else
            {
                sb.Append(Length);
            }

            sb.Append("}");
            return sb.ToString();
        }
    }
}
