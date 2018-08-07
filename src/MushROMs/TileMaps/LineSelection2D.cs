// <copyright file="LineSelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.TileMaps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using static Helper.ThrowHelper;

    public sealed class LineSelection2D : Selection2D
    {
        public LineSelection2D(
            Point startPosition,
            int regionWidth,
            int length)
            : base(startPosition)
        {
            if (length < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(length),
                    length);
            }

            if (regionWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(regionWidth),
                    regionWidth);
            }

            Length = length;
        }

        public override int Count
        {
            get
            {
                return Length;
            }
        }

        private int Length
        {
            get;
        }

        private int RegionWidth
        {
            get;
        }

        public override Point this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Length)
                {
                    throw ValueNotInArrayBounds(
                        nameof(index),
                        index,
                        Count);
                }

                index += StartPosition.X;
                index += StartPosition.Y * RegionWidth;

                var x = index % RegionWidth;
                var y = index / RegionWidth;

                return new Point(x, y);
            }
        }

        public override Selection2D Copy()
        {
            return new LineSelection2D(StartPosition, RegionWidth, Length);
        }

        public override bool Contains(Point position)
        {
            var x = position.X - StartPosition.X;
            var y = position.Y - StartPosition.Y;
            var index = (y * RegionWidth) + x;
            return index >= 0 && index < Length;
        }

        public override IEnumerator<Point> GetEnumerator()
        {
            return new Enumerator(StartPosition, RegionWidth, Length);
        }

        private struct Enumerator : IEnumerator<Point>
        {
            public Enumerator(
                Point startPosition,
                int regionWidth,
                int length)
            {
                StartIndex =
                    (startPosition.Y * regionWidth) + startPosition.X;

                RegionWidth = regionWidth;
                Last = StartIndex + length;
                Index = StartIndex;
                Current = default;
            }

            public Point Current
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

            private int StartIndex
            {
                get;
            }

            private int RegionWidth
            {
                get;
            }

            private int Index
            {
                get;
                set;
            }

            private int Last
            {
                get;
            }

            public void Reset()
            {
                Index = StartIndex;
                Current = default;
            }

            public bool MoveNext()
            {
                if (Index >= Last)
                {
                    return false;
                }

                var x = Index % RegionWidth;
                var y = Index / RegionWidth;
                Current = new Point(x, y);
                Index++;
                return true;
            }

            void IDisposable.Dispose()
            {
            }
        }
    }
}
