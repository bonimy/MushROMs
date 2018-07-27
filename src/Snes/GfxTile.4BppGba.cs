// <copyright file="GfxTile.4BppGba.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData4BppGba(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += sizeof(uint), dst += PlanesPerTile)
            {
                var val = *(uint*)src;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    dst[x] = (byte)((val >> (x << 2)) & 0x0F);
                }
            }
        }

        private static void TileToData4BppGba(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += sizeof(uint), src += PlanesPerTile)
            {
                var val = 0u;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    val |= (uint)((src[x] & 3) << (x << 2));
                }

                *(uint*)dst = val;
            }
        }
    }
}
