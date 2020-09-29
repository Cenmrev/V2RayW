using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using V2RayW.Resources;

namespace V2RayW
{
    /// <summary>
    /// Interaction logic for TransportWindow.xaml
    /// </summary>
    /// 

    public partial class TransportWindow : Window
    {
        private ConfigWindow configWindow;
        public TransportWindow()
        {
            InitializeComponent();
            kcpCongestionBox.Items.Add("false");
            kcpCongestionBox.Items.Add("true");
            foreach (string obfu in Utilities.OBFU_LIST)
            {
                kcpHeaderBox.Items.Add(obfu);
            }
        }

        public void InitializeData()
        {
            configWindow = this.Owner as ConfigWindow;
            Dictionary<string, object> muxSettings = configWindow.profiles[configWindow.vmessListBox.SelectedIndex]["mux"] as Dictionary<string, object>;
            Dictionary<string, object> streamSettings = configWindow.profiles[configWindow.vmessListBox.SelectedIndex]["streamSettings"] as Dictionary<string, object>;
            FillinData(streamSettings, muxSettings);
        }

        private void FillinData(Dictionary<string, object> streamSettings, Dictionary<string, object> muxSettings)
        {
            muxEnableBox.IsChecked = (bool)muxSettings["enabled"];
            muxConcurrencyBox.Text = muxSettings["concurrency"].ToString();

            #region kcp
            Dictionary<string, object> kcpSettings = streamSettings["kcpSettings"] as Dictionary<string, object>;
            kcpMtuBox.Text = kcpSettings["mtu"].ToString();
            kcpTtiBox.Text = kcpSettings["tti"].ToString();
            kcpReadBox.Text = kcpSettings["readBufferSize"].ToString();
            kcpWriteBox.Text = kcpSettings["writeBufferSize"].ToString();
            kcpDownlinkBox.Text = kcpSettings["downlinkCapacity"].ToString();
            kcpUplinkBox.Text = kcpSettings["uplinkCapacity"].ToString();
            kcpCongestionBox.SelectedIndex = (bool)kcpSettings["congestion"] ? 1 : 0;
            kcpHeaderBox.SelectedIndex = Utilities.OBFU_LIST.FindIndex(x => x == (kcpSettings["header"] as Dictionary<string, object>)["type"] as string);
            #endregion

            #region tcp
            Dictionary<string, object> tcpSettings = streamSettings["tcpSettings"] as Dictionary<string, object>;
            tcpHeaderCheckBox.IsChecked = (tcpSettings[@"header"] as Dictionary<string, object>)["type"].ToString() != "none";
            tcpHeaderContentBox.Text = JsonConvert.SerializeObject(tcpSettings, Formatting.Indented);
            if (Utilities.IsWindows10())
            {
                tcpForceBox.IsChecked = (streamSettings["sockopt"] as Dictionary<string, object>).ContainsKey("tcpFastOpen");
            }
            else
            {
                tcpForceBox.IsChecked = false;
                tcpForceBox.Visibility = Visibility.Hidden;
            }
            #endregion tcp

            #region ws
            Dictionary<string, object> wsSettings = streamSettings["wsSettings"] as Dictionary<string, object>;
            wsPathBox.Text = wsSettings["path"].ToString();
            wsHeaderBox.Text = JsonConvert.SerializeObject(wsSettings["headers"], Formatting.Indented);
            #endregion

            #region http2
            Dictionary<string, object> httpSettings = streamSettings["httpSettings"] as Dictionary<string, object>;
            httpHostBox.Text = httpSettings.ContainsKey("host") ? String.Join(",", httpSettings["host"] as object[]) : "";
            httpPathBox.Text = httpSettings["path"].ToString();
            #endregion

            #region quick
            Dictionary<string, object> quicSettings = streamSettings["quicSettings"] as Dictionary<string, object>;
            quicSecurityBox.Items.Clear();
            foreach (string security in Utilities.QUIC_SECURITY_LIST)
            {
                quicSecurityBox.Items.Add(security);
            }
            quicSecurityBox.SelectedIndex = Utilities.QUIC_SECURITY_LIST.FindIndex(x => x == quicSettings["security"] as string);
            quicKeyBox.Text = quicSettings["key"].ToString();
            quicHeaderBox.Items.Clear();
            foreach (string obfu in Utilities.OBFU_LIST)
            {
                quicHeaderBox.Items.Add(obfu);
            }
            quicHeaderBox.SelectedIndex = Utilities.OBFU_LIST.FindIndex(x => x == (quicSettings["header"] as Dictionary<string, object>)["type"] as string);
            #endregion

            #region tls
            tlsEnableBox.IsChecked = streamSettings["security"] as string == "tls" || streamSettings["security"] as string == "xtls";
            Dictionary<string, object> tlsSettings = streamSettings["tlsSettings"] as Dictionary<string, object>;
            if (streamSettings["security"] as string == "xtls")
            {
                tlsSettings = streamSettings["xtlsSettings"] as Dictionary<string, object>;
            }
            tlsAlpnBox.Text = String.Join(",", tlsSettings["alpn"] as object[]);
            tlsServerBox.Text = tlsSettings["serverName"] as string;
            tlsInsecureBox.IsChecked = (bool)tlsSettings["allowInsecure"];
            tlsInsecureCipherBox.IsChecked = (bool)tlsSettings["allowInsecureCiphers"];
            #endregion
        }

