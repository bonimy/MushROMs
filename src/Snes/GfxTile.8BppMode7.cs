// <copyright file="GfxTile.8BppMode7.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using static System.Buffer;

    public unsafe partial struct GfxTile
    {
        private static void TileFromData8BppMode7(byte* src, byte* dst)
        {
            MemoryCopy(src, dst, DotsPerTile, DotsPerTile);
        }

        private static void TileToData8BppMode7(byte* src, byte* dst)
        {
            MemoryCopy(src, dst, DotsPerTile, DotsPerTile);
        }
    }
}
