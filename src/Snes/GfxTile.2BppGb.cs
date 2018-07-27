// <copyright file="GfxTile.2BppGb.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData2BppGb(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0];
                var val2 = src[1];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        ((val2 >> x) & 1) << 1);
                }
            }
        }

        private static void TileToData2BppGb(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += 2)
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
                dst[1] = (byte)val2;
            }
        }
    }
}
