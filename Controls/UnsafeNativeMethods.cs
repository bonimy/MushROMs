// <copyright file="UnsafeNativeMethods.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Controls
{
    public static class UnsafeNativeMethods
    {
        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static unsafe extern int GetWindowRect(IntPtr hWnd, WinAPIRectangle* lpRect);

        internal static int GetWindowLong(IWin32Window window, int index)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var value = GetWindowLong(window.Handle, index);

            if (value == 0)
            {
                throw new ErrorCodeException();
            }

            return value;
        }

        internal static int SetWindowLong(IWin32Window window, int index, int value)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var code = SetWindowLong(window.Handle, index, value);

            if (code == 0)
            {
                throw new ErrorCodeException();
            }

            return code;
        }

        internal static Rectangle GetWindowRect(IWin32Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var rect = WinAPIRectangle.Empty;
            int code;

            unsafe
            {
                code = GetWindowRect(window.Handle, &rect);
            }

            if (code == 0)
            {
                throw new ErrorCodeException();
            }

            return rect;
        }
    }
}
