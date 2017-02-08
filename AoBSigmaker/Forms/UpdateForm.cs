namespace AoBSigmaker
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    public partial class UpdateForm : Form
    {
        #region Constructors and Destructors

        public UpdateForm()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/mztikk/AoBSigmaker/releases");
        }

        private void UpdateFormLoad(object sender, EventArgs e)
        {
            var diff = Updater.IsOnlineDiff();
            var display = diff
                              ? "Your version is outdated." + Environment.NewLine
                                + "Please check github to download the latest one"
                              : "Your version is the latest one, no need to update.";
            if (diff)
            {
                this.linkLabel1.Show();
            }
            else
            {
                this.linkLabel1.Hide();
            }

            this.updateInfo.Text = display;
            this.label1.Text += Environment.NewLine + Updater.GetAssemblyVersion();
            this.label2.Text += Environment.NewLine + Updater.GetGithubVersion();
        }

        #endregion
    }
}