// <copyright file="UnsafeNativeMethods.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Controls
{
    internal static class UnsafeNativeMethods
    {
        internal static int LastWin32Error
        {
            get;
            private set;
        }

        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(
            IntPtr hWnd,
            int nIndex,
            int dwNewLong);

        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static unsafe extern int GetWindowRect(
            IntPtr hWnd,
            WinApiRectangle* lpRect);

        internal static int GetWindowLong(IWin32Window window, int index)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var value = GetWindowLong(window.Handle, index);

            if (value == 0)
            {
                LastWin32Error = Marshal.GetLastWin32Error();
                throw new ErrorCodeException();
            }

            return value;
        }

        internal static int SetWindowLong(
            IWin32Window window,
            int index,
            int value)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var code = SetWindowLong(window.Handle, index, value);

            if (code == 0)
            {
                LastWin32Error = Marshal.GetLastWin32Error();
                throw new ErrorCodeException();
            }

            return code;
        }

        internal static Rectangle GetWindowRect(IWin32Window window)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var rect = WinApiRectangle.Empty;
            int code;

            unsafe
            {
                code = GetWindowRect(window.Handle, &rect);
            }

            if (code == 0)
            {
                LastWin32Error = Marshal.GetLastWin32Error();
                throw new ErrorCodeException();
            }

            return rect;
        }
    }
}
