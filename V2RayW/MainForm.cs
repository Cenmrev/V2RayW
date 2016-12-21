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
    public partial class MainForm : Form
    {

        ConfigForm configForm;
        FormCoreOutput outputForm;

        public MainForm()
        {
            InitializeComponent();
        }

        public void updateMenu()
        {
            statusToolStripMenuItem.Text = Program.proxyIsOn ? "V2Ray: On" : "V2Ray: Off";
            startStopToolStripMenuItem.Enabled = Program.profiles.Count > 0;
            startStopToolStripMenuItem.Text = Program.proxyIsOn ? "Stop V2Ray" : "Start V2Ray";

            v2RayRulesToolStripMenuItem.Checked = false;
            pacModeToolStripMenuItem.Checked = false;
            globalModeToolStripMenuItem.Checked = false;
            switch (Program.proxyMode)
            {
                case 0: v2RayRulesToolStripMenuItem.Checked = true; break;
                case 1: pacModeToolStripMenuItem.Checked = true; break;
                case 2: globalModeToolStripMenuItem.Checked = true; break;
            }
            serversToolStripMenuItem.DropDownItems.Clear();
            var serverMenuItems = Program.profiles.Select(p => new ToolStripMenuItem(p.remark == "" ? p.address : p.remark, null, switchToServer)).ToArray();
            if (Program.profiles.Count > 0 )
            {
                serverMenuItems[Program.selectedServerIndex].Checked = true;
                foreach (var p in Program.profiles)
                {
                    serversToolStripMenuItem.DropDownItems.AddRange(serverMenuItems);
                }
            } else
            {
                serversToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem("no available servers."));
            }

        }

        private void switchToServer(object sender, EventArgs e)
        {
            Program.selectedServerIndex = serversToolStripMenuItem.DropDownItems.IndexOf((ToolStripMenuItem)sender);
            foreach (ToolStripMenuItem i in serversToolStripMenuItem.DropDownItems)
            {
                i.Checked = false;
            }
            ((ToolStripMenuItem)serversToolStripMenuItem.DropDownItems[Program.selectedServerIndex]).Checked = true;
            Program.updateSystemProxy();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.configForm == null || this.configForm.IsDisposed)
            {
                configForm = new ConfigForm();
                configForm.Show();
            }
            configForm.Focus();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.proxyIsOn = Program.proxyIsOn;
            Properties.Settings.Default.proxyMode = Program.proxyMode;
            Properties.Settings.Default.selectedServerIndex = Program.selectedServerIndex;
            var profileArray = Program.profiles.Select(p => Program.profileToStr(p));
            Properties.Settings.Default.profilesStr = String.Join("\t", profileArray);
            //Debug.WriteLine(String.Format("property profile {0}", Properties.Settings.Default.profilesStr));
            Properties.Settings.Default.Save();
            Program.proxyIsOn = false;
            Program.finalAction = true;
            Program.updateSystemProxy();
            Program._resetEvent.WaitOne();
            Application.Exit();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://v2ray.com");
        }

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            configureToolStripMenuItem_Click(sender, e);
        }

        private void startStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.proxyIsOn = !Program.proxyIsOn;
            Program.updateSystemProxy();
            this.updateMenu();
        }

        private void v2RayRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.proxyMode = 0;
            this.updateMenu();
            Program.updateSystemProxy();
        }

        private void pacModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.proxyMode = 1;
            this.updateMenu();
            Program.updateSystemProxy();
        }

        private void globalModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.proxyMode = 2;
            this.updateMenu();
            Program.updateSystemProxy();
        }

        internal void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.outputForm == null || this.outputForm.IsDisposed)
            {
                outputForm = new FormCoreOutput();
                outputForm.Show();
            }
            outputForm.Focus();
            /*
            if(Program.proxyIsOn)
            {
                startStopToolStripMenuItem_Click(sender,e);
            }
            System.Threading.Thread.Sleep(500);
            MessageBox.Show(Program.v2rayoutput);*/
        }
    }
}
