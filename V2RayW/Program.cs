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

namespace V2RayW
{
    static class Program
    {

        public static List<Profile> profiles = new List<Profile>();
        public static int selectedServerIndex = 0;
        public static bool proxyIsOn = false;
        public static int proxyMode = 0;
        public static MainForm mainForm;
        const string v2rayVersion = "v2.11";
        static BackgroundWorker v2rayCoreWorker = new System.ComponentModel.BackgroundWorker();
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //check v2ray core binary
            if (!checkV2RayCore())
            {
                DialogResult res = MessageBox.Show("Wrong or missing v2ray core file!\nDownload it right now?", "Error!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.OK)
                {
                    Process.Start(String.Format(@"https://github.com/v2ray/v2ray-core/releases/tag/{0}", v2rayVersion));
                }
                return;
            }

            // save settings when exiting the program
            // AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit); 
            
            //Properties.Settings.Default.Reset();
            Properties.Settings.Default.Upgrade();
            var dProfilesStrArray = Properties.Settings.Default.profilesStr.Split('\t');
            foreach (string pstr in dProfilesStrArray)
            {
                try
                {
                    var p = new Profile();
                    dynamic dp = JObject.Parse(pstr);
                    p.address = dp.address;
                    p.allowPassive = dp.allowPassive;
                    p.alterId = dp.alterId;
                    p.network = dp.network;
                    p.remark = dp.remark;
                    p.userId = dp.userId;
                    Program.profiles.Add(p);
                } catch
                {
                    continue;
                }
            }

            Program.selectedServerIndex = Properties.Settings.Default.selectedServerIndex;
            Program.proxyIsOn = profiles.Count > 0 ? Properties.Settings.Default.proxyIsOn : false;
            Program.proxyMode = Properties.Settings.Default.proxyMode % 3;
            if (! (Program.selectedServerIndex < Program.profiles.Count) )
            {
                Program.selectedServerIndex = Program.profiles.Count - 1;
            }

            mainForm = new MainForm();
            mainForm.updateMenu();
            Program.updateSystemProxy();

            generateConfigJson();

            Application.Run();
        }

        private static void V2rayCoreWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.proxyIsOn = Program.proxyIsOn;
            Properties.Settings.Default.proxyMode = Program.proxyMode;
            Properties.Settings.Default.selectedServerIndex = Program.selectedServerIndex;
            var profileArray = Program.profiles.Select(p => profileToStr(p));
            Properties.Settings.Default.profilesStr = String.Join("\t", profileArray);
            //Debug.WriteLine(String.Format("property profile {0}", Properties.Settings.Default.profilesStr));
            Properties.Settings.Default.Save();
            proxyIsOn = false;
            Debug.WriteLine("going to stop v");
            finalAction = true;
            MessageBox.Show("dsaf");
            updateSystemProxy();
            _resetEvent.WaitOne();
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
                network = p.network
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

        public static void startV2Ray()
        {
            v2rayCoreWorker.RunWorkerAsync();
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
            if (finalAction) runSysproxy();

            if (proxyIsOn)
            {
                await stopV2Ray();
                //generate config.json
                if (generateConfigJson())
                {
                    v2rayCoreWorker.RunWorkerAsync();
                } else
                {
                    proxyIsOn = false;
                    mainForm.updateMenu();
                }
            } else
            {
                await stopV2Ray();
                Debug.WriteLine("v stopped");
            }
            //change system proxy
            if (!finalAction && runSysproxy() != 0)
            {
                MessageBox.Show("Fained to modify system proxy settings!");
            }
        }

        public static int runSysproxy()
        {
            var sysproxyProcess = new Process();
            sysproxyProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + (Environment.Is64BitOperatingSystem ? "sysproxy64.exe" : "sysproxy.exe");
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

        public static bool generateConfigJson()
        {
            string templateStr = System.Text.Encoding.UTF8.GetString(proxyMode == 0 ? Properties.Resources.config_rules : Properties.Resources.config_simple);
            dynamic json = JObject.Parse(templateStr);
            json.transport = JObject.Parse(Properties.Settings.Default.transportSettings);
            json.inbound.port = Properties.Settings.Default.localPort;
            json.inbound.settings.udp = Properties.Settings.Default.udpSupport;
            json.inbound.allowPassive = profiles[selectedServerIndex].allowPassive;
            json.outbound.settings.vnext[0].address = profiles[selectedServerIndex].address;
            json.outbound.settings.vnext[0].port = profiles[selectedServerIndex].port;
            json.outbound.settings.vnext[0].users[0].id = profiles[selectedServerIndex].userId;
            json.outbound.settings.vnext[0].users[0].alterId = profiles[selectedServerIndex].alterId;
            json.outbound.streamSettings.network = (new string[]{ "tcp", "kcp", "ws" })[profiles[selectedServerIndex].network];
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

        private static bool checkV2RayCore()
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
                return false;
            }
            var versionOutput = v2rayProcess.StandardOutput.ReadToEnd();
            v2rayProcess.WaitForExit();
            return v2rayProcess.ExitCode == 0 && versionOutput.StartsWith("V2Ray " + v2rayVersion);
        }
    }
    
    public class Profile
    {
        internal string address = "v2ray.cool";
        internal bool allowPassive = false;
        internal int alterId = 64;
        internal int network = 0;
        internal int port = 10086;
        internal string remark = "";
        internal string userId = Guid.NewGuid().ToString();
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
            return p;
        }
    }
}
