// <copyright file="GfxTile.2BppNes.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData2BppNes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0];
                var val2 = src[PlanesPerTile];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        ((val2 >> x) & 1) << 1);
                }
            }
        }

        private static void TileToData2BppNes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst++)
            {
                var val1 = 0;
                var val2 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }
                }

                dst[0] = (byte)val1;
                dst[PlanesPerTile] = (byte)val2;
            }
        }
    }
}
