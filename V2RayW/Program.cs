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

namespace V2RayW
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit); 

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            //Properties.Settings.Default.Reset();
            Properties.Settings.Default.Upgrade();
            //Debug.WriteLine(String.Format("property profile {0}", Properties.Settings.Default.profilesStr));
            //MessageBox.Show(Properties.Settings.Default.profilesStr);
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
            //Debug.WriteLine(Program.profileToStr(profiles[0]));
            //Debug.WriteLine(Program.selectedServerIndex);
            Application.Run();
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
        }


        public static List<Profile> profiles = new List<Profile>();
        public static int selectedServerIndex = 0;
        public static bool proxyIsOn = false;
        public static int proxyMode = 0;
        public static MainForm mainForm;

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

        }

        public static void stopV2Ray()
        {

        }

        public static void updateSystemProxy()
        {

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
