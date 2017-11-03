// <copyright file="ColorValueControl.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Controls
{
    public class ColorValueControl : DesignControl
    {
        private static readonly Size FallbackClientSize = new Size(0x10, 0x10);

        private static readonly Color FallbackColor = Color.Empty;

        private const Keys FallbackClickKey = Keys.Space;

        private static readonly DashedPenPair FallbackBorderDashedPens = new DashedPenPair(
            Color.Black,
            Color.White,
            1,
            1);

        private Color _selectedColor;

        private DashedPenPair _borderDashedPens;

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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DashedPenPair BorderDashedPens
        {
            get
            {
                return _borderDashedPens;
            }

            set
            {
                _borderDashedPens = value;
                Invalidate();
            }
        }

        public ColorValueControl()
        {
            SelectedColor = FallbackColor;
            BorderDashedPens = FallbackBorderDashedPens;
            ClientSize = FallbackClientSize;
        }

        protected override void OnClick(EventArgs e)
        {
            SelectColor(e);
            base.OnClick(e);
        }

        protected virtual void SelectColor(EventArgs e)
        {
            using (var dlg = new ColorDialog())
            {
                dlg.FullOpen = true;
                dlg.Color = SelectedColor;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SelectedColor = dlg.Color;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.KeyCode == FallbackClickKey)
            {
                OnClick(EventArgs.Empty);
            }

            base.OnKeyDown(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected virtual void OnColorValueChanged(EventArgs e)
        {
            Invalidate();
            ColorValueChanged?.Invoke(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawColorValue(e);
            if (Focused)
            {
                DrawFocusedBorder(e);
            }

            base.OnPaint(e);
        }

        protected virtual void DrawColorValue(PaintEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            using (var brush = new SolidBrush(SelectedColor))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        protected virtual void DrawFocusedBorder(PaintEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var border = new Rectangle(0, 0, ClientWidth - 1, ClientHeight - 1);

            using (Pen pen1 = new Pen(Color.Empty, 1),
                       pen2 = new Pen(Color.Empty, 1))
            {
                BorderDashedPens.SetPenProperties(pen1, pen2);

                e.Graphics.DrawRectangle(pen1, border);
                e.Graphics.DrawRectangle(pen2, border);
            }
        }
    }
}
