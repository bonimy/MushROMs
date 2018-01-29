// <copyright file="GrayscaleForm.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using Helper;
using static System.Math;

namespace Controls.Editors
{
    internal sealed partial class GrayscaleForm : DialogForm, IDialogForm
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

        internal GrayscaleForm(DialogProxy dialogForm) : this()
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
