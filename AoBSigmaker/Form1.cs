namespace AoBPatternMaker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

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
            var patternWorking = new List<string>();
            var lastPattern = new List<string>();
            foreach (var pattern in patterns)
            {
                var loopPattern = pattern.Split(null);
                for (var i = 0; i < loopPattern.Length; i++)
                {
                    if (i + 1 > lastPattern.Count || !lastPattern.Any() || loopPattern[i] == string.Empty)
                    {
                        continue;
                    }

                    if (i + 1 <= patternWorking.Count && patternWorking[i] == "??")
                    {
                        continue;
                    }

                    if (loopPattern[i] == lastPattern[i])
                    {
                        if (i + 1 <= patternWorking.Count)
                        {
                            patternWorking.RemoveAt(i);
                            patternWorking.Insert(i, loopPattern[i]);
                        }
                        else
                        {
                            patternWorking.Add(loopPattern[i]);
                        }
                    }

                    if (loopPattern[i] != lastPattern[i] && i + 1 > patternWorking.Count)
                    {
                        patternWorking.Add("??");
                    }

                    if (i + 1 <= patternWorking.Count)
                    {
                        if (loopPattern[i] != lastPattern[i] && loopPattern[i] != patternWorking[i])
                        {
                            patternWorking.RemoveAt(i);
                            patternWorking.Insert(i, "??");
                        }
                    }
                }

                lastPattern = loopPattern.ToList();
            }

            string rtnPattern = null;
            foreach (var by in patternWorking)
            {
                rtnPattern += by + " ";
            }

            this.richTextBox2.Text = rtnPattern;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.richTextBox2.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        #endregion
    }
}