// <copyright file="SaveFileHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using MushROMs;
    using Snes;
    using static Controls.ExceptionDialog;

    public class SaveFileHelper
    {
        public event EventHandler<EditorEventArgs> FileSaved;

        private Dictionary<Type, SaveEditorCallback> SaveFileAssociations
        {
            get;
        }

        public SaveFileHelper()
        {
            SaveFileAssociations =
                new Dictionary<Type, SaveEditorCallback>()
            {
                { typeof(PaletteEditor), SavePalette },
            };
        }

        public void SaveFile(IEditor editor)
        {
            SaveFile(null, editor);
        }

        public void SaveFile(IWin32Window owner, IEditor editor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            SaveFile(owner, editor, editor.Path);
        }

        public void SaveFile(
            IWin32Window owner,
            IEditor editor,
            string path)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            var type = editor.GetType();
            if (!TryGetValue(type, out var saveEditor))
            {
                return;
            }

            SaveFile(owner, editor, path, saveEditor);

            bool TryGetValue(Type key, out SaveEditorCallback value)
            {
                return SaveFileAssociations.TryGetValue(
                    key,
                    out value);
            }
        }

        public void SaveFile(
            IWin32Window owner,
            IEditor editor,
            string path,
            SaveEditorCallback saveEditor)
        {
            if (saveEditor is null)
            {
                throw new ArgumentNullException(nameof(saveEditor));
            }

            if (!TrySaveFile(owner, editor, path, saveEditor))
            {
                return;
            }

            var e = new EditorEventArgs(editor);
            OnFileSaved(e);
        }

        protected virtual bool TrySaveFile(
            IWin32Window owner,
            IEditor editor,
            string path,
            SaveEditorCallback saveEditor)
        {
            if (saveEditor is null)
            {
                return false;
            }

            loop:

            try
            {
                saveEditor(path, editor);
            }
            catch (IOException ex)
            {
                if (ShowExceptionAndRetry(owner, ex))
                {
                    goto loop;
                }

                return false;
            }
            catch (Exception ex)
            {
                ShowException(owner, ex);

                return false;
            }

            return true;
        }

        public void SaveAs(IEditor editor)
        {
            SaveAs(null, editor);
        }

        public void SaveAs(IWin32Window owner, IEditor editor)
        {
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog(owner) != DialogResult.OK)
                {
                    return;
                }

                SaveFile(owner, editor, dlg.FileName);
            }
        }

        public void SaveAll(IEnumerable<IEditor> editors)
        {
            SaveAll(null, editors);
        }

        public void SaveAll(
            IWin32Window owner,
            IEnumerable<IEditor> editors)
        {
            if (editors is null)
            {
                throw new ArgumentNullException(nameof(editors));
            }

            foreach (var editor in editors)
            {
                SaveFile(owner, editor);
            }
        }

        private void SavePalette(string path, IEditor editor)
        {
            var paletteEditor = editor as PaletteEditor;
            var palette = paletteEditor.Palette;
            var data = new List<byte>(palette).ToArray();
            File.WriteAllBytes(path, data);
        }

        protected virtual void OnFileSaved(EditorEventArgs e)
        {
            FileSaved?.Invoke(this, e);
        }
    }
}
