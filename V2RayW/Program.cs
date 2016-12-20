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
            var config = new MainForm();
            Application.Run();
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
}
