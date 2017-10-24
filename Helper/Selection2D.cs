// <copyright file="Selection2D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public abstract class Selection2D : IEnumerable<Position2D>
    {
        public static readonly Selection1D Empty = new EmptySelection1D();

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

        protected Selection2D()
        {
        }

        public abstract bool Contains(Position2D position);

        public abstract IEnumerator<Position2D> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
