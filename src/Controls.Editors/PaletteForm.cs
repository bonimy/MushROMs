// <copyright file="PaletteForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Helper.PixelFormat;

    public partial class PaletteForm : TileMapForm
    {
        public event PaintEventHandler DrawPalette
        {
            add
            {
                tileMapControl.Paint += value;
            }

            remove
            {
                tileMapControl.Paint -= value;
            }
        }

        public event EventHandler NextByte
        {
            add
            {
                paletteStatus.NextByte += value;
            }

            remove
            {
                paletteStatus.NextByte -= value;
            }
        }

        public event EventHandler LastByte
        {
            add
            {
                paletteStatus.LastByte += value;
            }

            remove
            {
                paletteStatus.LastByte -= value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public string Status
        {
            get
            {
                return tssMain.Text;
            }

            set
            {
                tssMain.Text = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public Color ActiveColor
        {
            get
            {
                return paletteStatus.ActiveColor;
            }

            set
            {
                paletteStatus.ActiveColor = (Color15BppBgr)value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public bool ShowRomScrolling
        {
            get
            {
                return paletteStatus.ShowAddressScrolling;
            }

            set
            {
                paletteStatus.ShowAddressScrolling = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        private int LastZoomScaleIndex
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        private Size[] ZoomedViewSizes
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public PaletteZoomScale PaletteZoomScale
        {
            get
            {
                return paletteStatus.PaletteZoomScale;
            }

            set
            {
                paletteStatus.PaletteZoomScale = value;
            }
        }

        public new TileMapControl1D TileMapControl
        {
            get
            {
                return base.TileMapControl as TileMapControl1D;
            }

            set
            {
                base.TileMapControl = value;
            }
        }

        public PaletteForm()
        {
            InitializeComponent();
        }

        private void PaletteForm_Load(object sender, EventArgs e)
        {
            AnchorFormToTileMap();

            ZoomedViewSizes = new Size[paletteStatus.ZoomScaleCount];
            LastZoomScaleIndex = paletteStatus.PaletteZoomIndex;
            for (var i = ZoomedViewSizes.Length; --i >= 0;)
            {
                ZoomedViewSizes[i] = TileMapControl.ViewSize;
            }
        }

        private void ZoomScaleChanged(object sender, EventArgs e)
        {
            ZoomedViewSizes[LastZoomScaleIndex] = TileMapControl.ViewSize;

            var zoom = (int)PaletteZoomScale;
            TileMapControl.ZoomSize = new Size(zoom, zoom);
            TileMapControl.ViewSize =
                ZoomedViewSizes[paletteStatus.PaletteZoomIndex];

            LastZoomScaleIndex = paletteStatus.PaletteZoomIndex;
        }

        private void PaletteForm_ResizeEnd(object sender, EventArgs e)
        {
            for (var i = ZoomedViewSizes.Length; --i >= 0;)
            {
                ZoomedViewSizes[i] = TileMapControl.ViewSize;
            }
        }

        private void TileMapControl_ClientSizeChanged(
            object sender,
            EventArgs e)
        {
            SetSizeFromTileMapControl();
        }
    }
}
