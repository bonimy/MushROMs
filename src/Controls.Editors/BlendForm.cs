// <copyright file="BlendForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Windows.Forms;
using Helper;
using static System.Math;

namespace Controls.Editors
{
    internal sealed partial class BlendForm : Form
    {
        public event EventHandler ColorValueChanged;

        private bool RunEvent
        {
            get;
            set;
        }

        public float Red
        {
            get
            {
                return (float)ltbRed.Value / ltbRed.Maximum;
            }

            set
            {
                ltbRed.Value = (int)Round(value * ltbRed.Maximum);
            }
        }

        public float Green
        {
            get
            {
                return (float)ltbGreen.Value / ltbGreen.Maximum;
            }

            set
            {
                ltbGreen.Value = (int)Round(value * ltbGreen.Maximum);
            }
        }

        public float Blue
        {
            get
            {
                return (float)ltbBlue.Value / ltbBlue.Maximum;
            }

            set
            {
                ltbBlue.Value = (int)Round(value * ltbBlue.Maximum);
            }
        }

        public ColorF Color
        {
            get
            {
                return ColorF.FromArgb(Red, Green, Blue);
            }

            set
            {
                RunEvent = false;
                Red = value.Red;
                Green = value.Green;
                Blue = value.Blue;
                RunEvent = true;

                OnColorValueChanged(EventArgs.Empty);
            }
        }

        public BlendMode BlendMode
        {
            get
            {
                return (BlendMode)cbxBlendMode.SelectedIndex;
            }

            set
            {
                cbxBlendMode.SelectedIndex = (int)value;
            }
        }

        public bool Preview
        {
            get
            {
                return chkPreview.Checked;
            }

            set
            {
                chkPreview.Checked = value;
            }
        }

        public BlendForm()
        {
            InitializeComponent();

            ResetValues();
            RunEvent = true;
        }

        public void ResetValues()
        {
            RunEvent = false;
            ltbRed.Value = ltbRed.Maximum;
            ltbGreen.Value = ltbGreen.Maximum;
            ltbBlue.Value = ltbBlue.Maximum;
            RunEvent = true;

            OnColorValueChanged(EventArgs.Empty);
        }

        private void OnColorValueChanged(EventArgs e)
        {
            if (RunEvent)
            {
                ColorValueChanged?.Invoke(this, e);
            }
        }

        private void Object_ValueChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }
    }
}
