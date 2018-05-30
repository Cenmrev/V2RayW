namespace V2RayW
{
    partial class FormTransSetting
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
            this.buttonTSCancel = new System.Windows.Forms.Button();
            this.buttonTSSave = new System.Windows.Forms.Button();
            this.buttonTSHelp = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageKcp = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxKcpCon = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxKcpHt = new System.Windows.Forms.ComboBox();
            this.textBoxKcpWb = new System.Windows.Forms.TextBox();
            this.textBoxKcpDc = new System.Windows.Forms.TextBox();
            this.textBoxKcpTti = new System.Windows.Forms.TextBox();
            this.textBoxKcpRb = new System.Windows.Forms.TextBox();
            this.textBoxKcpUc = new System.Windows.Forms.TextBox();
            this.textBoxKcpMtu = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageTcp = new System.Windows.Forms.TabPage();
            this.textBoxTcpHeader = new System.Windows.Forms.TextBox();
            this.checkBoxTcpHeader = new System.Windows.Forms.CheckBox();
            this.tabPageWs = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxWsHeader = new System.Windows.Forms.TextBox();
            this.textBoxWsPath = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPageHttp = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxHttp2Path = new System.Windows.Forms.TextBox();
            this.labelHttp2Path = new System.Windows.Forms.Label();
            this.textBoxHttp2Hosts = new System.Windows.Forms.TextBox();
            this.labelHttp2Hosts = new System.Windows.Forms.Label();
            this.tabPageMux = new System.Windows.Forms.TabPage();
            this.textBoxMuxCc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxMuxEnable = new System.Windows.Forms.CheckBox();
            this.tabPageTLS = new System.Windows.Forms.TabPage();
            this.checkBoxTLSEnable = new System.Windows.Forms.CheckBox();
            this.textBoxTLSSn = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBoxTLSAI = new System.Windows.Forms.CheckBox();
            this.buttonTsReset = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageKcp.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageTcp.SuspendLayout();
            this.tabPageWs.SuspendLayout();
            this.tabPageHttp.SuspendLayout();
            this.tabPageMux.SuspendLayout();
            this.tabPageTLS.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTSCancel
            // 
            this.buttonTSCancel.AutoSize = true;
            this.buttonTSCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonTSCancel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.buttonTSCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonTSCancel.Location = new System.Drawing.Point(399, 186);
            this.buttonTSCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTSCancel.Name = "buttonTSCancel";
            this.buttonTSCancel.Size = new System.Drawing.Size(87, 27);
            this.buttonTSCancel.TabIndex = 8;
            this.buttonTSCancel.Text = "Cancel";
            this.buttonTSCancel.UseVisualStyleBackColor = true;
            this.buttonTSCancel.Click += new System.EventHandler(this.buttonTSCancel_Click);
            // 
            // buttonTSSave
            // 
            this.buttonTSSave.AutoSize = true;
            this.buttonTSSave.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.buttonTSSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonTSSave.Location = new System.Drawing.Point(306, 186);
            this.buttonTSSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTSSave.Name = "buttonTSSave";
            this.buttonTSSave.Size = new System.Drawing.Size(87, 27);
            this.buttonTSSave.TabIndex = 7;
            this.buttonTSSave.Text = "Save";
            this.buttonTSSave.UseVisualStyleBackColor = true;
            this.buttonTSSave.Click += new System.EventHandler(this.buttonTSSave_Click);
            // 
            // buttonTSHelp
            // 
            this.buttonTSHelp.AutoSize = true;
            this.buttonTSHelp.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.buttonTSHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonTSHelp.Location = new System.Drawing.Point(12, 186);
            this.buttonTSHelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTSHelp.Name = "buttonTSHelp";
            this.buttonTSHelp.Size = new System.Drawing.Size(87, 27);
            this.buttonTSHelp.TabIndex = 9;
            this.buttonTSHelp.Text = "Help";
            this.buttonTSHelp.UseVisualStyleBackColor = true;
            this.buttonTSHelp.Click += new System.EventHandler(this.buttonTSHelp_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageKcp);
            this.tabControlMain.Controls.Add(this.tabPageTcp);
            this.tabControlMain.Controls.Add(this.tabPageWs);
            this.tabControlMain.Controls.Add(this.tabPageHttp);
            this.tabControlMain.Controls.Add(this.tabPageMux);
            this.tabControlMain.Controls.Add(this.tabPageTLS);
            this.tabControlMain.Location = new System.Drawing.Point(12, 13);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(478, 169);
            this.tabControlMain.TabIndex = 10;
            // 
            // tabPageKcp
            // 
            this.tabPageKcp.Controls.Add(this.panel1);
            this.tabPageKcp.Location = new System.Drawing.Point(4, 26);
            this.tabPageKcp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageKcp.Name = "tabPageKcp";
            this.tabPageKcp.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageKcp.Size = new System.Drawing.Size(470, 139);
            this.tabPageKcp.TabIndex = 0;
            this.tabPageKcp.Text = "KCP";
            this.tabPageKcp.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBoxKcpCon);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.comboBoxKcpHt);
            this.panel1.Controls.Add(this.textBoxKcpWb);
            this.panel1.Controls.Add(this.textBoxKcpDc);
            this.panel1.Controls.Add(this.textBoxKcpTti);
            this.panel1.Controls.Add(this.textBoxKcpRb);
            this.panel1.Controls.Add(this.textBoxKcpUc);
            this.panel1.Controls.Add(this.textBoxKcpMtu);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(458, 126);
            this.panel1.TabIndex = 22;
            // 
            // comboBoxKcpCon
            // 
            this.comboBoxKcpCon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKcpCon.FormattingEnabled = true;
            this.comboBoxKcpCon.Items.AddRange(new object[] {
            "false",
            "true"});
            this.comboBoxKcpCon.Location = new System.Drawing.Point(113, 91);
            this.comboBoxKcpCon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxKcpCon.Name = "comboBoxKcpCon";
            this.comboBoxKcpCon.Size = new System.Drawing.Size(100, 25);
            this.comboBoxKcpCon.TabIndex = 37;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 17);
            this.label10.TabIndex = 36;
            this.label10.Text = "congestion";
            // 
            // comboBoxKcpHt
            // 
            this.comboBoxKcpHt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKcpHt.FormattingEnabled = true;
            this.comboBoxKcpHt.Items.AddRange(new object[] {
            "none",
            "srtp",
            "utp",
            "wechat-video",
            "dtls"});
            this.comboBoxKcpHt.Location = new System.Drawing.Point(346, 91);
            this.comboBoxKcpHt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxKcpHt.Name = "comboBoxKcpHt";
            this.comboBoxKcpHt.Size = new System.Drawing.Size(100, 25);
            this.comboBoxKcpHt.TabIndex = 35;
            // 
            // textBoxKcpWb
            // 
            this.textBoxKcpWb.Location = new System.Drawing.Point(346, 61);
            this.textBoxKcpWb.Name = "textBoxKcpWb";
            this.textBoxKcpWb.Size = new System.Drawing.Size(100, 23);
            this.textBoxKcpWb.TabIndex = 34;
            // 
            // textBoxKcpDc
            // 
            this.textBoxKcpDc.Location = new System.Drawing.Point(346, 32);
            this.textBoxKcpDc.Name = "textBoxKcpDc";
            this.textBoxKcpDc.Size = new System.Drawing.Size(100, 23);
            this.textBoxKcpDc.TabIndex = 33;
            // 
            // textBoxKcpTti
            // 
            this.textBoxKcpTti.Location = new System.Drawing.Point(346, 3);
            this.textBoxKcpTti.Name = "textBoxKcpTti";
            this.textBoxKcpTti.Size = new System.Drawing.Size(100, 23);
            this.textBoxKcpTti.TabIndex = 32;
            // 
            // textBoxKcpRb
            // 
            this.textBoxKcpRb.Location = new System.Drawing.Point(113, 61);
            this.textBoxKcpRb.Name = "textBoxKcpRb";
            this.textBoxKcpRb.Size = new System.Drawing.Size(100, 23);
            this.textBoxKcpRb.TabIndex = 31;
            // 
            // textBoxKcpUc
            // 
            this.textBoxKcpUc.Location = new System.Drawing.Point(113, 32);
            this.textBoxKcpUc.Name = "textBoxKcpUc";
            this.textBoxKcpUc.Size = new System.Drawing.Size(100, 23);
            this.textBoxKcpUc.TabIndex = 30;
            // 
            // textBoxKcpMtu
            // 
            this.textBoxKcpMtu.Location = new System.Drawing.Point(113, 3);
            this.textBoxKcpMtu.Name = "textBoxKcpMtu";
            this.textBoxKcpMtu.Size = new System.Drawing.Size(100, 23);
            this.textBoxKcpMtu.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(262, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 17);
            this.label8.TabIndex = 28;
            this.label8.Text = "header type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(239, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 17);
            this.label7.TabIndex = 27;
            this.label7.Text = "write buffer size";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(229, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 17);
            this.label6.TabIndex = 26;
            this.label6.Text = "downlink capacity";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(321, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 17);
            this.label5.TabIndex = 25;
            this.label5.Text = "tti";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
            this.label3.TabIndex = 24;
            this.label3.Text = "read buffer size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 17);
            this.label2.TabIndex = 23;
            this.label2.Text = "uplink capacity";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "mtu";
            // 
            // tabPageTcp
            // 
            this.tabPageTcp.Controls.Add(this.textBoxTcpHeader);
            this.tabPageTcp.Controls.Add(this.checkBoxTcpHeader);
            this.tabPageTcp.Location = new System.Drawing.Point(4, 26);
            this.tabPageTcp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageTcp.Name = "tabPageTcp";
            this.tabPageTcp.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageTcp.Size = new System.Drawing.Size(470, 139);
            this.tabPageTcp.TabIndex = 1;
            this.tabPageTcp.Text = "TCP";
            this.tabPageTcp.UseVisualStyleBackColor = true;
            // 
            // textBoxTcpHeader
            // 
            this.textBoxTcpHeader.Location = new System.Drawing.Point(19, 52);
            this.textBoxTcpHeader.Multiline = true;
            this.textBoxTcpHeader.Name = "textBoxTcpHeader";
            this.textBoxTcpHeader.Size = new System.Drawing.Size(412, 80);
            this.textBoxTcpHeader.TabIndex = 1;
            // 
            // checkBoxTcpHeader
            // 
            this.checkBoxTcpHeader.AutoSize = true;
            this.checkBoxTcpHeader.Location = new System.Drawing.Point(19, 25);
            this.checkBoxTcpHeader.Name = "checkBoxTcpHeader";
            this.checkBoxTcpHeader.Size = new System.Drawing.Size(163, 21);
            this.checkBoxTcpHeader.TabIndex = 0;
            this.checkBoxTcpHeader.Text = "Customize Http Header";
            this.checkBoxTcpHeader.UseVisualStyleBackColor = true;
            // 
            // tabPageWs
            // 
            this.tabPageWs.Controls.Add(this.label13);
            this.tabPageWs.Controls.Add(this.textBoxWsHeader);
            this.tabPageWs.Controls.Add(this.textBoxWsPath);
            this.tabPageWs.Controls.Add(this.label9);
            this.tabPageWs.Location = new System.Drawing.Point(4, 26);
            this.tabPageWs.Name = "tabPageWs";
            this.tabPageWs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWs.Size = new System.Drawing.Size(470, 139);
            this.tabPageWs.TabIndex = 2;
            this.tabPageWs.Text = "WebSocket";
            this.tabPageWs.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 51);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 17);
            this.label13.TabIndex = 11;
            this.label13.Text = "Headers:";
            // 
            // textBoxWsHeader
            // 
            this.textBoxWsHeader.Location = new System.Drawing.Point(23, 72);
            this.textBoxWsHeader.Multiline = true;
            this.textBoxWsHeader.Name = "textBoxWsHeader";
            this.textBoxWsHeader.Size = new System.Drawing.Size(412, 56);
            this.textBoxWsHeader.TabIndex = 10;
            // 
            // textBoxWsPath
            // 
            this.textBoxWsPath.Location = new System.Drawing.Point(63, 21);
            this.textBoxWsPath.Name = "textBoxWsPath";
            this.textBoxWsPath.Size = new System.Drawing.Size(376, 23);
            this.textBoxWsPath.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "path:";
            // 
            // tabPageHttp
            // 
            this.tabPageHttp.Controls.Add(this.label11);
            this.tabPageHttp.Controls.Add(this.textBoxHttp2Path);
            this.tabPageHttp.Controls.Add(this.labelHttp2Path);
            this.tabPageHttp.Controls.Add(this.textBoxHttp2Hosts);
            this.tabPageHttp.Controls.Add(this.labelHttp2Hosts);
            this.tabPageHttp.Location = new System.Drawing.Point(4, 26);
            this.tabPageHttp.Name = "tabPageHttp";
            this.tabPageHttp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHttp.Size = new System.Drawing.Size(470, 139);
            this.tabPageHttp.TabIndex = 5;
            this.tabPageHttp.Text = "HTTP/2";
            this.tabPageHttp.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(78, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(147, 17);
            this.label11.TabIndex = 4;
            this.label11.Text = "seperated by comma (,)";
            // 
            // textBoxHttp2Path
            // 
            this.textBoxHttp2Path.Location = new System.Drawing.Point(78, 84);
            this.textBoxHttp2Path.Name = "textBoxHttp2Path";
            this.textBoxHttp2Path.Size = new System.Drawing.Size(366, 23);
            this.textBoxHttp2Path.TabIndex = 3;
            // 
            // labelHttp2Path
            // 
            this.labelHttp2Path.AutoSize = true;
            this.labelHttp2Path.Location = new System.Drawing.Point(28, 87);
            this.labelHttp2Path.Name = "labelHttp2Path";
            this.labelHttp2Path.Size = new System.Drawing.Size(36, 17);
            this.labelHttp2Path.TabIndex = 2;
            this.labelHttp2Path.Text = "Path:";
            // 
            // textBoxHttp2Hosts
            // 
            this.textBoxHttp2Hosts.Location = new System.Drawing.Point(78, 25);
            this.textBoxHttp2Hosts.Name = "textBoxHttp2Hosts";
            this.textBoxHttp2Hosts.Size = new System.Drawing.Size(366, 23);
            this.textBoxHttp2Hosts.TabIndex = 1;
            // 
            // labelHttp2Hosts
            // 
            this.labelHttp2Hosts.AutoSize = true;
            this.labelHttp2Hosts.Location = new System.Drawing.Point(28, 28);
            this.labelHttp2Hosts.Name = "labelHttp2Hosts";
            this.labelHttp2Hosts.Size = new System.Drawing.Size(44, 17);
            this.labelHttp2Hosts.TabIndex = 0;
            this.labelHttp2Hosts.Text = "Hosts:";
            // 
            // tabPageMux
            // 
            this.tabPageMux.Controls.Add(this.textBoxMuxCc);
            this.tabPageMux.Controls.Add(this.label4);
            this.tabPageMux.Controls.Add(this.checkBoxMuxEnable);
            this.tabPageMux.Location = new System.Drawing.Point(4, 26);
            this.tabPageMux.Name = "tabPageMux";
            this.tabPageMux.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMux.Size = new System.Drawing.Size(470, 139);
            this.tabPageMux.TabIndex = 3;
            this.tabPageMux.Text = "Mux.Cool";
            this.tabPageMux.UseVisualStyleBackColor = true;
            // 
            // textBoxMuxCc
            // 
            this.textBoxMuxCc.Location = new System.Drawing.Point(109, 74);
            this.textBoxMuxCc.Name = "textBoxMuxCc";
            this.textBoxMuxCc.Size = new System.Drawing.Size(50, 23);
            this.textBoxMuxCc.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "concurrency:";
            // 
            // checkBoxMuxEnable
            // 
            this.checkBoxMuxEnable.AutoSize = true;
            this.checkBoxMuxEnable.Location = new System.Drawing.Point(29, 38);
            this.checkBoxMuxEnable.Name = "checkBoxMuxEnable";
            this.checkBoxMuxEnable.Size = new System.Drawing.Size(74, 21);
            this.checkBoxMuxEnable.TabIndex = 2;
            this.checkBoxMuxEnable.Text = "enabled";
            this.checkBoxMuxEnable.UseVisualStyleBackColor = true;
            // 
            // tabPageTLS
            // 
            this.tabPageTLS.Controls.Add(this.checkBoxTLSEnable);
            this.tabPageTLS.Controls.Add(this.textBoxTLSSn);
            this.tabPageTLS.Controls.Add(this.label12);
            this.tabPageTLS.Controls.Add(this.checkBoxTLSAI);
            this.tabPageTLS.Location = new System.Drawing.Point(4, 26);
            this.tabPageTLS.Name = "tabPageTLS";
            this.tabPageTLS.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTLS.Size = new System.Drawing.Size(470, 139);
            this.tabPageTLS.TabIndex = 4;
            this.tabPageTLS.Text = "TLS";
            this.tabPageTLS.UseVisualStyleBackColor = true;
            // 
            // checkBoxTLSEnable
            // 
            this.checkBoxTLSEnable.AutoSize = true;
            this.checkBoxTLSEnable.Location = new System.Drawing.Point(25, 34);
            this.checkBoxTLSEnable.Name = "checkBoxTLSEnable";
            this.checkBoxTLSEnable.Size = new System.Drawing.Size(74, 21);
            this.checkBoxTLSEnable.TabIndex = 15;
            this.checkBoxTLSEnable.Text = "enabled";
            this.checkBoxTLSEnable.UseVisualStyleBackColor = true;
            // 
            // textBoxTLSSn
            // 
            this.textBoxTLSSn.Location = new System.Drawing.Point(109, 71);
            this.textBoxTLSSn.Name = "textBoxTLSSn";
            this.textBoxTLSSn.Size = new System.Drawing.Size(175, 23);
            this.textBoxTLSSn.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 74);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 17);
            this.label12.TabIndex = 13;
            this.label12.Text = "serverName:";
            // 
            // checkBoxTLSAI
            // 
            this.checkBoxTLSAI.AutoSize = true;
            this.checkBoxTLSAI.Location = new System.Drawing.Point(132, 34);
            this.checkBoxTLSAI.Name = "checkBoxTLSAI";
            this.checkBoxTLSAI.Size = new System.Drawing.Size(106, 21);
            this.checkBoxTLSAI.TabIndex = 12;
            this.checkBoxTLSAI.Text = "allowInsecure";
            this.checkBoxTLSAI.UseVisualStyleBackColor = true;
            // 
            // buttonTsReset
            // 
            this.buttonTsReset.AutoSize = true;
            this.buttonTsReset.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.buttonTsReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonTsReset.Location = new System.Drawing.Point(213, 186);
            this.buttonTsReset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTsReset.Name = "buttonTsReset";
            this.buttonTsReset.Size = new System.Drawing.Size(87, 27);
            this.buttonTsReset.TabIndex = 11;
            this.buttonTsReset.Text = "Reset";
            this.buttonTsReset.UseVisualStyleBackColor = true;
            this.buttonTsReset.Click += new System.EventHandler(this.buttonTsReset_Click);
            // 
            // FormTransSetting
            // 
            this.AcceptButton = this.buttonTSSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonTSCancel;
            this.ClientSize = new System.Drawing.Size(488, 225);
            this.ControlBox = false;
            this.Controls.Add(this.buttonTsReset);
            this.Controls.Add(this.buttonTSSave);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.buttonTSHelp);
            this.Controls.Add(this.buttonTSCancel);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTransSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transport Settings";
            this.Load += new System.EventHandler(this.FormTransSetting_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageKcp.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageTcp.ResumeLayout(false);
            this.tabPageTcp.PerformLayout();
            this.tabPageWs.ResumeLayout(false);
            this.tabPageWs.PerformLayout();
            this.tabPageHttp.ResumeLayout(false);
            this.tabPageHttp.PerformLayout();
            this.tabPageMux.ResumeLayout(false);
            this.tabPageMux.PerformLayout();
            this.tabPageTLS.ResumeLayout(false);
            this.tabPageTLS.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTSCancel;
        private System.Windows.Forms.Button buttonTSSave;
        private System.Windows.Forms.Button buttonTSHelp;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageKcp;
        private System.Windows.Forms.TabPage tabPageTcp;
        private System.Windows.Forms.TabPage tabPageWs;
        private System.Windows.Forms.TextBox textBoxWsPath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxKcpCon;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxKcpHt;
        private System.Windows.Forms.TextBox textBoxKcpWb;
        private System.Windows.Forms.TextBox textBoxKcpDc;
        private System.Windows.Forms.TextBox textBoxKcpTti;
        private System.Windows.Forms.TextBox textBoxKcpRb;
        private System.Windows.Forms.TextBox textBoxKcpUc;
        private System.Windows.Forms.TextBox textBoxKcpMtu;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonTsReset;
        private System.Windows.Forms.TabPage tabPageMux;
        private System.Windows.Forms.TextBox textBoxMuxCc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxMuxEnable;
        private System.Windows.Forms.TabPage tabPageTLS;
        private System.Windows.Forms.CheckBox checkBoxTLSEnable;
        private System.Windows.Forms.TextBox textBoxTLSSn;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBoxTLSAI;
        private System.Windows.Forms.TabPage tabPageHttp;
        private System.Windows.Forms.TextBox textBoxTcpHeader;
        private System.Windows.Forms.CheckBox checkBoxTcpHeader;
        private System.Windows.Forms.TextBox textBoxWsHeader;
        private System.Windows.Forms.Label labelHttp2Hosts;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxHttp2Path;
        private System.Windows.Forms.Label labelHttp2Path;
        private System.Windows.Forms.TextBox textBoxHttp2Hosts;
        private System.Windows.Forms.Label label13;
    }
}