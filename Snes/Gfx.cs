// <copyright file="Gfx.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using Helper;

namespace Snes
{
    public class Gfx
    {
        private GfxTile[] Tiles
        {
            get;
            set;
        }

        public int Count
        {
            get
            {
                return Tiles.Length;
            }
        }

        public Selection1D Selection
        {
            get;
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

            Selection = new LineSelection1D(0, tiles.Length);
        }

        internal Gfx(GfxTile[] tiles, Selection1D selection)
        {
        }
    }
}
