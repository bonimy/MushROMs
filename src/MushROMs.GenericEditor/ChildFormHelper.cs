// <copyright file="ChildFormHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.GenericEditor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Controls.Editors;
    using Helper;
    using Snes;

    public class ChildFormHelper
    {
        public ChildFormHelper()
        {
            Editors = new EditorFormDictionary(this);
            EditorFormHelpers =
                new Dictionary<IEditor, IEditorFormHelper>();
        }

        public event EventHandler<EditorFormEventArgs> EditorFormAdded;

        public event EventHandler<EditorFormEventArgs>
            EditorFormGotFocus;

        public event EventHandler<EditorEventArgs> EditorFormClosed;

        private EditorFormDictionary Editors
        {
            get;
        }

        private IDictionary<IEditor, IEditorFormHelper>
            EditorFormHelpers
        {
            get;
        }

        public void AddEditorForm(IWin32Window owner, IEditor editor)
        {
            var formEditor = CreateFormEditor(editor);
            var form = formEditor.Form;

            Editors.Add(form, editor);
            EditorFormHelpers.Add(editor, formEditor);

            form.FormClosed += ChildFormClosed;
            form.GotFocus += Form_GotFocus;

            var e = new EditorFormEventArgs(editor, form);
            OnEditorFormAdded(e);
        }

        public bool TryGetEditor(Form form, out IEditor editor)
        {
            return Editors.TryGetValue(form, out editor);
        }

        public bool TryGetForm(IEditor editor, out Form form)
        {
            if (TryGetEditorFormHelper(editor, out var editorFormHelper))
            {
                form = editorFormHelper.Form;
                return true;
            }

            form = null;
            return false;
        }

        public bool TryGetEditorFormHelper(
            IEditor editor,
            out IEditorFormHelper editorFormHelper)
        {
            return EditorFormHelpers.TryGetValue(
                editor,
                out editorFormHelper);
        }

        protected virtual void OnEditorFormAdded(EditorFormEventArgs e)
        {
            EditorFormAdded?.Invoke(this, e);
        }

        protected virtual void OnEditorFormGotFocus(
            EditorFormEventArgs e)
        {
            EditorFormGotFocus?.Invoke(this, e);
        }

        protected virtual void OnFormClosed(EditorEventArgs e)
        {
            EditorFormClosed?.Invoke(this, e);
        }

        private static IEditorFormHelper CreateFormEditor(
            IEditor editor)
        {
            switch (editor)
            {
                case null:
                    throw new ArgumentNullException(nameof(editor));

                case PaletteEditor paletteEditor:
                    return new PaletteFormEditor(
                        new PaletteForm(),
                        paletteEditor);

                default:
                    return null;
            }
        }

        private void Form_GotFocus(object sender, EventArgs e)
        {
            var form = sender as Form;
            var editor = Editors[form];
            var args = new EditorFormEventArgs(editor, form);
            OnEditorFormGotFocus(args);
        }

        private void ChildFormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            var form = sender as Form;
            var editor = Editors[form];
            Editors.Remove(form);
            EditorFormHelpers.Remove(editor);

            var editorArgs = new EditorEventArgs(editor);
            OnFormClosed(editorArgs);
        }

        private sealed class EditorFormDictionary :
            IDictionary<Form, IEditor>,
            IReadOnlyDictionary<Form, IEditor>
        {
            public EditorFormDictionary(
                ChildFormHelper childFormHelper)
            {
                ChildFormHelper = childFormHelper ??
                    throw new ArgumentNullException(
                        nameof(childFormHelper));

                Forms = new Dictionary<Form, IEditor>();
            }

            public int Count
            {
                get
                {
                    return Forms.Count;
                }
            }

            public ICollection<Form> Keys
            {
                get
                {
                    return Forms.Keys;
                }
            }

            IEnumerable<Form> IReadOnlyDictionary<Form, IEditor>.Keys
            {
                get
                {
                    return Keys;
                }
            }

            public ICollection<IEditor> Values
            {
                get
                {
                    return Forms.Values;
                }
            }

            IEnumerable<IEditor>
                IReadOnlyDictionary<Form, IEditor>.Values
            {
                get
                {
                    return Values;
                }
            }

            bool ICollection<KeyValuePair<Form, IEditor>>.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            private ChildFormHelper ChildFormHelper
            {
                get;
            }

            private IDictionary<Form, IEditor> Forms
            {
                get;
            }

            public IEditor this[Form key]
            {
                get
                {
                    return Forms[key];
                }

                set
                {
                    Forms[key] = value;
                }
            }

            public bool ContainsKey(Form key)
            {
                return Forms.ContainsKey(key);
            }

            public bool ContainsValue(IEditor value)
            {
                if (value is null)
                {
                    return false;
                }

                var comparer = PathComparer.Default;
                foreach (var editor in Values)
                {
                    if (comparer.Equals(editor.Path, value.Path))
                    {
                        return true;
                    }
                }

                return false;
            }

            public void Add(Form key, IEditor value)
            {
                Forms.Add(key, value);
            }

            public bool Remove(Form key)
            {
                return Forms.Remove(key);
            }

            public bool TryGetValue(Form key, out IEditor value)
            {
                return Forms.TryGetValue(key, out value);
            }

            void ICollection<KeyValuePair<Form, IEditor>>.Add(
                KeyValuePair<Form, IEditor> item)
            {
                Add(item.Key, item.Value);
            }

            public void Clear()
            {
                Forms.Clear();
            }

            bool ICollection<KeyValuePair<Form, IEditor>>.Contains(
                KeyValuePair<Form, IEditor> item)
            {
                return Forms.Contains(item);
            }

            bool ICollection<KeyValuePair<Form, IEditor>>.Remove(
                KeyValuePair<Form, IEditor> item)
            {
                return Forms.Remove(item);
            }

            void ICollection<KeyValuePair<Form, IEditor>>.CopyTo(
                KeyValuePair<Form, IEditor>[] array,
                int arrayIndex)
            {
                Forms.CopyTo(array, arrayIndex);
            }

            public IEnumerator<KeyValuePair<Form, IEditor>>
                GetEnumerator()
            {
                return Forms.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
