// <copyright file="EmptySelection1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace MushROMs
{
    internal sealed class EmptySelection1D : Selection1D
    {
        public override int Count
        {
            get
            {
                return 0;
            }
        }

        public override int this[int index]
        {
            get
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        internal EmptySelection1D()
        {
        }

        public override Selection1D Copy()
        {
            return Empty;
        }

        public override bool Contains(int index)
        {
            return false;
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return new Enumerator();
        }

        private struct Enumerator : IEnumerator<int>
        {
            public int Current
            {
                get;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public void Reset()
            {
            }

            public bool MoveNext()
            {
                return false;
            }

            public void Dispose()
            {
            }
        }
    }
}
