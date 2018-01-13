// <copyright file="BlendForm.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using Helper;
using static System.Math;

namespace Controls.Editors
{
    internal sealed partial class BlendForm : DialogForm
    {
        public event EventHandler ColorValueChanged;

        private readonly DialogProxy _dialogForm;

        protected override object ProxySender
        {
            get
            {
                if (_dialogForm != null)
                {
                    return _dialogForm;
                }

                return base.ProxySender;
            }
        }

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
                Red = Color.Red;
                Green = Color.Green;
                Blue = Color.Blue;
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
            get { return chkPreview.Checked; }
            set { chkPreview.Checked = value; }
        }

        public BlendForm()
        {
            InitializeComponent();

            ResetValues();
            RunEvent = true;
        }

        internal BlendForm(DialogProxy dialogForm) : this()
        {
            _dialogForm = dialogForm;
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
                ColorValueChanged?.Invoke(ProxySender, e);
            }
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

        private void BlendMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }
    }
}
