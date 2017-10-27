// <copyright file="WinApiMethods.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Controls.UnsafeNativeMethods;

namespace Controls
{
    public static class WinAPIMethods
    {
        private const int GWL_EXSTYLE = -20;
        private const int GWL_STYLE = -16;

        private const int WS_BORDER = 0x800000;
        private const int WS_EX_CLIENTEDGE = 0x200;

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOREDRAW = 0x0008;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_HIDEWINDOW = 0x0080;
        private const uint SWP_NOCOPYBITS = 0x0100;
        private const uint SWP_NOOWNERZORDER = 0x0200;
        private const uint SWP_NOSENDCHANGING = 0x0400;

        private const int SM_CXPADDEDBORDER = 92;

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        public static int PaddedBorderWidth
        {
            get
            {
                return GetSystemMetrics(SM_CXPADDEDBORDER);
            }
        }

        public static Size PaddedBorderSize
        {
            get
            {
                return new Size(PaddedBorderWidth, PaddedBorderWidth);
            }
        }

        public static BorderStyle GetBorderStyle(IWin32Window window)
        {
            if ((GetWindowLong(window, GWL_EXSTYLE) & WS_EX_CLIENTEDGE) != 0)
            {
                return BorderStyle.Fixed3D;
            }

            if ((GetWindowLong(window, GWL_STYLE) & WS_BORDER) != 0)
            {
                return BorderStyle.FixedSingle;
            }

            return BorderStyle.None;
        }

        public static void SetBorderStyle(IWin32Window window, BorderStyle value)
        {
            var style = GetWindowLong(window, GWL_STYLE) & ~WS_BORDER;
            var exstyle = GetWindowLong(window, GWL_EXSTYLE) & ~WS_EX_CLIENTEDGE;

            switch (value)
            {
            case BorderStyle.Fixed3D:
            SetWindowLong(window, GWL_STYLE, style);
            SetWindowLong(window, GWL_EXSTYLE, exstyle | WS_EX_CLIENTEDGE);
            return;

            case BorderStyle.FixedSingle:
            SetWindowLong(window, GWL_STYLE, style | WS_BORDER);
            SetWindowLong(window, GWL_EXSTYLE, exstyle);
            return;

            case BorderStyle.None:
            SetWindowLong(window, GWL_STYLE, style);
            SetWindowLong(window, GWL_EXSTYLE, exstyle);
            return;

            default:
            return;
            }
        }

        public static Rectangle GetWindowRectangle(IWin32Window window)
        {
            // Do not call method in window's constructor!
            return GetWindowRect(window);
        }

        public static Size GetBorderSize(IWin32Window window)
        {
            switch (GetBorderStyle(window))
            {
            case BorderStyle.None:
            return Size.Empty;

            case BorderStyle.FixedSingle:
            return SystemInformation.BorderSize;

            case BorderStyle.Fixed3D:
            return SystemInformation.Border3DSize;

            default:
            throw new ArgumentException();
            }
        }

        public static Padding GetBorderPadding(IWin32Window window)
        {
            var sz = GetBorderSize(window);
            return new Padding(sz.Width, sz.Height, sz.Width, sz.Height);
        }

        public static Padding GetPadding(Rectangle large, Rectangle small)
        {
            return new Padding(
                small.Left - large.Left,
                small.Top - large.Top,
                large.Right - small.Right,
                large.Bottom - small.Bottom);
        }

        public static Rectangle InflateRectangle(Rectangle rectangle, Padding padding)
        {
            return Rectangle.FromLTRB(
                rectangle.Left - padding.Left,
                rectangle.Top - padding.Top,
                rectangle.Right + padding.Right,
                rectangle.Bottom + padding.Bottom);
        }

        public static Rectangle DeflateRectangle(Rectangle rectangle, Padding padding)
        {
            return InflateRectangle(rectangle, Padding.Empty - padding);
        }

        public static Size InflateSize(Size size, Padding padding)
        {
            return InflateRectangle(new Rectangle(Point.Empty, size), padding).Size;
        }

        public static Size DeflateSize(Size size, Padding padding)
        {
            return DeflateRectangle(new Rectangle(Point.Empty, size), padding).Size;
        }
    }
}
