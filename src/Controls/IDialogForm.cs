// <copyright file="IDialogForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Controls
{
    public interface IDialogForm : IComponent
    {
        event HelpEventHandler HelpRequested;

        string Text
        {
            get;
            set;
        }

        bool HelpButton
        {
            get;
            set;
        }

        object Tag
        {
            get;
            set;
        }

        DialogResult ShowDialog(IWin32Window owner);

        ObjRef CreateObjRef(Type requestedType);

        object InitializeLifetimeService();
    }
}
