// <copyright file="BoxSelection2D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public sealed class BoxSelection2D : Selection2D
    {
        private Range2D Range
        {
            get;
            set;
        }

        public override int Count
        {
            get
            {
                return Range.Area;
            }
        }

        public BoxSelection2D(Position2D startPosition, Range2D range)
        {
            if (range.Width <= 0 || range.Height <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(range),
                    SR.ErrorLowerBoundExclusive(nameof(range), range, Range2D.Empty));
            }

            StartPosition = startPosition;
            Range = range;
        }

        public override bool Contains(Position2D position)
        {
            position -= StartPosition;
            return Range.Contains(position);
        }

        public override IEnumerator<Position2D> GetEnumerator()
        {
            return new Enumerator(StartPosition, Range);
        }

        private struct Enumerator : IEnumerator<Position2D>
        {
            private Position2D StartPosition
            {
                get;
                set;
            }

            private Position2D Index
            {
                get;
                set;
            }

            private Position2D Last
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

            public Enumerator(Position2D startPosition, Range2D range)
            {
                StartPosition = startPosition;
                Last = StartPosition + range;
                Index = StartPosition;
                Current = default(Position2D);
            }

            public void Reset()
            {
                Index = StartPosition;
            }

            public bool MoveNext()
            {
                var x = Index.X;
                var y = Index.Y;

                x++;

                if (x >= Last.X)
                {
                    x = StartPosition.X;
                    y = 0;
                }

                if (y < Last.Y)
                {
                    Current = Index;
                    Index = new Position2D(x, y);
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
