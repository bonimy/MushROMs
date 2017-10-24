// <copyright file="LineSelection2D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public sealed class LineSelection2D : Selection2D
    {
        private int Length
        {
            get;
            set;
        }

        private int RegionWidth
        {
            get;
            set;
        }

        public override int Count
        {
            get
            {
                return Length;
            }
        }

        public LineSelection2D(Position2D startPosition, int regionWidth, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(length),
                    SR.ErrorLowerBoundInclusive(nameof(length), length, 0));
            }

            if (regionWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(regionWidth),
                    SR.ErrorLowerBoundExclusive(nameof(regionWidth), regionWidth, 0));
            }

            StartPosition = startPosition;
            Length = length;
        }

        public override bool Contains(Position2D position)
        {
            position -= StartPosition;
            var index = (position.Y * RegionWidth) + position.X;
            return index >= 0 && index < Length;
        }

        public override IEnumerator<Position2D> GetEnumerator()
        {
            return new Enumerator(StartPosition, RegionWidth, Length);
        }

        private struct Enumerator : IEnumerator<Position2D>
        {
            private int StartIndex
            {
                get;
                set;
            }

            private int RegionWidth
            {
                get;
                set;
            }

            private int Index
            {
                get;
                set;
            }

            private int Last
            {
                get;
                set;
            }

            public Position2D Current
            {
                get;
                private set;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public Enumerator(Position2D startPosition, int regionWidth, int length)
            {
                StartIndex = (startPosition.Y * regionWidth) + startPosition.X;
                RegionWidth = regionWidth;
                Last = StartIndex + length;
                Index = StartIndex;
                Current = default(Position2D);
            }

            public void Reset()
            {
                Index = StartIndex;
            }

            public bool MoveNext()
            {
                if (Index < Last)
                {
                    Current = new Position2D(
                        Index % RegionWidth,
                        Index / RegionWidth);
                    Index++;
                    return true;
                }

                return false;
            }

            public void Dispose()
            {
            }
        }
    }
}
