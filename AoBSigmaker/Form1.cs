namespace AoBSigmaker
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using AoBSigmaker.Helpers;

    using Process.NET;
    using Process.NET.Memory;
    using Process.NET.Patterns;

    public partial class Form1 : Form
    {
        #region Constructors and Destructors

        public Form1()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void AboutToolStripMenuItem1Click(object sender, EventArgs e)
        {
            var aboutform = new AboutForm();
            aboutform.Show();
        }

        private void button_copycb_Click(object sender, EventArgs e)
        {
            var rtn = this.richTextBox_result.Text;
            if (!string.IsNullOrEmpty(rtn))
            {
                Clipboard.SetText(rtn);
            }
        }

        private void button_gensig_Click(object sender, EventArgs e)
        {
            var patterns = this.richTextBox_input.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            for (var index = 0; index < patterns.Length; index++)
            {
                var pattern = patterns[index];
                if (AoBHelper.IsValid(pattern))
                {
                    continue;
                }
                this.richTextBox_result.Text = @"Invalid AoB Pattern";
                return;
            }
            var sig = AoBHelper.GenerateSigFromAobs(patterns, this.checkBox_halfbyte.Checked);

            if (string.IsNullOrEmpty(sig))
            {
                this.richTextBox_result.Text = @"Invalid AoB Pattern";
                return;
            }

            if (this.checkBox_shorten.Checked)
            {
                while (sig.StartsWith("?") || sig.StartsWith(" "))
                {
                    sig = sig.TrimStart('?', ' ');
                }

                while (sig.EndsWith("?") || sig.EndsWith(" "))
                {
                    sig = sig.TrimEnd('?', ' ');
                }
            }

            var result = string.Empty;
            switch (this.comboBox_returnstyle.SelectedIndex)
            {
                case 0:
                    result = sig;
                    break;
                case 1:
                    var splitThis = sig.Split(null);
                    var mask = string.Empty;
                    foreach (var by in splitThis)
                    {
                        if (by == string.Empty)
                        {
                            continue;
                        }

                        if (by == "??")
                        {
                            result += "\\x" + "00";
                            mask += "?";
                        }
                        else
                        {
                            result += "\\x" + by;
                            mask += "x";
                        }
                    }

                    result += Environment.NewLine + mask;
                    break;
            }

            this.richTextBox_result.Text = result;
        }

        private void button_loadfile_Click(object sender, EventArgs e)
        {
            try
            {
                var fDialog = new OpenFileDialog
                                  {
                                      Title = @"Open Text File", Filter = @"TXT files|*.txt",
                                      InitialDirectory = @"C:\"
                                  };
                if (fDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                this.richTextBox_input.Text = string.Empty;
                var lines = File.ReadAllLines(fDialog.FileName);
                var str = new StringBuilder();
                for (var i = 0; i < lines.Length; i++)
                {
                    str.Append(lines[i]);
                    str.Append(Environment.NewLine);
                }
                this.richTextBox_input.Text = str.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Could not read file from disk. Error:" + Environment.NewLine + ex.Message);
            }
        }

        private void button_procrefresh_Click(object sender, EventArgs e)
        {
            var procs = new List<Process>();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    var temptest = new ProcessSharp(process, MemoryType.Remote);
                    procs.Add(process);
                }
                catch (Exception)
                {
                }
            }
            this.comboBox_procs.Items.Clear();
            foreach (var proc in procs.OrderBy(x => x.StartTime.ToUniversalTime()))
            {
                if (proc == null)
                {
                    continue;
                }

                this.comboBox_procs.Items.Add(proc.ProcessName + "(" + proc.Id + ")");
            }

            if (procs.Any())
            {
                this.comboBox_procs.SelectedItem = this.comboBox_procs.Items[0];
            }
        }

        private void button_readaobaddress_Click(object sender, EventArgs e)
        {
            var aobsig = this.richTextBox_aobinput.Text;
            if (string.IsNullOrEmpty(aobsig) || string.IsNullOrWhiteSpace(aobsig) || !AoBHelper.IsValid(aobsig))
            {
                this.richTextBox_aobaddress.Text = @"Pattern not valid";
                return;
            }

            var split = this.comboBox_procs.Text.Split(new[] { "(" }, StringSplitOptions.None);
            var procidstring = split[1].Remove(split[1].Length - 1);
            var procId = int.Parse(procidstring);
            Process proc;
            try
            {
                proc = Process.GetProcessById(procId);
            }
            catch (Exception)
            {
                this.richTextBox_aobaddress.Text = @"Process not valid";
                return;
            }

            var procsharp = new ProcessSharp(proc, MemoryType.Remote);
            var sigscan = new PatternScanner(procsharp.ModuleFactory.MainModule);
            var sigres = sigscan.Find(new DwordPattern(aobsig));
            if (sigres.Found)
            {
                this.richTextBox_aobaddress.Text = sigres.ReadAddress.ToString("X");
                switch (this.comboBox_readtype.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        this.richTextBox_aobaddressvalue.Text =
                            procsharp.Memory.Read<byte>(sigres.ReadAddress).ToString();
                        break;
                    case 2:
                        this.richTextBox_aobaddressvalue.Text =
                            procsharp.Memory.Read<ushort>(sigres.ReadAddress).ToString();
                        break;
                    case 3:
                        this.richTextBox_aobaddressvalue.Text =
                            procsharp.Memory.Read<uint>(sigres.ReadAddress).ToString();
                        break;
                    case 4:
                        this.richTextBox_aobaddressvalue.Text =
                            procsharp.Memory.Read<float>(sigres.ReadAddress).ToString(CultureInfo.InvariantCulture);
                        break;
                    case 5:
                        this.richTextBox_aobaddressvalue.Text =
                            procsharp.Memory.Read<double>(sigres.ReadAddress).ToString(CultureInfo.InvariantCulture);
                        break;
                    case 6:
                        this.richTextBox_aobaddressvalue.Text = procsharp.Memory.Read(
                            sigres.ReadAddress,
                            Encoding.ASCII,
                            50);
                        break;
                    case 7:
                        this.richTextBox_aobaddressvalue.Text =
                            procsharp.Memory.Read<IntPtr>(sigres.ReadAddress).ToString("X");
                        break;
                }
            }
            else
            {
                this.richTextBox_aobaddress.Text = @"Couldn't find the pattern";
            }
        }

        private void checkBox_halfbyte_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_halfbyte.Checked)
            {
                this.comboBox_returnstyle.SelectedIndex = 0;
                this.comboBox_returnstyle.Items.RemoveAt(1);
            }
            else
            {
                this.comboBox_returnstyle.Items.Add("C++");
            }
        }

        private void CheckForUpdateToolStripMenuItemClick(object sender, EventArgs e)
        {
            var updtform = new UpdateForm();
            updtform.Show();
        }

        private void comboBox_procs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.richTextBox_aobinput.Enabled = true;
            this.button_readaobaddress.Enabled = true;
        }

        private void comboBox_returnstyle_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Form1Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.comboBox_returnstyle.SelectedIndex = 0;
            this.comboBox_readtype.SelectedIndex = 0;
            this.tabControl1.Selected += this.TabSelection;
        }

        private void TabSelection(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.TabIndex == 1)
            {
                if (!SystemHelper.IsAdministrator())
                {
                    if (MessageBox.Show(
                            @"Please restart this Application as Admin",
                            @"Elevated Permissions",
                            MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }

                if (this.comboBox_procs.Items.Count < 1)
                {
                    this.button_procrefresh_Click(this.button_procrefresh, new EventArgs());
                }
            }
        }

        #endregion
    }
}