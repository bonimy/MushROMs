// <copyright file="IIntegerComponent.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls
{
    using System;

    public interface IIntegerComponent
    {
        event EventHandler ValueChanged;

        int Value { get; set; }
    }
}
