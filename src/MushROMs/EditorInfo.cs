// <copyright file="EditorInfo.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;

    public class EditorInfo : ITypeInfo
    {
        public EditorInfo(
            Type type,
            string displayName)
            : this(type, displayName, null)
        {
        }

        public EditorInfo(
            Type editorType,
            string displayName,
            string description)
        {
            if (editorType == null)
            {
                throw new ArgumentNullException(nameof(editorType));
            }

            if (editorType.GetInterface(typeof(IEditor).FullName) == null)
            {
                throw new ArgumentException();
            }

            Type = editorType;
            DisplayName = displayName ?? editorType.Name;
            Description = description;
        }

        public Type Type
        {
            get;
        }

        public string DisplayName
        {
            get;
        }

        public string Description
        {
            get;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
