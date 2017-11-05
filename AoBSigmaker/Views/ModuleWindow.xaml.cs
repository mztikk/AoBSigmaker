namespace AoBSigmaker
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    ///     Interaction logic for ModuleWindow.xaml
    /// </summary>
    public partial class ModuleWindow : Window
    {
        #region Fields

        private ProcessModule module;

        private Process proc;

        #endregion

        #region Constructors and Destructors

        public ModuleWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        public bool? SelectModule(Process selectedProcess, out ProcessModule processModule)
        {
            this.proc = selectedProcess;
            var dr = this.ShowDialog();
            if (dr.HasValue && dr.Value)
            {
                processModule = this.module;
            }
            else
            {
                processModule = null;
            }

            return dr;
        }

        #endregion

        #region Methods

        private void AllModules_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            if (this.allModules.SelectedIndex == -1)
            {
                return;
            }

            this.Select_OnClick(this.allModules, null);
        }

        private void AllModules_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void ModuleWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.proc == null || !this.proc.IsRunning())
            {
                this.Close(false);
            }
            this.Refresh_OnClick(null, null);

            this.allModules.Focus();
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            this.allModules.Items.Clear();
            foreach (ProcessModule processModule in this.proc.Modules)
            {
                this.allModules.Items.Add(processModule.ModuleName);
            }
        }

        private void Select_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (ProcessModule processModule in this.proc.Modules)
            {
                if (processModule.ModuleName == this.allModules.SelectedItem.ToString())
                {
                    this.module = processModule;
                }
            }

            this.Close(true);
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion
    }
}