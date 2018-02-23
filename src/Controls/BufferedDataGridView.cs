// <copyright file="BufferedDataGridView.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Windows.Forms;

namespace Controls
{
    public class BufferedDataGridView : DataGridView
    {
        public BufferedDataGridView()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }
    }
}
