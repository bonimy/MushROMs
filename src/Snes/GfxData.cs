// <copyright file="GfxData.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Snes
{
    public delegate GfxTile GfxTileMethod(GfxTile tile);

    public class GfxData : IReadOnlyList<GfxTile>
    {
        private GfxTile[] Tiles
        {
            get;
        }

        public IGfxSelection Selection
        {
            get;
        }

        public GraphicsFormat GraphicsFormat
        {
            get;
        }

        public int BytesPerTile
        {
            get;
        }

        public int Count
        {
            get
            {
                return Tiles.Length;
            }
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

        internal GfxData(GfxTile[] tiles, IGfxSelection selection)
        {
            Tiles = tiles ??
                throw new ArgumentNullException(nameof(tiles));

            Selection = selection ??
                throw new ArgumentNullException(nameof(selection));

            GraphicsFormat = Selection.GraphicsFormat;
            BytesPerTile = Selection.BytesPerTile;
        }

        public void FlipX()
        {
            AlterTiles(tile => tile.FlipX());
        }

        public void FlipY()
        {
            AlterTiles(tile => tile.FlipY());
        }

        public void Rotate90()
        {
            AlterTiles(tile => tile.Rotate90());
        }

        public void Rotate180()
        {
            AlterTiles(tile => tile.Rotate180());
        }

        public void Rotate270()
        {
            AlterTiles(tile => tile.Rotate270());
        }

        public void ReplaceColor(byte original, byte replacement)
        {
            AlterTiles(tile => tile.ReplaceColor(original, replacement));
        }

        public void SwapColors(byte color1, byte color2)
        {
            AlterTiles(tile => tile.SwapColors(color1, color2));
        }

        public void RotateColors(byte first, byte last, byte shift)
        {
            AlterTiles(tile => tile.RotateColors(first, last, shift));
        }

        public void AlterTiles(GfxTileMethod method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            for (var i = Count; --i >= 0;)
            {
                this[i] = method(this[i]);
            }
        }

        public IEnumerator<GfxTile> GetEnumerator()
        {
            return Tiles.GetEnumerator() as IEnumerator<GfxTile>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
