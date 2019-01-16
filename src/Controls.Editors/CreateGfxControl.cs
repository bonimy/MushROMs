// <copyright file="CreateGfxControl.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System.Windows.Forms;
    using Snes;

    internal partial class CreateGfxControl : UserControl
    {
        public int NumTiles
        {
            get
            {
                return (int)nudNumTiles.Value;
            }

            set
            {
                nudNumTiles.Value = value;
            }
        }

        public GraphicsFormat GraphicsFormat
        {
            get
            {
                return (GraphicsFormat)cbxGraphicsFormat.SelectedValue;
            }

            set
            {
                var index = cbxGraphicsFormat.Items.IndexOf(value);
                if (index == -1)
                {
                    return;
                }

                cbxGraphicsFormat.SelectedIndex = index;
            }
        }

        public bool CopyFrom
        {
            get
            {
                return chkFromCopy.Enabled && chkFromCopy.Checked;
            }

            set
            {
                chkFromCopy.Checked = value;
            }
        }

        public CreateGfxControl()
        {
            InitializeComponent();

            var list = new object[]
            {
                GraphicsFormat.Format1Bpp8x8,
                GraphicsFormat.Format2BppNes,
                GraphicsFormat.Format2BppGb,
                GraphicsFormat.Format2BppNgp,
                GraphicsFormat.Format2BppVb,
                GraphicsFormat.Format3BppSnes,
                GraphicsFormat.Format3Bpp8x8,
                GraphicsFormat.Format4BppSnes,
                GraphicsFormat.Format4BppGba,
                GraphicsFormat.Format4BppSms,
                GraphicsFormat.Format4BppMsx2,
                GraphicsFormat.Format4Bpp8x8,
                GraphicsFormat.Format8BppSnes,
                GraphicsFormat.Format8BppMode7
            };

            cbxGraphicsFormat.Items.AddRange(list);
        }
    }
}
