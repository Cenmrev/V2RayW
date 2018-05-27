using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace V2RayW
{
    public partial class FormImport : Form
    {
        public static List<String> configJsonPaths = new List<string>();

        public FormImport()
        {
            InitializeComponent();
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            labelIndicator.Text = "verifying...";
            foreach(var jsonpath in configJsonPaths)
            {
                try
                {
                    ProcessStartInfo info = new ProcessStartInfo();
                    string v2rayBin = AppDomain.CurrentDomain.BaseDirectory + "v2ray.exe";
                    info.FileName = v2rayBin;
                    info.Arguments = "-test -config " + jsonpath;
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    Process proBach = Process.Start(info);
                    proBach.WaitForExit();
                    int returnValue = proBach.ExitCode;
                    if(returnValue != 0)
                    {
                        MessageBox.Show(jsonpath + " is not a valid v2ray config file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        labelIndicator.Text = "";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    labelIndicator.Text = "";
                    return;
                }
            }
            Properties.Settings.Default["cusProfiles"] = string.Join("*", configJsonPaths);
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                configJsonPaths.Add(dialog.FileName);
                listBoxCusConfig.Items.Clear();
                foreach (var path in configJsonPaths)
                {
                    listBoxCusConfig.Items.Add(path);
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxCusConfig.SelectedIndex > configJsonPaths.Count || listBoxCusConfig.SelectedIndex < 0)
            {
                return;
            }
            configJsonPaths.RemoveAt(listBoxCusConfig.SelectedIndex);
            listBoxCusConfig.Items.Clear();
            foreach (var path in configJsonPaths)
            {
                listBoxCusConfig.Items.Add(path);
            }
            listBoxCusConfig_SelectedValueChanged(sender, e);
        }

        private void FormImport_Load(object sender, EventArgs e)
        {
            labelIndicator.Text = "";
            string[] savedProfiles = Properties.Settings.Default["cusProfiles"].ToString().Split('*');
            configJsonPaths.Clear();
            foreach(var p in savedProfiles)
            {
                if(p.Trim().Length > 0)
                {
                    configJsonPaths.Add(p);
                    listBoxCusConfig.Items.Add(p);
                }
            }
            listBoxCusConfig_SelectedValueChanged(sender, e);
        }

        private void listBoxCusConfig_SelectedValueChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = listBoxCusConfig.SelectedIndex < configJsonPaths.Count && listBoxCusConfig.SelectedIndex >= 0;
        }
    }
}
