// <copyright file="LineSelection1D.cs" company="Public Domain">
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
    using static Helper.ThrowHelper;

    public sealed class LineSelection1D : Selection1D
    {
        public LineSelection1D(int startIndex, int length)
            : base(startIndex)
        {
            if (length < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(length),
                    length);
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

        public override int this[int index]
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

                return StartIndex + index;
            }
        }

        public override ISelection1D Copy()
        {
            return new LineSelection1D(StartIndex, Length);
        }

        public override bool Contains(int index)
        {
            index -= StartIndex;
            return index >= 0 && index < Length;
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(StartIndex, Length);
        }

        private struct Enumerator : IEnumerator<int>
        {
            public Enumerator(int startIndex, int length)
            {
                StartIndex = startIndex;
                Last = StartIndex + length;
                Index = StartIndex;
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

            private int Last
            {
                get;
            }

            private int Index
            {
                get;
                set;
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

                Current = Index++;
                return true;
            }

            void IDisposable.Dispose()
            {
            }
        }
    }
}
