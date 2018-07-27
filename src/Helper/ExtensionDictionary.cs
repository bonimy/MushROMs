// <copyright file="ExtensionDictionary.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using System.Runtime.Serialization;

    public class ExtensionDictionary<TValue> :
        AssertDictionary<string, TValue>
    {
        public ExtensionDictionary()
            : base(ExtensionComparer.Default)
        {
        }

        public ExtensionDictionary(int capacity)
            : base(capacity, ExtensionComparer.Default)
        {
        }

        public ExtensionDictionary(ExtensionComparer comparer)
            : base(comparer ?? ExtensionComparer.Default)
        {
        }

        public ExtensionDictionary(
            ExtensionDictionary<TValue> dictionary)
            : base(dictionary)
        {
        }

        public ExtensionDictionary(
            int capacity,
            ExtensionComparer comparer)
            : base(capacity, comparer ?? ExtensionComparer.Default)
        {
        }

        public ExtensionDictionary(
            ExtensionDictionary<TValue> dictionary,
            ExtensionComparer comparer)
            : base(dictionary, comparer ?? ExtensionComparer.Default)
        {
        }

        protected ExtensionDictionary(
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

            var comparer = Comparer as ExtensionComparer;

            try
            {
                comparer.StringModifier(key);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(
                    ex.Message,
                    nameof(key),
                    ex);
            }
        }
    }
}
