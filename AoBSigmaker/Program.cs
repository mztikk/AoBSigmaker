namespace AoBSigmaker
{
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        #region Properties

        internal static bool IsDebugMode { get; set; }

        internal static Mainform Mainform { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if DEBUG
            IsDebugMode = true;
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Mainform = new Mainform();
            Application.Run(Mainform);
        }

        #endregion
    }
}