// <copyright file="DesignControl.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Permissions;
using System.Windows.Forms;
using Helper.PixelFormats;
using static Controls.DrawHelper;

namespace Controls
{
    [DefaultEvent("Paint")]
    [Description("Provides a control to be used for design purposes.")]
    public partial class DesignControl : UserControl
    {
        public static readonly Point MouseOutOfRange = new Point(-1, -1);

        private static readonly CheckerPattern FallbackCheckerPattern = new CheckerPattern(
            SystemColors.ControlLightLight,
            SystemColors.ControlDarkDark,
            new Size(4, 4));

        internal static readonly ICollection<Keys> FallbackOverrideInputKeys = new HashSet<Keys>
        {
            Keys.Up,
            Keys.Up    | Keys.Shift,
            Keys.Up                 | Keys.Control,
            Keys.Up    | Keys.Shift | Keys.Control,
            Keys.Left,
            Keys.Left  | Keys.Shift,
            Keys.Left               | Keys.Control,
            Keys.Left  | Keys.Shift | Keys.Control,
            Keys.Down,
            Keys.Down  | Keys.Shift,
            Keys.Down               | Keys.Control,
            Keys.Down  | Keys.Shift | Keys.Control,
            Keys.Right,
            Keys.Right | Keys.Shift,
            Keys.Right              | Keys.Control,
            Keys.Right | Keys.Shift | Keys.Control
        };

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Event raised when the value of the BorderStyle property is changed on Control.")]
        public event EventHandler BorderStyleChanged;

        [Browsable(true)]   // This event is overridden so it can be browsable in the designer.
        [Category("Mouse")]
        [Description("Occurs when the mouse wheel moves while the control has focus.")]
        public new event MouseEventHandler MouseWheel
        {
            add { base.MouseWheel += value; }
            remove { base.MouseWheel -= value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckerPattern BackgroundPattern
        {
            get;
            set;
        }

        [Browsable(true)]   // This property is overridden so it can be browsable in the designer.
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("A value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new bool DoubleBuffered
        {
            get
            {
                return base.DoubleBuffered;
            }

            set
            {
                base.DoubleBuffered = value;
            }
        }

        [Browsable(true)]   // This property is overridden so it can be browsable in the designer.
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
        public static Keys CurrentKeys
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Keys PreviousKeys
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Keys ActiveKeys
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool ControlKeyHeld
        {
            get
            {
                return (ModifierKeys & Keys.Control) != Keys.None;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool ShiftKeyHeld
        {
            get
            {
                return (ModifierKeys & Keys.Shift) != Keys.None;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool AltKeyHeld
        {
            get
            {
                return (ModifierKeys & Keys.Alt) != Keys.None;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MouseHovering
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point CurrentMousePosition
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point PreviousMousePosition
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static MouseButtons CurrentMouseButtons
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static MouseButtons PreviousMouseButtons
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static MouseButtons ActiveMouseButtons
        {
            get;
            private set;
        }

        // This property is overridden to attach an event to its assignment.
        public new BorderStyle BorderStyle
        {
            get
            {
                return base.BorderStyle;
            }

            set
            {
                base.BorderStyle = value;
                OnBorderStyleChanged(EventArgs.Empty);
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
        }

        protected override void OnLoad(EventArgs e)
        {
            SetTiledBackground(BackgroundPattern);
            base.OnLoad(e);
        }

        public void SetTiledBackground(CheckerPattern pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var size = pattern.Size;
            var size2 = size + size;
            BackgroundImage = new Bitmap(size2.Width, size2.Height, PixelFormat.Format32bppRgb);
            var bmp = (Bitmap)BackgroundImage;
            var data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            var color1 = Color32BppArgbFromColor(pattern.Color1);
            var color2 = Color32BppArgbFromColor(pattern.Color2);

            unsafe
            {
                var pixels = (Color32BppArgb*)data.Scan0;

                for (var y = size.Height; --y >= 0;)
                {
                    for (var x = size.Width; --x >= 0;)
                    {
                        pixels[(y * size2.Width) + x] = color1;
                        pixels[(y * size2.Width) + (x + size.Width)] = color2;
                        pixels[((y + size.Height) * size2.Width) + x] = color2;
                        pixels[((y + size.Height) * size2.Width) + (x + size.Width)] = color1;
                    }
                }
            }

            bmp.UnlockBits(data);
            BackgroundImageLayout = ImageLayout.Tile;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
            case WindowMessages.KeyDown:
            case WindowMessages.SystemKeyDown:
            PreviousKeys = CurrentKeys;
            CurrentKeys = (Keys)m.WParam | ModifierKeys;
            ActiveKeys = CurrentKeys & ~PreviousKeys;
            break;

            case WindowMessages.KeyUp:
            case WindowMessages.SystemKeyUp:
            PreviousKeys = CurrentKeys;
            CurrentKeys &= ~((Keys)m.WParam | ModifierKeys);
            ActiveKeys = Keys.None;
            break;

            case WindowMessages.MouseLeave:
            PreviousMousePosition = CurrentMousePosition;
            CurrentMousePosition = MouseOutOfRange;
            break;

            case WindowMessages.MouseMove:
            PreviousMousePosition = CurrentMousePosition;
            CurrentMousePosition = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 0x10);
            MouseHovering = PreviousMousePosition == CurrentMousePosition;
            break;
            }

            base.DefWndProc(ref m);
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

        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            BorderStyleChanged?.Invoke(this, e);
        }
    }
}
