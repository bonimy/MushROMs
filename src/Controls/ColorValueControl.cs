// <copyright file="ColorValueControl.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Helper;

namespace Controls
{
    public class ColorValueControl : DesignControl
    {
        private Color _selectedColor;

        [Category("Editor")]
        [Description("Occurs when the selected color value of the control changes.")]
        public event EventHandler ColorValueChanged;

        [Category("Appearance")]
        [DefaultValue("Black")]
        [Description("The selected color for the control.")]
        public Color SelectedColor
        {
            get
            {
                return _selectedColor;
            }

            set
            {
                if (SelectedColor == value)
                {
                    return;
                }

                _selectedColor = value;
                OnColorValueChanged(EventArgs.Empty);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected virtual void OnColorValueChanged(EventArgs e)
        {
            Invalidate();
            ColorValueChanged?.Invoke(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawColorValue(e);

            base.OnPaint(e);
        }

        protected virtual void DrawColorValue(PaintEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var color = SelectedColor;
            if (!Enabled)
            {
                var colorF = (ColorF)color;
                var gray = colorF.Grayscale();
                color = (Color)gray;
            }

            using (var brush = new SolidBrush(color))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }
    }
}
