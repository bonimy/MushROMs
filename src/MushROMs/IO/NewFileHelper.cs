// <copyright file="NewFileHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.IO
{
    using System;
    using Helper;

    public abstract class NewFileHelper
    {
        public NewFileHelper(IExceptionHandler exceptionHandler)
        {
            ExceptionHandler = exceptionHandler ??
                throw new ArgumentNullException(
                    nameof(exceptionHandler));
        }

        public event EventHandler<EditorEventArgs> EditorCreated;

        private IExceptionHandler ExceptionHandler
        {
            get;
        }

        public void NewFile()
        {
            _loop:
            var createEditor = ChooseCreateEditor();
            if (createEditor is null)
            {
                return;
            }

            if (!TryCreateEditor(createEditor, out var editor))
            {
                goto _loop;
            }

            NewFile(createEditor);
        }

        public void NewFile(CreateEditorCallback createEditor)
        {
            if (!TryCreateEditor(createEditor, out var editor))
            {
                return;
            }

            var e = new EditorEventArgs(editor);
            OnEditorCreated(e);
        }

        protected virtual bool TryCreateEditor(
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
                ExceptionHandler.ShowException(ex);
                editor = null;
                return false;
            }

            return true;
        }

        protected virtual void OnEditorCreated(EditorEventArgs e)
        {
            EditorCreated?.Invoke(this, e);
        }

        protected abstract CreateEditorCallback ChooseCreateEditor();
    }
}
