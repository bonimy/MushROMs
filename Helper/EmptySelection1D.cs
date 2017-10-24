// <copyright file="EmptySelection1D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
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

        internal EmptySelection1D()
        {
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
                private set;
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
