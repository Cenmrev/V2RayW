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
        public static List<Profile> profiles = new List<Profile>();
        public static int selectedServerIndex;

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
            Properties.Settings.Default.dns = textBoxDNS.Text != "" ? textBoxDNS.Text : "localhost";
            Properties.Settings.Default.selectedServerIndex = listBoxServers.SelectedIndex;
            Properties.Settings.Default.Save();
            Program.profiles.Clear();
            if (profiles.Count > 0)
            {
                foreach (Profile p in profiles)
                {
                    Program.profiles.Add(p.DeepCopy());
                }
            } else
            {
                Program.proxyIsOn = false;
            }
            Program.selectedServerIndex = selectedServerIndex;
            Program.updateSystemProxy();
            Program.mainForm.updateMenu();
            this.Close();
        }

        private void buttonTS_Click(object sender, EventArgs e)
        {
            var tsWindow = new FormTransSetting();
            tsWindow.ShowDialog(this);
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.vw256;
            selectedServerIndex = Program.selectedServerIndex;
            profiles.Clear();
            foreach (Profile p in Program.profiles)
            {
                profiles.Add(p.DeepCopy());
            }

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
            profiles.Add(new Profile());
            selectedServerIndex = profiles.Count() - 1;
            /*
            for(int i = 0; i < profiles.Count(); i++)
            {
                Debug.WriteLine(profiles[i].address);
            }
            Debug.WriteLine(selectedServerIndex);*/
            this.loadProfiles();
        }

        private void loadProfiles()
        {
            listBoxServers.Items.Clear();
            foreach (var p in profiles)
            {
                listBoxServers.Items.Add(p.remark == "" ? p.address : p.remark);
            }
            if (selectedServerIndex >= 0)
            {
                listBoxServers.SelectedIndex = selectedServerIndex;
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
            selectedServerIndex = listBoxServers.SelectedIndex;
            var sp = profiles[selectedServerIndex];
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
            if (profiles.Count <= 0)
            {
                return;
            }
            profiles.RemoveAt(selectedServerIndex);
            if(selectedServerIndex >= profiles.Count())
            {
                selectedServerIndex -= 1;
            }
            loadProfiles();
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            if(selectedServerIndex >= 0)
            {
                profiles[selectedServerIndex].address = textBoxAddress.Text;
                loadProfiles();
            }
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0)
            {
                profiles[selectedServerIndex].port = Program.strToInt(textBoxPort.Text, 10086);
            }
        }

        private void textBoxUserId_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0)
            {
                profiles[selectedServerIndex].userId = textBoxUserId.Text;
            }
        }

        private void textBoxAlterID_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0)
            {
                profiles[selectedServerIndex].alterId = Program.strToInt(textBoxAlterID.Text, 0);
            }
        }

        private void textBoxRemark_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0)
            {
                profiles[selectedServerIndex].remark = textBoxRemark.Text;
                loadProfiles();
            }
        }

        private void checkBoxAllowP_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0)
            {
                profiles[selectedServerIndex].allowPassive = checkBoxAllowP.Checked;
            }
        }

        private void comboBoxNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0)
            {
                profiles[selectedServerIndex].network = comboBoxNetwork.SelectedIndex;
            }
        }
    }
}
