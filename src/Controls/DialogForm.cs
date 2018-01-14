// <copyright file="DialogForm.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System.Windows.Forms;

namespace Controls
{
    public class DialogForm : Form, IDialogForm
    {
        protected virtual object ProxySender
        {
            get
            {
                return this;
            }
        }
    }
}
