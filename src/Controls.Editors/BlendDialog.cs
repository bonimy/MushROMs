// <copyright file="BlendDialog.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System;
    using Helper;

    public sealed class BlendDialog : DialogProxy
    {
        public event EventHandler ColorValueChanged;

        private BlendForm BlendForm
        {
            get;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope")]
        public BlendDialog() : base(new BlendForm())
        {
            BlendForm = BaseForm as BlendForm;
            BlendForm.ColorValueChanged += BlendForm_ColorValueChanged;
        }

        private void BlendForm_ColorValueChanged(
            object sender,
            EventArgs e)
        {
            ColorValueChanged?.Invoke(this, e);
        }

        public void ResetValues()
        {
            BlendForm.ResetValues();
        }
    }
}
