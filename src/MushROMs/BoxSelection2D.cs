// <copyright file="BoxSelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using static Helper.ThrowHelper;

    public sealed class BoxSelection2D : Selection2D
    {
        public BoxSelection2D(
            Point startPosition,
            Size range)
            : base(startPosition)
        {
            if (range.Width <= 0 || range.Height <= 0)
            {
                throw new ArgumentException();
            }

            Size = range;
        }

        public BoxSelection2D(
            Point startPosition,
            int width,
            int height)
            : base(startPosition)
        {
            if (width <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(width),
                    width);
            }

            if (height <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(height),
                    height);
            }

            Size = new Size(width, height);
        }

        public override int Count
        {
            get
            {
                return Size.Width * Size.Height;
            }
        }

        private Size Size
        {
            get;
        }

        public override Point this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                var result = StartPosition;
                result.X += index % Size.Width;
                result.Y += index / Size.Width;

                return result;
            }
        }

        public override Selection2D Copy()
        {
            return new BoxSelection2D(StartPosition, Size);
        }

        public override bool Contains(Point position)
        {
            var x = position.X - StartPosition.X;
            var y = position.Y - StartPosition.Y;
            var bounds = new Rectangle(Point.Empty, Size);
            return bounds.Contains(x, y);
        }

        public override IEnumerator<Point> GetEnumerator()
        {
            return new Enumerator(StartPosition, Size);
        }

        private struct Enumerator : IEnumerator<Point>
        {
            public Enumerator(Point startPosition, Size range)
            {
                StartPosition = startPosition;
                Last = StartPosition + range;
                Index = StartPosition;
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

            private Point StartPosition
            {
                get;
                set;
            }

            private Point Index
            {
                get;
                set;
            }

            private Point Last
            {
                get;
                set;
            }

            public void Reset()
            {
                Index = StartPosition;
                Current = default;
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
                    Index = new Point(x, y);
                    return true;
                }

                return false;
            }

            void IDisposable.Dispose()
            {
            }
        }
    }
}
