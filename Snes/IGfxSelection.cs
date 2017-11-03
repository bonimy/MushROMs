// <copyright file="IGfxSelection.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System.Collections.Generic;
using Helper;

namespace Snes
{
    public interface IGfxSelection : IReadOnlyList<int>
    {
        int StartAddress
        {
            get;
        }

        GraphicsFormat GraphicsFormat
        {
            get;
        }

        ISelection<int> Selection
        {
            get;
        }
    }
}
