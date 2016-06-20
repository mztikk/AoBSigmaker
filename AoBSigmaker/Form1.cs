namespace AoBPatternMaker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using AoBSigmaker;

    public partial class Form1 : Form
    {
        #region Constructors and Destructors

        public Form1()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            var patterns = this.richTextBox1.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var result = string.Empty;
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    result = AoBHandler.GenerateSigFromAoBs(patterns);
                    break;
                case 1:
                    var splitThis = AoBHandler.GenerateSigFromAoBs(patterns).Split(null);
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
                default:
                    break;
            }
            this.richTextBox2.Text = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.richTextBox2.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }

        #endregion
    }
}