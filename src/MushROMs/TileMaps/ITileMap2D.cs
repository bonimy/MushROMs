// <copyright file="ITileMap2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.TileMaps
{
    using System.Drawing;

    public interface ITileMap2D : ITileMap
    {
        Size GridSize
        {
            get;
            set;
        }

        Point ZeroTile
        {
            get;
            set;
        }

        Size ViewableTileRange
        {
            get;
            set;
        }

        Point ActiveTile
        {
            get;
            set;
        }
    }
}
