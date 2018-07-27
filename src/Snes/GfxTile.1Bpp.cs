// <copyright file="GfxTile.1Bpp.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData1Bpp(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val = *src;
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)((val >> x) & 1);
                }
            }
        }

        private static void TileToData1Bpp(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst++)
            {
                var val = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if (*src != 0)
                    {
                        val |= 1 << x;
                    }
                }

                *dst = (byte)val;
            }
        }
    }
}
