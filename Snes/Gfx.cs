// <copyright file="Gfx.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Snes
{
    public class Gfx
    {
        private GfxTile[] Tiles
        {
            get;
            set;
        }

        public GfxTile this[int index]
        {
            get
            {
                return Tiles[index];
            }

            set
            {
                Tiles[index] = value;
            }
        }

        public Gfx(int count) : this(new GfxTile[count])
        {
        }

        public Gfx(GfxTile[] tiles)
        {
            Tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        }
    }
}
