namespace AoBSigmaker
{
    partial class Mainform
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.textBox_input = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_loadfile = new System.Windows.Forms.Button();
            this.button_gensig = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.button_copycb = new System.Windows.Forms.Button();
            this.comboBox_returnstyle = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox_halfbyte = new System.Windows.Forms.CheckBox();
            this.checkBox_shorten = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_aobaddressvalue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_readtype = new System.Windows.Forms.ComboBox();
            this.textBox_aobaddress = new System.Windows.Forms.TextBox();
            this.button_readaobaddress = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_aobinput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_procrefresh = new System.Windows.Forms.Button();
            this.comboBox_procs = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_input
            // 
            this.textBox_input.Location = new System.Drawing.Point(6, 19);
            this.textBox_input.Multiline = true;
            this.textBox_input.Name = "textBox_input";
            this.textBox_input.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_input.Size = new System.Drawing.Size(495, 96);
            this.textBox_input.TabIndex = 0;
            this.textBox_input.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "AoBs:";
            // 
            // button_loadfile
            // 
            this.button_loadfile.Location = new System.Drawing.Point(509, 19);
            this.button_loadfile.Name = "button_loadfile";
            this.button_loadfile.Size = new System.Drawing.Size(79, 45);
            this.button_loadfile.TabIndex = 2;
            this.button_loadfile.Text = "Load from File";
            this.button_loadfile.UseVisualStyleBackColor = true;
            this.button_loadfile.Click += new System.EventHandler(this.Button_Loadfile_Click);
            // 
            // button_gensig
            // 
            this.button_gensig.Location = new System.Drawing.Point(509, 70);
            this.button_gensig.Name = "button_gensig";
            this.button_gensig.Size = new System.Drawing.Size(79, 45);
            this.button_gensig.TabIndex = 3;
            this.button_gensig.Text = "Generate Sig";
            this.button_gensig.UseVisualStyleBackColor = true;
            this.button_gensig.Click += new System.EventHandler(this.Button_GenSig_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Result:";
            // 
            // textBox_result
            // 
            this.textBox_result.Location = new System.Drawing.Point(9, 147);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ReadOnly = true;
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(492, 72);
            this.textBox_result.TabIndex = 5;
            this.textBox_result.WordWrap = false;
            // 
            // button_copycb
            // 
            this.button_copycb.Location = new System.Drawing.Point(509, 174);
            this.button_copycb.Name = "button_copycb";
            this.button_copycb.Size = new System.Drawing.Size(79, 45);
            this.button_copycb.TabIndex = 6;
            this.button_copycb.Text = "Copy to Clipboard";
            this.button_copycb.UseVisualStyleBackColor = true;
            this.button_copycb.Click += new System.EventHandler(this.Button_Copycb_Click);
            // 
            // comboBox_returnstyle
            // 
            this.comboBox_returnstyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_returnstyle.FormattingEnabled = true;
            this.comboBox_returnstyle.Items.AddRange(new object[] {
            "Cheat Engine",
            "C++"});
            this.comboBox_returnstyle.Location = new System.Drawing.Point(474, 120);
            this.comboBox_returnstyle.Name = "comboBox_returnstyle";
            this.comboBox_returnstyle.Size = new System.Drawing.Size(112, 21);
            this.comboBox_returnstyle.TabIndex = 7;
            this.comboBox_returnstyle.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Returnstyle_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(402, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Return style:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(604, 257);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 9;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabSelection);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox_halfbyte);
            this.tabPage1.Controls.Add(this.button_loadfile);
            this.tabPage1.Controls.Add(this.checkBox_shorten);
            this.tabPage1.Controls.Add(this.button_copycb);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.comboBox_returnstyle);
            this.tabPage1.Controls.Add(this.textBox_result);
            this.tabPage1.Controls.Add(this.button_gensig);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBox_input);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(596, 231);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sigmaker";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_halfbyte
            // 
            this.checkBox_halfbyte.AutoSize = true;
            this.checkBox_halfbyte.Location = new System.Drawing.Point(172, 122);
            this.checkBox_halfbyte.Name = "checkBox_halfbyte";
            this.checkBox_halfbyte.Size = new System.Drawing.Size(108, 17);
            this.checkBox_halfbyte.TabIndex = 10;
            this.checkBox_halfbyte.Text = "Halfbyte Masking";
            this.checkBox_halfbyte.UseVisualStyleBackColor = true;
            this.checkBox_halfbyte.CheckedChanged += new System.EventHandler(this.CheckBox_Halfbyte_CheckedChanged);
            // 
            // checkBox_shorten
            // 
            this.checkBox_shorten.AutoSize = true;
            this.checkBox_shorten.Location = new System.Drawing.Point(286, 122);
            this.checkBox_shorten.Name = "checkBox_shorten";
            this.checkBox_shorten.Size = new System.Drawing.Size(110, 17);
            this.checkBox_shorten.TabIndex = 9;
            this.checkBox_shorten.Text = "Shorten wildcards";
            this.checkBox_shorten.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.textBox_aobaddressvalue);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.comboBox_readtype);
            this.tabPage2.Controls.Add(this.textBox_aobaddress);
            this.tabPage2.Controls.Add(this.button_readaobaddress);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.textBox_aobinput);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.button_procrefresh);
            this.tabPage2.Controls.Add(this.comboBox_procs);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(596, 231);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "AoB Scan";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(161, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Value:";
            // 
            // textBox_aobaddressvalue
            // 
            this.textBox_aobaddressvalue.Location = new System.Drawing.Point(204, 156);
            this.textBox_aobaddressvalue.Name = "textBox_aobaddressvalue";
            this.textBox_aobaddressvalue.ReadOnly = true;
            this.textBox_aobaddressvalue.Size = new System.Drawing.Size(145, 20);
            this.textBox_aobaddressvalue.TabIndex = 10;
            this.textBox_aobaddressvalue.WordWrap = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(355, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Type:";
            // 
            // comboBox_readtype
            // 
            this.comboBox_readtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_readtype.FormattingEnabled = true;
            this.comboBox_readtype.Items.AddRange(new object[] {
            "Don\'t read Value",
            "Byte",
            "2 Bytes",
            "4 Bytes",
            "Float",
            "Double",
            "String",
            "IntPtr"});
            this.comboBox_readtype.Location = new System.Drawing.Point(395, 127);
            this.comboBox_readtype.Name = "comboBox_readtype";
            this.comboBox_readtype.Size = new System.Drawing.Size(137, 21);
            this.comboBox_readtype.TabIndex = 8;
            // 
            // textBox_aobaddress
            // 
            this.textBox_aobaddress.Location = new System.Drawing.Point(204, 123);
            this.textBox_aobaddress.Name = "textBox_aobaddress";
            this.textBox_aobaddress.ReadOnly = true;
            this.textBox_aobaddress.Size = new System.Drawing.Size(145, 20);
            this.textBox_aobaddress.TabIndex = 7;
            this.textBox_aobaddress.WordWrap = false;
            // 
            // button_readaobaddress
            // 
            this.button_readaobaddress.Enabled = false;
            this.button_readaobaddress.Location = new System.Drawing.Point(11, 123);
            this.button_readaobaddress.Name = "button_readaobaddress";
            this.button_readaobaddress.Size = new System.Drawing.Size(98, 27);
            this.button_readaobaddress.TabIndex = 6;
            this.button_readaobaddress.Text = "Read";
            this.button_readaobaddress.UseVisualStyleBackColor = true;
            this.button_readaobaddress.Click += new System.EventHandler(this.Button_ReadAobAddress_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(115, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Address of AoB:";
            // 
            // textBox_aobinput
            // 
            this.textBox_aobinput.Enabled = false;
            this.textBox_aobinput.Location = new System.Drawing.Point(11, 82);
            this.textBox_aobinput.Name = "textBox_aobinput";
            this.textBox_aobinput.Size = new System.Drawing.Size(535, 20);
            this.textBox_aobinput.TabIndex = 4;
            this.textBox_aobinput.WordWrap = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "AoB:";
            // 
            // button_procrefresh
            // 
            this.button_procrefresh.Location = new System.Drawing.Point(308, 3);
            this.button_procrefresh.Name = "button_procrefresh";
            this.button_procrefresh.Size = new System.Drawing.Size(117, 30);
            this.button_procrefresh.TabIndex = 2;
            this.button_procrefresh.Text = "Refresh";
            this.button_procrefresh.UseVisualStyleBackColor = true;
            this.button_procrefresh.Click += new System.EventHandler(this.Button_ProcRefresh_Click);
            // 
            // comboBox_procs
            // 
            this.comboBox_procs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_procs.FormattingEnabled = true;
            this.comboBox_procs.Location = new System.Drawing.Point(62, 6);
            this.comboBox_procs.Name = "comboBox_procs";
            this.comboBox_procs.Size = new System.Drawing.Size(240, 21);
            this.comboBox_procs.TabIndex = 1;
            this.comboBox_procs.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Procs_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Process:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(604, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1,
            this.checkForUpdateToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.AboutToolStripMenuItem1Click);
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.checkForUpdateToolStripMenuItem.Text = "Check for Update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.CheckForUpdateToolStripMenuItemClick);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 281);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Mainform";
            this.Text = "AoB Sigmaker";
            this.Load += new System.EventHandler(this.Form1Load);
            this.Shown += new System.EventHandler(this.AfterLoad);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_input;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_loadfile;
        private System.Windows.Forms.Button button_gensig;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Button button_copycb;
        private System.Windows.Forms.ComboBox comboBox_returnstyle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_procrefresh;
        private System.Windows.Forms.ComboBox comboBox_procs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_aobinput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_aobaddress;
        private System.Windows.Forms.Button button_readaobaddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_aobaddressvalue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_readtype;
        private System.Windows.Forms.CheckBox checkBox_shorten;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox_halfbyte;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    }
}

