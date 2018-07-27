// <copyright file="EditorSelector.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

/*
The editor selector comprises a collection of all editors that are running in the program, and a path dictionary of editors associated with files.

1. If an editor is created, it is added to the editor collection. There are no changes to the editor path dictionary. The editor is given a unique name that defines its type and number. The untitled number is specific to each editor type. The untitled number strictly increases.

2. If an editor is opened, the editor path dictionary is checked for the path. If the path exists, the open is canceled and the associated editor is returned instead.

3. If an opened editor is saved to its original path, no changes to either collection is made.

4. If a new editor is saved to a path not in the path dictionary, the editor is added to the path dictionary.

5. If an opened editor is saved to a path not in the path dictionary, the editor collection is unchanged and the path dictionary is updated so that the new path references the editor. The old path is removed from the dictionary since the program now associates this editor with the new path.

6. If an editor is saved to a path that is already in the path dictionary but does not belong to this editor, the save fails and cannot be completed.
*/

namespace MushROMs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Helper;

    public class EditorSelector
    {
        private IEditor currentEditor;

        public EditorSelector()
        {
            Items = new EditorCollection(this);
        }

        public event EventHandler<EditorEventArgs> CurrentEditorChanged;

        public event EventHandler CurrentEditorRemoved;

        public event EventHandler<EditorEventArgs> EditorAdded;

        public event EventHandler<EditorEventArgs> EditorRemoved;

        public EditorCollection Items
        {
            get;
        }

        public IEditor CurrentEditor
        {
            get
            {
                return currentEditor;
            }

            set
            {
                if (currentEditor == value)
                {
                    return;
                }

                if (value is null)
                {
                    currentEditor = null;
                    OnCurrentEditorRemoved(EventArgs.Empty);
                    return;
                }

                // Add this editor if it does not exist.
                if (!Items.Contains(value))
                {
                    Items.Add(value);
                }

                currentEditor = value;
                var e = new EditorEventArgs(value);
                OnCurrentEditorChanged(e);
            }
        }

        protected virtual void OnCurrentEditorChanged(EditorEventArgs e)
        {
            CurrentEditorChanged?.Invoke(this, e);
        }

        protected virtual void OnCurrentEditorRemoved(EventArgs e)
        {
            CurrentEditorRemoved?.Invoke(this, e);
        }

        protected virtual void OnEditorAdded(EditorEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            EditorAdded?.Invoke(this, e);

            CurrentEditor = e.Editor;
        }

        protected virtual void OnEditorRemoved(EditorEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            EditorRemoved?.Invoke(this, e);

            if (CurrentEditor == e.Editor)
            {
                CurrentEditor = null;
            }
        }

        public class EditorCollection :
            ICollection<IEditor>,
            IReadOnlyCollection<IEditor>
        {
            public EditorCollection(EditorSelector editorSelector)
            {
                EditorSelector = editorSelector ??
                    throw new ArgumentNullException(
                        nameof(editorSelector));

                Editors = new HashSet<IEditor>();
            }

            public int Count
            {
                get
                {
                    return Editors.Count;
                }
            }

            bool ICollection<IEditor>.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            private EditorSelector EditorSelector
            {
                get;
            }

            private ICollection<IEditor> Editors
            {
                get;
            }

            public void Add(IEditor item)
            {
                if (item is null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                var e = new EditorEventArgs(item);
                if (Editors.Contains(item))
                {
                    EditorSelector.CurrentEditor = item;
                    return;
                }

                Editors.Add(item);
                EditorSelector.OnEditorAdded(e);
            }

            public void Clear()
            {
                var items = new List<IEditor>(Editors);
                foreach (var item in items)
                {
                    Remove(item);
                }
            }

            public bool Contains(IEditor item)
            {
                return Editors.Contains(item);
            }

            void ICollection<IEditor>.CopyTo(
                IEditor[] array,
                int arrayIndex)
            {
                Editors.CopyTo(array, arrayIndex);
            }

            public bool Remove(IEditor item)
            {
                if (!Editors.Remove(item))
                {
                    return false;
                }

                var e = new EditorEventArgs(item);
                EditorSelector.OnEditorRemoved(e);
                return true;
            }

            public IEnumerator<IEditor> GetEnumerator()
            {
                return Editors.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
