using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Web.Script.Serialization;

namespace V2RayW
{

    static class Program
    {
        public const int N_MODES = 4;
        public const int RULES_MODE = 0;
        public const int PAC_MODE = 1;
        public const int GLOBAL_MODE = 2;
        public const int MANUAL_MODE = 3;

        public static Dictionary<string, string> proxyBackup = new Dictionary<string, string>();

        public static bool coreLoaded = false;
        public static int proxyMode = RULES_MODE;
        public static int localPort = 1081;
        public static int httpPort = 8081;
        public static bool udpSupport = false;
        public static bool shareOverLan = false;
        public static string dnsString = "localhost";
        public static LogLevel logLevel = LogLevel.none;
        public static bool useCusProfile = false;
        public static List<ServerProfile> profiles = new List<ServerProfile>();
        public static int selectedServerIndex = 1;
        public static List<string> cusProfiles = new List<string>();
        public static int selectedCusServerIndex = 1;
        public static bool useMultipleServer = false;
        public static int mainInboundType = 0;
        public static bool alarmUnknown = true;

        public static MainForm mainForm;
        const string v2rayVersion = "v3.23";
        static BackgroundWorker v2rayCoreWorker = new BackgroundWorker();
        public static AutoResetEvent _resetEvent = new AutoResetEvent(false);
        public static bool finalAction = false;
        public static StringBuilder output = new StringBuilder();
        private static int lineCount = 0;



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Properties.Settings.Default.Reset();
            //backgourdworker
            v2rayCoreWorker.WorkerSupportsCancellation = true;
            v2rayCoreWorker.DoWork += new DoWorkEventHandler(Program.v2rayCoreWorker_DoWork);
            v2rayCoreWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Program.RunWorkerCompleted);
            Application.ApplicationExit += new EventHandler(OnProcessExit);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //check v2ray core binary
            var cr = checkV2RayCore();
            switch (cr)
            {
                case 2:
                    {
                        DialogResult res = MessageBox.Show("Wrong or missing v2ray core file!\nDownload it right now?", "Error!", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                        if (res == DialogResult.OK)
                        {
                            Process.Start(String.Format(@"https://github.com/v2ray/v2ray-core/releases/tag/{0}", v2rayVersion));
                        }
                        return;
                    }
                case 1:
                    {
                        if (Properties.Settings.Default.alarmUnknown == true)
                        {
          
                            DialogResult res = MessageBox.Show(String.Format("Unknown version of v2ray core detected, which may not be compatible with V2RayW.\n{0} is suggested. Do you want to continue to use the existing core?", Program.v2rayVersion), "Unknown v2ray.exe!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            if (res == DialogResult.OK)
                            {
                                break;
                            }
                            else
                            {
                                DialogResult dres = MessageBox.Show(String.Format("Do you want to download official core {0} right now?", Program.v2rayVersion), "Unknown v2ray.exe!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                if (dres == DialogResult.OK)
                                {
                                    Process.Start(String.Format(@"https://github.com/v2ray/v2ray-core/releases/tag/{0}", v2rayVersion));
                                }
                                return;
                            }
                        }
                        else
                        {
                            break;
                        }

                    }
                default: break;
            }

            
            mainForm = new MainForm();
            readSettings();
            if (coreLoaded == true && proxyMode != 3)
            {
                Program.backUpProxy();
            }
            configurationDidChange();

            Application.Run(mainForm);
        }

        public static void OnProcessExit(object sender, EventArgs e)
        {
            finalAction = true;
            if (Program.coreLoaded)
            {
                Program.stopV2Ray();
                Properties.Settings.Default.Save();
                Program._resetEvent.WaitOne();
            }
            if (coreLoaded && proxyMode != MANUAL_MODE)
            {
                restoreProxy();
            }
            Program.saveSettings();
        }

        //{"address":"v2ray.cool","allowPassive":0,"alterId":64,"network":0,"port":10086,"remark":"test server","userId":"23ad6b10-8d1a-40f7-8ad0-e3e35cd38297"}

        internal static int strToInt(string str, int defaultValue)
        {
            int result = 0;
            if (Int32.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
        

        public static async Task stopV2Ray()
        { // make sure v2ray is stopped
            if (v2rayCoreWorker.IsBusy)
                v2rayCoreWorker.CancelAsync();
            while (v2rayCoreWorker.IsBusy)
            {
                await Task.Delay(100);
            }
        }

        public static void loadV2ray()
        {
            if (!useCusProfile && selectedServerIndex >= 0 && selectedServerIndex < profiles.Count)
            {
                generateConfigJson();
            }
            v2rayCoreWorker.RunWorkerAsync();
        }

        public static async void configurationDidChange()
        {
            await stopV2Ray();
            if(coreLoaded)
            {
                if ((selectedServerIndex >= 0 && selectedServerIndex < profiles.Count) ||
                    (selectedCusServerIndex >= 0 && selectedCusServerIndex < cusProfiles.Count))
                {
                    loadV2ray();
                } else
                {
                    coreLoaded = false;
                    if (proxyMode != 3)
                    {
                        restoreProxy();
                    }
                    MessageBox.Show("No available servers!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                updateSystemProxy();
            }
            mainForm.updateMenu();
        }

        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/19517edf-8348-438a-a3da-5fbe7a46b61a/how-to-change-global-windows-proxy-using-c-net-with-immediate-effect?forum=csharpgeneral
        [DllImport("wininet.dll")]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        const int INTERNET_OPTION_REFRESH = 37;

        public static void backUpProxy()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            proxyBackup["ProxyEnable"] = registry.GetValue("ProxyEnable").ToString();
            proxyBackup["ProxyServer"] = registry.GetValue("ProxyServer").ToString();
            proxyBackup["ProxyOverride"] = registry.GetValue("ProxyOverride").ToString();
        }

        public static void restoreProxy()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", Program.proxyBackup["ProxyEnable"] == "0" ? 0 : 1);
            registry.SetValue("ProxyServer", Program.proxyBackup["ProxyServer"]);
            registry.SetValue("ProxyOverride", Program.proxyBackup["ProxyOverride"]);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public static int updateSystemProxy()
        {
            // manual mode 
            if (Program.proxyMode == 3)
            {
                return 0;
            }
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

            bool settingsReturn, refreshReturn;

            registry.SetValue("ProxyEnable", coreLoaded ? 1 : 0);
            var proxyServer = (mainInboundType == 1 ? "socks=" : "http://") + $"127.0.0.1:{(mainInboundType == 1 ? localPort : httpPort)}";
            var proxyOverride = "<local>;localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;172.32.*;192.168.*";
            if (coreLoaded)
            {
                registry.SetValue("ProxyServer", proxyServer);
                registry.SetValue("ProxyOverride", proxyOverride);
            }
            var sysState = registry.GetValue("ProxyEnable").ToString() == (coreLoaded ? "1" : "0");
            var sysServer = coreLoaded ? registry.GetValue("ProxyServer").ToString() == proxyServer : true;
            var sysOverride = coreLoaded ? registry.GetValue("ProxyOverride").ToString() == proxyOverride : true;
            settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
            if (sysServer && sysState && sysOverride)
            {
                if (settingsReturn && refreshReturn)
                {
                    return 0;
                } else
                {
                    return 1;
                }
            } else
            {
                return 2;
            }
        }

        private static void readSettings()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var defaults = Properties.Settings.Default;
            defaults.Upgrade();
            logLevel = (LogLevel)defaults.logLevel;
            coreLoaded = defaults.coreLoaded;
            proxyMode = defaults.proxyMode;
            selectedServerIndex = defaults.selectedServerIndex;
            localPort = defaults.localPort;
            httpPort = defaults.httpPort;
            udpSupport = defaults.udpSupport;
            shareOverLan = defaults.shareOverLan;
            dnsString = defaults.dnsString;
            selectedCusServerIndex = defaults.selectedCusServerIndex;
            useCusProfile = defaults.useCusProfile;
            useMultipleServer = defaults.useMultipleServer;
            foreach(var p in defaults.cusProfilesStr.Split('\t'))
            {
                if (p.Trim().Length > 0)
                {
                    cusProfiles.Add(p.Trim());
                }
            }
            foreach(var p in defaults.profilesStr.Split('\t'))
            {
                var aP = js.Deserialize<ServerProfile>(p);
                if (aP is null)
                {
                    continue;
                }
                profiles.Add(aP);
            }
            if(profiles.Count() == 0)
            {
                profiles.Add(new ServerProfile { remark = "sample" });
                selectedServerIndex = 0;
            }
            if(cusProfiles.Count() == 0)
            {
                useCusProfile = false;
                selectedCusServerIndex = -1;
            }
            mainInboundType = defaults.mainInboundType;
            alarmUnknown = defaults.alarmUnknown;
        }

        public static void saveSettings()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var defaults = Properties.Settings.Default;
            defaults.logLevel = (int)logLevel;
            defaults.coreLoaded = coreLoaded;
            defaults.proxyMode = proxyMode;
            defaults.selectedServerIndex = selectedServerIndex;
            defaults.localPort = localPort;
            defaults.httpPort = httpPort;
            defaults.udpSupport = udpSupport;
            defaults.shareOverLan = shareOverLan;
            defaults.dnsString = dnsString;
            defaults.selectedCusServerIndex = selectedCusServerIndex;
            defaults.useCusProfile = useCusProfile;
            defaults.useMultipleServer = useMultipleServer;
            defaults.profilesStr = String.Join("\t", profiles.Select(p => js.Serialize(p)));
            defaults.cusProfilesStr = cusProfiles.Count > 0 ? String.Join("\t", cusProfiles) : "";
            defaults.mainInboundType = mainInboundType;
            defaults.alarmUnknown = alarmUnknown;
            defaults.Save();
        }
        
        public static bool generateConfigJson()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string templateStr = Encoding.UTF8.GetString(Properties.Resources.config_simple);
            dynamic fullConfig = js.Deserialize<dynamic>(templateStr);
            fullConfig["log"]["loglevel"] = logLevel.ToString();
            fullConfig["inbound"]["port"] = localPort;
            fullConfig["inbound"]["listen"] = shareOverLan ? "0.0.0.0" : "127.0.0.1";
            fullConfig["inboundDetour"][0]["listen"] = shareOverLan ? "0.0.0.0" : "127.0.0.1";
            fullConfig["inboundDetour"][0]["port"] = httpPort;
            fullConfig["inbound"]["settings"]["udp"] = udpSupport;
            fullConfig["outbound"] = profiles[selectedServerIndex].OutboundProfile();
            if (useMultipleServer)
            {
                List<Dictionary<string, object>> vpoints = new List<Dictionary<string, object>>();
                foreach (var p in profiles)
                {
                    dynamic op = p.OutboundProfile();
                    vpoints.Add(op["settings"]["vnext"][0]);
                }
                fullConfig["outbound"]["settings"]["vnext"] = vpoints;
            }
            string[] dnsArray = dnsString.Split(',');
            if(dnsArray.Count() > 0)
            {
                fullConfig["dns"]["servers"] = dnsArray;
            } else
            {
                fullConfig["dns"]["servers"] = new string[] { "localhost " };
            }

            if(proxyMode == RULES_MODE)
            {
                List<object> domainRuls = new List<object>();
                foreach(var p in fullConfig["routing"]["settings"]["rules"][0]["domain"])
                {
                    domainRuls.Add(p);
                }
                domainRuls.Add("geosite:cn");
                fullConfig["routing"]["settings"]["rules"][0]["domain"] = domainRuls;
                List<object> ipRuls = new List<object>();
                foreach(var p in fullConfig["routing"]["settings"]["rules"][1]["ip"])
                {
                    ipRuls.Add(p);
                }
                ipRuls.Add("geoip:cn");
                fullConfig["routing"]["settings"]["rules"][1]["ip"] = ipRuls;
            } else if (proxyMode == MANUAL_MODE)
            {
                fullConfig["routing"]["settings"]["rules"] = new object[] { };
            } 
            try
            {
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "configw.json", js.Serialize(fullConfig));
                return true;
            } catch
            {
                MessageBox.Show("cannot create config files!");
                return false;
            }
        }

        //back_ground functions 
        private static void v2rayCoreWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;

            var v2rayProcess = new Process();
            v2rayProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "v2ray.exe";
            v2rayProcess.StartInfo.Arguments = "-config " + @"""" + 
                (useCusProfile ? cusProfiles[selectedCusServerIndex] : (AppDomain.CurrentDomain.BaseDirectory + "configw.json") )+ @"""";
            v2rayProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            v2rayProcess.StartInfo.UseShellExecute = false;
            v2rayProcess.StartInfo.RedirectStandardOutput = true;
            v2rayProcess.StartInfo.CreateNoWindow = true;
            //https://msdn.microsoft.com/en-us/library/system.diagnostics.process.outputdatareceived.aspx
            output.Clear();
            lineCount = 0;
            v2rayProcess.OutputDataReceived += new DataReceivedEventHandler((psender, pe) =>
            {
                // Prepend line numbers to each line of the output.
                if (!String.IsNullOrEmpty(pe.Data))
                {
                    lineCount++;
                    output.Append("[" + lineCount + "]: " + pe.Data + "\r\n");
                }
            });
            v2rayProcess.Start();
            v2rayProcess.BeginOutputReadLine();
            while (!bw.CancellationPending && !v2rayProcess.HasExited)
            {
                v2rayProcess.WaitForExit(500);
            }
            Debug.WriteLine("going to kill");
            try
            {
                v2rayProcess.Kill();
            }
            catch { }
            Debug.WriteLine("killed");
            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }
            if(finalAction)
            {
                Debug.WriteLine("final action, set");
                _resetEvent.Set();
            }
        }

        private static void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                DialogResult res = MessageBox.Show("v2ray core exited unexpectedly! \n View log information?","Error", MessageBoxButtons.OKCancel,MessageBoxIcon.Stop);
                if (res == DialogResult.OK)
                {
                    mainForm.viewLogToolStripMenuItem_Click(sender, e);
                }
                coreLoaded = false;
                mainForm.updateMenu();
                updateSystemProxy();
            }
        }

        private static int checkV2RayCore() // 0: ok, 1: unknown v2ray version; 2 error or not v2ray
        {
            var v2rayProcess = new Process();
            v2rayProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "v2ray";
            v2rayProcess.StartInfo.Arguments = "-version";
            v2rayProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            v2rayProcess.StartInfo.UseShellExecute = false;
            v2rayProcess.StartInfo.RedirectStandardOutput = true;
            v2rayProcess.StartInfo.CreateNoWindow = true;
            try
            {
                v2rayProcess.Start();
            } catch
            {
                return 2;
            }
            var versionOutput = v2rayProcess.StandardOutput.ReadToEnd();
            v2rayProcess.WaitForExit();
            if (v2rayProcess.ExitCode != 0)
            {
                return 2;
            } else
            {
                if (versionOutput.StartsWith("V2Ray "))
                {
                    if (versionOutput.StartsWith("V2Ray " + v2rayVersion))
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 2;
                }
            }
        }
    }

    public enum LogLevel
    {
        none = 0,
        error = 1,
        warning = 2,
        info = 3,
        debug = 4
    };
}
