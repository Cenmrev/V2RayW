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
using System.Diagnostics;

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
            Properties.Settings.Default.localPort = Program.strToInt(textBoxLocalPort.Text, 8080);
            Properties.Settings.Default.udpSupport = checkBoxUDP.Checked;
            Properties.Settings.Default.dns = textBoxDNS.Text;
            Properties.Settings.Default.selectedServerIndex = listBoxServers.SelectedIndex;
            Properties.Settings.Default.Save();
            this.Close();
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
            loadProfiles();
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Program.profiles.Add(new Profile());
            Program.selectedServerIndex = Program.profiles.Count() - 1;
            /*
            for(int i = 0; i < Program.profiles.Count(); i++)
            {
                Debug.WriteLine(Program.profiles[i].address);
            }
            Debug.WriteLine(Program.selectedServerIndex);*/
            this.loadProfiles();
        }

        private void loadProfiles()
        {
            listBoxServers.Items.Clear();
            foreach (var p in Program.profiles)
            {
                listBoxServers.Items.Add(p.remark == "" ? p.address : p.remark);
            }
            //Debug.WriteLine(String.Format("lis cout = {0}", listBoxServers.Items.Count));
            if (Program.selectedServerIndex >= 0)
            {
                listBoxServers.SelectedIndex = Program.selectedServerIndex;
            } else
            {
                textBoxAddress.Text = "";
                textBoxPort.Text = "";
                textBoxUserId.Text = "";
                textBoxAlterID.Text = "";
                textBoxRemark.Text = "";
                checkBoxAllowP.Checked = false;
                comboBoxNetwork.SelectedIndex = 0;
            }
        }

        private void listBoxServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.selectedServerIndex = listBoxServers.SelectedIndex;
            var sp = Program.profiles[Program.selectedServerIndex];
            textBoxAddress.Text = sp.address;
            textBoxPort.Text = sp.port.ToString();
            textBoxUserId.Text = sp.userId;
            textBoxAlterID.Text = sp.alterId.ToString();
            textBoxRemark.Text = sp.remark;
            checkBoxAllowP.Checked = sp.allowPassive;
            comboBoxNetwork.SelectedIndex = sp.network;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (Program.profiles.Count <= 0)
            {
                return;
            }
            Program.profiles.RemoveAt(Program.selectedServerIndex);
            if(Program.selectedServerIndex >= Program.profiles.Count())
            {
                Program.selectedServerIndex -= 1;
            }
            loadProfiles();
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            if(Program.selectedServerIndex >= 0)
            {
                Program.profiles[Program.selectedServerIndex].address = textBoxAddress.Text;
            }
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            if (Program.selectedServerIndex >= 0)
            {
                Program.profiles[Program.selectedServerIndex].port = Program.strToInt(textBoxPort.Text, 10086);
            }
        }

        private void textBoxUserId_TextChanged(object sender, EventArgs e)
        {
            if (Program.selectedServerIndex >= 0)
            {
                Program.profiles[Program.selectedServerIndex].userId = textBoxUserId.Text;
            }
        }

        private void textBoxAlterID_TextChanged(object sender, EventArgs e)
        {
            if (Program.selectedServerIndex >= 0)
            {
                Program.profiles[Program.selectedServerIndex].alterId = Program.strToInt(textBoxAlterID.Text, 20);
            }
        }

        private void textBoxRemark_TextChanged(object sender, EventArgs e)
        {
            if (Program.selectedServerIndex >= 0)
            {
                Program.profiles[Program.selectedServerIndex].remark = textBoxRemark.Text;
            }
        }

        private void checkBoxAllowP_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.selectedServerIndex >= 0)
            {
                Program.profiles[Program.selectedServerIndex].allowPassive = checkBoxAllowP.Checked;
            }
        }

        private void comboBoxNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.selectedServerIndex >= 0)
            {
                Program.profiles[Program.selectedServerIndex].network = comboBoxNetwork.SelectedIndex;
            }
        }
    }
}
