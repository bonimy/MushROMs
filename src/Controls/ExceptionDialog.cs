// <copyright file="ExceptionDialog.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Windows.Forms;

namespace Controls
{
    public static class ExceptionDialog
    {
        public static void ShowException(Exception ex)
        {
            ShowException(null, ex, null);
        }

        public static void ShowException(IWin32Window owner, Exception ex)
        {
            ShowException(owner, ex, null);
        }

        public static void ShowException(Exception ex, string title)
        {
            ShowException(null, ex, title);
        }

        public static void ShowException(
            IWin32Window owner,
            Exception ex,
            string title)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            RtlAwareMessageBox.Show(
                owner,
                ex.Message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static bool ShowExceptionAndRetry(Exception ex)
        {
            return ShowExceptionAndRetry(null, ex, null);
        }

        public static bool ShowExceptionAndRetry(
            IWin32Window owner,
            Exception ex)
        {
            return ShowExceptionAndRetry(owner, ex, null);
        }

        public static bool ShowExceptionAndRetry(
            Exception ex,
            string title)
        {
            return ShowExceptionAndRetry(null, ex, title);
        }

        public static bool ShowExceptionAndRetry(
            IWin32Window owner,
            Exception ex,
            string title)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var dialogResult = RtlAwareMessageBox.Show(
                owner,
                ex.Message,
                title,
                MessageBoxButtons.RetryCancel,
                MessageBoxIcon.Warning);

            return dialogResult == DialogResult.Retry;
        }
    }
}
