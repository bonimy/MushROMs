// <copyright file="ColorizeForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.ComponentModel;
using System.Windows.Forms;
using static System.Math;

namespace Controls.Editors
{
    internal sealed partial class ColorizeForm : Form
    {
        public event EventHandler ColorValueChanged;

        private static readonly Hsl FallbackAdjust = new Hsl(0, 0, 0);

        private static readonly Hsl FallbackColorize = new Hsl(
            0.25f,
            0.50f,
            0.50f);

        private const float FallbackEffectiveness = 1.00f;

        private Hsl SavedAdjust
        {
            get;
            set;
        }

        private Hsl SavedColorize
        {
            get;
            set;
        }

        private bool RunEvent
        {
            get;
            set;
        }

        public float Hue
        {
            get
            {
                return (float)ltbHue.Value / ltbHue.Maximum;
            }

            set
            {
                ltbHue.Value = (int)Round(value * ltbHue.Maximum);
            }
        }

        public float Saturation
        {
            get
            {
                return (float)ltbSaturation.Value / ltbSaturation.Maximum;
            }

            set
            {
                ltbSaturation.Value = (int)Round(value * ltbSaturation.Maximum);
            }
        }

        public float Lightness
        {
            get
            {
                return (float)ltbLightness.Value / ltbLightness.Maximum;
            }

            set
            {
                ltbLightness.Value = (int)Round(value * ltbLightness.Maximum);
            }
        }

        private Hsl CurrentHSL
        {
            get
            {
                return new Hsl(Hue, Saturation, Lightness);
            }

            set
            {
                RunEvent = false;
                Hue = value.Hue;
                Saturation = value.Saturation;
                Lightness = value.Lightness;
                RunEvent = true;

                OnColorValueChanged(EventArgs.Empty);
            }
        }

        public float Weight
        {
            get
            {
                return (float)ltnWeight.Value / ltnWeight.Maximum;
            }

            set
            {
                ltnWeight.Value = (int)Round(value * ltnWeight.Maximum);
            }
        }

        public ColorizeMode ColorizeMode
        {
            get
            {
                return chkColorize.Checked ?
                    ColorizeMode.Colorize :
                    ColorizeMode.Adjust;
            }

            set
            {
                switch (value)
                {
                case ColorizeMode.Adjust:
                    chkColorize.Checked = false;
                    return;

                case ColorizeMode.Colorize:
                    chkColorize.Checked = true;
                    return;

                default:
                    throw new InvalidEnumArgumentException(
                        nameof(value),
                        (int)value,
                        typeof(ColorizeMode));
                }
            }
        }

        public bool Luma
        {
            get
            {
                return chkLuma.Checked;
            }

            set
            {
                chkLuma.Checked = value;
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

        public ColorizeForm()
        {
            InitializeComponent();

            SavedAdjust = FallbackAdjust;
            SavedColorize = FallbackColorize;

            ResetValues();

            RunEvent = true;
        }

        public void ResetValues()
        {
            if (ColorizeMode == ColorizeMode.Colorize)
            {
                CurrentHSL = FallbackColorize;
                Weight = FallbackEffectiveness;
            }
            else
            {
                CurrentHSL = FallbackAdjust;
            }

            btnReset.Enabled = false;
        }

        private void SwitchValues()
        {
            if (ColorizeMode == ColorizeMode.Colorize)
            {
                SavedAdjust = CurrentHSL;

                ltbHue.Minimum = 0;
                ltbHue.Maximum = 360;
                ltbSaturation.Minimum = ltbLightness.Minimum = 0;
                ltbSaturation.Maximum = ltbLightness.Maximum = 100;
                ltbSaturation.TickFrequency = ltbLightness.TickFrequency = 5;

                CurrentHSL = SavedColorize;
            }
            else
            {
                SavedColorize = CurrentHSL;

                ltbHue.Maximum = 180;
                ltbHue.Minimum = -ltbHue.Maximum;
                ltbSaturation.Maximum = 100;
                ltbSaturation.Minimum = -ltbSaturation.Maximum;
                ltbLightness.Maximum = 100;
                ltbLightness.Minimum = -ltbLightness.Maximum;
                ltbSaturation.TickFrequency = ltbLightness.TickFrequency = 10;

                CurrentHSL = SavedAdjust;
            }
        }

        private void OnColorValueChanged(EventArgs e)
        {
            if (RunEvent)
            {
                ColorValueChanged?.Invoke(this, e);
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ResetValues();
        }

        private void Colorize_CheckedChanged(object sender, EventArgs e)
        {
            SwitchValues();
        }

        private void Preview_CheckedChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            btnReset.Enabled = true;
            OnColorValueChanged(EventArgs.Empty);
        }

        private void ColorizeForm_Shown(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void Luma_CheckedChanged(object sender, EventArgs e)
        {
            var dummy = lblSaturation.Text;
            lblSaturation.Text = (string)lblSaturation.Tag;
            lblSaturation.Tag = dummy;

            dummy = lblLightness.Text;
            lblLightness.Text = (string)lblLightness.Tag;
            lblLightness.Tag = dummy;

            OnColorValueChanged(EventArgs.Empty);
        }

        private struct Hsl
        {
            public float Hue
            {
                get;
                set;
            }

            public float Saturation
            {
                get;
                set;
            }

            public float Lightness
            {
                get;
                set;
            }

            public Hsl(float hue, float saturation, float luminosity)
            {
                Hue = hue;
                Saturation = saturation;
                Lightness = luminosity;
            }
        }
    }
}
