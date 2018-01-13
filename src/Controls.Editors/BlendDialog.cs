// <copyright file="BlendDialog.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using Helper;

namespace Controls.Editors
{
    public sealed class BlendDialog : DialogProxy
    {
        private BlendForm BlendForm
        {
            get;
        }

        protected sealed override IDialogForm BaseForm
        {
            get
            {
                return BlendForm;
            }
        }

        public event EventHandler ValueChanged
        {
            add
            {
                BlendForm.ColorValueChanged += value;
            }

            remove
            {
                BlendForm.ColorValueChanged -= value;
            }
        }

        public float Red
        {
            get
            {
                return BlendForm.Red;
            }

            set
            {
                BlendForm.Red = value;
            }
        }

        public float Green
        {
            get
            {
                return BlendForm.Green;
            }

            set
            {
                BlendForm.Green = value;
            }
        }

        public float Blue
        {
            get
            {
                return BlendForm.Blue;
            }

            set
            {
                BlendForm.Blue = value;
            }
        }

        public ColorF Color
        {
            get
            {
                return BlendForm.Color;
            }

            set
            {
                BlendForm.Color = value;
            }
        }

        public BlendMode BlendMode
        {
            get
            {
                return BlendForm.BlendMode;
            }

            set
            {
                BlendForm.BlendMode = value;
            }
        }

        public bool Preview
        {
            get
            {
                return BlendForm.Preview;
            }

            set
            {
                BlendForm.Preview = value;
            }
        }

        public BlendDialog()
        {
            BlendForm = new BlendForm(this);
        }

        public void ResetValues()
        {
            BlendForm.ResetValues();
        }
    }
}