        private void TcpExampleButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Strings.tcpHelpPage);
        }

        private int ParseNumberFromBox(TextBox box, int defaultValue, int? min, int? max)
        {
            int result;
            try
            {
                result = Int32.Parse(box.Text);
            } catch
            {
                result = defaultValue;
            }
            result = Math.Min(Math.Max(result, min ?? int.MinValue), max ?? int.MaxValue);
            return result;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> tcpSettings = new Dictionary<string, object>
            {
                {"header", new Dictionary<string, object>{ {"type", "none"} } }
            };
            if (tcpHeaderCheckBox.IsChecked ?? false)
            {
                try
                {
                    tcpSettings = Utilities.javaScriptSerializer.Deserialize<dynamic>(tcpHeaderContentBox.Text);
                    if (tcpSettings == null)
                    {
                        throw new NullReferenceException();
                    }
                } catch
                {
                    MessageBox.Show(this, "TCP " + Strings.header + "\n" + tcpHeaderContentBox.Text, Strings.messagenotvalidjson);
                    return;
                }
            }

            Dictionary<string, object> wsHeaders;
            try
            {
                wsHeaders = Utilities.javaScriptSerializer.Deserialize<dynamic>(wsHeaderBox.Text);
            } catch
            {
                MessageBox.Show("WebSocket " + Strings.header + "\n" + wsHeaderBox.Text, Strings.messagenotvalidjson);
                return;
            }
            if (wsHeaders == null)
            {
                wsHeaders = new Dictionary<string, object>();
            }

            configWindow.profiles[configWindow.vmessListBox.SelectedIndex]["mux"] = new Dictionary<string, object>
            {
                {"enabled", muxEnableBox.IsChecked },
                {"concurrency", ParseNumberFromBox(muxConcurrencyBox, 8, 1, 1024) }
            };

            Dictionary<string, object> streamSettings = configWindow.profiles[configWindow.vmessListBox.SelectedIndex]["streamSettings"] as Dictionary<string, object>;

            streamSettings["kcpSettings"] = new Dictionary<string, object>
            {
                { "header" ,new Dictionary<string, object> { { "type" ,  kcpHeaderBox.SelectedItem.ToString() } } },
                { "mtu", ParseNumberFromBox(kcpMtuBox, 1350, 576, 1460) },
                { "congestion" , kcpCongestionBox.SelectedIndex == 1 },
                { "tti" , ParseNumberFromBox(kcpTtiBox, 50, 10, 100) },
                { "uplinkCapacity" , ParseNumberFromBox(kcpUplinkBox, 5, null, null) },
                { "writeBufferSize" , ParseNumberFromBox(kcpWriteBox, 2, null, null) },
                { "readBufferSize" , ParseNumberFromBox(kcpWriteBox, 2, null, null) },
                { "downlinkCapacity" , ParseNumberFromBox(kcpDownlinkBox, 20, null, null) }
            };

            streamSettings["tcpSettings"] = tcpSettings;
            if (tcpForceBox.IsChecked ?? false && Utilities.IsWindows10())
            {
                streamSettings["sockopt"] = new Dictionary<string, object> { { "tcpFastOpen", true } };
            } else
            {
                streamSettings["sockopt"] = new Dictionary<string, object>();
            }
            streamSettings["wsSettings"] = new Dictionary<string, object>
            {
                {"path", wsPathBox.Text.Trim() },
                {"headers", wsHeaders }
            };
            var httpHostList = httpHostBox.Text.Trim().Split(',');
            streamSettings["httpSettings"] = httpHostList.Count() > 0 ? new Dictionary<string, object>
            {
                {"host", httpHostList },
                {"path", httpPathBox.Text.Trim() }
            } : new Dictionary<string, object>
            {
                {"path", httpPathBox.Text.Trim() }
            };
            streamSettings["quicSettings"] = new Dictionary<string, object>
            {
                {"key", quicKeyBox.Text },
                {"security", quicSecurityBox.SelectedItem.ToString() },
                {"header", new Dictionary<string, object>{ { "type", quicHeaderBox.SelectedItem.ToString() } } }
            };
            streamSettings["security"] = tlsEnableBox.IsChecked ?? false ? streamSettings["security"].ToString() : "none";
            Dictionary<string, object> tlsxtlsSettings = new Dictionary<string, object> {
                { "allowInsecure", tlsInsecureBox.IsChecked ?? false },
                { "alpn", tlsAlpnBox.Text.Split(',') },
                { "serverName", tlsServerBox.Text.Trim() },
                { "allowInsecureCiphers", tlsInsecureCipherBox.IsChecked ?? false }
            };
            if (streamSettings["security"] as string == "tls")
            {
                streamSettings["tlsSettings"] = tlsxtlsSettings;
            }
            if(streamSettings["security"] as string == "xtls")
            {
                streamSettings["xtlsSettings"] = tlsxtlsSettings;
            }
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if(mainTabControl.SelectedIndex == mainTabControl.Items.Count - 1)
            {
                Process.Start(Strings.muxHelpPage);
            } else
            {
                Process.Start(Strings.transportHelpPage);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> vmessTemplate = Utilities.VmessOutboundTemplate();
            FillinData(vmessTemplate["streamSettings"] as Dictionary<string, object>, vmessTemplate["mux"] as Dictionary<string, object>);
        }
    }
}
