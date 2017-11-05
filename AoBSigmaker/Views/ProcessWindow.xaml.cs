namespace AoBSigmaker
{
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    ///     Interaction logic for ProcessWindow.xaml
    /// </summary>
    public partial class ProcessWindow : Window
    {
        #region Fields

        private Process proc;

        #endregion

        #region Constructors and Destructors

        public ProcessWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        public bool? SelectProcess(out Process process, Process selectedProcess = null)
        {
            this.proc = selectedProcess;
            var dr = this.ShowDialog();
            if (dr.HasValue && dr.Value)
            {
                process = this.proc;
            }
            else
            {
                process = null;
            }

            return dr;
        }

        #endregion

        #region Methods

        private void AllProcs_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            if (this.allProcs.SelectedIndex == -1)
            {
                return;
            }

            this.Select_OnClick(this.allProcs, null);
        }

        private void AllProcs_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Select_OnClick(null, null);
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close(false);
        }

        private void Close(bool dialogResult)
        {
            this.DialogResult = dialogResult;
            this.Close();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close(false);
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.MaximizeClick(this, null);
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ProcessWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Refresh_OnClick(null, null);
            if (this.proc != null && this.proc.IsRunning())
            {
                this.allProcs.SelectedItem = this.proc.ToNameId();
            }

            this.allProcs.Focus();
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            this.allProcs.Items.Clear();

            foreach (var process in Processes.GetAllProcs().OrderByDescending(x => x.StartTime))
            {
                this.allProcs.Items.Add(process.ToNameId());
            }
        }

        private void Select_OnClick(object sender, RoutedEventArgs e)
        {
            this.proc = Processes.NameIdToProc(this.allProcs.SelectedItem.ToString());

            this.Close(true);
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion
    }
}