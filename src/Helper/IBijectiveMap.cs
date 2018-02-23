// <copyright file="IBijectiveMap.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System.Collections.Generic;

    public interface IBijectiveMap<TKey, TValue> :
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>
    {
        IBijectiveMap<TValue, TKey> Reverse
        {
            get;
        }
    }
}
