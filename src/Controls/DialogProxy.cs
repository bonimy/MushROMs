// <copyright file="DialogProxy.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    public abstract class DialogProxy :
        MarshalByRefObject,
        IComponent,
        IDisposable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Microsoft")]
        public event HelpEventHandler HelpRequested;

        public event EventHandler Disposed;

        protected Form BaseForm
        {
            get;
        }

        public bool ShowHelp
        {
            get
            {
                return BaseForm.HelpButton;
            }

            set
            {
                BaseForm.HelpButton = value;
            }
        }

        public string Title
        {
            get
            {
                return BaseForm.Text;
            }

            set
            {
                BaseForm.Text = value;
            }
        }

        public object Tag
        {
            get
            {
                return BaseForm.Tag;
            }

            set
            {
                BaseForm.Tag = value;
            }
        }

        public ISite Site
        {
            get
            {
                return BaseForm.Site;
            }

            set
            {
                BaseForm.Site = value;
            }
        }

        protected DialogProxy(Form baseForm)
        {
            BaseForm = baseForm ??
                throw new ArgumentNullException(nameof(baseForm));

            BaseForm.HelpRequested += BaseForm_HelpRequested;
            BaseForm.Disposed += BaseForm_Disposed;
        }

        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return BaseForm.ShowDialog(owner);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnHelpRequested(HelpEventArgs e)
        {
            HelpRequested?.Invoke(this, e);
        }

        protected virtual void OnDisposed(EventArgs e)
        {
            Disposed?.Invoke(this, e);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                BaseForm.Dispose();
            }
        }

        private void BaseForm_Disposed(object sender, EventArgs e)
        {
            OnDisposed(e);
        }

        private void BaseForm_HelpRequested(
            object sender,
            HelpEventArgs hlpevent)
        {
            OnHelpRequested(hlpevent);
        }
    }
}
