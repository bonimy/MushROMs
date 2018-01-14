// <copyright file="ISelection.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System.Collections.Generic;

namespace Helper
{
    public interface ISelection<T> : IReadOnlyList<T>
    {
        bool Contains(T value);

        ISelection<T> Copy();
    }
}
