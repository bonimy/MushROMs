// <copyright file="PathDictionary.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using System.Runtime.Serialization;

    public class PathDictionary<TValue> : AssertDictionary<string, TValue>
    {
        public PathDictionary()
            : base(PathComparer.Default)
        {
        }

        public PathDictionary(int capacity)
            : base(capacity, PathComparer.Default)
        {
        }

        public PathDictionary(PathComparer comparer)
            : base(comparer ?? PathComparer.Default)
        {
        }

        public PathDictionary(PathDictionary<TValue> dictionary)
            : base(dictionary)
        {
        }

        public PathDictionary(int capacity, PathComparer comparer)
            : base(capacity, comparer ?? PathComparer.Default)
        {
        }

        public PathDictionary(
            PathDictionary<TValue> dictionary,
            PathComparer comparer)
            : base(dictionary, comparer)
        {
        }

        protected PathDictionary(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        protected override void AssertKey(string key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var comparer = Comparer as PathComparer;

            try
            {
                comparer.StringModifier(key);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, nameof(key), ex);
            }
        }
    }
}
