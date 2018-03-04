// <copyright file="GfxFormatter.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System.Collections.Generic;

    public abstract class GfxFormatter
    {
        public static readonly GfxFormatter Chr = new ChrFormatter();

        public abstract Gfx CreateGfx(byte[] data);

        public abstract byte[] CreateData(Gfx gfx);

        private class ChrFormatter : GfxFormatter
        {
            public override Gfx CreateGfx(byte[] data)
            {
                return new Gfx(data);
            }

            public override byte[] CreateData(Gfx gfx)
            {
                return new List<byte>(gfx).ToArray();
            }
        }
    }
}
