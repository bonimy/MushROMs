// <copyright file="DesignControl.Keys.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Windows.Forms;

namespace Controls
{
    partial class DesignControl
    {
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

        protected override bool IsInputKey(Keys keyData)
        {
            if (FallbackOverrideInputKeys.Contains(keyData))
            {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        [SecuritySafeCritical]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (FallbackOverrideInputKeys.Contains(keyData))
            {
                return false;
            }

            return base.ProcessDialogKey(keyData);
        }

        private void ProcessKeyDown(ref Message m)
        {
            PreviousKeys = CurrentKeys;
            CurrentKeys = (Keys)m.WParam | ModifierKeys;
            ActiveKeys = CurrentKeys & ~PreviousKeys;
        }

        private void ProcessKeyUp(ref Message m)
        {
            PreviousKeys = CurrentKeys;
            CurrentKeys &= ~((Keys)m.WParam | ModifierKeys);
            ActiveKeys = Keys.None;
        }
    }
}
