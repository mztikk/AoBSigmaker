namespace AoBSigmaker
{
    using System;
    using System.Windows.Forms;

    public partial class OptionsForm : Form
    {
        #region Constructors and Destructors

        public OptionsForm()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void CheckBox_CheckForUpdateOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.CheckForUpdateOnStartup = this.checkBox_checkforupdateonstartup.Checked;
        }

        private void CheckBox_Trustvalidity_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.TrustValidity = this.checkBox_trustvalidity.Checked;
        }

        private void ComboBox_FileReadmode_SelectionChangeCommited(object sender, EventArgs e)
        {
            FileReadMode res;
            if (Enum.TryParse(this.comboBox_filereadmode.SelectedItem.ToString(), out res))
            {
                SettingsManager.FileReadMode = res;
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.checkBox_trustvalidity.Checked = SettingsManager.TrustValidity;
            this.checkBox_checkforupdateonstartup.Checked = SettingsManager.CheckForUpdateOnStartup;
            this.comboBox_filereadmode.DataSource = Enum.GetValues(typeof(FileReadMode));
            this.comboBox_filereadmode.SelectedItem = SettingsManager.FileReadMode;
        }

        #endregion
    }
}