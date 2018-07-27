// <copyright file="GfxTile.2BppVb.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData2BppVb(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += DotsPerPlane, src += sizeof(ushort))
            {
                var val = *(ushort*)src;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    dst[x] = (byte)((val >> (x << 1)) & 3);
                }
            }
        }

        private static void TileToData2BppVb(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += sizeof(ushort))
            {
                var val = 0;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    val |= (src[x] & 3) << (x << 1);
                }

                *(ushort*)dst = (ushort)val;
            }
        }
    }
}
