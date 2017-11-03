// <copyright file="DialogForm.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System.Windows.Forms;

namespace Controls
{
    public class DialogForm : Form
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
