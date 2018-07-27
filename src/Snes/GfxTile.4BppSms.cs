// <copyright file="GfxTile.4BppSms.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData4BppSms(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += 4)
            {
                var val1 = src[0];
                var val2 = src[1];
                var val3 = src[2];
                var val4 = src[3];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
                }
            }
        }

        private static void TileToData4BppSms(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += 4)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
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

                    if ((value & 8) != 0)
                    {
                        val4 |= 1 << x;
                    }
                }

                dst[0] = (byte)val1;
                dst[1] = (byte)val2;
                dst[2] = (byte)val3;
                dst[3] = (byte)val4;
            }
        }
    }
}
