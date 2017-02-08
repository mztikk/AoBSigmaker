namespace AoBSigmaker
{
    using AoBSigmaker.Properties;

    internal static class SettingsManager
    {
        #region Static Fields

        private static FileReadMode frm = Settings.Default.FileReadMode;

        private static bool tv = Settings.Default.TrustValidity;

        private static bool upd = Settings.Default.CheckForUpdateOnStartup;

        #endregion

        #region Properties

        internal static bool CheckForUpdateOnStartup
        {
            get
            {
                return upd;
            }

            set
            {
                upd = value;
                Settings.Default.CheckForUpdateOnStartup = value;
                Settings.Default.Save();
            }
        }

        internal static FileReadMode FileReadMode
        {
            get
            {
                return frm;
            }

            set
            {
                frm = value;
                Settings.Default.FileReadMode = value;
                Settings.Default.Save();
            }
        }

        internal static bool TrustValidity
        {
            get
            {
                return tv;
            }

            set
            {
                tv = value;
                Settings.Default.TrustValidity = value;
                Settings.Default.Save();
            }
        }

        #endregion
    }
}