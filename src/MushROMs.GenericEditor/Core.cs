// <copyright file="Core.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.GenericEditor
{
    using System;
    using System.Windows.Forms;
    using Controls.Editors;
    using Helper;

    public class Core : IDisposable
    {
        private bool _disposed;

        public Core()
        {
            EditorSelector = new EditorSelector();
            EditorSelector.EditorAdded += EditorAdded;
            EditorSelector.EditorRemoved += EditorRemoved;
            EditorSelector.CurrentEditorChanged += CurrentEditorChanged;

            NewFileHelper = new NewFileHelper();
            NewFileHelper.EditorCreated += EditorCreated;

            OpenFileHelper = new OpenFileHelper();
            OpenFileHelper.EditorOpened += EditorCreated;

            SaveFileHelper = new SaveFileHelper();
            SaveFileHelper.FileSaved += EditorSaved;

            ChildFormHelper = new ChildFormHelper();
            ChildFormHelper.EditorFormAdded += EditorFormCreated;
            ChildFormHelper.EditorFormClosed += EditorFormClosed;
            ChildFormHelper.EditorFormGotFocus += EditorFormGotFocus;

            MainForm = new MainForm();
            MainForm.NewFileClick += GetEvent(NewFileHelper.NewFile);
            MainForm.OpenFileClick += GetEvent(OpenFileHelper.OpenFile);
            MainForm.OpenRecentClick += OpenRecent;
            MainForm.CloseFileClick += CloseFile;
            MainForm.SaveFileClick += SaveFile;
            MainForm.SaveAsClick += SaveAs;
            MainForm.SaveAllClick += SaveAll;
        }

        public MainForm MainForm
        {
            get;
        }

        public EditorSelector EditorSelector
        {
            get;
        }

        public NewFileHelper NewFileHelper
        {
            get;
        }

        public OpenFileHelper OpenFileHelper
        {
            get;
        }

        public SaveFileHelper SaveFileHelper
        {
            get;
        }

        public ChildFormHelper ChildFormHelper
        {
            get;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                MainForm.Dispose();
            }

            _disposed = true;
        }

        private static EventHandler GetEvent(
            Action<IWin32Window> action)
        {
            return (sender, e) => action(sender as IWin32Window);
        }

        private void OpenRecent(object sender, PathEventArgs e)
        {
            OpenFileHelper.OpenFile(sender as IWin32Window, e.Path);
        }

        private void CloseFile(object sender, EventArgs e)
        {
            EditorSelector.Items.Remove(EditorSelector.CurrentEditor);
        }

        private void SaveFile(object sender, EventArgs e)
        {
            SaveFileHelper.SaveFile(
                sender as IWin32Window,
                EditorSelector.CurrentEditor);
        }

        private void SaveAs(object sender, EventArgs e)
        {
            SaveFileHelper.SaveAs(
                sender as IWin32Window,
                EditorSelector.CurrentEditor);
        }

        private void SaveAll(object sender, EventArgs e)
        {
            SaveFileHelper.SaveAll(
                sender as IWin32Window,
                EditorSelector.Items);
        }

        private void AddEditor(IEditor editor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            if (EditorSelector.Items.Contains(editor))
            {
                EditorSelector.CurrentEditor = editor;
            }
            else
            {
                EditorSelector.Items.Add(editor);
            }

            MainForm.RecentFiles.Add(editor.Path);
        }

        private void RemoveEditorForm(IEditor editor)
        {
            if (ChildFormHelper.TryGetForm(editor, out var form))
            {
                form.Close();
            }
        }

        private void EditorCreated(object sender, EditorEventArgs e)
        {
            AddEditor(e.Editor);
        }

        private void EditorAdded(object sender, EditorEventArgs e)
        {
            ChildFormHelper.AddEditorForm(MainForm, e.Editor);
        }

        private void EditorSaved(object sender, EditorEventArgs e)
        {
            MainForm.RecentFiles.Add(e.Editor.Path);
        }

        private void CurrentEditorChanged(
            object sender,
            EditorEventArgs e)
        {
        }

        private void EditorRemoved(object sender, EditorEventArgs e)
        {
            RemoveEditorForm(e.Editor);
        }

        private void AddEditorForm(Form form)
        {
            if (form is null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            form.MdiParent = MainForm;
            form.Show();
        }

        private void EditorFormCreated(
            object sender,
            EditorFormEventArgs e)
        {
            AddEditorForm(e.Form);
        }

        private void EditorFormGotFocus(
            object sender,
            EditorFormEventArgs e)
        {
            EditorSelector.CurrentEditor = e.Editor;
        }

        private void EditorFormClosed(
            object sender,
            EditorEventArgs e)
        {
            EditorSelector.Items.Remove(e.Editor);
        }
    }
}
