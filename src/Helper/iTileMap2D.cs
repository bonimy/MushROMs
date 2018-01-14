// <copyright file="iTileMap2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Helper
{
    public interface ITileMap2D : ITileMap
    {
        Range2D GridSize
        {
            get;
            set;
        }

        Position2D ZeroTile
        {
            get;
            set;
        }

        Range2D ViewableTileRange
        {
            get;
            set;
        }

        Position2D ActiveTile
        {
            get;
            set;
        }
    }
}
