// <copyright file="ITileMap.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.TileMaps
{
    using System.Drawing;

    public interface ITileMap
    {
        Size ViewSize
        {
            get;
            set;
        }
    }
}
