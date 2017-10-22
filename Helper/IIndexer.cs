// <copyright file="IIndexer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

namespace Helper
{
    public interface IIndexer<T>
    {
        T this[int index]
        {
            get;
            set;
        }
    }
}
