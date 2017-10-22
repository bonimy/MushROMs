// <copyright file="IReadOnlyIndexer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

namespace Helper
{
    public interface IReadOnlyIndexer<T>
    {
        T this[int index] { get; }
    }
}
