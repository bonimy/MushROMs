// <copyright file="SingleSelection1D.cs" company="Public Domain">
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

    public sealed class SingleSelection1D : Selection1D
    {
        public SingleSelection1D(int index)
            : base(index)
        {
        }

        public override int Count
        {
            get
            {
                return 1;
            }
        }

        public override int this[int index]
        {
            get
            {
                if (index != 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index));
                }

                return StartIndex;
            }
        }

        public override ISelection1D Copy()
        {
            return new SingleSelection1D(StartIndex);
        }

        public override bool Contains(int index)
        {
            return index == StartIndex;
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(StartIndex);
        }

        private struct Enumerator : IEnumerator<int>
        {
            public Enumerator(int startIndex)
            {
                CanMove = true;
                StartIndex = startIndex;
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
                    CanMove = false;
                    Current = StartIndex;
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
