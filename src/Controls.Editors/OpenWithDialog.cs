// <copyright file="OpenWithDialog.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using System.Collections.Generic;
    using MushROMs;

    public class OpenWithDialog : DialogProxy
    {
        private OpenWithForm OpenWithForm
        {
            get;
        }

        public OpenEditorCallback OpenEditorMethod
        {
            get
            {
                return OpenWithForm.SelectedOpenEditorMethod;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope")]
        public OpenWithDialog() : base(new OpenWithForm())
        {
            OpenWithForm = BaseForm as OpenWithForm;
        }

        public void ClearEditors()
        {
            OpenWithForm.ClearEditors();
        }

        public void AddEditor(
            OpenEditorCallbackInfo openEditorCallbackInfo)
        {
            OpenWithForm.AddEditor(openEditorCallbackInfo);
        }

        public void AddEditors(
            IEnumerable<OpenEditorCallbackInfo> types)
        {
            OpenWithForm.AddEditors(types);
        }
    }
}
