// <copyright file="ISelection1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System.Collections.Generic;

    public interface ISelection1D : IReadOnlyList<int>
    {
        int StartIndex { get; }

        bool Contains(int index);

        ISelection1D Copy();
    }
}
