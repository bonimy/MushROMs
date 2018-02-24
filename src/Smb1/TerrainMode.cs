// <copyright file="TerrainMode.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.SMB1
{
    /// <summary>
    /// The floor plan to use in the area.
    /// </summary>
    public enum TerrainMode
    {
        /// <summary>
        /// N gorund.
        /// </summary>
        None,

        /// <summary>
        /// 2 tile high floor with no ceiling.
        /// </summary>
        Ceiling0Floor2,

        /// <summary>
        /// 2 tile high floor and 1 tile high ceiling.
        /// </summary>
        Ceiling1Floor2,

        /// <summary>
        /// 2 tile high floor and 3 tile high ceiling.
        /// </summary>
        Ceiling3Floor2,

        /// <summary>
        /// 2 tile high floor and 4 tile high ceiling.
        /// </summary>
        Ceiling4Floor2,

        /// <summary>
        /// 2 tile high floor and 8 tile high ceiling.
        /// </summary>
        Ceiling8Floor2,

        /// <summary>
        /// 5 tile high floor and 1 tile high ceiling.
        /// </summary>
        Ceiling1Floor5,

        /// <summary>
        /// 5 tile high floor and 3 tile high ceiling.
        /// </summary>
        Ceiling3Floor5,

        /// <summary>
        /// 5 tile high floor and 4 tile high ceiling.
        /// </summary>
        Ceiling4Floor5,

        /// <summary>
        /// 6 tile high floor and 1 tile high ceiling.
        /// </summary>
        Ceiling1Floor6,

        /// <summary>
        /// No floor and 1 tile high ceiling.
        /// </summary>
        Ceiling1Floor0,

        /// <summary>
        /// 6 tile high floor and 4 tile high ceiling.
        /// </summary>
        Ceiling4Floor6,

        /// <summary>
        /// 9 tile high floor and 1 tile high ceiling.
        /// </summary>
        Ceiling1Floor9,

        /// <summary>
        /// 2 tile high floor, 1 tile high ceiling, and 5 layers in the middle.
        /// </summary>
        Ceiling1Middle5Floor2,

        /// <summary>
        /// 2 tile high floor, 1 tile high ceiling, and 4 layers in the middle.
        /// </summary>
        Ceiling1Middle4Floor2,

        /// <summary>
        /// All heights have floor tile.
        /// </summary>
        Solid
    }
}
