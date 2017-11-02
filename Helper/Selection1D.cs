// <copyright file="Selection1D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public abstract class Selection1D : ISelection<int>
    {
        public static readonly Selection1D Empty = new EmptySelection1D();

        public int StartIndex
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

        public abstract int this[int index]
        {
            get;
        }

        protected Selection1D()
        {
        }

        public abstract Selection1D Copy();

        ISelection<int> ISelection<int>.Copy()
        {
            return Copy();
        }

        public abstract bool Contains(int index);

        public abstract IEnumerator<int> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
