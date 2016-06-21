namespace AoBPatternMaker
{
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        #region Static Fields

        internal static Form1 Mainform;

        #endregion

        #region Methods

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Mainform = new Form1();
            Application.Run(Mainform);
        }

        #endregion
    }
}