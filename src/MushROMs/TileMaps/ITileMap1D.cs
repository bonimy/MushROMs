// <copyright file="ITileMap1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.TileMaps
{
    public interface ITileMap1D : ITileMap
    {
        int GridLength
        {
            get;
            set;
        }

        int ZeroTile
        {
            get;
            set;
        }

        int ViewableTiles
        {
            get;
            set;
        }

        int ActiveTile
        {
            get;
            set;
        }
    }
}
