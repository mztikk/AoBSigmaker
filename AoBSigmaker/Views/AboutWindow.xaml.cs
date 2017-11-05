namespace AoBSigmaker
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Navigation;

    /// <summary>
    ///     Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        #region Constructors and Destructors

        public AboutWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void AboutWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.versionText.Text += Updater.GetAssemblyVersion();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.MaximizeClick(this, null);
        }

        private void Github_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start("https://github.com/mztikk/AoBSigmaker");
            e.Handled = true;
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion
    }
}