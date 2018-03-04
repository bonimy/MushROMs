// <copyright file="PathComparer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using System.IO;

    public sealed class PathComparer : StringModifierComparer
    {
        public static readonly PathComparer Default = new PathComparer();

        public PathComparer(StringComparer baseComparer)
            : base(baseComparer)
        {
        }

        private PathComparer()
            : this(OrdinalIgnoreCase)
        {
        }

        public override string StringModifier(string value)
        {
            return Path.GetFullPath(value) ??
                throw new ArgumentNullException(nameof(BaseComparer));
        }
    }
}
