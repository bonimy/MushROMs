// <copyright file="CreateEditorDialog.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls.Editors
{
    using MushROMs;

    public class CreateEditorDialog : DialogProxy
    {
        private CreateEditorForm CreateEditorForm
        {
            get;
        }

        public CreateEditorCallback CreateEditorCallback
        {
            get
            {
                return CreateEditorForm.CreateEditorCallback;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope")]
        public CreateEditorDialog() : base(new CreateEditorForm())
        {
            CreateEditorForm = BaseForm as CreateEditorForm;
        }
    }
}
