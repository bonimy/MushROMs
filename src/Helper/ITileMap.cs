// <copyright file="ITileMap.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Helper
{
    public interface ITileMap
    {
        Range2D ViewSize
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
