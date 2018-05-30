using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace V2RayW
{

    public partial class ConfigForm : Form
    {
        private int localPort = 1081;
        private int httpPort = 8081;
        private bool udpSupport = false;
        private bool shareOverLan = false;
        private string dnsString = "localhost";
        private LogLevel logLevel = LogLevel.none;
        public List<ServerProfile> profiles = new List<ServerProfile>();
        public int selectedServerIndex = 1;
        private int mainInboundType = 0;
        private bool alarmUnknown = true;

        public ServerProfile SelectedProfile()
        {
            return this.profiles[this.selectedServerIndex];
        }

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
            Program.localPort = this.localPort;
            Program.httpPort = this.httpPort;
            Program.udpSupport = this.udpSupport;
            Program.shareOverLan = this.shareOverLan;
            Program.dnsString = String.Copy(this.dnsString);
            Program.logLevel = this.logLevel;
            Program.profiles.Clear();
            foreach (var p in this.profiles)
            {
                
                Program.profiles.Add(p.DeepCopy());
            };
            if(this.selectedServerIndex < Program.profiles.Count && this.selectedServerIndex >= 0)
            {
                Program.selectedServerIndex = this.selectedServerIndex;
            } else
            {
                if(Program.profiles.Count > 0)
                {
                    Program.selectedServerIndex = 0;
                } else
                {
                    Program.selectedServerIndex = -1;
                }
            }
            Program.selectedServerIndex = this.selectedServerIndex;
            Program.mainInboundType = this.mainInboundType;
            Program.alarmUnknown = this.alarmUnknown;
            Program.configurationDidChange();
            this.Close();
        }

        private void buttonTS_Click(object sender, EventArgs e)
        {
            var tsWindow = new FormTransSetting();
            tsWindow.ShowDialog(this);
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            //copy settings
            this.localPort = Program.localPort;
            this.httpPort = Program.httpPort;
            this.udpSupport = Program.udpSupport;
            this.shareOverLan = Program.shareOverLan;
            this.dnsString = String.Copy(Program.dnsString);
            this.logLevel = Program.logLevel;
            this.profiles.Clear();
            foreach(var p in Program.profiles)
            {
                this.profiles.Add(p.DeepCopy());
            };
            this.selectedServerIndex = Program.selectedServerIndex;
            this.mainInboundType = Program.mainInboundType;
            this.alarmUnknown = Program.alarmUnknown;
            
            // update views
            this.Icon = Properties.Resources.vw256;

            comboBoxInP.SelectedIndex = Program.mainInboundType;
            textBoxLocalPort.Text = this.localPort.ToString();
            checkBoxUDP.Checked = this.udpSupport;
            textBoxHttpPort.Text = this.httpPort.ToString();
            checkBoxShareOverLan.Checked = this.shareOverLan;
            textBoxDNS.Text = this.dnsString;
            comboBoxLog.SelectedIndex = (int)this.logLevel;
            checkBoxAlarm.Checked = this.alarmUnknown;
            loadProfiles();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            profiles.Add(new ServerProfile());
            selectedServerIndex = profiles.Count() - 1;
            this.loadProfiles();
        }

        private void loadProfiles()
        {
            listBoxServers.Items.Clear();
            foreach (var p in profiles)
            {
                listBoxServers.Items.Add(p.remark == "" ? p.address : p.remark);
            }
            if (selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                listBoxServers.SelectedIndex = selectedServerIndex;
            }
            listBoxServers_SelectedIndexChanged(null, null);
        }

        private void listBoxServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedServerIndex = listBoxServers.SelectedIndex;
            if (selectedServerIndex >= 0 && selectedServerIndex < this.profiles.Count())
            {
                var sp = profiles[selectedServerIndex];
                textBoxAddress.Text = sp.address;
                textBoxPort.Text = sp.port.ToString();
                textBoxUserId.Text = sp.userId;
                textBoxAlterID.Text = sp.alterId.ToString();
                textBoxRemark.Text = sp.remark;
                comboBoxNetwork.SelectedIndex = (int)sp.network;
                comboBoxSecurity.SelectedIndex = (int)sp.security;
                buttonTS.Enabled = true;
            } else
            {
                textBoxPort.Text = "";
                textBoxUserId.Text = "";
                textBoxAlterID.Text = "";
                textBoxRemark.Text = "";
                comboBoxNetwork.SelectedIndex = 0;
                comboBoxSecurity.SelectedIndex = 0;
                buttonTS.Enabled = false;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (profiles.Count <= 0)
            {
                return;
            }
            if(selectedServerIndex >= 0 &&
                selectedServerIndex < profiles.Count())
            {
                this.profiles.RemoveAt(this.selectedServerIndex);
                selectedServerIndex -= 1;
                if(selectedServerIndex == -1 && this.profiles.Count() > 0 )
                {
                    selectedServerIndex = 0;
                }
                loadProfiles();

            }
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            if(selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                profiles[selectedServerIndex].address = textBoxAddress.Text;
                loadProfiles();
            }
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                profiles[selectedServerIndex].port = Program.strToInt(textBoxPort.Text, 10086);
            }
        }

        private void textBoxUserId_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                profiles[selectedServerIndex].userId = textBoxUserId.Text;
            }
        }

        private void textBoxAlterID_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                profiles[selectedServerIndex].alterId = Program.strToInt(textBoxAlterID.Text, 0);
            }
        }

        private void textBoxRemark_TextChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                profiles[selectedServerIndex].remark = textBoxRemark.Text;
                loadProfiles();
            }
        }
        

        private void comboBoxNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                profiles[selectedServerIndex].network = (NetWorkType)comboBoxNetwork.SelectedIndex;
            }
        }

        private void comboBoxInP_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mainInboundType = comboBoxInP.SelectedIndex; // 0: http, 1:socks
        }

        private void comboBoxSecurity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedServerIndex >= 0 && selectedServerIndex < profiles.Count())
            {
                profiles[selectedServerIndex].security = (SecurityType)comboBoxSecurity.SelectedIndex;
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            var importWindow = new FormImport();
            importWindow.ShowDialog(this);
        }

        private void checkBoxShareOverLan_CheckedChanged(object sender, EventArgs e)
        {
            this.shareOverLan = checkBoxShareOverLan.Checked;
        }

        private void textBoxLocalPort_TextChanged(object sender, EventArgs e)
        {
            this.localPort = Program.strToInt(textBoxLocalPort.Text, 1081);
        }

        private void checkBoxUDP_CheckedChanged(object sender, EventArgs e)
        {
            this.udpSupport = checkBoxUDP.Checked;
        }

        private void textBoxHttpPort_TextChanged(object sender, EventArgs e)
        {
            this.httpPort = Program.strToInt(textBoxHttpPort.Text, 8081);

        }

        private void textBoxDNS_TextChanged(object sender, EventArgs e)
        {
            this.dnsString = textBoxDNS.Text.Trim();
        }

        private void checkBoxAlarm_CheckedChanged(object sender, EventArgs e)
        {
            this.alarmUnknown = checkBoxAlarm.Checked;
        }

        private void comboBoxLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.logLevel = (LogLevel)comboBoxLog.SelectedIndex;
        }
    }
}
