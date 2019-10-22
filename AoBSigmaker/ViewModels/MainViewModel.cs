﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using AoBSigmaker.AoB;
using Microsoft.Win32;
using RFReborn.AoB;
using RFReborn.Windows.Extensions;
using RFReborn.Windows.Memory;
using Stylet;

namespace AoBSigmaker.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IAobGenerator _aobGenerator;
        private readonly IAobShortener _aobShortener;
        private readonly Func<OptionsViewModel> _getOptionsVM;
        private readonly Func<ProcessSelectorViewModel> _getProcessSelectorVM;
        private readonly IWindowManager _windowManager;

        public MainViewModel(IWindowManager windowManager, IAobGenerator aobGenerator, IAobShortener aobShortener, Func<OptionsViewModel> getOptionsVM, Func<ProcessSelectorViewModel> getProcessSelectorVM)
        {
            _aobGenerator = aobGenerator;
            _aobShortener = aobShortener;
            _getOptionsVM = getOptionsVM;
            _getProcessSelectorVM = getProcessSelectorVM;
            _windowManager = windowManager;
        }

        private string _aobInput;

        public string AobInput
        {
            get => _aobInput;
            set
            {
                if (value != _aobInput)
                {
                    _aobInput = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _aobResult;

        public string AobResult
        {
            get => _aobResult;
            set
            {
                if (value != _aobResult)
                {
                    _aobResult = value;
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
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
        //            NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
                }
            }
        }

        private string _aobScanInput;

        public string AobScanInput
        {
            get => _aobScanInput;
            set
            {
                if (value != _aobScanInput)
                {
                    _aobScanInput = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _aobScanResult;

        public string AobScanResult
        {
            get => _aobScanResult;
            set
            {
                if (value != _aobScanResult)
                {
                    _aobScanResult = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _aobScanValue;

        public string AobScanValue
        {
            get => _aobScanValue;
            set
            {
                if (value != _aobScanValue)
                {
                    _aobScanValue = value;
                    NotifyPropertyChanged();
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

            Signature sig = new Signature(generated);
            AobResult = sig.Sig;
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
            }
        }

        private MemoryType _selectedMemoryType;

        public MemoryType SelectedMemoryType
        {
            get => _selectedMemoryType;
            set
            {
                _selectedMemoryType = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<MemoryType> MyEnumTypeValues => Enum.GetValues(typeof(MemoryType)).Cast<MemoryType>();

        public void ReadSignature()
        {
            if (SelectedProcess is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(AobScanInput))
            {
                return;
            }

            Signature sig = new Signature(AobScanInput);

            using (RemoteMemory memory = new RemoteMemory(SelectedProcess))
            {
                long ptr = memory.FindSignature(sig);
                AobScanResult = ptr.ToString();
                if (ptr != -1)
                {
                    IntPtr intptr = new IntPtr(ptr);
                    string readValue;
                    switch (SelectedMemoryType)
                    {
                        case MemoryType.None:
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}