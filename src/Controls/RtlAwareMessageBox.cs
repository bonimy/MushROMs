// <copyright file="RtlAwareMessageBox.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

/*
Taken from:
http://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=EN-US&k=k%28CA1300%29;k%28TargetFrameworkMoniker-.NETFramework
*/

using System;
using System.Globalization;
using System.Windows.Forms;

namespace Controls
{
    public static class RtlAwareMessageBox
    {
        public static DialogResult Show(
            string text)
        {
            return Show(
                null,
                text,
                String.Empty);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text)
        {
            return Show(
                owner,
                text,
                String.Empty);
        }

        public static DialogResult Show(
            string text,
            string caption)
        {
            return Show(
                null,
                text,
                caption);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption)
        {
            return Show(
                owner,
                text,
                caption,
                MessageBoxButtons.OK);
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons)
        {
            return Show(
                null,
                text,
                caption,
                buttons);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons)
        {
            return Show(
                owner,
                text,
                caption,
                buttons,
                MessageBoxIcon.None);
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon)
        {
            return Show(
                null,
                text,
                caption,
                buttons,
                icon);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon)
        {
            return Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton)
        {
            return Show(
                null,
                text,
                caption,
                buttons,
                icon,
                defaultButton);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton)
        {
            return Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                0);
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options)
        {
            return Show(
                null,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                options);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options)
        {
            return MessageBox.Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(owner, options));
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            bool displayHelpButton)
        {
            return MessageBox.Show(
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(null, options),
                displayHelpButton);
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath)
        {
            return MessageBox.Show(
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(null, options),
                helpFilePath);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath)
        {
            return MessageBox.Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(owner, options),
                helpFilePath);
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            string keyword)
        {
            return MessageBox.Show(
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(null, options),
                helpFilePath,
                keyword);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            string keyword)
        {
            return MessageBox.Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(owner, options),
                helpFilePath,
                keyword);
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator)
        {
            return MessageBox.Show(
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(null, options),
                helpFilePath,
                navigator);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator)
        {
            return MessageBox.Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(owner, options),
                helpFilePath,
                navigator);
        }

        public static DialogResult Show(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator,
            object param)
        {
            return MessageBox.Show(
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(null, options),
                helpFilePath,
                navigator,
                param);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            string helpFilePath,
            HelpNavigator navigator,
            object param)
        {
            return MessageBox.Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(owner, options),
                helpFilePath,
                navigator,
                param);
        }

        public static MessageBoxOptions RightToLeftAwareOptions(
            IWin32Window owner,
            MessageBoxOptions options)
        {
            if (IsRightToLeft(owner))
            {
                options |= MessageBoxOptions.RtlReading;
                options |= MessageBoxOptions.RightAlign;
            }

            return options;
        }

        public static bool IsRightToLeft(IWin32Window owner)
        {
            var current = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
            if (owner is null)
            {
                return current;
            }

            var control = Control.FromHandle(owner.Handle);
            if (control is null)
            {
                return current;
            }

            return control.RightToLeft == RightToLeft.Yes;
        }
    }
}
