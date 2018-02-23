// <copyright file="GrayscaleForm.cs" company="Public Domain">
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
    internal sealed partial class GrayscaleForm : Form
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

        public GrayscaleForm()
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

        private void Luma_Click(object sender, EventArgs e)
        {
            RunEvent = false;
            ltbRed.Value = (int)Round(ColorF.LumaRedWeight * 100.0f);
            ltbGreen.Value = (int)Round(ColorF.LumaGreenWeight * 100.0f);
            ltbBlue.Value = (int)Round(ColorF.LumaBlueWeight * 100.0f);
            RunEvent = true;

            OnColorValueChanged(EventArgs.Empty);
        }

        private void Even_Click(object sender, EventArgs e)
        {
            RunEvent = false;
            ltbRed.Value = 100;
            ltbGreen.Value = 100;
            ltbBlue.Value = 100;
            RunEvent = true;

            OnColorValueChanged(EventArgs.Empty);
        }

        private void Color_ValueChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void Preview_CheckedChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }
    }
}
