namespace V2RayW
{
    partial class ConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.labelLocalPort = new System.Windows.Forms.Label();
            this.checkBoxUDP = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelDNS = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxLocalPort = new System.Windows.Forms.TextBox();
            this.textBoxDNS = new System.Windows.Forms.TextBox();
            this.buttonTS = new System.Windows.Forms.Button();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxServers = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxSecurity = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxNetwork = new System.Windows.Forms.ComboBox();
            this.textBoxRemark = new System.Windows.Forms.TextBox();
            this.textBoxAlterID = new System.Windows.Forms.TextBox();
            this.textBoxUserId = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxInP = new System.Windows.Forms.ComboBox();
            this.checkBoxAlarm = new System.Windows.Forms.CheckBox();
            this.buttonImport = new System.Windows.Forms.Button();
            this.checkBoxShareOverLan = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxLog = new System.Windows.Forms.ComboBox();
            this.labelHttpPort = new System.Windows.Forms.Label();
            this.textBoxHttpPort = new System.Windows.Forms.TextBox();
            this.groupBoxServer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelLocalPort
            // 
            resources.ApplyResources(this.labelLocalPort, "labelLocalPort");
            this.labelLocalPort.Name = "labelLocalPort";
            // 
            // checkBoxUDP
            // 
            resources.ApplyResources(this.checkBoxUDP, "checkBoxUDP");
            this.checkBoxUDP.Name = "checkBoxUDP";
            this.checkBoxUDP.UseVisualStyleBackColor = true;
            this.checkBoxUDP.CheckedChanged += new System.EventHandler(this.checkBoxUDP_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labelDNS
            // 
            resources.ApplyResources(this.labelDNS, "labelDNS");
            this.labelDNS.Name = "labelDNS";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxLocalPort
            // 
            resources.ApplyResources(this.textBoxLocalPort, "textBoxLocalPort");
            this.textBoxLocalPort.Name = "textBoxLocalPort";
            this.textBoxLocalPort.TextChanged += new System.EventHandler(this.textBoxLocalPort_TextChanged);
            // 
            // textBoxDNS
            // 
            resources.ApplyResources(this.textBoxDNS, "textBoxDNS");
            this.textBoxDNS.Name = "textBoxDNS";
            this.textBoxDNS.TextChanged += new System.EventHandler(this.textBoxDNS_TextChanged);
            // 
            // buttonTS
            // 
            resources.ApplyResources(this.buttonTS, "buttonTS");
            this.buttonTS.Name = "buttonTS";
            this.buttonTS.UseVisualStyleBackColor = true;
            this.buttonTS.Click += new System.EventHandler(this.buttonTS_Click);
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.buttonRemove);
            this.groupBoxServer.Controls.Add(this.buttonAdd);
            this.groupBoxServer.Controls.Add(this.listBoxServers);
            this.groupBoxServer.Controls.Add(this.panel1);
            resources.ApplyResources(this.groupBoxServer, "groupBoxServer");
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.TabStop = false;
            // 
            // buttonRemove
            // 
            resources.ApplyResources(this.buttonRemove, "buttonRemove");
            this.buttonRemove.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listBoxServers
            // 
            this.listBoxServers.FormattingEnabled = true;
            resources.ApplyResources(this.listBoxServers, "listBoxServers");
            this.listBoxServers.Name = "listBoxServers";
            this.listBoxServers.SelectedIndexChanged += new System.EventHandler(this.listBoxServers_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.comboBoxSecurity);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.comboBoxNetwork);
            this.panel1.Controls.Add(this.textBoxRemark);
            this.panel1.Controls.Add(this.buttonTS);
            this.panel1.Controls.Add(this.textBoxAlterID);
            this.panel1.Controls.Add(this.textBoxUserId);
            this.panel1.Controls.Add(this.textBoxPort);
            this.panel1.Controls.Add(this.textBoxAddress);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // comboBoxSecurity
            // 
            this.comboBoxSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity.FormattingEnabled = true;
            this.comboBoxSecurity.Items.AddRange(new object[] {
            resources.GetString("comboBoxSecurity.Items"),
            resources.GetString("comboBoxSecurity.Items1"),
            resources.GetString("comboBoxSecurity.Items2"),
            resources.GetString("comboBoxSecurity.Items3"),
            resources.GetString("comboBoxSecurity.Items4")});
            resources.ApplyResources(this.comboBoxSecurity, "comboBoxSecurity");
            this.comboBoxSecurity.Name = "comboBoxSecurity";
            this.comboBoxSecurity.SelectedIndexChanged += new System.EventHandler(this.comboBoxSecurity_SelectedIndexChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // comboBoxNetwork
            // 
            this.comboBoxNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNetwork.FormattingEnabled = true;
            this.comboBoxNetwork.Items.AddRange(new object[] {
            resources.GetString("comboBoxNetwork.Items"),
            resources.GetString("comboBoxNetwork.Items1"),
            resources.GetString("comboBoxNetwork.Items2"),
            resources.GetString("comboBoxNetwork.Items3")});
            resources.ApplyResources(this.comboBoxNetwork, "comboBoxNetwork");
            this.comboBoxNetwork.Name = "comboBoxNetwork";
            this.comboBoxNetwork.SelectedIndexChanged += new System.EventHandler(this.comboBoxNetwork_SelectedIndexChanged);
            // 
            // textBoxRemark
            // 
            resources.ApplyResources(this.textBoxRemark, "textBoxRemark");
            this.textBoxRemark.Name = "textBoxRemark";
            this.textBoxRemark.TextChanged += new System.EventHandler(this.textBoxRemark_TextChanged);
            // 
            // textBoxAlterID
            // 
            resources.ApplyResources(this.textBoxAlterID, "textBoxAlterID");
            this.textBoxAlterID.Name = "textBoxAlterID";
            this.textBoxAlterID.TextChanged += new System.EventHandler(this.textBoxAlterID_TextChanged);
            // 
            // textBoxUserId
            // 
            resources.ApplyResources(this.textBoxUserId, "textBoxUserId");
            this.textBoxUserId.Name = "textBoxUserId";
            this.textBoxUserId.TextChanged += new System.EventHandler(this.textBoxUserId_TextChanged);
            // 
            // textBoxPort
            // 
            resources.ApplyResources(this.textBoxPort, "textBoxPort");
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.TextChanged += new System.EventHandler(this.textBoxPort_TextChanged);
            // 
            // textBoxAddress
            // 
            resources.ApplyResources(this.textBoxAddress, "textBoxAddress");
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.TextChanged += new System.EventHandler(this.textBoxAddress_TextChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // comboBoxInP
            // 
            this.comboBoxInP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInP.FormattingEnabled = true;
            this.comboBoxInP.Items.AddRange(new object[] {
            resources.GetString("comboBoxInP.Items"),
            resources.GetString("comboBoxInP.Items1")});
            resources.ApplyResources(this.comboBoxInP, "comboBoxInP");
            this.comboBoxInP.Name = "comboBoxInP";
            this.comboBoxInP.SelectedIndexChanged += new System.EventHandler(this.comboBoxInP_SelectedIndexChanged);
            // 
            // checkBoxAlarm
            // 
            resources.ApplyResources(this.checkBoxAlarm, "checkBoxAlarm");
            this.checkBoxAlarm.Name = "checkBoxAlarm";
            this.checkBoxAlarm.UseVisualStyleBackColor = true;
            this.checkBoxAlarm.CheckedChanged += new System.EventHandler(this.checkBoxAlarm_CheckedChanged);
            // 
            // buttonImport
            // 
            resources.ApplyResources(this.buttonImport, "buttonImport");
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // checkBoxShareOverLan
            // 
            resources.ApplyResources(this.checkBoxShareOverLan, "checkBoxShareOverLan");
            this.checkBoxShareOverLan.Name = "checkBoxShareOverLan";
            this.checkBoxShareOverLan.UseVisualStyleBackColor = true;
            this.checkBoxShareOverLan.CheckedChanged += new System.EventHandler(this.checkBoxShareOverLan_CheckedChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // comboBoxLog
            // 
            this.comboBoxLog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLog.FormattingEnabled = true;
            this.comboBoxLog.Items.AddRange(new object[] {
            resources.GetString("comboBoxLog.Items"),
            resources.GetString("comboBoxLog.Items1"),
            resources.GetString("comboBoxLog.Items2"),
            resources.GetString("comboBoxLog.Items3"),
            resources.GetString("comboBoxLog.Items4")});
            resources.ApplyResources(this.comboBoxLog, "comboBoxLog");
            this.comboBoxLog.Name = "comboBoxLog";
            this.comboBoxLog.SelectedIndexChanged += new System.EventHandler(this.comboBoxLog_SelectedIndexChanged);
            // 
            // labelHttpPort
            // 
            resources.ApplyResources(this.labelHttpPort, "labelHttpPort");
            this.labelHttpPort.Name = "labelHttpPort";
            // 
            // textBoxHttpPort
            // 
            resources.ApplyResources(this.textBoxHttpPort, "textBoxHttpPort");
            this.textBoxHttpPort.Name = "textBoxHttpPort";
            this.textBoxHttpPort.TextChanged += new System.EventHandler(this.textBoxHttpPort_TextChanged);
            // 
            // ConfigForm
            // 
            this.AcceptButton = this.buttonSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.textBoxHttpPort);
            this.Controls.Add(this.labelHttpPort);
            this.Controls.Add(this.comboBoxLog);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.checkBoxShareOverLan);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.checkBoxAlarm);
            this.Controls.Add(this.comboBoxInP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBoxServer);
            this.Controls.Add(this.textBoxDNS);
            this.Controls.Add(this.textBoxLocalPort);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelDNS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxUDP);
            this.Controls.Add(this.labelLocalPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ConfigForm";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.groupBoxServer.ResumeLayout(false);
            this.groupBoxServer.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLocalPort;
        private System.Windows.Forms.CheckBox checkBoxUDP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelDNS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxLocalPort;
        private System.Windows.Forms.TextBox textBoxDNS;
        private System.Windows.Forms.Button buttonTS;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxNetwork;
        private System.Windows.Forms.TextBox textBoxRemark;
        private System.Windows.Forms.TextBox textBoxAlterID;
        private System.Windows.Forms.TextBox textBoxUserId;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxServers;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxInP;
        private System.Windows.Forms.ComboBox comboBoxSecurity;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBoxAlarm;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.CheckBox checkBoxShareOverLan;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxLog;
        private System.Windows.Forms.Label labelHttpPort;
        private System.Windows.Forms.TextBox textBoxHttpPort;
    }
}