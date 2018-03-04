// <copyright file="TileFlipMode.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;

    [Flags]
    public enum TileFlipMode
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVeritcal = 2,
        FlipBoth = FlipHorizontal | FlipVeritcal
    }
}
