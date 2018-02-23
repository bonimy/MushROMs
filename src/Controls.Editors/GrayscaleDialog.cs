// <copyright file="GrayscaleDialog.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
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

        public event EventHandler ColorValueChanged;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public GrayscaleDialog() : base(new GrayscaleForm())
        {
            GrayscaleForm = BaseForm as GrayscaleForm;
            GrayscaleForm.ColorValueChanged += GrayscaleForm_ColorValueChanged;
        }

        public void ResetValues()
        {
            GrayscaleForm.ResetValues();
        }

        private void GrayscaleForm_ColorValueChanged(object sender, EventArgs e)
        {
            ColorValueChanged?.Invoke(this, e);
        }
    }
}
