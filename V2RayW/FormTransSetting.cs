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

        private void buttonTSSave_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Make sure you have read the help before clicking OK!", "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (res != DialogResult.OK)
            {
                return;
            }
            var transportSettings = new {
                kcpSettings = new 
                {
                    mtu = strToInt(textBoxKcpMtu.Text, 1350),
                    tti = strToInt(textBoxKcpTti.Text, 50),
                    uplinkCapacity = strToInt(textBoxKcpUc.Text, 5),
                    downlinkCapacity = strToInt(textBoxKcpDc.Text, 20),
                    readBufferSize = strToInt(textBoxKcpRb.Text, 2),
                    writeBufferSize = strToInt(textBoxKcpWb.Text, 2),
                    congestion = comboBoxKcpCon.Text,
                    header = comboBoxKcpHt.Text
                },
                tcpSettings = new 
                {
                    connectionReuse = checkBoxTcpCr.Checked ? "true" : "false",
                },
                wsSettings = new
                {
                    connectionReuse = checkBoxWsCr.Checked ? "true" : "false",
                    path = textBoxWsPath.Text,
                } 
            };
            Properties.Settings.Default["transportSettings"] = JsonConvert.SerializeObject(transportSettings);
            Properties.Settings.Default.Save();
            this.Close();
        }

        private int strToInt(string str, int defaultValue)
        {
            int result = 0;
            if (Int32.TryParse(str, out result))
            {
                return result;
            } else
            {
                return defaultValue;
            }
        }

        private void FormTransSetting_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Upgrade();
            //var transportSettings = JsonConvert.DeserializeObject(Properties.Settings.Default["transportSettings"].ToString());
            //dynamic transportSettings = JObject.Parse(Properties.Settings.Default["transportSettings"].ToString());
            string transportSettingsStr = (string)Properties.Settings.Default["transportSettings"];
            dynamic transportSettings = JObject.Parse(transportSettingsStr);
            textBoxKcpMtu.Text = transportSettings.kcpSettings.mtu;
            textBoxKcpTti.Text = transportSettings.kcpSettings.tti;
            textBoxKcpUc.Text = transportSettings.kcpSettings.uplinkCapacity;
            textBoxKcpDc.Text = transportSettings.kcpSettings.downlinkCapacity;
            textBoxKcpRb.Text = transportSettings.kcpSettings.readBufferSize;
            textBoxKcpWb.Text = transportSettings.kcpSettings.writeBufferSize;
            comboBoxKcpCon.SelectedIndex = transportSettings.kcpSettings.congestion == "false" ? 0 : 1;
            var headertype = transportSettings.kcpSettings.header;
            comboBoxKcpHt.SelectedIndex = headertype == "srtp" ? 1 : (headertype == "utp" ? 2 : 0);
            checkBoxTcpCr.Checked = transportSettings.tcpSettings.connectionReuse == "true";
            checkBoxWsCr.Checked = transportSettings.wsSettings.connectionReuse == "true";
            textBoxWsPath.Text = transportSettings.wsSettings.path;
            System.Diagnostics.Debug.WriteLine(transportSettingsStr);
            //textBoxKcpMtu.Text = transportSettings.kcpSettings.mtu;
        }
    }
}
