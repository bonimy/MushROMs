// <copyright file="CompressCommand.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    public enum CompressCommand
    {
        DirectCopy = 0,
        RepeatedByte = 1,
        RepeatedWord = 2,
        IncrementingByte = 3,
        CopySection = 4,
        LongCommand = 7
    }
}
