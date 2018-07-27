// <copyright file="GfxTile.8BppSnes.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData8BppSnes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0 + (0 * PlanesPerTile)];
                var val2 = src[1 + (0 * PlanesPerTile)];
                var val3 = src[0 + (2 * PlanesPerTile)];
                var val4 = src[1 + (2 * PlanesPerTile)];
                var val5 = src[0 + (4 * PlanesPerTile)];
                var val6 = src[1 + (4 * PlanesPerTile)];
                var val7 = src[0 + (6 * PlanesPerTile)];
                var val8 = src[1 + (6 * PlanesPerTile)];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val2 >> x) & 1) << 3) |
                        (((val3 >> x) & 1) << 4) |
                        (((val4 >> x) & 1) << 5) |
                        (((val2 >> x) & 1) << 6) |
                        (((val3 >> x) & 1) << 7));
                }
            }
        }

        private static void TileToData8BppSnes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += 2)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                var val5 = 0;
                var val6 = 0;
                var val7 = 0;
                var val8 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1 << 0) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 1 << 1) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((value & 1 << 2) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((value & 1 << 3) != 0)
                    {
                        val4 |= 1 << x;
                    }

                    if ((value & 1 << 4) != 0)
                    {
                        val5 |= 1 << x;
                    }

                    if ((value & 1 << 5) != 0)
                    {
                        val6 |= 1 << x;
                    }

                    if ((value & 1 << 6) != 0)
                    {
                        val7 |= 1 << x;
                    }

                    if ((value & 1 << 7) != 0)
                    {
                        val8 |= 1 << x;
                    }
                }

                dst[0 + (0 * PlanesPerTile)] = (byte)val1;
                dst[1 + (0 * PlanesPerTile)] = (byte)val2;
                dst[0 + (2 * PlanesPerTile)] = (byte)val3;
                dst[1 + (2 * PlanesPerTile)] = (byte)val4;
                dst[0 + (4 * PlanesPerTile)] = (byte)val5;
                dst[1 + (4 * PlanesPerTile)] = (byte)val6;
                dst[0 + (6 * PlanesPerTile)] = (byte)val7;
                dst[1 + (6 * PlanesPerTile)] = (byte)val8;
            }
        }
    }
}
