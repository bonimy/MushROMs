// <copyright file="ExtensionComparer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using System.IO;

    public sealed class ExtensionComparer : StringModifierComparer
    {
        public static readonly ExtensionComparer Default = new ExtensionComparer();

        public ExtensionComparer(StringComparer baseComparer)
            : base(baseComparer)
        {
        }

        private ExtensionComparer()
            : this(OrdinalIgnoreCase)
        {
        }

        public override string StringModifier(string value)
        {
            return Path.GetExtension(value) ??
                throw new ArgumentNullException(nameof(BaseComparer));
        }
    }
}
