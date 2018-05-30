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
        private List<string> configJsonPaths = new List<string>();
        private int selectedCusServerIndex = 1;

        public FormImport()
        {
            InitializeComponent();
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            labelIndicator.Text = "verifying...";
            foreach (var jsonpath in configJsonPaths)
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
                    if (returnValue != 0)
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
            Program.cusProfiles.Clear();
            foreach(var p in this.configJsonPaths)
            {
                Program.cusProfiles.Add(p.Trim());
            }
            if(this.selectedCusServerIndex >= 0 && this.selectedCusServerIndex < this.configJsonPaths.Count())
            {
                Program.selectedCusServerIndex = this.selectedCusServerIndex;
            } else
            {
                if (this.configJsonPaths.Count > 0)
                {
                    Program.selectedCusServerIndex = 0;
                } else
                {
                    Program.selectedCusServerIndex = -1;
                }
            }
            Program.configurationDidChange();
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
            configJsonPaths.Clear();
            foreach (var p in Program.cusProfiles)
            {
                if (p.Trim().Length > 0)
                {
                    configJsonPaths.Add(p.Trim());
                    listBoxCusConfig.Items.Add(p.Trim());
                }
            }
            this.selectedCusServerIndex = Program.selectedCusServerIndex;
            if (this.selectedCusServerIndex >= 0 && this.selectedCusServerIndex < configJsonPaths.Count())
            {
                this.listBoxCusConfig.SelectedIndex = this.selectedCusServerIndex;
                listBoxCusConfig_SelectedValueChanged(sender, e);
            }
        }

        private void listBoxCusConfig_SelectedValueChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = listBoxCusConfig.SelectedIndex < configJsonPaths.Count && listBoxCusConfig.SelectedIndex >= 0;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
