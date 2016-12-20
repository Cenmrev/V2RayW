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
    public partial class FormTransSetting : Form
    {
        public FormTransSetting()
        {
            InitializeComponent();
        }

        private void buttonTSCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonTSHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.v2ray.com/chapter_02/05_transport.html");
        }
    }
}
