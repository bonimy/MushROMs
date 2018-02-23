// <copyright file="TileMapResizeMode.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

/* An enumeration for controlling how a change in tilemap size is affected throughout the program flow. For example, if we change the tilemap's view size, then we need to change the control's client size, and then its parent form's size.

But if we instead change the parent form's size first, then we need to change the view size, and then client size last. Also, we need to modify the form size according by discrete amounts to bind to the
 */

namespace Controls
{
    public enum TileMapResizeMode
    {
        None,
        TileMapCellResize,
        ControlResize,
        FormResize
    }
}
