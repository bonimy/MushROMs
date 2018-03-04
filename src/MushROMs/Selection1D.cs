// <copyright file="Selection1D.cs" company="Public Domain">
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

    public abstract class Selection1D : ISelection1D
    {
        public static readonly ISelection1D Empty = new EmptySelection1D();

        protected Selection1D(int startIndex)
        {
            StartIndex = startIndex;
        }

        public int StartIndex
        {
            get;
        }

        public abstract int Count
        {
            get;
        }

        public abstract int this[int index]
        {
            get;
        }

        public abstract ISelection1D Copy();

        public abstract bool Contains(int index);

        public abstract IEnumerator<int> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class EmptySelection1D : ISelection1D
        {
            int ISelection1D.StartIndex
            {
                get
                {
                    return 0;
                }
            }

            int IReadOnlyCollection<int>.Count
            {
                get
                {
                    return 0;
                }
            }

            int IReadOnlyList<int>.this[int index]
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            ISelection1D ISelection1D.Copy()
            {
                // There only ever needs to be one instance.
                return Empty;
            }

            bool ISelection1D.Contains(int index)
            {
                return false;
            }

            IEnumerator<int> IEnumerable<int>.GetEnumerator()
            {
                return default(Enumerator);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return default(Enumerator);
            }

            private struct Enumerator : IEnumerator<int>
            {
                int IEnumerator<int>.Current
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
