using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AoBSigmaker.Views;
using RFReborn.Windows;
using RFReborn.Windows.Memory;
using RFReborn.Windows.Native;
using RFReborn.Windows.Native.Enums;
using Stylet;

namespace AoBSigmaker.ViewModels
{
    public class ProcessSelectorViewModel : Screen
    {
        public async Task MetroWindow_ContentRendered(object sender, EventArgs e) => await FillProcessList().ConfigureAwait(false);

        private BindableCollection<Process> _processList;

        public BindableCollection<Process> ProcessList
        {
            get => _processList;
            set
            {
                if (value != _processList)
                {
                    _processList = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private Process? _selectedProcess;

        public Process? SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                if (value != _selectedProcess)
                {
                    _selectedProcess = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public void SelectProcess(IList selectedItems)
        {
            if (selectedItems.Count == 0)
            {
                return;
            }

            object item = selectedItems[0];
            if (item is Process proc)
            {
                SelectedProcess = proc;
                RequestClose(true);
            }
        }

        public void ListViewProcesses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView lv)
            {
                SelectProcess(lv.SelectedItems);
            }
        }

        public async Task FillProcessList()
        {
            (ProcessList ?? (ProcessList = new BindableCollection<Process>())).Clear();

            await foreach (Process proc in GetValidProcesses())
            {
                await Execute.OnUIThreadAsync(() =>
                {
                    ProcessList.Add(proc);

                    // look for a better way to update width of GridView
                    if (View is ProcessSelectorView v && v.ListViewProcesses.View is GridView gv)
                    {
                        UpdateColumnWidths(gv);
                    }
                }).ConfigureAwait(false);
            }
        }

        private static IEnumerable<Process> GetAllProcesses()
        {
            foreach (Process proc in Process.GetProcesses())
            {
                yield return proc;
            }
        }

        private static async IAsyncEnumerable<Process> GetValidProcesses()
        {
            foreach (Process proc in GetAllProcesses())
            {
                if (await IsValid(proc).ConfigureAwait(false))
                {
                    yield return proc;
                }
            }
        }

        private static async Task<bool> IsValid(Process proc)
        {
            Task<bool> tsk = Task.Run(() =>
            {
                if (ProcessHelpers.ProcessExists(proc))
                {
                    RemoteMemory temp = null;
                    try
                    {
                        temp = new RemoteMemory(proc);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        temp?.Dispose();
                    }
                }

                return false;
            });

            return await tsk.ConfigureAwait(false);
        }

        public void ListViewProcesses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IList items = e.AddedItems;
            if (items.Count == 0)
            {
                return;
            }

            object item = items[0];
            if (item is Process proc)
            {
                SelectedProcess = proc;
            }
        }

        private static void UpdateColumnWidths(GridView gridView)
        {
            // For each column...
            foreach (GridViewColumn column in gridView.Columns)
            {
                // If this is an "auto width" column...
                if (double.IsNaN(column.Width))
                {
                    // Set its Width back to NaN to auto-size again
                    column.Width = 0;
                    column.Width = double.NaN;
                }
            }
        }

        //private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
