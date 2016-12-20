using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace V2RayW
{

    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }

        private void buttonTS_Click(object sender, EventArgs e)
        {
            var tsWindow = new FormTransSetting();
            tsWindow.ShowDialog(this);
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Upgrade();
            textBoxLocalPort.Text = Properties.Settings.Default.localPort.ToString();
            checkBoxUDP.Checked = Properties.Settings.Default.udpSupport;
            textBoxDNS.Text = Properties.Settings.Default.dns;
            dynamic[] dProfiles = Properties.Settings.Default.profilesStr.Split('\t').Select(pstr => JObject.Parse(pstr)).ToArray();
            foreach (dynamic dp in dProfiles)
            {
                var p = new Profile();
                p.address = dp.address;
                p.allowPassive = dp.allowPassive;
                p.alterId = dp.alterId;
                p.network = dp.network;
                p.remark = dp.remark;
                p.userId = dp.userId;
                Program.profiles.Add(p);
                listBoxServers.Items.Add(p.remark == "" ? p.address : p.remark);
            }
            Program.selectedServerIndex = Properties.Settings.Default.selectedServerIndex;
            listBoxServers.SelectedIndex = Program.selectedServerIndex;
        }

        private void groupBoxServer_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
