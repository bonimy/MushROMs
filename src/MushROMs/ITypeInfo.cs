// <copyright file="ITypeInfo.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;

    public interface ITypeInfo
    {
        Type Type
        {
            get;
        }

        string DisplayName
        {
            get;
        }

        string Description
        {
            get;
        }
    }
}
