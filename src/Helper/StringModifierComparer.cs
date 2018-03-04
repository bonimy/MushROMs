// <copyright file="StringModifierComparer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;

    public abstract class StringModifierComparer : StringComparer
    {
        protected StringModifierComparer()
            : this(CurrentCulture)
        {
        }

        protected StringModifierComparer(StringComparer baseComparer)
        {
            BaseComparer = baseComparer ??
                throw new ArgumentNullException(nameof(baseComparer));
        }

        public virtual StringComparer BaseComparer
        {
            get;
        }

        public override sealed int Compare(string x, string y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null || y is null)
            {
                return BaseComparer.Compare(x, y);
            }

            var modifiedX = StringModifier(x);
            var modifiedY = StringModifier(y);

            return BaseComparer.Compare(modifiedX, modifiedY);
        }

        public override sealed bool Equals(string x, string y)
        {
            if (BaseComparer.Equals(x, y))
            {
                return true;
            }

            string modifiedX, modifiedY;
            try
            {
                modifiedX = StringModifier(x);
                modifiedY = StringModifier(y);
            }
            catch
            {
                return false;
            }

            return BaseComparer.Equals(modifiedX, modifiedY);
        }

        public override sealed int GetHashCode(string obj)
        {
            var modified = StringModifier(obj);
            return BaseComparer.GetHashCode(modified);
        }

        public abstract string StringModifier(string value);
    }
}
