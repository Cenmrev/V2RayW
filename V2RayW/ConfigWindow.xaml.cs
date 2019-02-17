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
        public List<string> subscriptions;
        public List<Dictionary<string, object>> routingRuleSets;
        public bool enableRestore;
        private BackgroundWorker coreVersionCheckWorker = new BackgroundWorker();

        public ConfigWindow()
        {
            InitializeComponent();

            // initialize UI
            foreach(string security in Utilities.VMESS_SECURITY_LIST) {
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
                if (outbound["protocol"].ToString() != "vmess")
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
            subscriptions = Utilities.DeepClone(mainWindow.subscriptions);
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

        private void RefreshListBox()
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
                alterIdBox.Text = selectedUserInfo["alterId"].ToString();
                securityComboBox.SelectedIndex = Utilities.VMESS_SECURITY_LIST.FindIndex(x => x == selectedUserInfo["security"] as string);
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
                mainWindow.subscriptions = subscriptions;
                mainWindow.httpPort = UInt16.Parse(httpPortBox.Text);
                mainWindow.localPort = UInt16.Parse(localPortBox.Text);
                mainWindow.dnsString = dnsBox.Text;
                mainWindow.enableRestore = enableRestore;
                mainWindow.shareOverLan = shareOverLanBox.IsChecked ?? false;
                mainWindow.udpSupport = udpSupportBox.IsChecked ?? false;
                mainWindow.logLevel = logLevelBox.SelectedItem.ToString();


                mainWindow.OverallChanged(this, null);

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
                int alterId = (int)selectedUserInfo["alterId"];
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
            if (sender == securityComboBox)
            {
                selectedUserInfo["security"] = (sender as ComboBox).SelectedItem.ToString();
            } else if (sender == networkBox)
            {
                (selectedProfile["streamSettings"] as Dictionary<string, object>)["network"] = networkBox.SelectedItem.ToString();
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
    }
}
