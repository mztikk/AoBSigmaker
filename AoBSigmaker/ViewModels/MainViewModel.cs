using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AoBSigmaker.AoB;
using AoBSigmaker.Converter;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using RFReborn.AoB;
using RFReborn.Windows;
using RFReborn.Windows.Extensions;
using RFReborn.Windows.Memory;
using Stylet;

namespace AoBSigmaker.ViewModels
{
    public class MainViewModel : Screen
    {
        private readonly IAobGenerator _aobGenerator;
        private readonly IAobShortener _aobShortener;
        private readonly Func<OptionsViewModel> _getOptionsVM;
        private readonly Func<ProcessSelectorViewModel> _getProcessSelectorVM;
        private readonly Func<ModuleSelectorViewModel> _getModuleSelectorVM;
        private readonly IWindowManager _windowManager;

        public MainViewModel(
            IWindowManager windowManager,
            IAobGenerator aobGenerator,
            IAobShortener aobShortener,
            Func<OptionsViewModel> getOptionsVM,
            Func<ProcessSelectorViewModel> getProcessSelectorVM,
            Func<ModuleSelectorViewModel> getModuleSelectorVM)
        {
            _aobGenerator = aobGenerator;
            _aobShortener = aobShortener;
            _getOptionsVM = getOptionsVM;
            _getProcessSelectorVM = getProcessSelectorVM;
            _getModuleSelectorVM = getModuleSelectorVM;
            _windowManager = windowManager;
        }

        private string? _aobInput;

