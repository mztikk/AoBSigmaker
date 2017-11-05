namespace AoBSigmaker
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    ///     Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        #region Constructors and Destructors

        public OptionsWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void CheckUpdates_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!(sender is CheckBox cb))
            {
                return;
            }

            Settings.CheckForUpdateOnStartup = cb.IsChecked.Value;
        }

        private void CheckUpdates_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (!(sender is CheckBox cb))
            {
                return;
            }

            Settings.CheckForUpdateOnStartup = cb.IsChecked.Value;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.MaximizeClick(this, null);
        }

        private void FileReadMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox cb))
            {
                return;
            }

            Settings.FileReadMode = (AoBGen.FileReadMode)cb.SelectedIndex;
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OptionsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.trustValidity.IsChecked = Settings.TrustValidity;
            this.checkUpdates.IsChecked = Settings.CheckForUpdateOnStartup;
            this.fileReadMode.SelectedIndex = (int)Settings.FileReadMode;

            this.fileReadMode.SelectionChanged += this.FileReadMode_OnSelectionChanged;
        }

        private void TrustValidity_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!(sender is CheckBox cb))
            {
                return;
            }

            Settings.TrustValidity = cb.IsChecked.Value;
        }

        private void TrustValidity_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (!(sender is CheckBox cb))
            {
                return;
            }

            Settings.TrustValidity = cb.IsChecked.Value;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion
    }
}