using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MushROMs.GenericEditor
{
    public partial class MainForm : Form
    {
        public string StatusText
        {
            get
            {
                return tssStatus.Text;
            }
            set
            {
                tssStatus.Text = value;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void About_Click(object sender, EventArgs e)
        {
            using (var dialog = new AboutDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                foreach (var file in dlg.FileNames)
                {
                }
            }
        }
    }
}
