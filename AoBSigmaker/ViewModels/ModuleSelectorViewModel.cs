using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using Stylet;

namespace AoBSigmaker.ViewModels
{
    public class ModuleSelectorViewModel : Screen
    {
        public Process Process { get; set; }

        public void MetroWindow_ContentRendered(object sender, EventArgs e) => FillModuleList();

        private BindableCollection<ProcessModule> _moduleList;

        public BindableCollection<ProcessModule> ModuleList
        {
            get => _moduleList;
            set
            {
                if (value != _moduleList)
                {
                    _moduleList = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private ProcessModule? _selectedModule;

        public ProcessModule? SelectedModule
        {
            get => _selectedModule;
            set
            {
                if (value != _selectedModule)
                {
                    _selectedModule = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public void SelectModule(IList selectedItems)
        {
            if (selectedItems.Count == 0)
            {
                return;
            }

            object item = selectedItems[0];
            if (item is ProcessModule module)
            {
                SelectedModule = module;
                RequestClose(true);
            }
        }

        public void ListViewModules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView lv)
            {
                SelectModule(lv.SelectedItems);
            }
        }

        public void FillModuleList()
        {
            if (Process is null)
            {
                return;
            }

            (ModuleList ?? (ModuleList = new BindableCollection<ProcessModule>())).Clear();

            foreach (ProcessModule module in Process.Modules)
            {
                ModuleList.Add(module);
            }
        }

        public void ListViewModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IList items = e.AddedItems;
            if (items.Count == 0)
            {
                return;
            }

            object item = items[0];
            if (item is ProcessModule module)
            {
                SelectedModule = module;
            }
        }
    }
}
