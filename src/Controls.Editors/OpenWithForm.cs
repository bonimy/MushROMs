// <copyright file="OpenWithForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using MushROMs;

    internal partial class OpenWithForm : Form
    {
        public OpenEditorCallback SelectedOpenEditorMethod
        {
            get
            {
                var info = lbxEditors.SelectedItem as
                    OpenEditorCallbackInfo;

                return info.OpenEditorMethod;
            }
        }

        public OpenWithForm()
        {
            InitializeComponent();
        }

        public void ClearEditors()
        {
            lbxEditors.Items.Clear();
        }

        public void AddEditor(
            OpenEditorCallbackInfo openEditorCallbackInfo)
        {
            lbxEditors.Items.Add(openEditorCallbackInfo);
        }

        public void AddEditors(
            IEnumerable<OpenEditorCallbackInfo> types)
        {
            foreach (var info in types)
            {
                AddEditor(info);
            }
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            if (lbxEditors.SelectedItem is null)
            {
                return;
            }

            btnOK.PerformClick();
        }

        private void OpenWithForm_Load(object sender, EventArgs e)
        {
            if (lbxEditors.Items.Count > 0)
            {
                lbxEditors.SelectedIndex = 0;
            }
        }
    }
}
