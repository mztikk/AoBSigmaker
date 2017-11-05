namespace AoBSigmaker
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private byte[] bytes;

        private Process proc;

        private ProcessModule procModule;

        #endregion

        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow { Owner = this };
            about.ShowDialog();
        }

        private void CheckUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            var update = new UpdateWindow { Owner = this };
            update.ShowDialog();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.MaximizeClick(this, null);
        }

        private void CopyClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.aobResult.Text))
            {
                return;
            }

            Clipboard.SetText(this.aobResult.Text);
        }

        private void Generate_OnClick(object sender, RoutedEventArgs e)
        {
            var temp = this.userInput.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var res = AoBGen.Generate(
                temp,
                this.halfByte.IsChecked.Value,
                this.shortenWildcards.IsChecked.Value,
                (AoBGen.ReturnStyle)this.returnStyle.SelectedIndex);

            this.aobResult.Text = string.IsNullOrWhiteSpace(res) ? "No valid AoBs" : res;
        }

        private void LoadFile_OnClick(object sender, RoutedEventArgs e)
        {
            var file = Files.ChooseFile();
            if (string.IsNullOrWhiteSpace(file))
            {
                return;
            }

            var res = Files.ReadFile(file);
            if (string.IsNullOrWhiteSpace(res))
            {
                return;
            }

            this.userInput.Text = res;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Settings.CheckForUpdateOnStartup && Updater.IsOnlineDiff())
            {
                this.CheckUpdate_OnClick(null, null);
            }
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ModuleSelector_OnClick(object sender, RoutedEventArgs e)
        {
            var modules = new ModuleWindow { Owner = this };
            var returned = modules.SelectModule(this.proc, out var procModule);
            if (returned.Value)
            {
                if (procModule != null)
                {
                    this.procModule = procModule;
                }
                this.selectedModuleText.Text = procModule?.ModuleName;
            }
        }

        private void Options_OnClick(object sender, RoutedEventArgs e)
        {
            var options = new OptionsWindow { Owner = this };
            options.ShowDialog();
        }

        private void ProcessSelector_OnClick(object sender, RoutedEventArgs e)
        {
            var procs = new ProcessWindow { Owner = this };
            Process proc;
            var returned = procs.SelectProcess(out proc, this.proc);
            if (returned.Value)
            {
                if (proc != null)
                {
                    this.proc = proc;
                    this.procModule = this.proc.MainModule;
                    this.selectedModuleText.Text = this.procModule.ModuleName;
                    this.moduleSelection.Visibility = Visibility.Visible;
                }

                this.selectedProcText.Text = proc.ToNameId();
            }
        }

        private void ReadSig_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.proc == null || !this.proc.IsRunning())
            {
                this.aobAddress.Text = "Invalid Process";
                return;
            }

            if (string.IsNullOrWhiteSpace(this.aobSigScan.Text) || !AoBGen.IsValid(this.aobSigScan.Text))
            {
                this.aobAddress.Text = "Invalid AoB Sig";
                return;
            }

            RemoteMemory remoteMem = null;

            try
            {
                remoteMem = new RemoteMemory(this.proc);

                var res = remoteMem.FindPatternInModule(this.aobSigScan.Text, 0, this.procModule);
                if (res == IntPtr.Zero)
                {
                    this.aobAddress.Text = "Couldn't find the pattern";
                }

                this.aobAddress.Text = res.ToString("X");
                var readValue = string.Empty;

                try
                {
                    switch (this.valueType.SelectedIndex)
                    {
                        case 0:
                            break;
                        case 1:
                            readValue = remoteMem.Read<byte>(res).ToString();
                            break;
                        case 2:
                            readValue = remoteMem.Read<ushort>(res).ToString();
                            break;
                        case 3:
                            readValue = remoteMem.Read<short>(res).ToString();
                            break;
                        case 4:
                            readValue = remoteMem.Read<uint>(res).ToString();
                            break;
                        case 5:
                            readValue = remoteMem.Read<int>(res).ToString();
                            break;
                        case 6:
                            readValue = remoteMem.Read<ulong>(res).ToString();
                            break;
                        case 7:
                            readValue = remoteMem.Read<long>(res).ToString();
                            break;
                        case 8:
                            readValue = remoteMem.Read<float>(res).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 9:
                            readValue = remoteMem.Read<double>(res).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 10:
                            readValue = remoteMem.ReadString(res, Encoding.ASCII, 50);
                            break;
                        case 11:
                            readValue = remoteMem.Read<IntPtr>(res).ToString("X");
                            break;
                        default:
                            break;
                    }
                    this.aobValue.Text = readValue;
                }
                catch (Exception exception)
                {
                    this.aobValue.Text = "Couldn't read from Address";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                remoteMem?.Dispose();
            }
        }

        private void TabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl tc))
            {
                return;
            }

            if (tc.SelectedIndex == 1)
            {
                if (!Processes.IsAdministrator())
                {
                    var res = MessageBox.Show(
                        "We need admin rights to access processes. Restart as Admin?",
                        "Elevated Permissions",
                        MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        var p = new Process();
                        p.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                        p.StartInfo.Verb = "runas";
                        p.Start();
                        this.CloseClick(this, null);
                    }
                }
            }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow?.DragMove();
        }

        #endregion
    }
}