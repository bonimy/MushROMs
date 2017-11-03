// <copyright file="ITileMap1D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

namespace Helper
{
    public interface ITileMap1D : ITileMap
    {
        int GridLength
        {
            get;
            set;
        }

        int ZeroTile
        {
            get;
            set;
        }

        int ViewableTiles
        {
            get;
            set;
        }

        int ActiveTile
        {
            get;
            set;
        }
    }
}
