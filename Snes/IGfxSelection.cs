// <copyright file="IGfxSelection.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System.Collections.Generic;

namespace Snes
{
    public interface IGfxSelection : IReadOnlyCollection<int>
    {
        int StartAddress
        {
            get;
        }

        GraphicsFormat GraphicsFormat
        {
            get;
        }
    }
}
