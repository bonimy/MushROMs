// <copyright file="ColorizeDialog.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;

namespace Controls.Editors
{
    public sealed class ColorizeDialog : DialogProxy
    {
        private ColorizeForm ColorizeForm
        {
            get;
        }

        protected override IDialogForm BaseForm
        {
            get { return ColorizeForm; }
        }

        public event EventHandler ValueChanged
        {
            add { ColorizeForm.ColorValueChanged += value; }
            remove { ColorizeForm.ColorValueChanged -= value; }
        }

        public float Hue
        {
            get { return ColorizeForm.Hue; }
            set { ColorizeForm.Hue = value; }
        }

        public float Saturation
        {
            get { return ColorizeForm.Saturation; }
            set { ColorizeForm.Saturation = value; }
        }

        public float Lightness
        {
            get { return ColorizeForm.Lightness; }
            set { ColorizeForm.Lightness = value; }
        }

        public float Weight
        {
            get { return ColorizeForm.Weight; }
            set { ColorizeForm.Weight = value; }
        }

        public ColorizeMode ColorizeMode
        {
            get { return ColorizeForm.ColorizeMode; }
            set { ColorizeForm.ColorizeMode = value; }
        }

        public bool Luma
        {
            get { return ColorizeForm.Luma; }
            set { ColorizeForm.Luma = value; }
        }

        public bool Preview
        {
            get { return ColorizeForm.Preview; }
            set { ColorizeForm.Preview = value; }
        }

        public ColorizeDialog()
        {
            ColorizeForm = new ColorizeForm(this);
        }

        public void ResetValues()
        {
            ColorizeForm.ResetValues();
        }
    }
}
