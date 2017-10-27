// <copyright file="ITileMap.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

namespace Helper
{
    public interface ITileMap
    {
        Range2D View
        {
            get;
            set;
        }

        Position2D ActiveViewTile
        {
            get;
            set;
        }
    }
}
