// <copyright file="IExceptionHandler.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;

    public interface IExceptionHandler
    {
        void ShowException(Exception ex);

        bool ShowExceptionAndRetry(Exception ex);
    }
}
