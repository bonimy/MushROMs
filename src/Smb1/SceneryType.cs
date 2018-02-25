// <copyright file="SceneryType.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Smb1
{
    /// <summary>
    /// The layer 1 scenery to draw for the area.
    /// </summary>
    public enum SceneryType
    {
        /// <summary>
        /// Use no scenery.
        /// </summary>
        None,

        /// <summary>
        /// Clouds in sky.
        /// </summary>
        Clouds,

        /// <summary>
        /// Mountains and hills on ground.
        /// </summary>
        MountainsAndHills,

        /// <summary>
        /// Fences and trees on ground.
        /// </summary>
        FenceAndTrees
    }
}
