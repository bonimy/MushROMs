// <copyright file="SubstringInfo.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using System.Diagnostics;
    using Helper.Properties;
    using static Helper.SR;
    using static Helper.ThrowHelper;

    [DebuggerDisplay("Start = {Start}, Length = {Length}")]
    public struct SubstringInfo : IEquatable<SubstringInfo>
    {
        public const int EndOfString = Int32.MinValue;

        public static readonly SubstringInfo Empty = default;

        public SubstringInfo(int start, int end)
        {
            if (start < 0)
            {
                throw ValueNotGreaterThan(
                    nameof(start),
                    start);
            }

            if (end < start && end != EndOfString)
            {
                throw new ArgumentException();
            }

            Start = start;
            End = end;
            Length = end == EndOfString ? EndOfString : (end - start);
        }

        private SubstringInfo(int start, int end, int length)
        {
            Start = start;
            End = end;
            Length = length;
        }

        public int Start
        {
            get;
        }

        public int End
        {
            get;
        }

        public int Length
        {
            get;
        }

        public static bool operator ==(
            SubstringInfo left,
            SubstringInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            SubstringInfo left,
            SubstringInfo right)
        {
            return !(left == right);
        }

        public static SubstringInfo FromStartAndLength(int start, int length)
        {
            if (length == EndOfString)
            {
                return new SubstringInfo(start, EndOfString);
            }

            if (length < 0)
            {
                throw new ArgumentException();
            }

            return new SubstringInfo(start, start + length);
        }

        public static SubstringInfo FromLengthAndEnd(int length, int end)
        {
            if (length == EndOfString)
            {
                return new SubstringInfo(0, end);
            }

            if (length < 0)
            {
                throw new ArgumentException();
            }

            if (end == EndOfString)
            {
                return new SubstringInfo(
                    EndOfString,
                    length,
                    EndOfString);
            }

            if (end < length)
            {
                throw new ArgumentException();
            }

            return new SubstringInfo(end - length, end);
        }

        public string GetSubstring(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (Length == EndOfString)
            {
                return value.Substring(Start);
            }

            if (End > value.Length)
            {
                throw new ArgumentException();
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

        public bool Equals(SubstringInfo obj)
        {
            return
                Start.Equals(obj.Start) &&
                End.Equals(obj.End);
        }

        public override bool Equals(object obj)
        {
            if (obj is SubstringInfo value)
            {
                return Equals(value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Start ^ End;
        }

        public override string ToString()
        {
            var length = Length == EndOfString ?
                Length.ToString() :
                Resources.EndOfString;

            return GetString(
                "{{{0}: {1}, {2}: {3}}}",
                nameof(Start),
                Start,
                nameof(Length),
                length);
        }
    }
}
