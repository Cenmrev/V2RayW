using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace V2RayW
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        ConfigForm configForm;

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.configForm == null || this.configForm.IsDisposed)
            {
                configForm = new ConfigForm();
            }
            configForm.Show();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://v2ray.com");
        }

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            configureToolStripMenuItem_Click(sender, e);
        }
    }
}
