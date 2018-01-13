// <copyright file="Selection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public abstract class Selection2D : ISelection<Position2D>
    {
        public static readonly Selection2D Empty = new EmptySelection2D();

        public Position2D StartPosition
        {
            get;
            protected set;
        }

        public abstract int Count
        {
            get;
        }

        public bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public abstract Position2D this[int index]
        {
            get;
        }

        protected Selection2D()
        {
        }

        public abstract Selection2D Copy();

        ISelection<Position2D> ISelection<Position2D>.Copy()
        {
            return Copy();
        }

        public abstract bool Contains(Position2D position);

        public abstract IEnumerator<Position2D> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
