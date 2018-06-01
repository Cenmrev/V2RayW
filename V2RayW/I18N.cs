using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace V2RayW
{
    static class I18N
    {
        private static Dictionary<string, string> wordsDictionary;
        static I18N()
        {
            wordsDictionary = new Dictionary<string, string>();

            string currentName = System.Globalization.CultureInfo.CurrentCulture.EnglishName; ;
            string langData;
            if (currentName.IndexOf("Chinese") != -1)
            {
                langData = Properties.Resources.Lang_zh_cn;
            }
            else
            {
                langData = "";
            }

            string[] langKeyValArr = Regex.Split(langData, "\r\n|\r|\n");
            foreach (string item in langKeyValArr)
            {
                if (item.StartsWith("#"))
                {
                    continue;
                }

                string[] splited = item.Split('=');
                if (splited.Length == 2)
                {
                    wordsDictionary[splited[0]] = splited[1];
                }
            }
        }

        public static string GetValue(string key)
        {
            return wordsDictionary.ContainsKey(key) ? wordsDictionary[key] : key;
        }

        public static void InitControl(Control control)
        {
            control.Text = GetValue(control.Text);
            foreach(Control child in control.Controls)
            {
                InitControl(child);
            }
        }
    }
}
