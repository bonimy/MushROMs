namespace MushROMs.IO
{
    using System;
    using System.IO;
    using Helper;

    public abstract class OpenFileHelper
    {
        public OpenFileHelper(
            ExtensionDictionary<OpenEditorCallback> openFileAssociations,
            IExceptionHandler exceptionHandler)
        {
            OpenFileAssociations = openFileAssociations ??
                throw new ArgumentNullException(
                    nameof(openFileAssociations));

            ExceptionHandler = exceptionHandler ??
                throw new ArgumentNullException(
                    nameof(exceptionHandler));
        }

        public event EventHandler<EditorEventArgs> EditorOpened;

        private ExtensionDictionary<OpenEditorCallback>
            OpenFileAssociations
        {
            get;
        }

        private IExceptionHandler ExceptionHandler
        {
            get;
        }

        public void OpenFile()
        {
            var files = ChooseFiles();
            if (files is null)
            {
                return;
            }

            foreach (var file in files)
            {
                OpenFile(file);
            }
        }

        public abstract string[] ChooseFiles();

        public void OpenFile(string path)
        {
            if (!TryGetFileAssociation(path, out var openEditor))
            {
                openEditor = ChooseOpenEditorCallback(path);
                if (openEditor is null)
                {
                    return;
                }
            }

            OpenFile(path, openEditor);
        }

        public bool TryGetFileAssociation(
            string path,
            out OpenEditorCallback openEditor)
        {
            return OpenFileAssociations.TryGetValue(
                path,
                out openEditor);
        }

        public abstract OpenEditorCallback ChooseOpenEditorCallback(
            string path);

        public void OpenFile(string path, OpenEditorCallback openEditor)
        {
            if (openEditor is null)
            {
                throw new ArgumentNullException(
                    nameof(openEditor));
            }

            if (!TryOpenFile(path, openEditor, out var editor))
            {
                return;
            }

            var e = new EditorEventArgs(editor);
            OnEditorOpened(e);
        }

        public bool TryOpenFile(
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
                if (ExceptionHandler.ShowExceptionAndRetry(ex))
                {
                    goto loop;
                }

                editor = null;
                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.ShowException(ex);

                editor = null;
                return false;
            }

            return true;
        }

        protected virtual void OnEditorOpened(EditorEventArgs e)
        {
            EditorOpened?.Invoke(this, e);
        }
    }
}
