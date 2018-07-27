// <copyright file="GfxTile.3BppSnes.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData3BppSnes(byte* src, byte* dst)
        {
            for (var y = 0; y < PlanesPerTile; y++)
            {
                var val1 = src[y << 1];
                var val2 = src[(y << 1) + 1];
                var val3 = src[y + (PlanesPerTile << 1)];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2));
                }
            }
        }

        private static void TileToData3BppSnes(byte* src, byte* dst)
        {
            for (var y = 0; y < PlanesPerTile; y++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
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

                    if ((value & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }
                }

                dst[y << 1] = (byte)val1;
                dst[(y << 1) + 1] = (byte)val2;
                dst[y + (PlanesPerTile << 1)] = (byte)val3;
            }
        }
    }
}
