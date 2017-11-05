namespace AoBSigmaker
{
    using System.Windows;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constructors and Destructors

        public App()
        {
#if DEBUG
            IsDebugMode = true;
#endif
        }

        #endregion

        #region Public Properties

        public static bool IsDebugMode { get; private set; }

        #endregion
    }
}