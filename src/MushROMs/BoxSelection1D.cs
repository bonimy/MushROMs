// <copyright file="BoxSelection1D.cs" company="Public Domain">
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

    public sealed class BoxSelection1D : Selection1D
    {
        public BoxSelection1D(
            int startIndex,
            int regionWidth,
            Size range)
            : this(startIndex, regionWidth, range.Width, range.Height)
        {
        }

        public BoxSelection1D(
            int startIndex,
            int regionWidth,
            int width,
            int heigth)
            : base(startIndex)
        {
            if (regionWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(regionWidth),
                    regionWidth);
            }

            if (width <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(width),
                    width);
            }

            if (heigth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(heigth),
                    heigth);
            }

            RegionWidth = regionWidth;
            Size = new Size(width, heigth);
        }

        public int RegionWidth
        {
            get;
        }

        public Size Size
        {
            get;
        }

        public override int Count
        {
            get
            {
                return Size.Width * Size.Height;
            }
        }

        public override int this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Count)
                {
                    throw ValueNotInArrayBounds(
                        nameof(index),
                        index,
                        Count);
                }

                var x = index % Size.Width;
                var y = index / Size.Width;

                var result = StartIndex;
                result += x;
                result += y * RegionWidth;
                return result;
            }
        }

        public override ISelection1D Copy()
        {
            return new BoxSelection1D(StartIndex, RegionWidth, Size);
        }

        public override bool Contains(int index)
        {
            var viewIndex = index -= StartIndex;
            var x = viewIndex % RegionWidth;
            var y = viewIndex / RegionWidth;
            var bounds = new Rectangle(Point.Empty, Size);
            return bounds.Contains(x, y);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(StartIndex, RegionWidth, Size);
        }

        private struct Enumerator : IEnumerator<int>
        {
            public Enumerator(
                int startIndex,
                int regionWidth,
                Size range)
            {
                StartIndex = startIndex;
                Size = range;
                RegionWidth = regionWidth;
                Index = 0;
                Current = default;
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

            private int StartIndex
            {
                get;
            }

            private Size Size
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

            void IDisposable.Dispose()
            {
            }

            public void Reset()
            {
                Index = 0;
                Current = default;
            }

            public bool MoveNext()
            {
                // Get the X and Y coordinates of the Index.
                var x = Index % RegionWidth;
                var y = Index / RegionWidth;

                // Go to the next X-coordinate.
                x++;

                // If we've exceeded the width...
                if (x >= Size.Width)
                {
                    // ... go to the start of the next row.
                    x = 0;
                    y++;
                }

                // If we're still within our height...
                if (y < Size.Height)
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
        }
    }
}
