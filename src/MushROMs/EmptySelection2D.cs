// <copyright file="EmptySelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using Helper;

namespace MushROMs
{
    internal sealed class EmptySelection2D : Selection2D
    {
        public override int Count
        {
            get
            {
                return 0;
            }
        }

        public override Position2D this[int index]
        {
            get
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        internal EmptySelection2D()
        {
        }

        public override Selection2D Copy()
        {
            return Empty;
        }

        public override bool Contains(Position2D position)
        {
            return false;
        }

        public override IEnumerator<Position2D> GetEnumerator()
        {
            return new Enumerator();
        }

        private struct Enumerator : IEnumerator<Position2D>
        {
            public Position2D Current
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
