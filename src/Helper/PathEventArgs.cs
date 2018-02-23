// <copyright file="PathEventArgs.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;

    public class PathEventArgs : EventArgs
    {
        public PathEventArgs(string path)
        {
            Path = path ??
                throw new ArgumentNullException(nameof(path));
        }

        public string Path
        {
            get;
            set;
        }
    }
}
