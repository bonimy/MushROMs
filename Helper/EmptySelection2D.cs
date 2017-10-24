// <copyright file="EmptySelection2D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
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

        internal EmptySelection2D()
        {
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
                get
                {
                    return new Position2D(-1, -1);
                }
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
