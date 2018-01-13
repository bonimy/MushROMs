// <copyright file="LineSelection1D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public sealed class LineSelection1D : Selection1D
    {
        private int Length
        {
            get;
        }

        public override int Count
        {
            get
            {
                return Length;
            }
        }

        public override int this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return StartIndex + index;
            }
        }

        public LineSelection1D(int startIndex, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(length),
                    SR.ErrorLowerBoundInclusive(nameof(length), length, 0));
            }

            StartIndex = startIndex;
            Length = length;
        }

        public override Selection1D Copy()
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
            private int StartIndex
            {
                get;
                set;
            }

            private int Index
            {
                get;
                set;
            }

            private int Last
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

            public Enumerator(int startIndex, int length)
            {
                StartIndex = startIndex;
                Last = StartIndex + length;
                Index = StartIndex;
                Current = default(int);
            }

            public void Reset()
            {
                Index = StartIndex;
            }

            public bool MoveNext()
            {
                if (Index < Last)
                {
                    Current = Index++;
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
