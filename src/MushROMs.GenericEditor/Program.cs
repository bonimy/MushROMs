using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MushROMs.GenericEditor.Properties;

namespace MushROMs.GenericEditor
{
    internal static class Program
    {
        private static Settings Settings
        {
            get
            {
                return Settings.Default;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;

            if (Settings.FirstTime)
            {
                ResetSettings();

                Settings.FirstTime = false;
                Settings.Save();
            }

            using (var form = new MasterForm())
            {
                Application.Run(form);
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Settings.Save();
        }

        public static void ResetSettings()
        {
            Settings.RecentFiles = new System.Collections.Specialized.StringCollection();
        }
    }
}
