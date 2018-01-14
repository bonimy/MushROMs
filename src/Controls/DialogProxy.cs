// <copyright file="DialogProxy.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Controls
{
    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    public abstract class DialogProxy : MarshalByRefObject, IComponent, IDisposable
    {
        protected abstract IDialogForm BaseForm
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

        public event HelpEventHandler HelpRequested
        {
            add { BaseForm.HelpRequested += value; }
            remove { BaseForm.HelpRequested -= value; }
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

        public event EventHandler Disposed
        {
            add { BaseForm.Disposed += value; }
            remove { BaseForm.Disposed -= value; }
        }

        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return BaseForm.ShowDialog(owner);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override ObjRef CreateObjRef(Type requestedType)
        {
            return BaseForm.CreateObjRef(requestedType);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return BaseForm.InitializeLifetimeService();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                BaseForm.Dispose();
            }
        }
    }
}
