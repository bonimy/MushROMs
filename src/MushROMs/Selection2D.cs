// <copyright file="Selection2D.cs" company="Public Domain">
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

    public abstract class Selection2D : ISelection2D
    {
        public static readonly ISelection2D Empty =
            new EmptySelection2D();

        protected Selection2D(Point startPosition)
        {
            StartPosition = startPosition;
        }

        public Point StartPosition
        {
            get;
        }

        public abstract int Count
        {
            get;
        }

        public abstract Point this[int index]
        {
            get;
        }

        public abstract Selection2D Copy();

        ISelection2D ISelection2D.Copy()
        {
            return Copy();
        }

        public abstract bool Contains(Point position);

        public abstract IEnumerator<Point> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class EmptySelection2D : ISelection2D
        {
            Point ISelection2D.StartPosition
            {
                get
                {
                    return Point.Empty;
                }
            }

            int IReadOnlyCollection<Point>.Count
            {
                get
                {
                    return 0;
                }
            }

            Point IReadOnlyList<Point>.this[int index]
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            ISelection2D ISelection2D.Copy()
            {
                return Empty;
            }

            bool ISelection2D.Contains(Point position)
            {
                return false;
            }

            IEnumerator<Point> IEnumerable<Point>.GetEnumerator()
            {
                return default(Enumerator);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return default(Enumerator);
            }

            private struct Enumerator : IEnumerator<Point>
            {
                Point IEnumerator<Point>.Current
                {
                    get;
                }

                object IEnumerator.Current
                {
                    get;
                }

                void IEnumerator.Reset()
                {
                }

                bool IEnumerator.MoveNext()
                {
                    return false;
                }

                void IDisposable.Dispose()
                {
                }
            }
        }
    }
}
