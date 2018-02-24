// <copyright file="BackgroundType.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.SMB1
{
    /// <summary>
    /// The layer 1 background to use for the current area.
    /// </summary>
    public enum BackgroundType
    {
        /// <summary>
        /// Normal day time area.
        /// </summary>
        DayTime,

        /// <summary>
        /// User for underwater area (e.g. main area of W2-2).
        /// </summary>
        Underwater,

        /// <summary>
        /// A castle wall is behind the player (e.g. main area of W8-3).
        /// </summary>
        CastleWall,

        /// <summary>
        /// Water or lava is at ground level (e.g. main area of W2-3).
        /// </summary>
        OverWater,

        /// <summary>
        /// Normal night time area (e.g. main area of W3-1).
        /// </summary>
        NightTime,

        /// <summary>
        /// Day time area with snow on ground (e.g. main area of W5-1).
        /// </summary>
        DayTimeWithSnow,

        /// <summary>
        /// Night time area with snow on ground (e.g. main area of W6-3).
        /// </summary>
        NightTimeWithSnow,

        /// <summary>
        /// Makes area gray like any castle level.
        /// </summary>
        Castle
    }
}
