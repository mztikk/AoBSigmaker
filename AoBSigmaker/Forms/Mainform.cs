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

    using AoBSigmaker.Extensions;
    using AoBSigmaker.Helpers;

    using Process.NET;
    using Process.NET.Memory;
    using Process.NET.Patterns;

    public partial class Mainform : Form
    {
        #region Constructors and Destructors

        public Mainform()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void AboutToolStripMenuItem1Click(object sender, EventArgs e)
        {
            var aboutform = new AboutForm();
            aboutform.ShowDialog(this);
        }

        private void AfterLoad(object sender, EventArgs e)
        {
            if (SettingsManager.CheckForUpdateOnStartup && Updater.IsOnlineDiff())
            {
                this.checkForUpdateToolStripMenuItem.PerformClick();
            }
        }

        private void Button_Copycb_Click(object sender, EventArgs e)
        {
            var rtn = this.textBox_result.Text;
            if (!string.IsNullOrEmpty(rtn))
            {
                Clipboard.SetText(rtn);
            }
        }

        private void Button_GenSig_Click(object sender, EventArgs e)
        {
            string sig;
            switch (SettingsManager.FileReadMode)
            {
                case FileReadMode.FullCopy:
                    var temp = this.textBox_input.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                    var patterns = SettingsManager.TrustValidity
                                       ? temp.RemoveWhitespaces()
                                       : AoBGenerator.TakeValidAoBs(temp).ToArray();

                    if (!patterns.Any())
                    {
                        this.textBox_result.Text = @"Invalid AoBs";
                        return;
                    }

                    sig = AoBGenerator.GenerateSigFromAobs(patterns, this.checkBox_halfbyte.Checked);
                    break;
                case FileReadMode.ReadLines:
                    sig = AoBGenerator.GenerateSigFromAobFile(this.textBox_input.Text, this.checkBox_halfbyte.Checked);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (string.IsNullOrEmpty(sig))
            {
                this.textBox_result.Text = @"Invalid AoBs";
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

            var result = new StringBuilder();
            switch (this.comboBox_returnstyle.SelectedIndex)
            {
                case 0:
                    result.Append(sig);
                    break;
                case 1:
                    var splitThis = sig.Split(null);
                    var mask = new StringBuilder();
                    foreach (var by in splitThis)
                    {
                        if (by == string.Empty)
                        {
                            continue;
                        }

                        if (by == "??")
                        {
                            result.Append("\\x" + "00");
                            mask.Append("?");
                        }
                        else
                        {
                            result.Append("\\x" + by);
                            mask.Append("x");
                        }
                    }

                    result.Append(Environment.NewLine + mask);
                    break;
            }

            this.textBox_result.Text = result.ToString();
        }

        private void Button_Loadfile_Click(object sender, EventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog { Title = @"Open Text File", Filter = @"TXT files|*.txt" };
                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                this.textBox_input.Clear();
                this.textBox_input.ClearUndo();

                switch (SettingsManager.FileReadMode)
                {
                    case FileReadMode.FullCopy:
                        this.textBox_input.Text = File.ReadAllText(fileDialog.FileName);
                        break;
                    case FileReadMode.ReadLines:
                        this.textBox_input.Text = fileDialog.FileName;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                fileDialog.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Could not read file from disk. Error:" + Environment.NewLine + ex.Message);
            }
        }

        private void Button_ProcRefresh_Click(object sender, EventArgs e)
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
                    // ignored
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

        private void Button_ReadAobAddress_Click(object sender, EventArgs e)
        {
            var aobsig = this.textBox_aobinput.Text;
            if (string.IsNullOrEmpty(aobsig) || string.IsNullOrWhiteSpace(aobsig) || !AoBGenerator.IsValid(aobsig))
            {
                this.textBox_aobaddress.Text = @"Pattern not valid";
                return;
            }

            var split = this.comboBox_procs.Text.Split(new[] { "(" }, StringSplitOptions.None);
            var procid_string = split[1].Remove(split[1].Length - 1);
            var procId = int.Parse(procid_string);
            Process proc;
            try
            {
                proc = Process.GetProcessById(procId);
            }
            catch (Exception)
            {
                this.textBox_aobaddress.Text = @"Process not valid";
                return;
            }

            var procsharp = new ProcessSharp(proc, MemoryType.Remote);
            var sigscan = new PatternScanner(procsharp.ModuleFactory.MainModule);
            var sigres = sigscan.Find(new DwordPattern(aobsig));
            if (sigres.Found)
            {
                this.textBox_aobaddress.Text = sigres.ReadAddress.ToString("X");

                try
                {
                    switch (this.comboBox_readtype.SelectedIndex)
                    {
                        case 0:
                            break;
                        case 1:
                            this.textBox_aobaddressvalue.Text =
                                procsharp.Memory.Read<byte>(sigres.ReadAddress).ToString();
                            break;
                        case 2:
                            this.textBox_aobaddressvalue.Text =
                                procsharp.Memory.Read<ushort>(sigres.ReadAddress).ToString();
                            break;
                        case 3:
                            this.textBox_aobaddressvalue.Text =
                                procsharp.Memory.Read<uint>(sigres.ReadAddress).ToString();
                            break;
                        case 4:
                            this.textBox_aobaddressvalue.Text =
                                procsharp.Memory.Read<float>(sigres.ReadAddress).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 5:
                            this.textBox_aobaddressvalue.Text =
                                procsharp.Memory.Read<double>(sigres.ReadAddress).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 6:
                            this.textBox_aobaddressvalue.Text = procsharp.Memory.Read(
                                sigres.ReadAddress,
                                Encoding.ASCII,
                                50);
                            break;
                        case 7:
                            this.textBox_aobaddressvalue.Text =
                                procsharp.Memory.Read<IntPtr>(sigres.ReadAddress).ToString("X");
                            break;
                    }
                }
                catch (Exception)
                {
                    this.textBox_aobaddressvalue.Text = @"Couldn't read from Address";
                }
            }
            else
            {
                this.textBox_aobaddress.Text = @"Couldn't find the pattern";
            }
        }

        private void CheckBox_Halfbyte_CheckedChanged(object sender, EventArgs e)
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
            updtform.ShowDialog(this);
        }

        private void ComboBox_Procs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox_aobinput.Enabled = true;
            this.button_readaobaddress.Enabled = true;
        }

        private void ComboBox_Returnstyle_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Form1Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.comboBox_returnstyle.SelectedIndex = 0;
            this.comboBox_readtype.SelectedIndex = 0;
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var optform = new OptionsForm();
            optform.ShowDialog(this);
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
                    this.Button_ProcRefresh_Click(this.button_procrefresh, new EventArgs());
                }
            }
        }

        #endregion
    }
}