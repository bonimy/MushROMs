// <copyright file="EditorFormEventArgs.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.GenericEditor
{
    using System;
    using System.Windows.Forms;

    public class EditorFormEventArgs : EventArgs
    {
        public EditorFormEventArgs(IEditor editor, Form form)
        {
            Editor = editor ??
                throw new ArgumentNullException(nameof(editor));

            Form = form ??
                throw new ArgumentNullException(nameof(form));
        }

        public IEditor Editor
        {
            get;
        }

        public Form Form
        {
            get;
        }
    }
}
