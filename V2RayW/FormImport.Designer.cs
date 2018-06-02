namespace V2RayW
{
    partial class FormImport
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
            this.listBoxCusConfig = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonFinish = new System.Windows.Forms.Button();
            this.dialog = new System.Windows.Forms.OpenFileDialog();
            this.labelIndicator = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxCusConfig
            // 
            this.listBoxCusConfig.FormattingEnabled = true;
            this.listBoxCusConfig.ItemHeight = 20;
            this.listBoxCusConfig.Location = new System.Drawing.Point(12, 13);
            this.listBoxCusConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBoxCusConfig.Name = "listBoxCusConfig";
            this.listBoxCusConfig.Size = new System.Drawing.Size(500, 144);
            this.listBoxCusConfig.TabIndex = 0;
            this.listBoxCusConfig.SelectedValueChanged += new System.EventHandler(this.listBoxCusConfig_SelectedValueChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(12, 177);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(85, 30);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(103, 177);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(85, 30);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonFinish
            // 
            this.buttonFinish.Location = new System.Drawing.Point(394, 177);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(60, 30);
            this.buttonFinish.TabIndex = 3;
            this.buttonFinish.Text = "Save";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // dialog
            // 
            this.dialog.FileName = "config.json";
            this.dialog.Filter = "Json|*.json";
            this.dialog.RestoreDirectory = true;
            this.dialog.Title = "Choose config.json";
            // 
            // labelIndicator
            // 
            this.labelIndicator.AutoSize = true;
            this.labelIndicator.Location = new System.Drawing.Point(289, 182);
            this.labelIndicator.Name = "labelIndicator";
            this.labelIndicator.Size = new System.Drawing.Size(53, 20);
            this.labelIndicator.TabIndex = 4;
            this.labelIndicator.Text = "label1";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(460, 177);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(60, 30);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 213);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelIndicator);
            this.Controls.Add(this.buttonFinish);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listBoxCusConfig);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import customized config.json";
            this.Load += new System.EventHandler(this.FormImport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxCusConfig;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonFinish;
        private System.Windows.Forms.OpenFileDialog dialog;
        private System.Windows.Forms.Label labelIndicator;
        private System.Windows.Forms.Button buttonClose;
    }
}