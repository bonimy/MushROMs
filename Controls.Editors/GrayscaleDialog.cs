// <copyright file="GrayscaleDialog.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using Helper;

namespace Controls.Editors
{
    public sealed class GrayscaleDialog : DialogProxy
    {
        private GrayscaleForm GrayscaleForm
        {
            get;
        }

        protected sealed override IDialogForm BaseForm
        {
            get
            {
                return GrayscaleForm;
            }
        }

        public event EventHandler ValueChanged
        {
            add
            {
                GrayscaleForm.ColorValueChanged += value;
            }

            remove
            {
                GrayscaleForm.ColorValueChanged -= value;
            }
        }

        public float Red
        {
            get
            {
                return GrayscaleForm.Red;
            }

            set
            {
                GrayscaleForm.Red = value;
            }
        }

        public float Green
        {
            get
            {
                return GrayscaleForm.Green;
            }

            set
            {
                GrayscaleForm.Green = value;
            }
        }

        public float Blue
        {
            get
            {
                return GrayscaleForm.Blue;
            }

            set
            {
                GrayscaleForm.Blue = value;
            }
        }

        public ColorF Color
        {
            get
            {
                return GrayscaleForm.Color;
            }

            set
            {
                GrayscaleForm.Color = value;
            }
        }

        public bool Preview
        {
            get
            {
                return GrayscaleForm.Preview;
            }

            set
            {
                GrayscaleForm.Preview = value;
            }
        }

        public GrayscaleDialog()
        {
            GrayscaleForm = new GrayscaleForm(this);
        }

        public void ResetValues()
        {
            GrayscaleForm.ResetValues();
        }
    }
}
