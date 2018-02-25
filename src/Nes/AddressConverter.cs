// <copyright file="AddressConverter.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes
{
    public static class AddressConverter
    {
        public static int NesToPc(int nes)
        {
            return nes - 0x7FF0;
        }

        public static int PcToNes(int pc)
        {
            return pc + 0x7FF0;
        }
    }
}
