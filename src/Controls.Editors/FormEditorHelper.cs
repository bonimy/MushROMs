// <copyright file="FormEditorHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using MushROMs;

namespace Controls.Editors
{
    public abstract class FormEditorHelper : IEditorFormHelper
    {
        protected TileMapForm Form
        {
            get;
        }

        TileMapForm IEditorFormHelper.Form
        {
            get
            {
                return Form;
            }
        }

        protected IEditor Editor
        {
            get;
        }

        IEditor IEditorFormHelper.Editor
        {
            get
            {
                return Editor;
            }
        }

        protected FormEditorHelper(TileMapForm form, IEditor editor)
        {
            Form = form ?? throw new ArgumentNullException(nameof(form));
            Editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }
    }
}
