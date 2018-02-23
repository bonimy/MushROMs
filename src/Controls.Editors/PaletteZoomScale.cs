// <copyright file="PaletteZoomScale.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1027:MarkEnumsWithFlags",
        Justification = "Not Flags")]
    public enum PaletteZoomScale
    {
        Zoom8x = 8,
        Zoom16x = 0x10,
        Zoom24x = 0x18,
        Zoom32x = 0x20
    }
}
