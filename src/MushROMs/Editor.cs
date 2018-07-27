// <copyright file="Editor.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;
    using System.IO;
    using Helper;
    using static Helper.StringHelper;
    using static System.IO.Path;

    public abstract class Editor : IEditor
    {
        private string path;

        protected Editor(string path)
        {
            Path = path;
        }

        public event EventHandler PathChanged;

        public event EventHandler DataModified;

        public event EventHandler UndoApplied;

        public event EventHandler RedoApplied;

        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                var comparer = PathComparer.Default;
                if (comparer.Equals(path, value))
                {
                    return;
                }

                path = GetFullPath(value);
                OnPathChanged(EventArgs.Empty);
            }
        }

        public virtual bool CanUndo
        {
            get
            {
                return History.CanUndo;
            }
        }

        public virtual bool CanRedo
        {
            get
            {
                return History.CanRedo;
            }
        }

        public virtual bool CanCut
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanCopy
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanPaste
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanDelete
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanSelectAll
        {
            get
            {
                return false;
            }
        }

        private UndoFactory History
        {
            get;
            set;
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            History.Undo();
            OnUndoApplied(EventArgs.Empty);
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            History.Redo();
            OnRedoApplied(EventArgs.Empty);
        }

        public virtual void Cut()
        {
            throw new NotImplementedException();
        }

        public virtual void Copy()
        {
            throw new NotImplementedException();
        }

        public virtual void Paste()
        {
            throw new NotImplementedException();
        }

        public virtual void Delete()
        {
            throw new NotImplementedException();
        }

        public virtual void SelectAll()
        {
            throw new NotImplementedException();
        }

        public void WriteData(Action action)
        {
            WriteData(action, null);
        }

        public void WriteData(Action action, Action undo)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (undo != null)
            {
                History.Add(undo, action);
            }

            action();
            OnDataModified(EventArgs.Empty);
        }

        public bool Equals(IEditor obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!GetType().Equals(obj.GetType()))
            {
                return false;
            }

            var comparer = PathComparer.Default;
            return comparer.Equals(Path, obj.Path);
        }

        public override bool Equals(object obj)
        {
            if (obj is IEditor editor)
            {
                return Equals(editor);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var comparer = PathComparer.Default;
            return comparer.GetHashCode(Path);
        }

        public override string ToString()
        {
            return GetFileName(Path);
        }

        protected static string NextUntitledPath(
            string name,
            string extension,
            ref int number)
        {
            string path;
            do
            {
                path = Combine(
                    name,
                    extension,
                    (++number).ToString());
            }
            while (File.Exists(path));

            return path;
        }

        protected virtual void OnPathChanged(EventArgs e)
        {
            PathChanged?.Invoke(this, e);
        }

        protected virtual void OnUndoApplied(EventArgs e)
        {
            UndoApplied?.Invoke(this, e);
        }

        protected virtual void OnRedoApplied(EventArgs e)
        {
            RedoApplied?.Invoke(this, e);
        }

        protected virtual void OnDataModified(EventArgs e)
        {
            DataModified?.Invoke(this, e);
        }
    }
}
