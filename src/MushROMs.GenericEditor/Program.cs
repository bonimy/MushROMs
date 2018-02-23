// <copyright file="Program.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.GenericEditor
{
    using System;
    using System.Collections.Specialized;
    using System.Windows.Forms;
    using MushROMs.GenericEditor.Properties;

    public static class Program
    {
        internal static readonly Settings Settings = Settings.Default;

        public static void About()
        {
            About(null);
        }

        public static void About(IWin32Window owner)
        {
            using (var dialog = new AboutDialog())
            {
                dialog.ShowDialog(owner);
            }
        }

        internal static void InitializeSettings()
        {
            Settings.RecentFiles = new StringCollection();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += (s, e) =>
            {
                Settings.Save();
            };

            if (Settings.FirstTime)
            {
                InitializeSettings();

                Settings.FirstTime = false;
                Settings.Save();
            }

            using (var core = new Core())
            {
                core.MainForm.AboutClick += (sender, e) =>
                {
                    About(sender as IWin32Window);
                };

                Application.Run(core.MainForm);
            }
        }
    }
}
