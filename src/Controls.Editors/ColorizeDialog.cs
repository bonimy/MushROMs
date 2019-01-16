// <copyright file="ColorizeDialog.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System;

    public sealed class ColorizeDialog : DialogProxy
    {
        public event EventHandler ColorValueChanged;

        private ColorizeForm ColorizeForm
        {
            get;
        }

        public float Hue
        {
            get
            {
                return ColorizeForm.Hue;
            }

            set
            {
                ColorizeForm.Hue = value;
            }
        }

        public float Saturation
        {
            get
            {
                return ColorizeForm.Saturation;
            }

            set
            {
                ColorizeForm.Saturation = value;
            }
        }

        public float Lightness
        {
            get
            {
                return ColorizeForm.Lightness;
            }

            set
            {
                ColorizeForm.Lightness = value;
            }
        }

        public float Weight
        {
            get
            {
                return ColorizeForm.Weight;
            }

            set
            {
                ColorizeForm.Weight = value;
            }
        }

        public ColorizeMode ColorizeMode
        {
            get
            {
                return ColorizeForm.ColorizeMode;
            }

            set
            {
                ColorizeForm.ColorizeMode = value;
            }
        }

        public bool Luma
        {
            get
            {
                return ColorizeForm.Luma;
            }

            set
            {
                ColorizeForm.Luma = value;
            }
        }

        public bool Preview
        {
            get
            {
                return ColorizeForm.Preview;
            }

            set
            {
                ColorizeForm.Preview = value;
            }
        }

        public ColorizeDialog() : base(new ColorizeForm())
        {
            ColorizeForm = BaseForm as ColorizeForm;
            ColorizeForm.ColorValueChanged +=
                ColorizeForm_ColorValueChanged;
        }

        private void ColorizeForm_ColorValueChanged(
            object sender,
            EventArgs e)
        {
            ColorValueChanged?.Invoke(this, e);
        }

        public void ResetValues()
        {
            ColorizeForm.ResetValues();
        }
    }
}
