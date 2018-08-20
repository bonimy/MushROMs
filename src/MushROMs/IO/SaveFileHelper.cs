// <copyright file="SaveFileHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Helper;

    public abstract class SaveFileHelper
    {
        public SaveFileHelper(
            IDictionary<Type, SaveFileAssociations> supportedEditorTypes,
            IExceptionHandler exceptionHandler)
        {
            SupportedEditorTypes =
                new Dictionary<Type, SaveFileAssociations>(
                    supportedEditorTypes);

            ExceptionHandler = exceptionHandler ??
                throw new ArgumentNullException(
                    nameof(exceptionHandler));
        }

        public event EventHandler<EditorEventArgs> FileSaved;

        private Dictionary<Type, SaveFileAssociations>
            SupportedEditorTypes
        {
            get;
        }

        private IExceptionHandler ExceptionHandler
        {
            get;
        }

        public void SaveFile(IEditor editor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            SaveFile(editor, editor.Path);
        }

        public void SaveFileAs(IEditor editor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            var path = ChoosePath();
            SaveFile(editor, path);
        }

        public void SaveFile(IEditor editor, string path)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            var type = editor.GetType();
            if (!TryGetValue(type, out var saveFileAssociation))
            {
                // We should handle an exception here.
                return;
            }

            // If path is not a valid key, the default association is returned.
            var saveEditor = saveFileAssociation[path];
            SaveFile(editor, path, saveEditor);

            bool TryGetValue(Type key, out SaveFileAssociations value)
            {
                return SupportedEditorTypes.TryGetValue(
                    key,
                    out value);
            }
        }

        public void SaveFile(
            IEditor editor,
            string path,
            SaveEditorCallback saveEditor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            if (saveEditor is null)
            {
                throw new ArgumentNullException(nameof(saveEditor));
            }

            if (!TrySaveEditor(editor, path, saveEditor))
            {
                return;
            }

            var e = new EditorEventArgs(editor);
            OnFileSaved(e);
        }

        protected abstract string ChoosePath();

        protected virtual void OnFileSaved(EditorEventArgs e)
        {
            FileSaved?.Invoke(this, e);
        }

        private bool TrySaveEditor(
            IEditor editor,
            string path,
            SaveEditorCallback saveEditor)
        {
            _loop:
            try
            {
                saveEditor(path, editor);
            }
            catch (IOException ex)
            {
                if (ExceptionHandler.ShowExceptionAndRetry(ex))
                {
                    goto _loop;
                }

                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.ShowException(ex);
                return false;
            }

            return true;
        }
    }
}
