// <copyright file="GraphicsFormat.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public enum GraphicsFormat
    {
        Format1Bpp8x8 = 0x11,
        Format2BppNes = 0x22,
        Format2BppGb = 0x32,
        Format2BppNgp = 0x42,
        Format2BppVb = 0x52,
        Format3BppSnes = 0x03,
        Format3Bpp8x8 = 0x13,
        Format4BppSnes = 0x04,
        Format4BppGba = 0x14,
        Format4BppSms = 0x24,
        Format4BppMsx2 = 0x34,
        Format4Bpp8x8 = 0x44,
        Format8BppSnes = 0x08,
        Format8BppMode7 = 0x78
    }
}
