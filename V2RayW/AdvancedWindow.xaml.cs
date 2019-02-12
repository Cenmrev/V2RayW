using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for AdvancedWindow.xaml
    /// </summary>
    public partial class AdvancedWindow : Window
    {
        public AdvancedWindow()
        {
            InitializeComponent();
            domainStrategyBox.Items.Clear();
            foreach(string strategy in Utilities.DOMAIN_STRATEGY_LIST)
            {
                domainStrategyBox.Items.Add(strategy);
            }
            netWorkListBox.Items.Clear();
            foreach(string network in Utilities.routingNetwork)
            {
                netWorkListBox.Items.Add(network);
            }
        }

        private ConfigWindow configWindow;


        public void InitializeData()
        {
            configWindow = this.Owner as ConfigWindow;

            outbounds = Utilities.DeepClone(configWindow.outbounds);
            subscriptionBox.Text = String.Join("\n", configWindow.subscriptions);
            routingRuleSets = Utilities.DeepClone(configWindow.routingRuleSets);
            foreach(Dictionary<string, object> set in routingRuleSets)
            {
                set["rules"] = new List<object>(set["rules"] as IList<object>);
            }

            enableRestoreBox.Items.Clear();
            enableRestoreBox.Items.Add(V2RayW.Resources.Strings.RestoreTurnOff);
            enableRestoreBox.Items.Add(V2RayW.Resources.Strings.RestoreTurnOn);
            enableRestoreBox.SelectedIndex = configWindow.enableRestore ? 1 : 0;

            configScanner.DoWork += ConfigScanner_DoWork;
            configScanner.RunWorkerCompleted += ConfigScanner_RunWorkerCompleted;
            RefreshButton_Click(this, null);

            RefreshListBox(outboundListBox, outbounds, "tag");

            RefreshListBox(ruleSetListBox, routingRuleSets, "name");
            RefreshListBox(ruleSetListBox, routingRuleSets, "name");
            ruleSetListBox.SelectedIndex = 0;
        }




        #region subscriptions
        public List<string> subscriptions;
        #endregion

        #region customized configs
        private BackgroundWorker configScanner = new BackgroundWorker();

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"config\");
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            cusConfigBox.Items.Clear();
            refreshButton.IsEnabled = false;
            configScanner.RunWorkerAsync();
        }

        private void ConfigScanner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(() => {
                refreshButton.IsEnabled = true;
            });
        }

        private void ConfigScanner_DoWork(object sender, DoWorkEventArgs e)
        {
            Process v2rayProcess = new Process();
            v2rayProcess.StartInfo.FileName = Utilities.corePath;

            DirectoryInfo configDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"config\");
            var cusConfigs = configDirectoryInfo.GetFiles("*.json", SearchOption.TopDirectoryOnly);
            foreach (var cusConfig in cusConfigs)
            {
                Debug.WriteLine(v2rayProcess.StartInfo.FileName);
                v2rayProcess.StartInfo.Arguments = "-test -config " + "\"" + cusConfig.FullName + "\"";
                v2rayProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                try
                {
                    v2rayProcess.Start();
                    v2rayProcess.WaitForExit();
                    Debug.WriteLine("exit code is " + v2rayProcess.ExitCode.ToString());
                    if(v2rayProcess.ExitCode == 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            cusConfigBox.Items.Add(cusConfig.Name);
                        });
                    }
                } catch { };
            }
        }
        #endregion
        
        void RefreshListBox(ListBox listBox, List<Dictionary<string, object>> dataSource, string key)
        {
            RefreshListBox(listBox, dataSource, (index, item) => item[key] as string );
        }

        void RefreshListBox<T>(ListBox listBox, IList<T> dataSource, Func<int, Dictionary<string, object>, string> func)
        {
            int selectedIndex = listBox.SelectedIndex;
            int countDifference = dataSource.Count() - listBox.Items.Count;
            for (int i = 0; i < countDifference; i += 1)
            { 
                // add enough entries
                listBox.Items.Add(new ListBoxItem());
            }
            for (int i = 0; i < -countDifference; i += 1)
            {
                // remove unnecessary slots
                listBox.Items.RemoveAt(dataSource.Count());
            }
            for (int i = 0; i < dataSource.Count; i += 1)
            {
                (listBox.Items[i] as ListBoxItem).Content = func(i, dataSource[i] as Dictionary<string, object> );
            }
            int shouldSelect = Math.Min(dataSource.Count - 1, selectedIndex);
            if (shouldSelect == -1 && dataSource.Count > 0)
            {
                shouldSelect = 0;
            }
            if(listBox.SelectedIndex == shouldSelect)
            {
                var removeAdd = new List<object> { };
                SelectionChangedEventArgs e = new SelectionChangedEventArgs(ListBox.SelectionChangedEvent, removeAdd, removeAdd);
                listBox.RaiseEvent(e);
            } else
            {
                listBox.SelectedIndex = shouldSelect;
            }
        }

        #region outbounds

        private List<Dictionary<string, object>> outbounds;

        private void AddOutboundButton_Click(object sender, RoutedEventArgs e)
        {
            var newOutbound = Utilities.outboundTemplate;
            newOutbound["tag"] = $"tag{outbounds.Count}";
            outbounds.Add(Utilities.DeepClone(newOutbound));
            RefreshListBox(outboundListBox, outbounds, "tag");
        }

        private void RemoveOutboundButton_Click(object sender, RoutedEventArgs e)
        {
            if (outboundListBox.SelectedIndex < 0 || outboundListBox.SelectedIndex >= outbounds.Count) return;
            outbounds.RemoveAt(outboundListBox.SelectedIndex);
            RefreshListBox(outboundListBox, outbounds, "tag");
            if(outbounds.Count == 0)
            {
                outboundContentBox.Text = "";
            }
        }

        private void SaveOutboundButton_Click(object sender, RoutedEventArgs e)
        {
            if (outboundListBox.SelectedIndex < 0 || outboundListBox.SelectedIndex >= outbounds.Count) return;
            Dictionary<string, object> outbound;
            try
            {
                outbound = Utilities.javaScriptSerializer.Deserialize<dynamic>(outboundContentBox.Text) as Dictionary<string, object>;
            } catch
            {
                MessageBox.Show(Strings.messagenotvalidjson);
                return;
            }
            if (!outbound.ContainsKey("tag") || outbound["tag"] as string == "")
            {
                MessageBox.Show(Strings.messagetagrequired);
                return;
            }
            outbounds[outboundListBox.SelectedIndex] = outbound;
            RefreshListBox(outboundListBox, outbounds, "tag");
            saveOutboundButton.IsEnabled = false;
        }

        private void OutboundListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (outboundListBox.SelectedIndex < 0 || outboundListBox.SelectedIndex >= outbounds.Count) return;
            outboundContentBox.Text = JsonConvert.SerializeObject(outbounds[outboundListBox.SelectedIndex], Formatting.Indented);
            saveOutboundButton.IsEnabled = false;
        }

        private void OutboundContentBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveOutboundButton.IsEnabled = true;
        }

        #endregion

        #region rules

        public List<Dictionary<string, object>> routingRuleSets;

        private void RefreshRuleListBox()
        {
            var rules = routingRuleSets[ruleSetListBox.SelectedIndex]["rules"] as List<object>;
            RefreshListBox(ruleListBox, rules, (i, rule) =>
            {
                string routeto = (rule.ContainsKey("outboundTag") ? rule["outboundTag"] : rule["balancerTag"]) as string;
                return (i == rules.Count() - 1 ? "final" : i.ToString()) + ":" + routeto;
            });
        }


        private void RuleSetListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dictionary<string, object> selectedRuleSet;
            try
            {
                selectedRuleSet = routingRuleSets[ruleSetListBox.SelectedIndex];
            } catch { return; }
            ruleSetNameBox.Text = selectedRuleSet["name"] as string;
            domainStrategyBox.SelectedIndex = Utilities.DOMAIN_STRATEGY_LIST.IndexOf(selectedRuleSet["domainStrategy"] as string);
            RefreshRuleListBox();
        }

        private void RuleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dictionary<string, object> rule;
            try
            {
                rule = (routingRuleSets[ruleSetListBox.SelectedIndex]["rules"] as List<object>)[ruleListBox.SelectedIndex] as Dictionary<string, object>;
            } catch
            {
                return;
            }

            bool selectedLastRule = ruleListBox.SelectedIndex == ruleListBox.Items.Count - 1;
            domainIpEnableBox.IsEnabled = !selectedLastRule;
            networkEnableBox.IsEnabled = !selectedLastRule;
            
            domainIpEnableBox.IsChecked = rule.ContainsKey("domain") || rule.ContainsKey("ip");
            if(domainIpEnableBox.IsChecked ?? false)
            {
                string r = "";
                if(rule.ContainsKey("domain"))
                {
                    r += String.Join("\n", rule["domain"] as object[]);
                }
                r += "\n---\n";
                if (rule.ContainsKey("ip"))
                {
                    r += String.Join("\n", rule["ip"] as object[]);
                }
                domainIpBox.Text = r;
            } else
            {
                domainIpBox.Text = "";
            }

            portEnableBox.IsEnabled = !selectedLastRule;
            portEnableBox.IsChecked = rule.ContainsKey("port");
            if(portEnableBox.IsChecked ?? false)
            {
                portBox.Text = rule["port"].ToString();
            } else
            {
                portBox.Text = "";
            }

            networkEnableBox.IsChecked = rule.ContainsKey("network");
            if (rule.ContainsKey("network"))
            {
                netWorkListBox.SelectedIndex = Utilities.routingNetwork.IndexOf(rule["network"] as string);
            } else
            {
                netWorkListBox.SelectedIndex = 0;
            }

            routeToBox.Text = (rule.ContainsKey("outboundTag") ? rule["outboundTag"] : rule["balancerTag"]) as string;


        }


        private void RuleSetNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dictionary<string, object> ruleSet;
            try
            {
                ruleSet = routingRuleSets[ruleSetListBox.SelectedIndex] as Dictionary<string, object>;
            }
            catch
            {
                return;
            }
            ruleSet["name"] = ruleSetNameBox.Text;
        }

        private void AddRuleSetButton_Click(object sender, RoutedEventArgs e)
        {
            routingRuleSets.Add(Utilities.DeepClone(Utilities.ROUTING_DIRECT));
            RefreshListBox(ruleSetListBox, routingRuleSets, "name");
        }

        private void RemoveRuleSetButton_Click(object sender, RoutedEventArgs e)
        {
            if (routingRuleSets.Count == 1 || ruleSetListBox.SelectedIndex < 0 || ruleSetListBox.SelectedIndex >= routingRuleSets.Count) return;
            routingRuleSets.RemoveAt(ruleSetListBox.SelectedIndex);
            RefreshListBox(ruleSetListBox, routingRuleSets, "name");
        }

        private void DomainStrategyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dictionary<string, object> ruleSet;
            try
            {
                ruleSet = routingRuleSets[ruleSetListBox.SelectedIndex] as Dictionary<string, object>;
            }
            catch
            {
                return;
            }
            ruleSet["domainStrategy"] = domainStrategyBox.SelectedItem.ToString();
        }

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> ruleSet;
            try
            {
                ruleSet = routingRuleSets[ruleSetListBox.SelectedIndex] as Dictionary<string, object>;
            }
            catch
            {
                return;
            }
            List<object> rules = ruleSet["rules"] as List<object>;
            rules.Insert(rules.Count - 1, Utilities.DeepClone(Utilities.singleRule));
            RefreshRuleListBox();
        }

        private void RemoveRuleButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> ruleSet;
            try
            {
                ruleSet = routingRuleSets[ruleSetListBox.SelectedIndex] as Dictionary<string, object>;
            }
            catch
            {
                return;
            }
            List<object> rules = ruleSet["rules"] as List<object>;
            if (ruleListBox.SelectedIndex < 0 || ruleListBox.SelectedIndex >= rules.Count - 1) return;
            rules.RemoveAt(ruleListBox.SelectedIndex);
            RefreshRuleListBox();
        }

        Dictionary<String, object> GetSelectedRule()
        {
            Dictionary<string, object> ruleSet;
            try
            {
                ruleSet = routingRuleSets[ruleSetListBox.SelectedIndex] as Dictionary<string, object>;
            }
            catch
            { return null; }
            List<object> rules = ruleSet["rules"] as List<object>;
            Dictionary<string, object> rule;
            try
            {
                rule = rules[ruleListBox.SelectedIndex] as Dictionary<string, object>;
            }
            catch { return null; }
            return rule;
        }

        private void DomainIpEnableBox_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> rule = GetSelectedRule();
            if (rule == null) return;

            if (!(domainIpEnableBox.IsChecked??false))
            {
                rule.Remove("ip");
                rule.Remove("domain");
            } else
            {
                domainIpBox.Focus();
            }
        }

        private void NetworkEnableBox_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> rule = GetSelectedRule();
            if (rule == null) return;

            if (!(networkEnableBox.IsChecked ?? false))
            {
                rule.Remove("network");
            } else
            {
                rule["network"] = netWorkListBox.SelectedItem.ToString();
            }
        }

        private void PortEnableBox_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> rule = GetSelectedRule();
            if (rule == null) return;

            if (!portEnableBox.IsChecked ?? false)
            {
                rule.Remove("port");
            } else
            {
                portBox.Focus();
            }

        }

        private void DomainIpBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> rule = GetSelectedRule();
            if (rule == null) return;
            string[] parts = domainIpBox.Text.Split(new[] { "---" }, StringSplitOptions.None);
            Debug.WriteLine(parts.ToString());
            if(parts.Count() == 0)
            {
                rule.Remove("ip");
                rule.Remove("domain");
                domainIpEnableBox.IsChecked = false;
            } else if (parts.Count() == 1)
            {
                rule.Remove("ip");
                rule["domain"] = parts[0].Split(new[] { '\r', '\n' }).Select(line => line.Trim()).Where(line => line.Length > 0).ToArray();
            } else
            {
                rule["domain"] = parts[0].Split(new[] { '\r', '\n' }).Select(line => line.Trim()).Where(line => line.Length > 0).ToArray();
                rule["ip"] = parts[1].Split(new[] { '\r', '\n' }).Select(line => line.Trim()).Where(line => line.Length > 0).ToArray();
            }
            
        }

        private void NetWorkListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dictionary<string, object> rule = GetSelectedRule();
            if (rule == null) return;

            if (!networkEnableBox.IsChecked ?? false)
            {
                return;
            }
            rule["network"] = netWorkListBox.SelectedItem.ToString();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if(e.Text == "-" && !portBox.Text.Contains("-"))
            {
                return;
            }
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PortBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> rule = GetSelectedRule();
            if (rule == null) return;
            try
            {
                rule["port"] = UInt16.Parse(portBox.Text);
            } catch {
                rule["port"] = portBox.Text.Trim();
            }

        }

        private void RouteToBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("lost focus");
            Dictionary<string, object> rule = GetSelectedRule();
            if (rule == null) return;
            if (routeToBox.Text == "balance")
            {
                rule.Remove("outboundTag");
                rule["balancerTag"] = "balance";
            } else
            {
                rule.Remove("balancerTag");
                rule["outboundTag"] = routeToBox.Text.Trim();
            }
        }

        #endregion

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            configWindow.outbounds = outbounds;
            configWindow.subscriptions = subscriptionBox.Text.Split(new[] { '\r', '\n' }).Select(line => line.Trim()).Where(link => link.Length > 0).ToList();
            configWindow.routingRuleSets = routingRuleSets;
            configWindow.enableRestore = enableRestoreBox.SelectedIndex == 1;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            switch(mainTabControl.SelectedIndex)
            {
                case 0: Process.Start(Strings.outboundHelppage); break;
                case 2: Process.Start(Strings.ruleHelppage); break;
                case 3: Process.Start(Strings.configHelppage); break;
                case 4: Process.Start(Strings.coreHelppage); break;
            }
        }
    }
}
