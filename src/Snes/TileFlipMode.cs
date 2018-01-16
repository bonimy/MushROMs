// <copyright file="TileFlipMode.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;

namespace Snes
{
    [Flags]
    public enum TileFlipModes
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVeritcal = 2,
        FlipBoth = FlipHorizontal | FlipVeritcal
    }
}