        public string? AobInput
        {
            get => _aobInput;
            set
            {
                if (value != _aobInput)
                {
                    _aobInput = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private string? _aobResult;

        public string? AobResult
        {
            get => _aobResult;
            set
            {
                if (value != _aobResult)
                {
                    _aobResult = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private bool _shortenWildcards;

        public bool ShortenWildcards
        {
            get => _shortenWildcards;
            set
            {
                if (value != _shortenWildcards)
                {
                    _shortenWildcards = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        //private string _selectedProcessIdName = "None";

        //public string SelectedProcessIdName
        //{
        //    get => _selectedProcessIdName;
        //    set
        //    {
        //        if (value != _selectedProcessIdName)
        //        {
        //            _selectedProcessIdName = value;
        //            NotifyOfPropertyChange();
        //        }
        //    }
        //}

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

                    CanSelectModule = value is { };
                }
            }
        }

        private string? _aobScanInput;

        public string? AobScanInput
        {
            get => _aobScanInput;
            set
            {
                if (value != _aobScanInput)
                {
                    _aobScanInput = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private string? _aobScanResult;

        public string? AobScanResult
        {
            get => _aobScanResult;
            set
            {
                if (value != _aobScanResult)
                {
                    _aobScanResult = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private string? _aobScanValue;

        public string? AobScanValue
        {
            get => _aobScanValue;
            set
            {
                if (value != _aobScanValue)
                {
                    _aobScanValue = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private ProcessModule? _selectedModule;

        public ProcessModule? SelectedModule
        {
            get => _selectedModule;
            private set
            {
                if (value != _selectedModule)
                {
                    _selectedModule = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private bool _canSelectModule;

        public bool CanSelectModule
        {
            get => _canSelectModule;
            private set
            {
                if (value != _canSelectModule)
                {
                    _canSelectModule = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public void MakeSig()
        {
            if (string.IsNullOrWhiteSpace(AobInput))
            {
                return;
            }

            string[] aobs = AobInput.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            if (!AobValidator.AreValid(aobs))
            {
                return;
            }

            string generated = _aobGenerator.Make(aobs);

            if (ShortenWildcards)
            {
                generated = _aobShortener.Shorten(generated);
            }

            if (string.IsNullOrWhiteSpace(generated))
            {
                AobResult = string.Empty;
            }
            else
            {
                Signature sig = new Signature(generated);
                AobResult = sig.Sig;
            }
        }

        public void CopyResultToClipboard()
        {
            if (string.IsNullOrWhiteSpace(AobResult))
            {
                return;
            }

            Clipboard.SetText(AobResult);
        }

        public async Task LoadFromFile()
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFile.ShowDialog() == true)
            {
                AobInput = await File.ReadAllTextAsync(openFile.FileName).ConfigureAwait(false);
            }
        }

        public void OpenSettings() => _windowManager.ShowDialog(_getOptionsVM());

        public void SelectProcess()
        {
            ProcessSelectorViewModel processSelector = _getProcessSelectorVM();
            _windowManager.ShowDialog(processSelector);
            if (processSelector.SelectedProcess is { })
            {
                SelectedProcess = processSelector.SelectedProcess;
                SelectedModule = SelectedProcess.MainModule;
            }
        }

        public void SelectModule()
        {
            if (SelectedProcess is null)
            {
                return;
            }

            ModuleSelectorViewModel moduleSelector = _getModuleSelectorVM();
            moduleSelector.Process = SelectedProcess;
            _windowManager.ShowDialog(moduleSelector);
            if (moduleSelector.SelectedModule is { })
            {
                SelectedModule = moduleSelector.SelectedModule;
            }
        }

        private MemoryType _selectedMemoryType;

        public MemoryType SelectedMemoryType
        {
            get => _selectedMemoryType;
            set
            {
                _selectedMemoryType = value;
                NotifyOfPropertyChange();
            }
        }

        public static IEnumerable<MemoryType> MemoryTypeValues => Enum.GetValues(typeof(MemoryType)).Cast<MemoryType>();

        public async Task ReadSignature()
        {
            if (SelectedProcess is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(AobScanInput))
            {
                return;
            }

            if (!ProcessHelpers.ProcessExists(SelectedProcess))
            {
                ProcessToIdNameConverter converter = new ProcessToIdNameConverter();
                string msg = $"{converter.Convert(SelectedProcess, typeof(string), null, null)} does not exist.";
                if (View is MetroWindow v)
                {
                    await v.ShowMessageAsync("ERROR", msg).ConfigureAwait(false);
                }
                else
                {
                    _windowManager.ShowMessageBox(msg);
                }

                SelectedProcess = null;
                return;
            }

            Signature sig = new Signature(AobScanInput);

            using (RemoteMemory memory = new RemoteMemory(SelectedProcess))
            {
                long ptr = memory.FindSignature(sig, SelectedModule);
                if (ptr == -1)
                {
                    AobScanResult = "Didn't find an address for the given signature";
                }
                else
                {
                    AobScanResult = "0x" + ptr.ToString("X2");
                    IntPtr intptr = new IntPtr(ptr);
                    string readValue;
                    switch (SelectedMemoryType)
                    {
                        case MemoryType.None:
                            readValue = string.Empty
                            return;
                        case MemoryType.Byte:
                            readValue = memory.Read<byte>(intptr).ToString();
                            break;
                        case MemoryType.SByte:
                            readValue = memory.Read<sbyte>(intptr).ToString();
                            break;
                        case MemoryType.Short:
                            readValue = memory.Read<short>(intptr).ToString();
                            break;
                        case MemoryType.UShort:
                            readValue = memory.Read<ushort>(intptr).ToString();
                            break;
                        case MemoryType.Int:
                            readValue = memory.Read<int>(intptr).ToString();
                            break;
                        case MemoryType.Uint:
                            readValue = memory.Read<uint>(intptr).ToString();
                            break;
                        case MemoryType.Long:
                            readValue = memory.Read<long>(intptr).ToString();
                            break;
                        case MemoryType.ULong:
                            readValue = memory.Read<ulong>(intptr).ToString();
                            break;
                        case MemoryType.Float:
                            readValue = memory.Read<float>(intptr).ToString();
                            break;
                        case MemoryType.Double:
                            readValue = memory.Read<double>(intptr).ToString();
                            break;
                        case MemoryType.IntPtr:
                            readValue = memory.Read<IntPtr>(intptr).ToString("X");
                            break;
                        case MemoryType.String:
                            readValue = memory.ReadString(intptr);
                            break;
                        default:
                            return;
                    }

                    AobScanValue = readValue;
                }
            }
        }
    }
}
