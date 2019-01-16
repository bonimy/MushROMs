// <copyright file="PaletteStatus.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Controls.Editors.Properties;
    using Helper.PixelFormat;
    using static Helper.StringHelper;

    public partial class PaletteStatus : UserControl
    {
        private Color15BppBgr _activeColor;

        public PaletteStatus()
        {
            InitializeComponent();

            PaletteZoomScale = PaletteZoomScale.Zoom16x;
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("Occurs when the active color value changes.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler ActiveColorChanged;

        [Browsable(true)]
        [Category("Editor")]
        [Description("Occurs when the zoom scale changes.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler ZoomScaleChanged
        {
            add
            {
                cbxZoom.SelectedIndexChanged += value;
            }

            remove
            {
                cbxZoom.SelectedIndexChanged -= value;
            }
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("Occurs when the '+' button is pressed.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler NextByte
        {
            add
            {
                btnNextByte.Click += value;
            }

            remove
            {
                btnNextByte.Click -= value;
            }
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("Occurs when the '-' button is pressed.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler LastByte
        {
            add
            {
                btnLastByte.Click += value;
            }

            remove
            {
                btnLastByte.Click -= value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public Color15BppBgr ActiveColor
        {
            get
            {
                return _activeColor;
            }

            set
            {
                _activeColor = value;
                var color = (Color24BppRgb)ActiveColor;
                lblPcValue.Text = GetPcColorText(color);
                lblSnesValue.Text = GetSnesColorText(value);
                lblRedValue.Text = GetString(color.Red);
                lblGreenValue.Text = GetString(color.Green);
                lblBlueValue.Text = GetString(color.Blue);
                OnActiveColorChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int ZoomScaleCount
        {
            get
            {
                return cbxZoom.Items.Count;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int PaletteZoomIndex
        {
            get
            {
                return cbxZoom.SelectedIndex;
            }

            set
            {
                if (value <= 0)
                {
                    value = 0;
                }

                if (value > ZoomScaleCount)
                {
                    value = ZoomScaleCount - 1;
                }

                cbxZoom.SelectedIndex = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public PaletteZoomScale PaletteZoomScale
        {
            get
            {
                return (PaletteZoomScale)(8 * (PaletteZoomIndex + 1));
            }

            set
            {
                PaletteZoomIndex = ((int)value / 8) - 1;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public bool ShowAddressScrolling
        {
            get
            {
                return gbxROMViewing.Visible;
            }

            set
            {
                gbxROMViewing.Visible = value;
            }
        }

        private static string GetSnesColorText(Color15BppBgr color)
        {
            return GetString(
                Resources.SnesColorFormat,
                color.Value);
        }

        private static string GetPcColorText(Color24BppRgb color)
        {
            return GetString(
                Resources.PcColorFormat,
                color.Value);
        }

        protected virtual void OnActiveColorChanged(EventArgs e)
        {
            ActiveColorChanged?.Invoke(this, e);
        }
    }
}
