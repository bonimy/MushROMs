// <copyright file="DesignControl.Mouse.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Controls
{
    partial class DesignControl
    {
        public static readonly Point MouseOutOfRange = new Point(
            Int32.MinValue,
            Int32.MinValue);

        // This event is overridden so it can be browsable in the designer.
        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse wheel moves while the control has focus.")]
        public new event MouseEventHandler MouseWheel
        {
            add
            {
                base.MouseWheel += value;
            }

            remove
            {
                base.MouseWheel -= value;
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

        private void ProcessMouseLeave(ref Message m)
        {
            PreviousMousePosition = CurrentMousePosition;
            CurrentMousePosition = MouseOutOfRange;

            PreviousMouseButtons = CurrentMouseButtons;
            CurrentMouseButtons = MouseButtons.None;
            ActiveMouseButtons = MouseButtons.None;
        }

        private void ProcessMouseMove(ref Message m)
        {
            PreviousMousePosition = CurrentMousePosition;
            CurrentMousePosition = new Point(
                (int)m.LParam & 0xFFFF,
                (int)m.LParam >> 0x10);

            MouseHovering = PreviousMousePosition == CurrentMousePosition;

            PreviousMouseButtons = CurrentMouseButtons;
            CurrentMouseButtons = MouseButtons;
            ActiveMouseButtons = CurrentMouseButtons & ~PreviousMouseButtons;
        }
    }
}
