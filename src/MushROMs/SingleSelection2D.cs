// <copyright file="SingleSelection2D.cs" company="Public Domain">
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

    public sealed class SingleSelection2D : Selection2D
    {
        public SingleSelection2D(Point position)
            : base(position)
        {
        }

        public override int Count
        {
            get
            {
                return 1;
            }
        }

        public override Point this[int index]
        {
            get
            {
                if (index != 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return StartPosition;
            }
        }

        public override Selection2D Copy()
        {
            return new SingleSelection2D(StartPosition);
        }

        public override bool Contains(Point position)
        {
            return position == StartPosition;
        }

        public override IEnumerator<Point> GetEnumerator()
        {
            return new Enumerator(StartPosition);
        }

        private struct Enumerator : IEnumerator<Point>
        {
            public Enumerator(Point startPosition)
            {
                CanMove = true;
                StartPosition = startPosition;
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
            }

            private bool CanMove
            {
                get;
                set;
            }

            public void Reset()
            {
                CanMove = true;
                Current = default;
            }

            public bool MoveNext()
            {
                if (CanMove)
                {
                    Current = StartPosition;
                    CanMove = false;
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
