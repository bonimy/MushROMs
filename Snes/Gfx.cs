using System;
using System.Collections.Generic;
using System.Text;

namespace Snes
{
    public class Gfx
    {
        private GFXTile[] Tiles
        {
            get;
            set;
        }

        public GFXTile this[int index]
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
    }
}
