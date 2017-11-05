namespace AoBSigmaker
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Navigation;

    /// <summary>
    ///     Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        #region Constructors and Destructors

        public UpdateWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

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

        private void UpdateWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.currentVersion.Text = Updater.GetAssemblyVersion();
            this.onlineVersion.Text = Updater.GetGithubVersion();

            this.updateText.Text = Updater.IsOnlineDiff()
                                       ? "Your version is outdated." + Environment.NewLine
                                         + "Please check github to download the latest one."
                                       : "Your version is the latest one, no need to update.";

            this.githubLink.Visibility = Updater.IsOnlineDiff() ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion
    }
}