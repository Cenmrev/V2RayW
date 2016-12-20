using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
