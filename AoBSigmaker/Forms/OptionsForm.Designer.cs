namespace AoBSigmaker
{
    partial class OptionsForm
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
            this.checkBox_trustvalidity = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_checkforupdateonstartup = new System.Windows.Forms.CheckBox();
            this.comboBox_filereadmode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox_trustvalidity
            // 
            this.checkBox_trustvalidity.AutoSize = true;
            this.checkBox_trustvalidity.Location = new System.Drawing.Point(12, 12);
            this.checkBox_trustvalidity.Name = "checkBox_trustvalidity";
            this.checkBox_trustvalidity.Size = new System.Drawing.Size(126, 17);
            this.checkBox_trustvalidity.TabIndex = 0;
            this.checkBox_trustvalidity.Text = "Trust Validity of AoBs";
            this.toolTip1.SetToolTip(this.checkBox_trustvalidity, "Performance boost, but gives false results and/or crashes if invalid aobs are put" +
        " in");
            this.checkBox_trustvalidity.UseVisualStyleBackColor = true;
            this.checkBox_trustvalidity.CheckedChanged += new System.EventHandler(this.CheckBox_Trustvalidity_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 32000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // checkBox_checkforupdateonstartup
            // 
            this.checkBox_checkforupdateonstartup.AutoSize = true;
            this.checkBox_checkforupdateonstartup.Location = new System.Drawing.Point(12, 35);
            this.checkBox_checkforupdateonstartup.Name = "checkBox_checkforupdateonstartup";
            this.checkBox_checkforupdateonstartup.Size = new System.Drawing.Size(167, 17);
            this.checkBox_checkforupdateonstartup.TabIndex = 3;
            this.checkBox_checkforupdateonstartup.Text = "Check for Updates on Startup";
            this.checkBox_checkforupdateonstartup.UseVisualStyleBackColor = true;
            this.checkBox_checkforupdateonstartup.CheckedChanged += new System.EventHandler(this.CheckBox_CheckForUpdateOnStartup_CheckedChanged);
            // 
            // comboBox_filereadmode
            // 
            this.comboBox_filereadmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_filereadmode.FormattingEnabled = true;
            this.comboBox_filereadmode.Location = new System.Drawing.Point(97, 58);
            this.comboBox_filereadmode.Name = "comboBox_filereadmode";
            this.comboBox_filereadmode.Size = new System.Drawing.Size(121, 21);
            this.comboBox_filereadmode.TabIndex = 4;
            this.toolTip1.SetToolTip(this.comboBox_filereadmode, "FullCopy is better 99% of the time, ReadLines is only for very large files(50mb+)" +
        " because it consumes very little memory");
            this.comboBox_filereadmode.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_FileReadmode_SelectionChangeCommited);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "FileReadMode:";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 143);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_filereadmode);
            this.Controls.Add(this.checkBox_checkforupdateonstartup);
            this.Controls.Add(this.checkBox_trustvalidity);
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_trustvalidity;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_checkforupdateonstartup;
        private System.Windows.Forms.ComboBox comboBox_filereadmode;
        private System.Windows.Forms.Label label1;
    }
}