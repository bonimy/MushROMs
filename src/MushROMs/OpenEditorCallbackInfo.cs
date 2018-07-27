// <copyright file="OpenEditorCallbackInfo.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;

    public class OpenEditorCallbackInfo
    {
        public OpenEditorCallbackInfo(
            OpenEditorCallback openEditorMethod,
            string displayName,
            string description)
        {
            OpenEditorMethod = openEditorMethod ??
                throw new ArgumentNullException(
                    nameof(openEditorMethod));

            DisplayName = displayName;
            Description = description;
        }

        public OpenEditorCallback OpenEditorMethod
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
