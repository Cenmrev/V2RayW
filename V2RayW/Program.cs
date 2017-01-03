using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace V2RayW
{
    static class Program
    {

        public static List<Profile> profiles = new List<Profile>();
        public static int selectedServerIndex
        {
            get { return Properties.Settings.Default.selectedServerIndex; }
            set
            {
                Properties.Settings.Default.selectedServerIndex = value;
                Properties.Settings.Default.Save();
            }
        }
        public static bool proxyIsOn
        {
            get { return Properties.Settings.Default.proxyIsOn; }
            set
            {
                Properties.Settings.Default.proxyIsOn = value;
                Properties.Settings.Default.Save();
            }
        }
        public static int proxyMode
        {
            get { return Properties.Settings.Default.proxyMode % 3; }
            set
            {
                Properties.Settings.Default.proxyMode = value % 3;
                Properties.Settings.Default.Save();
            }
        }
        public static MainForm mainForm;
        const string v2rayVersion = "v2.13.1";
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
                        DialogResult res = MessageBox.Show(String.Format("Unknown version of v2ray core.\n{0} is suggested. Do you want to continue to use existing core?", Program.v2rayVersion), "Unknown v2ray.exe!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (res == DialogResult.OK)
                        {
                            break;
                        } else
                        {
                            DialogResult dres = MessageBox.Show(String.Format("Do you want to download official core {0} right now?", Program.v2rayVersion), "Unknown v2ray.exe!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (dres == DialogResult.OK)
                            {
                                Process.Start(String.Format(@"https://github.com/v2ray/v2ray-core/releases/tag/{0}", v2rayVersion));
                            }
                            return;
                        }
                    }
                default: break;
            }
            
            //Properties.Settings.Default.Reset();
            Properties.Settings.Default.Upgrade();
            //MessageBox.Show(Properties.Settings.Default.profilesStr);
            var dProfilesStrArray = Properties.Settings.Default.profilesStr.Split('\t');
            foreach (string pstr in dProfilesStrArray)
            {
                var p = new Profile();
                var dp = JObject.Parse(pstr);
                p.address = (dp["address"] ?? "").ToString();
                p.port = (int)(dp["port"] ?? 10086);
                p.allowPassive = (bool)(dp["allowPassive"] ?? false);
                p.alterId = (int)(dp["alterId"] ?? 0);
                p.network = (int)(dp["network"] ?? 0);
                p.remark = (dp["remark"] ?? "").ToString();
                p.userId = (dp["userId"] ?? "").ToString();
                p.security = (int)(dp["security"] ?? 0);
                Program.profiles.Add(p);
            }
            if (profiles.Count <= 0)
            {
                Program.proxyIsOn = false;
            }
            if (Program.selectedServerIndex >= Program.profiles.Count )
            {
                Program.selectedServerIndex = Program.profiles.Count - 1;
            }
            if (Program.profiles.Count > 0 && Program.selectedServerIndex < 0)
            {
                Program.selectedServerIndex = 0;
            }
            mainForm = new MainForm();
            mainForm.updateMenu();
            Program.updateSystemProxy();

            Application.Run();
        }
        /*
        private static void V2rayCoreWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }*/
        
        static void OnProcessExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        //{"address":"v2ray.cool","allowPassive":0,"alterId":64,"network":0,"port":10086,"remark":"test server","userId":"23ad6b10-8d1a-40f7-8ad0-e3e35cd38297"}
        internal static string profileToStr(Profile p)
        {
            var pd = new
            {
                address = p.address,
                port = p.port,
                userId = p.userId,
                alterId = p.alterId,
                remark = p.remark,
                allowPassive = p.allowPassive,
                network = p.network,
                security = p.security
            };
            return JsonConvert.SerializeObject(pd);
        }

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

        public static async void updateSystemProxy()
        {
            // for final action, change system proxy first and then stop v2ray;
            if (finalAction)
            {
                runSysproxy();
                Debug.WriteLine("system proxy changed, will stop v2ray");
            }

            if (proxyIsOn)
            {
                await stopV2Ray();
                //generate config.json
                if (generateConfigJson())
                {
                    v2rayCoreWorker.RunWorkerAsync();
                } else
                {
                    Program.proxyIsOn = false;
                    mainForm.updateMenu();
                }
            } else
            {
                await stopV2Ray();
                Debug.WriteLine("v stopped");
            }
            //change system proxy
            if (!finalAction)
            {
                var res = runSysproxy();
                if (res == 2)
                    MessageBox.Show("Fained to modify system proxy settings!");
                else if (res == 1)
                    Debug.WriteLine("Shall I tell the user to wait for a while? "); 
            }
        }

        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/19517edf-8348-438a-a3da-5fbe7a46b61a/how-to-change-global-windows-proxy-using-c-net-with-immediate-effect?forum=csharpgeneral
        [DllImport("wininet.dll")]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        const int INTERNET_OPTION_REFRESH = 37;

        public static int runSysproxy()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

            bool settingsReturn, refreshReturn;

            registry.SetValue("ProxyEnable", proxyIsOn ? 1 : 0);
            if (proxyIsOn)
            {
                registry.SetValue("ProxyServer", (Properties.Settings.Default.inProtocol == 0 ? "socks=" : "http://") + $"127.0.0.1:{Properties.Settings.Default.localPort}");
                registry.SetValue("ProxyOverride", "<local>;localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;172.32.*;192.168.*");
            }
            var sysState = registry.GetValue("ProxyEnable").ToString() == (proxyIsOn ? "1" : "0");
            var sysServer = proxyIsOn ? registry.GetValue("ProxyServer").ToString() == (Properties.Settings.Default.inProtocol == 0 ? "socks=" : "http://") +  $"127.0.0.1:{Properties.Settings.Default.localPort}" : true;
            //MessageBox.Show(registry.GetValue("ProxyServer").ToString());
            var sysOverride = proxyIsOn ? registry.GetValue("ProxyOverride").ToString() == "<local>;localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;172.32.*;192.168.*" : true;
            // They cause the OS to refresh the settings, causing IP to realy update
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

        /*
        public static int runSysproxy()
        {
            var sysproxyProcess = new Process();
            sysproxyProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "sysproxy.exe";
            if (proxyIsOn)
            {
                if (proxyMode != 1) // not pac mode
                {
                    sysproxyProcess.StartInfo.Arguments = $"global 127.0.0.1:{Properties.Settings.Default.localPort} <local>;localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;172.32.*;192.168.*";
                } else // pacmode 
                {
                    ;
                }
                
            } else
            {
                sysproxyProcess.StartInfo.Arguments = "off";
            }
            sysproxyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            sysproxyProcess.StartInfo.UseShellExecute = false;
            sysproxyProcess.StartInfo.RedirectStandardOutput = true;
            sysproxyProcess.StartInfo.CreateNoWindow = true;
            try
            {
                sysproxyProcess.Start();
                sysproxyProcess.WaitForExit();
                return sysproxyProcess.ExitCode;
            } catch (Exception e)
            {
                MessageBox.Show($"error!\n{ e.ToString() }");
                return 1;
            }
        }
        */
        public static bool generateConfigJson()
        {
            string templateStr = Encoding.UTF8.GetString(proxyMode == 0 ? Properties.Resources.config_rules : Properties.Resources.config_simple);
            dynamic json = JObject.Parse(templateStr);
            json.transport = JObject.Parse(Properties.Settings.Default.transportSettings);
            json.inbound.port = Properties.Settings.Default.localPort;
            json.inbound.protocol = Properties.Settings.Default.inProtocol == 0 ? "socks" : "http";
            if (Properties.Settings.Default.inProtocol == 0)
            {
                var inboundSettings = new
                {
                    auth = "noauth",
                    udp = Properties.Settings.Default.udpSupport,
                    ip = "127.0.0.1"
                };
                json.inbound.settings = JObject.Parse(JsonConvert.SerializeObject(inboundSettings));
            } else
            {
                json.inbound.settings = JObject.Parse("{\"timeout\": 0 }");
            }
            json.inbound.allowPassive = profiles[selectedServerIndex].allowPassive;
            json.outbound.settings.vnext[0].address = profiles[selectedServerIndex].address;
            json.outbound.settings.vnext[0].port = profiles[selectedServerIndex].port;
            json.outbound.settings.vnext[0].users[0].id = profiles[selectedServerIndex].userId;
            json.outbound.settings.vnext[0].users[0].alterId = profiles[selectedServerIndex].alterId;
            json.outbound.settings.vnext[0].users[0].security = (new string[] { "aes-128-cfb", "aes-128-gcm", "chacha20-poly1305" })[profiles[selectedServerIndex].security % 3];
            json.outbound.streamSettings.network = (new string[]{ "tcp", "kcp", "ws" })[profiles[selectedServerIndex].network % 3];
            var dnsArray = Properties.Settings.Default.dns.Split(',');
            json.dns = JObject.Parse(dnsArray.Count() > 0 ? JsonConvert.SerializeObject( new { servers = dnsArray }) : "{\"servers\":[\"localhost\"]}");   
            try
            {
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "configw.json", JsonConvert.SerializeObject(json));
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
            v2rayProcess.StartInfo.Arguments = "-config " + AppDomain.CurrentDomain.BaseDirectory + "configw.json";
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
                proxyIsOn = false;
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
    
    public class Profile
    {
        internal string address = "1.2.3.4";
        internal bool allowPassive = false;
        internal int alterId = 0;
        internal int network = 0;
        internal int port = 10086;
        internal string remark = "";
        internal string userId = "";
        internal int security = 0;
        public Profile DeepCopy()
        {
            Profile p = new Profile();
            p.address = String.Copy(this.address);
            p.allowPassive = this.allowPassive;
            p.alterId = this.alterId;
            p.network = this.network;
            p.port = this.port;
            p.remark = String.Copy(this.remark);
            p.userId = String.Copy(this.userId);
            p.security = this.security;
            return p;
        }
    }
}
