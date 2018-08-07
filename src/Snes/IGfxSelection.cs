// <copyright file="IGfxSelection.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using MushROMs.TileMaps;

    public interface IGfxSelection : ISelection1D
    {
        GraphicsFormat GraphicsFormat
        {
            get;
        }

        int BytesPerTile
        {
            get;
        }

        IGfxSelection Move(int startAddress);
    }
}
