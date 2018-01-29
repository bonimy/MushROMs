// <copyright file="DesignControl.cs" company="Public Domain">
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
    public delegate void ProcessMessage(ref Message m);

    [DefaultEvent("Paint")]
    [Description("Provides a control to be used for design purposes.")]
    public partial class DesignControl : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private IReadOnlyDictionary<int, ProcessMessage> ProcedureOverrides
        {
            get;
        }

        // This property is overridden so it can be browsable in the designer.
        [Browsable(true)]
        [Description("The size of the client area of the form.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new Size ClientSize
        {
            get
            {
                return base.ClientSize;
            }

            set
            {
                base.ClientSize = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ClientWidth
        {
            get
            {
                return ClientSize.Width;
            }

            set
            {
                ClientSize = new Size(value, ClientHeight);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ClientHeight
        {
            get
            {
                return ClientSize.Height;
            }

            set
            {
                ClientSize = new Size(ClientWidth, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size BorderSize
        {
            get
            {
                switch (BorderStyle)
                {
                    case BorderStyle.None:
                        return Size.Empty;

                    case BorderStyle.FixedSingle:
                        return SystemInformation.BorderSize;

                    case BorderStyle.Fixed3D:
                        return SystemInformation.Border3DSize;

                    default:
                        throw new InvalidEnumArgumentException(
                            nameof(BorderStyle),
                            (int)BorderStyle,
                            typeof(BorderStyle));
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding BorderPadding
        {
            get
            {
                var sz = BorderSize;
                return new Padding(sz.Width, sz.Height, sz.Width, sz.Height);
            }
        }

        public DesignControl()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BorderStyle = BorderStyle.FixedSingle;
            BackgroundPattern = FallbackCheckerPattern;
            SetTiledBackground(BackgroundPattern);

            ProcedureOverrides = new Dictionary<int, ProcessMessage>()
            {
                { WindowMessages.KeyDown, ProcessKeyDown },
                { WindowMessages.SystemKeyDown, ProcessKeyDown },
                { WindowMessages.KeyUp, ProcessKeyUp },
                { WindowMessages.SystemKeyUp, ProcessKeyUp },
                { WindowMessages.MouseMove, ProcessMouseMove },
                { WindowMessages.MouseLeave, ProcessMouseLeave }
            };
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
    }
}
