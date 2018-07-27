// <copyright file="GfxData.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class GfxData : IReadOnlyList<GfxTile>
    {
        internal GfxData(GfxTile[] tiles, IGfxSelection selection)
        {
            Tiles = tiles ??
                throw new ArgumentNullException(nameof(tiles));

            Selection = selection ??
                throw new ArgumentNullException(nameof(selection));

            GraphicsFormat = Selection.GraphicsFormat;
            BytesPerTile = Selection.BytesPerTile;
        }

        private GfxData(GfxData gfxData)
        {
            if (gfxData is null)
            {
                throw new ArgumentNullException(nameof(gfxData));
            }

            var result = new GfxTile[gfxData.Count];
            gfxData.CopyTo(result, 0);
            Tiles = result;
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
                return Tiles.Count;
            }
        }

        private IList<GfxTile> Tiles
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

        public GfxData Copy()
        {
            return new GfxData(this);
        }

        public void Empty()
        {
            AlterTiles(tile => default);
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
            AlterTiles(
                tile => tile.ReplaceColor(original, replacement));
        }

        public void SwapColors(byte color1, byte color2)
        {
            AlterTiles(tile => tile.SwapColors(color1, color2));
        }

        public void RotateColors(byte first, byte last, byte shift)
        {
            AlterTiles(tile => tile.RotateColors(first, last, shift));
        }

        public void AlterTiles(Func<GfxTile, GfxTile> alterTile)
        {
            if (alterTile is null)
            {
                throw new ArgumentNullException(nameof(alterTile));
            }

            for (var i = Count; --i >= 0;)
            {
                this[i] = alterTile(this[i]);
            }
        }

        public void CopyTo(GfxTile[] array, int arrayIndex)
        {
            Tiles.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GfxTile> GetEnumerator()
        {
            return Tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
