// <copyright file="SingleSelection1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public sealed class SingleSelection1D : Selection1D
    {
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
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return StartIndex;
            }
        }

        public SingleSelection1D(int index)
        {
            StartIndex = index;
        }

        public override Selection1D Copy()
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
            private bool CanMove
            {
                get;
                set;
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

            public Enumerator(int startIndex)
            {
                CanMove = true;
                Current = startIndex;
            }

            public void Reset()
            {
                CanMove = true;
            }

            public bool MoveNext()
            {
                if (CanMove)
                {
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
