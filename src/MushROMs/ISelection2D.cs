// <copyright file="ISelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System.Collections.Generic;
    using System.Drawing;

    public interface ISelection2D : IReadOnlyList<Point>
    {
        Point StartPosition { get; }

        bool Contains(Point point);

        ISelection2D Copy();
    }
}
