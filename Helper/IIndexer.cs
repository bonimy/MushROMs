// <copyright file="IIndexer.cs>
//     Copyright (c) 2017 Nelson Garcia
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
