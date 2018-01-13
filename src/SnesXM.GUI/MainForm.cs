using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnesXM.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void About_Click(object sender, EventArgs e)
        {
            using (var dlg = new AboutDialog())
            {
                dlg.ShowDialog(this);
            }
        }

        private string LoadGamePath()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.DefaultExt = "smc";
                dlg.ShowReadOnly = false;
                dlg.CheckFileExists = true;
                dlg.Multiselect = false;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                    return dlg.FileName;
            }

            return String.Empty;
        }

        private void tsmLoadGame_Click(object sender, EventArgs e)
        {
            LoadGamePath();
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
