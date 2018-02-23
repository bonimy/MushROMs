// <copyright file="NewFileHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Windows.Forms;
using MushROMs;
using static Controls.ExceptionDialog;

namespace Controls.Editors
{
    public class NewFileHelper
    {
        public event EventHandler<EditorEventArgs> EditorCreated;

        public void NewFile()
        {
            NewFile(null);
        }

        public void NewFile(IWin32Window owner)
        {
            if (!TryCreateEditor(owner, out var editor))
            {
                return;
            }

            var e = new EditorEventArgs(editor);
            OnEditorCreated(e);
        }

        public void NewFile(
            IWin32Window owner,
            CreateEditorCallback createEditor)
        {
            if (!TryCreateEditor(owner, createEditor, out var editor))
            {
                return;
            }

            var e = new EditorEventArgs(editor);
            OnEditorCreated(e);
        }

        private bool TryCreateEditor(IWin32Window owner, out IEditor editor)
        {
            using (var dlg = new CreateEditorDialog())
            {
                loop:
                var dialogResult = dlg.ShowDialog(owner);
                if (dialogResult != DialogResult.OK)
                {
                    editor = null;
                    return false;
                }

                var createEditor = dlg.CreateEditorCallback;
                if (TryCreateEditor(owner, createEditor, out editor))
                {
                    return true;
                }

                goto loop;
            }
        }

        protected virtual bool TryCreateEditor(
            IWin32Window owner,
            CreateEditorCallback createEditor,
            out IEditor editor)
        {
            if (createEditor is null)
            {
                editor = null;
                return false;
            }

            try
            {
                editor = createEditor();
            }
            catch (Exception ex)
            {
                ShowException(owner, ex);
                editor = null;
                return false;
            }

            return true;
        }

        protected virtual void OnEditorCreated(EditorEventArgs e)
        {
            EditorCreated?.Invoke(this, e);
        }
    }
}
