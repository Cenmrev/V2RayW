using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new MainForm();
            //Properties.Settings.Default.Reset();
            Properties.Settings.Default.Upgrade();
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
            Application.Run();
        }

        public static List<Profile> profiles = new List<Profile>();
        public static int selectedServerIndex = 0;

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
        internal string address = "";
        internal bool allowPassive = false;
        internal int alterId = 64;
        internal int network = 0;
        internal int port = 10086;
        internal string remark = "new server";
        internal string userId = "";
    }
}
