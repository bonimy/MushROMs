// <copyright file="CreateEditorForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Controls.Editors.Properties;
    using MushROMs;
    using Snes;

    internal sealed partial class CreateEditorForm : Form
    {
        private static readonly Color HoveringCellColor =
            Color.FromArgb(
                0xFF,
                0xFF,
                0xBF);

        public CreateEditorCallback CreateEditorCallback
        {
            get
            {
                return CreateEditor;
            }
        }

        private GridItem[] GridItems
        {
            get;
            set;
        }

        private GridItem CurrentGridItem
        {
            get
            {
                return GridItems[CurrentRowIndex];
            }

            set
            {
                GridItems[CurrentRowIndex] = value;
            }
        }

        private DataGridViewRowCollection Rows
        {
            get
            {
                return dgvNewFileList.Rows;
            }
        }

        private int CurrentRowIndex
        {
            get
            {
                return dgvNewFileList.CurrentCell.RowIndex;
            }
        }

        public CreateEditorForm()
        {
            InitializeComponent();

            var paletteGridItem = new GridItem(
                null,
                Resources.PaletteFileType,
                Resources.PaletteFileDescription,
                new CreatePaletteControl());

            var gfxGridItem = new GridItem(
                null,
                Resources.GfxFileDescription,
                Resources.GfxFileType,
                new CreateGfxControl());

            GridItems = new GridItem[]
            {
                paletteGridItem,
                gfxGridItem
            };

            for (var i = 0; i < GridItems.Length; i++)
            {
                Rows.Add(
                    GridItems[i].Icon,
                    GridItems[i].FileType,
                    String.Empty);
            }
        }

        private IEditor CreateEditor()
        {
            var options = CurrentGridItem.Options;
            switch (options)
            {
                case CreatePaletteControl createPaletteControl:
                    var palette = new Palette(
                        createPaletteControl.NumColors);

                    return new PaletteEditor(palette);
            }

            return null;
        }

        private void SetRowBackColor(int rowIndex, Color color)
        {
            Rows[rowIndex].DefaultCellStyle.BackColor = color;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.KeyCode == Keys.Enter && AcceptButton != null)
            {
                AcceptButton.PerformClick();
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        private void NewFileList_CellMouseEnter(
            object sender,
            DataGridViewCellEventArgs e)
        {
            SetRowBackColor(e.RowIndex, HoveringCellColor);
        }

        private void NewFileList_CellMouseLeave(
            object sender,
            DataGridViewCellEventArgs e)
        {
            SetRowBackColor(
                e.RowIndex,
                SystemColors.ControlLightLight);
        }

        private void CurrentCellChanged(object sender, EventArgs e)
        {
            lblDescription.Text = CurrentGridItem.FileDescription;

            pnlOptions.Controls.Clear();
            pnlOptions.Controls.Add(CurrentGridItem.Options);
        }

        private void Options_ControlAdded(
            object sender,
            ControlEventArgs e)
        {
            var width = pnlOptions.Width - e.Control.Width;
            var x = width / 2;

            var height = pnlOptions.Height - e.Control.Height;
            var y = height / 2;

            e.Control.Location = new Point(x, y);
        }

        private struct GridItem
        {
            public Image Icon
            {
                get;
            }

            public string FileType
            {
                get;
            }

            public string FileDescription
            {
                get;
            }

            public UserControl Options
            {
                get;
            }

            public GridItem(
                Image icon,
                string fileType,
                string fileDescription,
                UserControl options)
            {
                Icon = icon;
                FileType = fileType;
                FileDescription = fileDescription;
                Options = options;
            }
        }
    }
}
