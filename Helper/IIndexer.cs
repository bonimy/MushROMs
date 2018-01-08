// <copyright file="IIndexer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

namespace Helper
{
    public interface IIndexer<T> : IReadOnlyIndexer<T>
    {
        new T this[int index]
        {
            get;
            set;
        }
    }
}
