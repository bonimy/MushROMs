// <copyright file="CreatePaletteControl.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System.Windows.Forms;

    internal partial class CreatePaletteControl : UserControl
    {
        public int NumColors
        {
            get
            {
                return (int)nudNumColors.Value;
            }

            set
            {
                nudNumColors.Value = value;
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

        public CreatePaletteControl()
        {
            InitializeComponent();
        }
    }
}
