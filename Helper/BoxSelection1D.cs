// <copyright file="BoxSelection1D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public sealed class BoxSelection1D : Selection1D
    {
        public int RegionWidth
        {
            get;
            private set;
        }

        public Range2D Range
        {
            get;
            private set;
        }

        public override int Count
        {
            get
            {
                return Range.Area;
            }
        }

        public BoxSelection1D(int startIndex, int regionWidth, Range2D range)
        {
            if (regionWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(regionWidth),
                    SR.ErrorLowerBoundExclusive(nameof(regionWidth), regionWidth, 0));
            }

            if (range.Width <= 0 || range.Height <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(range),
                    SR.ErrorLowerBoundExclusive(nameof(range), range, Range2D.Empty));
            }

            StartIndex = startIndex;
            RegionWidth = regionWidth;
            Range = range;
        }

        public override bool Contains(int index)
        {
            index -= StartIndex;
            return Range.Contains(
                new Position2D(index % RegionWidth, index / RegionWidth));
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(StartIndex, RegionWidth, Range);
        }

        private struct Enumerator : IEnumerator<int>
        {
            private int StartIndex
            {
                get;
                set;
            }

            private int Index
            {
                get;
                set;
            }

            private Range2D Range
            {
                get;
                set;
            }

            private int RegionWidth
            {
                get;
                set;
            }

            public int Current
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

            public Enumerator(int startIndex, int regionWidth, Range2D range)
            {
                StartIndex = startIndex;
                Range = range;
                RegionWidth = regionWidth;
                Index = 0;
                Current = default(int);
            }

            public void Reset()
            {
                Index = 0;
            }

            public bool MoveNext()
            {
                // Get the X and Y coordinates of the Index.
                var x = Index % RegionWidth;
                var y = Index / RegionWidth;

                // Go to the next X-coordinate.
                x++;

                // If we've exceeded the width...
                if (x >= Range.Width)
                {
                    // ... go to the start of the next row.
                    x = 0;
                    y++;
                }

                // If we're still within our height...
                if (y < Range.Height)
                {
                    // Update the current value and our index.
                    Current = StartIndex + Index;
                    Index = GetIndex(x, y);
                    return true;
                }

                return false;
            }

            private int GetIndex(int x, int y)
            {
                return (y * RegionWidth) + x;
            }

            public void Dispose()
            {
            }
        }
    }
}
