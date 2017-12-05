﻿// <copyright file="WinApiMethods.cs" company="Public Domain">
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
        private const int GwlExStyle = -20;
        private const int GwlStyle = -16;

        private const int WsBorder = 0x800000;
        private const int WsExClientEdge = 0x200;

        private const uint SwpNoSize = 0x0001;
        private const uint SwpNoMove = 0x0002;
        private const uint SwpNoZOrder = 0x0004;
        private const uint SwpNoRedraw = 0x0008;
        private const uint SwpNoActivate = 0x0010;
        private const uint SwpFrameChanged = 0x0020;
        private const uint SwpShowWindow = 0x0040;
        private const uint SwpHideWindow = 0x0080;
        private const uint SwpNoCopyBits = 0x0100;
        private const uint SwpNoOwnerZOrder = 0x0200;
        private const uint SwpNoSendChanging = 0x0400;

        private const int SmCenterXPaddedBorder = 92;

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int index);

        public static int PaddedBorderWidth
        {
            get
            {
                return GetSystemMetrics(SmCenterXPaddedBorder);
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
            if ((GetWindowLong(window, GwlExStyle) & WsExClientEdge) != 0)
            {
                return BorderStyle.Fixed3D;
            }

            if ((GetWindowLong(window, GwlStyle) & WsBorder) != 0)
            {
                return BorderStyle.FixedSingle;
            }

            return BorderStyle.None;
        }

        public static void SetBorderStyle(IWin32Window window, BorderStyle value)
        {
            var style = GetWindowLong(window, GwlStyle) & ~WsBorder;
            var exstyle = GetWindowLong(window, GwlExStyle) & ~WsExClientEdge;

            switch (value)
            {
            case BorderStyle.Fixed3D:
            SetWindowLong(window, GwlStyle, style);
            SetWindowLong(window, GwlExStyle, exstyle | WsExClientEdge);
            return;

            case BorderStyle.FixedSingle:
            SetWindowLong(window, GwlStyle, style | WsBorder);
            SetWindowLong(window, GwlExStyle, exstyle);
            return;

            case BorderStyle.None:
            SetWindowLong(window, GwlStyle, style);
            SetWindowLong(window, GwlExStyle, exstyle);
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