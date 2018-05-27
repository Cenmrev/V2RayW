namespace V2RayW
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.v2RayRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pacModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.serversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editPacFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenuStrip = this.contextMenuStripMain;
            this.notifyIconMain.Icon = global::V2RayW.Properties.Resources.vw256;
            this.notifyIconMain.Text = "V2RayW";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconMain_MouseDoubleClick);
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem,
            this.startStopToolStripMenuItem,
            this.viewLogToolStripMenuItem,
            this.toolStripSeparator3,
            this.v2RayRulesToolStripMenuItem,
            this.pacModeToolStripMenuItem,
            this.globalModeToolStripMenuItem,
            this.manualModeToolStripMenuItem,
            this.toolStripSeparator2,
            this.serversToolStripMenuItem,
            this.editPacFileToolStripMenuItem,
            this.configureToolStripMenuItem,
            this.toolStripSeparator1,
            this.helpToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenuStripMain.Name = "contextMenuStripMain";
            this.contextMenuStripMain.Size = new System.Drawing.Size(181, 308);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Enabled = false;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.statusToolStripMenuItem.Text = "Status";
            // 
            // startStopToolStripMenuItem
            // 
            this.startStopToolStripMenuItem.Name = "startStopToolStripMenuItem";
            this.startStopToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.startStopToolStripMenuItem.Text = "StartStop";
            this.startStopToolStripMenuItem.Click += new System.EventHandler(this.startStopToolStripMenuItem_Click);
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewLogToolStripMenuItem.Text = "View log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // v2RayRulesToolStripMenuItem
            // 
            this.v2RayRulesToolStripMenuItem.Name = "v2RayRulesToolStripMenuItem";
            this.v2RayRulesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.v2RayRulesToolStripMenuItem.Text = "V2Ray Rules";
            this.v2RayRulesToolStripMenuItem.Click += new System.EventHandler(this.v2RayRulesToolStripMenuItem_Click);
            // 
            // pacModeToolStripMenuItem
            // 
            this.pacModeToolStripMenuItem.Name = "pacModeToolStripMenuItem";
            this.pacModeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pacModeToolStripMenuItem.Text = "Pac Mode";
            this.pacModeToolStripMenuItem.Visible = false;
            this.pacModeToolStripMenuItem.Click += new System.EventHandler(this.pacModeToolStripMenuItem_Click);
            // 
            // globalModeToolStripMenuItem
            // 
            this.globalModeToolStripMenuItem.Name = "globalModeToolStripMenuItem";
            this.globalModeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.globalModeToolStripMenuItem.Text = "Global Mode";
            this.globalModeToolStripMenuItem.Click += new System.EventHandler(this.globalModeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // serversToolStripMenuItem
            // 
            this.serversToolStripMenuItem.Name = "serversToolStripMenuItem";
            this.serversToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.serversToolStripMenuItem.Text = "Servers";
            // 
            // editPacFileToolStripMenuItem
            // 
            this.editPacFileToolStripMenuItem.Name = "editPacFileToolStripMenuItem";
            this.editPacFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editPacFileToolStripMenuItem.Text = "Edit Pac File";
            this.editPacFileToolStripMenuItem.Visible = false;
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.configureToolStripMenuItem.Text = "Configure...";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // manualModeToolStripMenuItem
            // 
            this.manualModeToolStripMenuItem.Name = "manualModeToolStripMenuItem";
            this.manualModeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.manualModeToolStripMenuItem.Text = "Manual Mode";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.contextMenuStripMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem startStopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem v2RayRulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem pacModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serversToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editPacFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualModeToolStripMenuItem;
    }
}