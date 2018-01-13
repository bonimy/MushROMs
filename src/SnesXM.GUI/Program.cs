using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnesXM.Emulator;

namespace SnesXM.GUI
{
    static class Program
    {
        static IEmulator Emulator { get; set; }
        static ISettings Settings => Emulator.Settings;
        static IDisplay Display => Emulator.Display;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Emulator = new Emulator.Emulator();

            Console.SetOut(new StreamWriter("stdout.txt"));
            Console.SetError(new StreamWriter("stderr.txt"));

            Settings.StopEmulation = true;
            Directory.SetCurrentDirectory(Display.GetDirectory(DirectoryType.Default));

            Application.Run(new MainForm());
        }
    }
}
