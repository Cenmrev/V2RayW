using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace V2RayW
{

    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public MainWindow mainWindow;

        public List<Dictionary<string, object>> profiles; // only vmess
        public List<Dictionary<string, object>> outbounds; // except vmess
        public List<string> subscribeUrl = new List<string>();
        public List<Dictionary<string, object>> routingRuleSets;
        public bool enableRestore;
        private BackgroundWorker coreVersionCheckWorker = new BackgroundWorker();

        public ConfigWindow()
        {
            InitializeComponent();

            // initialize UI
            foreach (string protocol in Utilities.PROTOCOL_LIST)
            {
                protocolComboBox.Items.Add(protocol);
            }
            foreach (string security in Utilities.VMESS_SECURITY_LIST) 
            {
                securityComboBox.Items.Add(security);
            }
            logLevelBox.Items.Clear();
            foreach(string level in Utilities.LOG_LEVEL_LIST)
            {
                logLevelBox.Items.Add(level);
            }
            foreach(string network in Utilities.NETWORK_LIST)
            {
                networkBox.Items.Add(network);
            }
            DataContext = this;


        }

        private bool VmessForUI(Dictionary<string, object> outbound)
        {
            try
            {
                if (outbound["protocol"].ToString() != "vmess" && outbound["protocol"].ToString() != "vless")
                {
                    return false;
                }
                Dictionary<string, object> settings = outbound["settings"] as Dictionary<string, object>;
                if ((settings["vnext"] as IList<object>).Count() != 1)
                {
                    return false;
                }
                Dictionary<string, object> vnext = (settings["vnext"] as IList<object>)[0] as Dictionary<string, object>;
                if((vnext["users"] as IList<object>).Count() != 1)
                {
                    return false;
                }
            } catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        public void InitializeData()
        {
            // copy data 
            routingRuleSets = Utilities.DeepClone(mainWindow.routingRuleSets);
            subscribeUrl = Utilities.DeepClone(mainWindow.subscribeUrl);
            enableRestore = mainWindow.enableRestore;
            outbounds = new List<Dictionary<string, object>>();
            profiles = new List<Dictionary<string, object>>();
            foreach (Dictionary<string, object> outbound in Utilities.DeepClone(mainWindow.profiles))
            {
                if (VmessForUI(outbound))
                {
                    Utilities.AddMissingKeysForVmess(outbound);
                    profiles.Add(outbound);
                }
                else
                {
                    outbounds.Add(outbound);
                }
            }

            // fill in data
            autoStartBox.IsChecked = ExtraUtils.AutoStartCheck();
            udpSupportBox.IsChecked = mainWindow.udpSupport;
            shareOverLanBox.IsChecked = mainWindow.shareOverLan;
            localPortBox.Text = mainWindow.localPort.ToString();
            httpPortBox.Text = mainWindow.httpPort.ToString();
            dnsBox.Text = mainWindow.dnsString;
            logLevelBox.SelectedIndex = Utilities.LOG_LEVEL_LIST.FindIndex(x => x == mainWindow.logLevel);

            vmessListBox.Items.Clear();
            foreach (Dictionary<string, object> profile in profiles)
            {
                vmessListBox.Items.Add(profile["tag"]);
            }

            vmessListBox.SelectionChanged += VmessListBox_SelectionChanged;
            vmessListBox.SelectedIndex = 0;
            
            
            coreVersionCheckWorker.DoWork += CoreVersionCheckWorker_DoWork;
            coreVersionCheckWorker.RunWorkerAsync();

        }

        public void RefreshListBox()
        {
            var selectedIndex = vmessListBox.SelectedIndex;
            vmessListBox.Items.Clear();
            foreach (Dictionary<string, object> profile in profiles)
            {
                vmessListBox.Items.Add(profile["tag"]);
            }
            vmessListBox.SelectedIndex = Math.Min(selectedIndex, profiles.Count - 1);
        }

        private void CoreVersionCheckWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Process v2rayProcess = new Process();
                v2rayProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"v2ray-core\v2ray.exe";
                v2rayProcess.StartInfo.Arguments = @"-version";
                v2rayProcess.StartInfo.RedirectStandardOutput = true;
                v2rayProcess.StartInfo.UseShellExecute = false;
                v2rayProcess.StartInfo.CreateNoWindow = true;
                v2rayProcess.Start();
                v2rayProcess.WaitForExit();
                string version = v2rayProcess.StandardOutput.ReadLine();
                version = version.Substring(0,version.IndexOf("("));
                Dispatcher.Invoke(() => {
                    versionLabel.Content = version;
                });
            } catch
            {
                Dispatcher.Invoke(() => {
                    versionLabel.Content = "No valid core found";
                });
            }
        }

        private void VmessListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(vmessListBox.SelectedIndex >= 0 && vmessListBox.SelectedIndex < profiles.Count)
            {
                Dictionary<string, object> selectedProfile = profiles[vmessListBox.SelectedIndex];
                Dictionary<string, object> selectedVnext = ((selectedProfile["settings"] as Dictionary<string, object>)["vnext"] as IList<object>)[0] as Dictionary<string, object>;
                Dictionary<string, object> selectedUserInfo = (selectedVnext["users"] as IList<object>)[0] as Dictionary<string, object>;
                addressBox.Text = selectedVnext["address"].ToString();
                portBox.Text = selectedVnext["port"].ToString();
                idBox.Text = selectedUserInfo["id"].ToString();
                protocolComboBox.SelectedIndex = Utilities.PROTOCOL_LIST.FindIndex(x => x == selectedProfile["protocol"] as string);
                if (selectedProfile["protocol"] as string == "vmess")
                {
                    securityComboBox.SelectedIndex = Utilities.VMESS_SECURITY_LIST.FindIndex(x => x == selectedUserInfo["security"] as string);
                    alterIdBox.Text = selectedUserInfo["alterId"].ToString();                  
                }
                levelBox.Text = selectedUserInfo["level"].ToString();
                tagBox.Text = selectedProfile["tag"].ToString();
                networkBox.SelectedIndex = Utilities.NETWORK_LIST.FindIndex(x => x == (selectedProfile["streamSettings"] as Dictionary<string, object>)["network"] as string);
            }
        }

        #region closewindow

        private void HideWindow(object sender, RoutedEventArgs e)
        {
            if(sender == saveConfigButton)
            {
                try
                {
                    UInt16.Parse(localPortBox.Text);
                    UInt16.Parse(httpPortBox.Text);
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Not a valid port number!");
                    return;
                }
                Dictionary<string, Dictionary<string, object>> allUniqueTagOutbounds = new Dictionary<string, Dictionary<string, object>>();
                List<Dictionary<string, object>> allOutbounds = new List<Dictionary<string, object>>(profiles);
                allOutbounds.AddRange(outbounds);
                foreach(Dictionary<string, object> outbound in allOutbounds)
                {
                    if(outbound["tag"] as string == "")
                    {
                        MessageBox.Show("tag can not be empty");
                        return;
                    }
                    if(Utilities.RESERVED_TAGS.FindIndex(x => x == outbound["tag"] as string) != -1)
                    {
                        MessageBox.Show($"tag {outbound["tag"]} is reserved.");
                        return;
                    }
                    if(allUniqueTagOutbounds.ContainsKey(outbound["tag"] as string))
                    {
                        MessageBox.Show($"tag {outbound["tag"]} is not unique");
                        return;
                    } else
                    {
                        allUniqueTagOutbounds[outbound["tag"] as string] = outbound;
                    }
                }
                mainWindow.profiles = new List<Dictionary<string, object>>(allUniqueTagOutbounds.Values);
                mainWindow.routingRuleSets = routingRuleSets;
                mainWindow.subscribeUrl = subscribeUrl;
                mainWindow.httpPort = UInt16.Parse(httpPortBox.Text);
                mainWindow.localPort = UInt16.Parse(localPortBox.Text);
                mainWindow.dnsString = dnsBox.Text;
                mainWindow.enableRestore = enableRestore;
                mainWindow.shareOverLan = shareOverLanBox.IsChecked ?? false;
                mainWindow.udpSupport = udpSupportBox.IsChecked ?? false;
                mainWindow.logLevel = logLevelBox.SelectedItem.ToString();
                mainWindow.OverallChanged(this, null);
                ExtraUtils.AutoStartSet((bool)autoStartBox.IsChecked);
            }
            this.Close();
        }
        #endregion


        public void ShowAdvancedWindow(object sender, RoutedEventArgs e)
        {
            var advancedWindow = new AdvancedWindow
            {
                Owner = this
            };
            advancedWindow.InitializeData();
            advancedWindow.ShowDialog();
        }

        public void ShowTransportWindow(object sender, RoutedEventArgs e)
        {
            if (vmessListBox.SelectedIndex < 0 || vmessListBox.SelectedIndex >= profiles.Count) return;
            var transportWindow = new TransportWindow
            {
                Owner = this
            };
            transportWindow.InitializeData();
            transportWindow.ShowDialog();
        }

        public void ShowEnhancedWindow(object sender, RoutedEventArgs e)
        {
            if (vmessListBox.SelectedIndex < 0 || vmessListBox.SelectedIndex >= profiles.Count) return;
            var enhancedWindow = new EnhancedWindow
            {
                Owner = this
            };
            enhancedWindow.InitializeData();
            enhancedWindow.ShowDialog();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (vmessListBox.SelectedIndex < 0 || vmessListBox.SelectedIndex >= profiles.Count) return;
            Dictionary<string, object> selectedProfile = profiles[vmessListBox.SelectedIndex];
            Dictionary<string, object> selectedVnext = ((selectedProfile["settings"] as Dictionary<string, object>)["vnext"] as IList<object>)[0] as Dictionary<string, object>;
            Dictionary<string, object> selectedUserInfo = (selectedVnext["users"] as IList<object>)[0] as Dictionary<string, object>;
            if (sender == addressBox)
            {
                selectedVnext["address"] = addressBox.Text;
            } else if (sender == portBox)
            {
                if (portBox.Text.Length == 0) return;
                int portNumber = (int)selectedVnext["port"];
                try
                {
                    portNumber = UInt16.Parse(portBox.Text);
                } catch
                {
                    MessageBox.Show("not a valid port number!");
                }
                selectedVnext["port"] = portNumber;
            } else if (sender == idBox)
            {
                selectedUserInfo["id"] = idBox.Text;
            } else if (sender == alterIdBox)
            {
                if (alterIdBox.Text.Length == 0) return;
                int alterId =int.Parse(selectedUserInfo.ContainsKey("alterId") ? selectedUserInfo["alterId"].ToString() : "0");
                try
                {
                    alterId = UInt16.Parse(alterIdBox.Text);
                }
                catch
                {
                    MessageBox.Show("not a valid alter id!");
                }
                selectedUserInfo["alterId"] = alterId;
            } else if (sender == levelBox)
            {
                if (levelBox.Text.Length == 0) return;
                int level = (int)selectedUserInfo["level"];
                try
                {
                    level = Int32.Parse(levelBox.Text);
                } catch
                {
                    MessageBox.Show("not a valid number");
                }
                selectedUserInfo["level"] = level;
            } else if (sender == tagBox)
            {
                selectedProfile["tag"] = tagBox.Text;
            } 
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vmessListBox.SelectedIndex < 0 || vmessListBox.SelectedIndex >= profiles.Count) return;
            Dictionary<string, object> selectedProfile = profiles[vmessListBox.SelectedIndex];
            Dictionary<string, object> selectedVnext = ((selectedProfile["settings"] as Dictionary<string, object>)["vnext"] as IList<object>)[0] as Dictionary<string, object>;
            Dictionary<string, object> selectedUserInfo = (selectedVnext["users"] as IList<object>)[0] as Dictionary<string, object>;
            Dictionary<string, object> streamSettings = selectedProfile["streamSettings"] as Dictionary<string, object>;
            if (sender == securityComboBox)
            {
                selectedUserInfo["security"] = (sender as ComboBox).SelectedItem.ToString();
            } else if (sender == networkBox)
            {
                (selectedProfile["streamSettings"] as Dictionary<string, object>)["network"] = networkBox.SelectedItem.ToString();
            } else if (sender == protocolComboBox)
            {
                selectedProfile["protocol"] = protocolComboBox.SelectedItem.ToString();
                if (selectedProfile["protocol"] as string == "vmess")
                {
                    alterIdBox.IsEnabled = true;
                    securityComboBox.IsEnabled = true;
                    enhancedButton.IsEnabled = false;
                    alterIdBox.Text = selectedUserInfo.ContainsKey("alterId") ? selectedUserInfo["alterId"].ToString() : "0"; 
                    selectedUserInfo["alterId"] = int.Parse(alterIdBox.Text);
                    selectedUserInfo["security"]= selectedUserInfo.ContainsKey("security") ? selectedUserInfo["security"].ToString() : "auto";
                    selectedUserInfo.Remove("encryption");
                    selectedUserInfo.Remove("flow");
                    if (streamSettings["security"] as string == "xtls")
                    {
                        streamSettings["security"] = "tls";
                        streamSettings.Remove("tlsSettings");
                        profiles[configWindow.vmessListBox.SelectedIndex]["streamSettings"] = streamSettings.ToDictionary(k => k.Key == "xtlsSettings" ? "tlsSettings" : k.Key, k => k.Value);
                    }
                }
                if (selectedProfile["protocol"] as string == "vless")
                {
                    alterIdBox.Text = "0";
                    securityComboBox.Text = "auto";
                    alterIdBox.IsEnabled = false;
                    securityComboBox.IsEnabled = false;
                    enhancedButton.IsEnabled = true; 
                    selectedUserInfo["encryption"] = selectedUserInfo.ContainsKey("encryption") ? selectedUserInfo["encryption"].ToString() : "none";
                    selectedUserInfo["flow"] = selectedUserInfo.ContainsKey("flow") ? selectedUserInfo["flow"].ToString() : "";
                    selectedUserInfo.Remove("alterId");
                    selectedUserInfo.Remove("security");
                }
            }
        }


        private void TagBox_LostFocus(object sender, RoutedEventArgs e)
        {
            RefreshListBox();
        }

        private void AddVmess(object sender, RoutedEventArgs e)
        {
            profiles.Add(Utilities.VmessOutboundTemplate());
            RefreshListBox();
            vmessListBox.SelectedIndex = profiles.Count - 1;
        }

        private void RemoveVmess(object sender, RoutedEventArgs e)
        {
            if (profiles.Count == 0 || vmessListBox.SelectedIndex < 0 || vmessListBox.SelectedIndex >= profiles.Count) return;
            profiles.RemoveAt(vmessListBox.SelectedIndex);
            RefreshListBox();
        }

        private void CloneMenuItem_Click(object sender, RoutedEventArgs e)
        {
            profiles.Add(Utilities.DeepClone(profiles[vmessListBox.SelectedIndex]));
            RefreshListBox();
        }

        private void ShareMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (vmessListBox.SelectedIndex >= 0 && vmessListBox.SelectedIndex < profiles.Count)
            {
                Dictionary<string, object> selectedProfile = profiles[vmessListBox.SelectedIndex];
                Dictionary<string, object> selectedVnext = ((selectedProfile["settings"] as Dictionary<string, object>)["vnext"] as IList<object>)[0] as Dictionary<string, object>;
                Dictionary<string, object> selectedUserInfo = (selectedVnext["users"] as IList<object>)[0] as Dictionary<string, object>;
                Dictionary<string, object> streamSettings = selectedProfile["streamSettings"] as Dictionary<string, object>;
                Dictionary<string, object> kcpSettings = streamSettings["kcpSettings"] as Dictionary<string, object>;
                Dictionary<string, object> kcpSettingsT = kcpSettings["header"] as Dictionary<string, object>;
                Dictionary<string, object> tcpSettings = streamSettings["tcpSettings"] as Dictionary<string, object>;
                Dictionary<string, object> tcpSettingsT = tcpSettings["header"] as Dictionary<string, object>;
                Dictionary<string, object> wsSettings = streamSettings["wsSettings"] as Dictionary<string, object>;
                Dictionary<string, object> wsSettingsT = wsSettings["headers"] as Dictionary<string, object>;
                Dictionary<string, object> httpSettings = streamSettings["httpSettings"] as Dictionary<string, object>;
                Dictionary<string, object> quicSettings = streamSettings["quicSettings"] as Dictionary<string, object>;
                Dictionary<string, object> quicSettingsT = quicSettings["header"] as Dictionary<string, object>;
                Dictionary<string, object> tlsSettings = streamSettings["tlsSettings"] as Dictionary<string, object>;
                VmessLink vlink = new VmessLink();
                vlink.v = "2";
                vlink.ps = selectedProfile["tag"].ToString();
                vlink.add = selectedVnext["address"].ToString();
                vlink.port = selectedVnext["port"].ToString();
                vlink.id = selectedUserInfo["id"].ToString();
                vlink.aid= selectedUserInfo["alterId"].ToString();
                vlink.net = streamSettings["network"].ToString();
                vlink.tls = streamSettings["security"].ToString();
                vlink.type = "";
                vlink.host = "";
                vlink.path = "";
                switch (streamSettings["network"].ToString())
                {
                    case "ws":
                        vlink.host = wsSettingsT["host"].ToString();
                        vlink.path = wsSettings["path"].ToString() == "" ? "/" : wsSettings["path"].ToString();
                        break;
                    case "http":
                        vlink.net = "h2";
                        vlink.host = httpSettings.ContainsKey("host") ? String.Join(",", httpSettings["host"] as object[]) : "";
                        vlink.path = httpSettings["path"].ToString() == "" ? "/" : httpSettings["path"].ToString();
                        break;
                    case "tcp":
                        vlink.type = tcpSettingsT["type"].ToString() == "" ? "none" : tcpSettingsT["type"].ToString();
                        break;
                    case "kcp":
                        vlink.type = kcpSettingsT["type"].ToString() == "" ? "none" : kcpSettingsT["type"].ToString();
                        break;
                    case "quic":
                        vlink.type = quicSettingsT["type"].ToString() == "" ? "none" : quicSettingsT["type"].ToString();
                        vlink.host = quicSettings["securty"].ToString() == "" ? "none" : quicSettings["securty"].ToString();
                        vlink.path = quicSettings["key"].ToString();
                        break;
                    default:
                        break;
                }
                var sharejson = Utilities.javaScriptSerializer.Serialize(vlink);
                ExtraUtils.SetClipboardData(ExtraUtils.Base64Encode(sharejson).Insert(0,"vmess://"));
            }
        }

        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"log\");
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ImportClipboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImportURL(ExtraUtils.GetClipboardData());
                RefreshListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "format exception!");
                return;
            }
        }  

        private void ImportSubscribe_Click(object sender, RoutedEventArgs e)
        {
            if (profiles.Count + outbounds.Count != 0)
            {
                foreach (Dictionary<string, object> tag in profiles.Concat(outbounds).ToList())
                {
                    foreach (string ps in mainWindow.subscribeTag.ToString().Split(",".ToCharArray()))
                    {
                        if (tag["tag"].ToString() == ps)
                        {
                            profiles.Remove(tag);
                            outbounds.Remove(tag);
                            RefreshListBox();
                        }
                    }
                }
                mainWindow.subscribeTag = "";
            }
            try
            {
                BackgroundWorker subscribeWorker = new BackgroundWorker();
                subscribeWorker.WorkerSupportsCancellation = true;
                subscribeWorker.DoWork += subscribeWorker_DoWork;
                if (subscribeWorker.IsBusy) return;
                subscribeWorker.RunWorkerAsync();   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "request timeout!");
                return;
            }
        }

        #region import & subscribe

        void subscribeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            mainWindow.subscribeTag = "";
            List<string> subscribeTag = new List<string>();
            foreach (string url in subscribeUrl)
            {
                var tag = ImportURL(ExtraUtils.Base64Decode(ExtraUtils.GetUrl(url)));
                subscribeTag = subscribeTag.Concat(tag).ToList();
                this.Dispatcher.Invoke(() =>
                {
                    RefreshListBox();
                });
            }
            mainWindow.subscribeTag = String.Join(",", subscribeTag);
        }

        List<string> ImportURL(string importUrl)
        {
            List<string> linkMark = new List<string>();
            foreach (var link in importUrl.Split(Environment.NewLine.ToCharArray()))
            {
                if (link.StartsWith("ss"))
                {
                    linkMark.Add(ImportShadowsocks(link));
                }

                if (link.StartsWith("vmess"))
                {
                    linkMark.Add(ImportVmess(link));
                }
            }
            Debug.WriteLine("importurl " + String.Join(",", linkMark));
            return linkMark;
        }

        public string ImportShadowsocks(string url)
        {
            var link = url.Contains("#") ? url.Substring(5, url.IndexOf("#") - 5) : url.Substring(5);
            var tag = url.Contains("#") ? url.Substring(url.IndexOf("#") + 1).Trim() : "Newtag" + new Random(Guid.NewGuid().GetHashCode()).Next(100, 1000);
            var linkParseArray = ExtraUtils.Base64Decode(link).Split(new char[2] { ':', '@' });
            Dictionary<string, object> ShadowsocksProfiles = Utilities.outboundTemplate;
            Dictionary<string, object> ShadowsocksTemplate = Utilities.ShadowsocksOutboundTemplateNew();
            ShadowsocksProfiles["protocol"] = "shadowsocks";
            ShadowsocksProfiles["tag"] = tag;
            ShadowsocksTemplate["email"] = "love@server.cc";
            ShadowsocksTemplate["address"] = linkParseArray[2];
            ShadowsocksTemplate["port"] = Convert.ToInt32(linkParseArray[3]);
            ShadowsocksTemplate["method"] = linkParseArray[0];
            ShadowsocksTemplate["password"] = linkParseArray[1];
            ShadowsocksTemplate["ota"] = false;
            ShadowsocksTemplate["level"] = 0;
            ShadowsocksProfiles["settings"] = new Dictionary<string, object> {
                    {"servers",  new List<Dictionary<string, object>>{ ShadowsocksTemplate } }
                };
            outbounds.Add(Utilities.DeepClone(ShadowsocksProfiles));
            return tag;
        }

        public string ImportVmess(string url)
        {
            Dictionary<string, object> VmessProfiles = Utilities.VmessOutboundTemplateNew();
            Dictionary<string, object> muxSettings = VmessProfiles["mux"] as Dictionary<string, object>;
            Dictionary<string, object> streamSettings = VmessProfiles["streamSettings"] as Dictionary<string, object>;
            Dictionary<string, object> settings = VmessProfiles["settings"] as Dictionary<string, object>;
            Dictionary<string, object> vnext = (settings["vnext"] as IList<object>)[0] as Dictionary<string, object>;
            Dictionary<string, object> UserInfo = (vnext["users"] as IList<object>)[0] as Dictionary<string, object>;
            Dictionary<string, object> kcpSettings = streamSettings["kcpSettings"] as Dictionary<string, object>;
            Dictionary<string, object> kcpSettingsT = kcpSettings["header"] as Dictionary<string, object>;
            Dictionary<string, object> tcpSettings = streamSettings["tcpSettings"] as Dictionary<string, object>;
            Dictionary<string, object> tcpSettingsT = tcpSettings["header"] as Dictionary<string, object>;
            Dictionary<string, object> wsSettings = streamSettings["wsSettings"] as Dictionary<string, object>;
            Dictionary<string, object> wsSettingsT = wsSettings["headers"] as Dictionary<string, object>;
            Dictionary<string, object> httpSettings = streamSettings["httpSettings"] as Dictionary<string, object>;
            Dictionary<string, object> quicSettings = streamSettings["quicSettings"] as Dictionary<string, object>;
            Dictionary<string, object> quicSettingsT = quicSettings["header"] as Dictionary<string, object>;
            Dictionary<string, object> tlsSettings = streamSettings["tlsSettings"] as Dictionary<string, object>;

            VmessLink VmessLink = JsonConvert.DeserializeObject<VmessLink>(ExtraUtils.Base64Decode(url.Substring(8)));
            UserInfo["id"] = VmessLink.id;
            UserInfo["alterId"] = Convert.ToInt32(VmessLink.aid);
            vnext["address"] = VmessLink.add;
            vnext["port"] = Convert.ToInt32(VmessLink.port);
            streamSettings["network"] = VmessLink.net == "h2" ? "http" : VmessLink.net;
            streamSettings["security"] = VmessLink.tls == "" ? "none" : VmessLink.tls;
            tlsSettings["serverName"] = VmessLink.add;
            VmessProfiles["tag"] = VmessLink.ps;
            switch (VmessLink.net)
            {
                case "ws":
                    wsSettingsT["host"] = VmessLink.host;
                    wsSettings["path"] = VmessLink.path;
                    break;
                case "h2":
                    httpSettings["host"] = VmessLink.host.Split(',');
                    httpSettings["path"] = VmessLink.path;
                    break;
                case "tcp":
                    tcpSettingsT["type"] = VmessLink.type;
                    break;
                case "kcp":
                    kcpSettingsT["type"] = VmessLink.type;
                    break;
                case "quic":
                    quicSettingsT["type"] = VmessLink.type;
                    quicSettings["securty"] = VmessLink.host;
                    quicSettings["key"] = VmessLink.path;
                    break;
                default:
                    break;
            }
            profiles.Add(VmessProfiles);
            return VmessLink.ps;
        }

        #endregion
    }
}
