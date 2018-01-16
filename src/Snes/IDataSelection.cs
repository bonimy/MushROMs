// <copyright file="IDataSelection.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Snes
{
    public interface IDataSelection : IReadOnlyList<int>
    {
        IDataSelection Copy();
    }
}
