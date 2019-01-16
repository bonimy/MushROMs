// <copyright file="OpenFileHelper.cs" company="Public Domain">
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
    using Helper;
    using MushROMs;
    using Snes;
    using static Controls.ExceptionDialog;

    public class OpenFileHelper
    {
        public event EventHandler<EditorEventArgs> EditorOpened;

        private ExtensionDictionary<OpenEditorCallback>
            OpenFileAssociations
        {
            get;
        }

        private List<OpenEditorCallbackInfo> OpenEditorCallbackInfo
        {
            get;
        }

        public OpenFileHelper()
        {
            OpenFileAssociations =
                new ExtensionDictionary<OpenEditorCallback>()
            {
                { ".rpf", OpenRpf },
            };

            OpenEditorCallbackInfo =
                new List<OpenEditorCallbackInfo>()
            {
                new OpenEditorCallbackInfo(
                    OpenRpf,
                    "Palette Editor",
                    null),
            };
        }

        public void OpenFile()
        {
            OpenFile(null as IWin32Window);
        }

        public void OpenFile(IWin32Window owner)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;
                var dialogResult = dlg.ShowDialog(owner);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                foreach (var file in dlg.FileNames)
                {
                    OpenFile(owner, file);
                }
            }
        }

        public void OpenFile(string path)
        {
            OpenFile(null as IWin32Window, path);
        }

        public void OpenFile(IWin32Window owner, string path)
        {
            if (TryGetValue(path, out var openEditor))
            {
                OpenFile(owner, path, openEditor);
                return;
            }

            OpenFileAs(owner, path);

            bool TryGetValue(string key, out OpenEditorCallback value)
            {
                return OpenFileAssociations.TryGetValue(
                    key,
                    out value);
            }
        }

        public void OpenFileAs(string path)
        {
            OpenFileAs(null, path);
        }

        public void OpenFileAs(IWin32Window owner, string path)
        {
            using (var dlg = new OpenWithDialog())
            {
                dlg.AddEditors(OpenEditorCallbackInfo);
                var dialogResult = dlg.ShowDialog(owner);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                OpenFile(owner, path, dlg.OpenEditorMethod);
            }
        }

        public void OpenFile(string path, OpenEditorCallback openEditor)
        {
            OpenFile(null, path, openEditor);
        }

        public void OpenFile(
            IWin32Window owner,
            string path,
            OpenEditorCallback openEditor)
        {
            if (openEditor is null)
            {
                throw new ArgumentNullException(nameof(openEditor));
            }

            if (!TryOpenFile(owner, path, openEditor, out var editor))
            {
                return;
            }

            var e = new EditorEventArgs(editor);
            OnEditorOpened(e);
        }

        protected virtual bool TryOpenFile(
            IWin32Window owner,
            string path,
            OpenEditorCallback openEditor,
            out IEditor editor)
        {
            if (openEditor is null)
            {
                editor = null;
                return false;
            }

            loop:
            try
            {
                editor = openEditor(path);
            }
            catch (IOException ex)
            {
                if (ShowExceptionAndRetry(owner, ex, "MushROMs"))
                {
                    goto loop;
                }

                editor = null;
                return false;
            }
            catch (Exception ex)
            {
                ShowException(owner, ex);

                editor = null;
                return false;
            }

            return true;
        }

        private IEditor OpenRpf(string path)
        {
            var data = File.ReadAllBytes(path);
            var palette = new Palette(data);
            var editor = new PaletteEditor(palette, path);

            return editor;
        }

        protected virtual void OnEditorOpened(EditorEventArgs e)
        {
            EditorOpened?.Invoke(this, e);
        }
    }
}
