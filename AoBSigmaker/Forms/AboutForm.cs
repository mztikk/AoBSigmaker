namespace AoBSigmaker
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    public partial class AboutForm : Form
    {
        #region Constructors and Destructors

        public AboutForm()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void AboutFormLoad(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.versionLabel.Text = Updater.GetAssemblyVersion();
        }

        private void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/mztikk/AoBSigmaker");
        }

        #endregion
    }
}