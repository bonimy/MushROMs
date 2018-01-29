// <copyright file="DesignForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Controls
{
    public class DesignForm : Form
    {
        internal static readonly ICollection<Keys> FallbackOverrideInputKeys = DesignControl.FallbackOverrideInputKeys;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private IReadOnlyDictionary<int, ProcessMessage> ProcedureOverrides
        {
            get;
        }

        [Browsable(true)]
        [Description("Preprocess the window rectangle before applying it during a resize operation.")]
        public event EventHandler<EventArgs<Rectangle>> AdjustWindowBounds;

        [Browsable(true)]
        [Description("Preprocess the window size before applying it during a resize operation.")]
        public event EventHandler<EventArgs<Size>> AdjustWindowSize;

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
                return WinApiMethods.GetWindowRectangle(this);
            }
        }

        public DesignForm()
        {
            KeyPreview = true;

            ProcedureOverrides = new Dictionary<int, ProcessMessage>()
            {
                { WindowMessages.Size, ProcessSize },
                { WindowMessages.Sizing, ProcessSizing }
            };
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (FallbackOverrideInputKeys.Contains(keyData))
            {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        [UIPermission(
            SecurityAction.LinkDemand,
            Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (FallbackOverrideInputKeys.Contains(keyData))
            {
                return false;
            }

            return base.ProcessDialogKey(keyData);
        }

        [SecurityPermission(
            SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void DefWndProc(ref Message m)
        {
            if (ProcedureOverrides.TryGetValue(m.Msg, out var action))
            {
                action(ref m);
            }

            base.DefWndProc(ref m);
        }

        private void ProcessSize(ref Message m)
        {
            if (m.WParam == IntPtr.Zero)
            {
                var clientSize = new Size(
                    (int)m.LParam & 0xFFFF,
                    (int)m.LParam >> 0x10);

                var windowSize = WinApiMethods.InflateSize(
                    clientSize,
                    WindowPadding);

                var e = new EventArgs<Size>(windowSize);
                OnAdjustWindowSize(e);

                var adjustedWindowSize = e.Data;

                var adjustedClientSize = WinApiMethods.DeflateSize(
                    adjustedWindowSize,
                    WindowPadding);

                m.LParam = (IntPtr)(
                    (adjustedClientSize.Width & 0xFFFF) |
                    ((adjustedClientSize.Height & 0xFFFF) << 0x10));
            }
        }

        private void ProcessSizing(ref Message m)
        {
            var windowBounds = Marshal.PtrToStructure<WinAPIRectangle>(m.LParam);

            var e = new EventArgs<Rectangle>(windowBounds);
            OnAdjustWindowBounds(e);

            WinAPIRectangle adjustedWindowBounds = e.Data;

            Marshal.StructureToPtr(adjustedWindowBounds, m.LParam, false);
        }

        protected virtual void OnAdjustWindowBounds(EventArgs<Rectangle> e)
        {
            AdjustWindowBounds?.Invoke(this, e);
        }

        protected virtual void OnAdjustWindowSize(EventArgs<Size> e)
        {
            AdjustWindowSize?.Invoke(this, e);
        }

        public static Size GetFormBorderSize(Form form)
        {
            if (form is null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            return GetFormBorderSize(form.FormBorderStyle);
        }

        public static Size GetFormBorderSize(FormBorderStyle formBorderStyle)
        {
            switch (formBorderStyle)
            {
                case FormBorderStyle.None:
                    return Size.Empty;

                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.FixedDialog:
                case FormBorderStyle.Sizable:
                    return
                        SystemInformation.FrameBorderSize +
                        WinApiMethods.PaddedBorderSize;

                case FormBorderStyle.FixedToolWindow:
                case FormBorderStyle.SizableToolWindow:
                    return
                        SystemInformation.FixedFrameBorderSize +
                        WinApiMethods.PaddedBorderSize;

                case FormBorderStyle.Fixed3D:
                    return
                        SystemInformation.FrameBorderSize +
                        SystemInformation.Border3DSize +
                        WinApiMethods.PaddedBorderSize;

                default:
                    throw new InvalidEnumArgumentException(
                        nameof(formBorderStyle),
                        (int)formBorderStyle,
                        typeof(FormBorderStyle));
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

            return GetCaptionHeight(form.FormBorderStyle);
        }

        public static int GetCaptionHeight(FormBorderStyle formBorderStyle)
        {
            switch (formBorderStyle)
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
                    throw new InvalidEnumArgumentException(
                        nameof(formBorderStyle),
                        (int)formBorderStyle,
                        typeof(FormBorderStyle));
            }
        }
    }
}
