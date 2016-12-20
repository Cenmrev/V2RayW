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
            this.tabPage1 = new System.Windows.Forms.TabPage();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxTcpCr = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxWsPath = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxWsCr = new System.Windows.Forms.CheckBox();
            this.buttonTsReset = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTSCancel
            // 
            this.buttonTSCancel.AutoSize = true;
            this.buttonTSCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonTSCancel.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
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
            this.buttonTSSave.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
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
            this.buttonTSHelp.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
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
            this.tabControlMain.Controls.Add(this.tabPage1);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Controls.Add(this.tabPage3);
            this.tabControlMain.Location = new System.Drawing.Point(12, 13);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(478, 169);
            this.tabControlMain.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Size = new System.Drawing.Size(470, 139);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "KCP";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            "utp"});
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxTcpCr);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Size = new System.Drawing.Size(470, 139);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TCP";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxTcpCr
            // 
            this.checkBoxTcpCr.AutoSize = true;
            this.checkBoxTcpCr.Location = new System.Drawing.Point(49, 60);
            this.checkBoxTcpCr.Name = "checkBoxTcpCr";
            this.checkBoxTcpCr.Size = new System.Drawing.Size(126, 21);
            this.checkBoxTcpCr.TabIndex = 0;
            this.checkBoxTcpCr.Text = "connection reuse";
            this.checkBoxTcpCr.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBoxWsPath);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.checkBoxWsCr);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(470, 139);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "WebSocket";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxWsPath
            // 
            this.textBoxWsPath.Location = new System.Drawing.Point(65, 69);
            this.textBoxWsPath.Name = "textBoxWsPath";
            this.textBoxWsPath.Size = new System.Drawing.Size(376, 23);
            this.textBoxWsPath.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "path:";
            // 
            // checkBoxWsCr
            // 
            this.checkBoxWsCr.AutoSize = true;
            this.checkBoxWsCr.Location = new System.Drawing.Point(25, 28);
            this.checkBoxWsCr.Name = "checkBoxWsCr";
            this.checkBoxWsCr.Size = new System.Drawing.Size(126, 21);
            this.checkBoxWsCr.TabIndex = 1;
            this.checkBoxWsCr.Text = "connection reuse";
            this.checkBoxWsCr.UseVisualStyleBackColor = true;
            // 
            // buttonTsReset
            // 
            this.buttonTsReset.AutoSize = true;
            this.buttonTsReset.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
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
            this.ClientSize = new System.Drawing.Size(504, 226);
            this.Controls.Add(this.buttonTsReset);
            this.Controls.Add(this.buttonTSSave);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.buttonTSHelp);
            this.Controls.Add(this.buttonTSCancel);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTSCancel;
        private System.Windows.Forms.Button buttonTSSave;
        private System.Windows.Forms.Button buttonTSHelp;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox checkBoxTcpCr;
        private System.Windows.Forms.TextBox textBoxWsPath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxWsCr;
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
    }
}