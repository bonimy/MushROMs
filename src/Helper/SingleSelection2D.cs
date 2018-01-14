// <copyright file="SingleSelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public sealed class SingleSelection2D : Selection2D
    {
        public override int Count
        {
            get
            {
                return 1;
            }
        }

        public override Position2D this[int index]
        {
            get
            {
                if (index != 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return StartPosition;
            }
        }

        public SingleSelection2D(Position2D position)
        {
            StartPosition = position;
        }

        public override Selection2D Copy()
        {
            return new SingleSelection2D(StartPosition);
        }

        public override bool Contains(Position2D position)
        {
            return position == StartPosition;
        }

        public override IEnumerator<Position2D> GetEnumerator()
        {
            return new Enumerator(StartPosition);
        }

        private struct Enumerator : IEnumerator<Position2D>
        {
            private bool CanMove
            {
                get;
                set;
            }

            public Position2D Current
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

            public Enumerator(Position2D startPosition)
            {
                CanMove = true;
                Current = startPosition;
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
