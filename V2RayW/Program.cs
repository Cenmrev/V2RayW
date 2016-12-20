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
            var mainForm = new MainForm();
            //Properties.Settings.Default.Reset();
            Properties.Settings.Default.Upgrade();
            //Debug.WriteLine(String.Format("property profile {0}", Properties.Settings.Default.profilesStr));
            dynamic[] dProfiles = Properties.Settings.Default.profilesStr.Split('\t').Select(pstr => JObject.Parse(pstr)).ToArray();
            foreach (dynamic dp in dProfiles)
            {
                var p = new Profile();
                p.address = dp.address;
                p.allowPassive = dp.allowPassive;
                p.alterId = dp.alterId;
                p.network = dp.network;
                p.remark = dp.remark;
                p.userId = dp.userId;
                Program.profiles.Add(p);
            }
            Program.selectedServerIndex = Properties.Settings.Default.selectedServerIndex;
            if (! (Program.selectedServerIndex < Program.profiles.Count) )
            {
                Program.selectedServerIndex = Program.profiles.Count - 1;
            }
            //Debug.WriteLine(Program.profileToStr(profiles[0]));
            //Debug.WriteLine(Program.selectedServerIndex);
            Application.Run();
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.selectedServerIndex = Program.selectedServerIndex;
            var profileArray = Program.profiles.Select(p => profileToStr(p));
            Properties.Settings.Default.profilesStr = String.Join("\t", profileArray);
            //Debug.WriteLine(String.Format("property profile {0}", Properties.Settings.Default.profilesStr));
            Properties.Settings.Default.Save();
        }


        public static List<Profile> profiles = new List<Profile>();
        public static int selectedServerIndex = 0;

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

        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
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
    }
    
    public class Profile
    {
        internal string address = "v2ray.cool";
        internal bool allowPassive = false;
        internal int alterId = 64;
        internal int network = 0;
        internal int port = 10086;
        internal string remark = "";
        internal string userId = "23ad6b10-8d1a-40f7-8ad0-e3e35cd38297";
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
