using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
namespace V2RayW
{
    public partial class FormTransSetting : Form
    {
        private ServerProfile currentProfile;

        public FormTransSetting()
        {
            InitializeComponent();
            I18N.InitControl(this);
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
            var res = MessageBox.Show(I18N.GetValue("Make sure you have read the help before clicking OK!"), "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (res != DialogResult.OK)
            {
                return;
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            //mux
            currentProfile.muxSettings["enabled"] = checkBoxMuxEnable.Checked;
            currentProfile.muxSettings["concurrency"] = Program.strToInt(textBoxMuxCc.Text, 8);
            //proxy
            //currentProfile.proxyOutboundJson = checkBoxProxy.Checked ? textBoxOutProxyJson.Text : "";
            //streamSettings
            currentProfile.streamSettings["security"] = checkBoxTLSEnable.Checked ? "tls" : "none";
            //tls
            currentProfile.streamSettings["tlsSettings"] = new Dictionary<string, object> {
                    { "serverName", textBoxTLSSn.Text },
                    { "allowInsecure", checkBoxTLSAI.Checked },
                    };
            currentProfile.streamSettings["kcpSettings"] = new Dictionary<string, object> {
                    { "mtu", Program.strToInt(textBoxKcpMtu.Text, 1350) },
                    { "tti", Program.strToInt(textBoxKcpTti.Text, 20) },
                    { "uplinkCapacity", Program.strToInt(textBoxKcpUc.Text, 5) },
                    { "downlinkCapacity", Program.strToInt(textBoxKcpDc.Text, 20) },
                    { "congestion", comboBoxKcpCon.SelectedIndex == 1 },
                    { "readBufferSize", Program.strToInt(textBoxKcpRb.Text, 1) },
                    { "writeBufferSize", Program.strToInt(textBoxKcpWb.Text, 1) },
                    { "header", new Dictionary<string, object> {
                        { "type", comboBoxKcpHt.SelectedItem.ToString() }
                    } } };
            currentProfile.streamSettings["tcpSettings"] = new Dictionary<string, object> {
                    { "header", checkBoxTcpHeader.Checked ?
                    js.Deserialize<object>(textBoxTcpHeader.Text)
                    : new Dictionary<string, string> { { "type", "none" } } } };
            currentProfile.streamSettings["wsSettings"] = new Dictionary<string, object> {
                    { "path", textBoxWsPath.Text },
                    { "headers", js.Deserialize<object>(textBoxWsHeader.Text) } };
            var hosts = textBoxHttp2Hosts.Text.Split(',');
            var hosts_filtered = new System.Collections.ArrayList();
            foreach(var h in hosts)
            {
                var h_trimed = h.Trim();
                if (h_trimed.Length > 0)
                {
                    hosts_filtered.Add(h_trimed);
                }
            }
            currentProfile.streamSettings["httpSettings"] = new Dictionary<string, object> {
                    { "host", hosts_filtered },
                    { "path", textBoxHttp2Path.Text.Trim() }
                };
            this.Close();
        }



        private void FormTransSetting_Load(object sender, EventArgs e)
        {
            currentProfile = Program.mainForm.configForm.SelectedProfile();
            JavaScriptSerializer js = new JavaScriptSerializer();

            // load current settings to view
            //kcp
            dynamic streamSettings = currentProfile.streamSettings;
            textBoxKcpMtu.Text = streamSettings["kcpSettings"]["mtu"].ToString();
            textBoxKcpTti.Text = streamSettings["kcpSettings"]["tti"].ToString();
            textBoxKcpUc.Text = streamSettings["kcpSettings"]["uplinkCapacity"].ToString();
            textBoxKcpDc.Text = streamSettings["kcpSettings"]["downlinkCapacity"].ToString();
            textBoxKcpRb.Text = streamSettings["kcpSettings"]["readBufferSize"].ToString();
            textBoxKcpWb.Text = streamSettings["kcpSettings"]["writeBufferSize"].ToString();
            comboBoxKcpCon.SelectedIndex = streamSettings["kcpSettings"]["congestion"] == false ? 0 : 1;
            var headertype = streamSettings["kcpSettings"]["header"]["type"].ToString();
            comboBoxKcpHt.SelectedIndex = headertype == "wechat-video" ? 3 : (int)Enum.Parse(typeof(KcpHeaderType), headertype);

            //tcp
            checkBoxTcpHeader.Checked = streamSettings["tcpSettings"]["header"]["type"] == "none" ? false : true;
            if(checkBoxTcpHeader.Checked)
            {
                textBoxTcpHeader.Text = js.Serialize(streamSettings["tcpSettings"]["header"]);
            } else
            {
                textBoxTcpHeader.Text = "{\"type\": \"none\"}";
            }

            // ws
            textBoxWsPath.Text = streamSettings["wsSettings"]["path"];
            textBoxWsHeader.Text = js.Serialize(streamSettings["wsSettings"]["headers"]);

            //http2
            textBoxHttp2Path.Text = streamSettings["httpSettings"]["path"];
            System.Collections.ArrayList httpHosts = streamSettings["httpSettings"]["host"];
            List<string> hostList = new List<string>();
            foreach(var h in httpHosts)
            {
                hostList.Add(h.ToString());
            }
            if(hostList.Count > 0)
            {
                textBoxHttp2Hosts.Text = String.Join(",", hostList);
            } else
            {
                textBoxHttp2Hosts.Text = "";
            }

            // tls
            checkBoxTLSEnable.Checked = streamSettings["security"] == "tls";
            checkBoxTLSAI.Checked = streamSettings["tlsSettings"]["allowInsecure"];
            textBoxTLSSn.Text = streamSettings["tlsSettings"]["serverName"];

            // mux
            dynamic muxSettings = currentProfile.muxSettings;
            checkBoxMuxEnable.Checked = muxSettings["enabled"];
            textBoxMuxCc.Text = muxSettings["concurrency"].ToString();

            // proxy
            //checkBoxProxy.Checked = currentProfile.proxyOutboundJson.Length > 0;
            //textBoxOutProxyJson.Text = currentProfile.proxyOutboundJson;

        }

        // not finished
        private void buttonTsReset_Click(object sender, EventArgs e)
        {
        }
    }
}
