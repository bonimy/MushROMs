// <copyright file="SaveFileAssociations.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Helper;

    public class SaveFileAssociations :
        IReadOnlyDictionary<string, SaveEditorCallback>
    {
        public SaveFileAssociations(
            ExtensionDictionary<SaveEditorCallback> associations,
            SaveEditorCallback defaultAssociation)
        {
            Associations = new ExtensionDictionary<SaveEditorCallback>(
                associations);

            DefaultAssociation = defaultAssociation ??
                throw new ArgumentNullException(
                    nameof(defaultAssociation));
        }

        public int Count
        {
            get
            {
                return Associations.Count;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return Associations.Keys;
            }
        }

        public IEnumerable<SaveEditorCallback> Values
        {
            get
            {
                return Associations.Values;
            }
        }

        private ExtensionDictionary<SaveEditorCallback> Associations
        {
            get;
        }

        private SaveEditorCallback DefaultAssociation
        {
            get;
        }

        public SaveEditorCallback this[string key]
        {
            get
            {
                if (TryGetValue(key, out var value))
                {
                    return value;
                }

                return DefaultAssociation;
            }
        }

        public bool ContainsKey(string key)
        {
            return Associations.ContainsKey(key);
        }

        public bool TryGetValue(
            string key,
            out SaveEditorCallback value)
        {
            return Associations.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, SaveEditorCallback>>
            GetEnumerator()
        {
            return Associations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
