namespace AoBSigmaker
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using Binarysharp.MemoryManagement;

    public partial class Form1 : Form
    {
        #region Constructors and Destructors

        public Form1()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            var aboutform = new AboutForm();
            aboutform.Show();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            try
            {
                var fDialog = new OpenFileDialog
                                  {
                                     Title = @"Open Text File", Filter = @"TXT files|*.txt", InitialDirectory = @"C:\" 
                                  };
                if (fDialog.ShowDialog() != DialogResult.OK) return;

                this.richTextBox1.Text = string.Empty;
                var lines = File.ReadAllLines(fDialog.FileName);
                for (var i = 0; i < lines.Length; i++)
                {
                    this.richTextBox1.Text += lines[i];
                    this.richTextBox1.Text += Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Could not read file from disk. Error:" + Environment.NewLine + ex.Message);
            }
        }

        private void Button2Click(object sender, EventArgs e)
        {
            var patterns = this.richTextBox1.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var result = string.Empty;
            var sig = AoBHandler.GenerateSigFromAoBs(patterns);
            if (string.IsNullOrEmpty(sig))
            {
                return;
            }

            if (this.checkBox1.Checked)
            {
                while (sig.StartsWith("?") || sig.StartsWith(" "))
                {
                    sig = sig.TrimStart(new[] { '?', ' ' });
                }

                while (sig.EndsWith("?") || sig.EndsWith(" "))
                {
                    sig = sig.TrimEnd(new[] { '?', ' ' });
                }
            }

            switch (this.comboBox1.SelectedIndex)
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

            this.richTextBox2.Text = result;
        }

        private void Button3Click(object sender, EventArgs e)
        {
            var rtn = this.richTextBox2.Text;
            if (!string.IsNullOrEmpty(rtn))
            {
                Clipboard.SetText(rtn);
            }
        }

        private void Button4Click(object sender, EventArgs e)
        {
            var procs = ProcessHandler.GetAllProcesses();
            this.comboBox2.Items.Clear();
            foreach (var proc in procs)
            {
                if (proc == null)
                {
                    continue;
                }

                this.comboBox2.Items.Add(proc.ProcessName + "(" + proc.Id + ")");
            }

            if (procs.Any())
            {
                this.comboBox2.SelectedItem = this.comboBox2.Items[0];
            }
        }

        private void Button5Click(object sender, EventArgs e)
        {
            var pattern = Regex.Replace(this.richTextBox3.Text, @"\s+", string.Empty).Replace("??", "00");
            if (pattern.Length % 2 != 0 || pattern.ToLower().Except("abcdef0123456789").Any())
            {
                this.richTextBox4.Text = @"Invalid AoB Pattern";
                this.richTextBox5.Text = string.Empty;
                return;
            }

            var split = this.comboBox2.Text.Split(new[] { "(" }, StringSplitOptions.None);
            var procidstring = split[1].Remove(split[1].Length - 1);
            var procId = int.Parse(procidstring);
            var proc = Process.GetProcessById(procId);
            var sigscan = new AoBHandler(proc, proc.MainModule.BaseAddress, proc.MainModule.ModuleMemorySize);
            var addr = sigscan.FindPattern(pattern);
            this.richTextBox4.Text = addr.ToString("X");
            if (addr == (IntPtr)0) return;
            var memsharp = new MemorySharp(proc);
            switch (this.comboBox3.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    this.richTextBox5.Text = memsharp.Read<byte>(addr, false).ToString();
                    break;
                case 2:
                    this.richTextBox5.Text = memsharp.Read<ushort>(addr, false).ToString();
                    break;
                case 3:
                    this.richTextBox5.Text = memsharp.Read<uint>(addr, false).ToString();
                    break;
                case 4:
                    this.richTextBox5.Text = memsharp.Read<float>(addr, false).ToString(CultureInfo.InvariantCulture);
                    break;
                case 5:
                    this.richTextBox5.Text = memsharp.Read<double>(addr, false).ToString(CultureInfo.InvariantCulture);
                    break;
                case 6:
                    this.richTextBox5.Text = memsharp.ReadString(addr, false);
                    break;
                case 7:
                    this.richTextBox5.Text = memsharp.Read<IntPtr>(addr, false).ToString("X");
                    break;
            }
        }

        private void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
        {
            this.richTextBox3.Enabled = true;
            this.button5.Enabled = true;
        }

        private void Form1Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.comboBox1.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
            this.tabControl1.Selected += this.TabSelection;
        }

        private void TabSelection(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.TabIndex == 1)
            {
                if (!ProcessHandler.IsAdministrator())
                {
                    if (MessageBox.Show(
                        @"Please restart this Application as Admin", 
                        @"Elevated Permissions", 
                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }

                this.Button4Click(new object(), new EventArgs());
            }
        }

        #endregion
    }
}