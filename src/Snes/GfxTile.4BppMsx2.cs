// <copyright file="GfxTile.4BppMsx2.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public unsafe partial struct GfxTile
    {
        private static void TileFromData4BppMsx2(byte* src, byte* dst)
        {
            for (var i = 0; i < DotsPerTile; i += 2, src++)
            {
                dst[i] = (byte)((*src >> 4) & 0x0F);
                dst[i + 1] = (byte)(*src & 0x0F);
            }
        }

        private static void TileToData4BppMsx2(byte* src, byte* dst)
        {
            for (var i = 0; i < DotsPerTile; i += 2, dst++)
            {
                var val1 = src[i] & 0x0F;
                var val2 = src[i + 1] & 0x0F;
                *dst = (byte)((val1 << 4) | val2);
            }
        }
    }
}
