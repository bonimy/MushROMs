// <copyright file="DesignForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Controls
{
    public class DesignForm : Form
    {
        internal static readonly ICollection<Keys> FallbackOverrideInputKeys = DesignControl.FallbackOverrideInputKeys;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size FormBorderSize
        {
            get
            {
                return GetFormBorderSize(this);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CaptionHeight
        {
            get
            {
                return GetCaptionHeight(this);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding BorderPadding
        {
            get
            {
                return GetFormBorderPadding(this);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding WindowPadding
        {
            get
            {
                var padding = BorderPadding;
                padding.Top += CaptionHeight;
                return padding;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle AbsoluteCoordinates
        {
            get
            {
                return WinAPIMethods.GetWindowRectangle(this);
            }
        }

        public DesignForm()
        {
            KeyPreview = true;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (FallbackOverrideInputKeys.Contains(keyData))
            {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (FallbackOverrideInputKeys.Contains(keyData))
            {
                return false;
            }

            return base.ProcessDialogKey(keyData);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WindowMessages.Size:
                    if (m.WParam == IntPtr.Zero)
                    {
                        unsafe
                        {
                            var client = new Size((int)m.LParam & 0xFFFF, (int)m.LParam >> 0x10);
                            var window = WinAPIMethods.InflateSize(client, WindowPadding);
                            window = AdjustSize(window);
                            client = WinAPIMethods.DeflateSize(window, WindowPadding);
                            m.LParam = (IntPtr)((client.Width & 0xFFFF) | ((client.Height & 0xFFFF) << 0x10));
                        }
                    }

                    break;

                case WindowMessages.Sizing:
                    unsafe
                    {
                        var rect = (WinAPIRectangle*)m.LParam;
                        *rect = AdjustSizingRectangle(*rect);
                    }

                    break;
            }

            base.DefWndProc(ref m);
        }

        protected virtual Rectangle AdjustSizingRectangle(Rectangle window)
        {
            return window;
        }

        protected virtual Size AdjustSize(Size window)
        {
            return window;
        }

        public static Size GetFormBorderSize(Form form)
        {
            if (form is null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            switch (form.FormBorderStyle)
            {
                case FormBorderStyle.None:
                    return Size.Empty;

                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.FixedDialog:
                case FormBorderStyle.Sizable:
                    return SystemInformation.FrameBorderSize +
                        WinAPIMethods.PaddedBorderSize;

                case FormBorderStyle.FixedToolWindow:
                case FormBorderStyle.SizableToolWindow:
                    return SystemInformation.FixedFrameBorderSize +
                        WinAPIMethods.PaddedBorderSize;

                case FormBorderStyle.Fixed3D:
                    return SystemInformation.FrameBorderSize +
                        SystemInformation.Border3DSize +
                        WinAPIMethods.PaddedBorderSize;

                default:
                    throw new ArgumentException();
            }
        }

        public static Padding GetFormBorderPadding(Form form)
        {
            var sz = GetFormBorderSize(form);
            return new Padding(sz.Width, sz.Height, sz.Width, sz.Height);
        }

        public static int GetCaptionHeight(Form form)
        {
            if (form is null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            switch (form.FormBorderStyle)
            {
                case FormBorderStyle.None:
                    return 0;

                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.Fixed3D:
                case FormBorderStyle.FixedDialog:
                case FormBorderStyle.Sizable:
                    return SystemInformation.CaptionHeight;

                case FormBorderStyle.FixedToolWindow:
                case FormBorderStyle.SizableToolWindow:
                    return SystemInformation.ToolWindowCaptionHeight;

                default:
                    throw new ArgumentException();
            }
        }
    }
}
