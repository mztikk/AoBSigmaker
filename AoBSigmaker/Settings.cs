namespace AoBSigmaker
{
    public static class Settings
    {
        #region Static Fields

        private static AoBGen.FileReadMode frm = (AoBGen.FileReadMode)Properties.Settings.Default.fileReadMode;

        private static bool tv = Properties.Settings.Default.trustValidity;

        private static bool upd = Properties.Settings.Default.checkUpdates;

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
                Properties.Settings.Default.checkUpdates = value;
                Properties.Settings.Default.Save();
            }
        }

        internal static AoBGen.FileReadMode FileReadMode
        {
            get
            {
                return frm;
            }
            set
            {
                frm = value;
                Properties.Settings.Default.fileReadMode = (int)value;
                Properties.Settings.Default.Save();
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
                Properties.Settings.Default.trustValidity = value;
                Properties.Settings.Default.Save();
            }
        }

        #endregion
    }
}