// <copyright file="SafeNativeMethods.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls
{
    using System.Runtime.InteropServices;

    internal static class SafeNativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int index);
    }
}
