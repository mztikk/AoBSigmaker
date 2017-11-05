namespace AoBSigmaker
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    using Microsoft.Win32.SafeHandles;

    public class RemoteMemory : IDisposable
    {
        #region Static Fields

        private static readonly Type IntPtrType = typeof(IntPtr);

        private static readonly bool Self64 = IntPtr.Size == 8;

        #endregion

        #region Fields

        public readonly Process NativeProcess;

        public readonly Handle ProcessHandle;

        private readonly IntPtr dumpAddress;

        private readonly int dumpSize;

        private byte[] dumpRegion;

        #endregion

        #region Constructors and Destructors

        public RemoteMemory(Process proc)
        {
            if (!IsValid(proc))
            {
                throw new ArgumentException();
            }
            this.NativeProcess = proc;
            this.ProcessHandle = OpenProcess(ProcessAccessFlags.AllAccess, this.NativeProcess.Id);
            this.dumpAddress = this.MainModule.BaseAddress;
            this.dumpSize = this.MainModule.ModuleMemorySize;
            this.Is64 = Is64Bit(this.NativeProcess);
        }

        ~RemoteMemory()
        {
            this.Dispose();
        }

        #endregion

        #region Enums

        [Flags]
        public enum MemoryProtection : uint
        {
            Execute = 0x10,

            ExecuteRead = 0x20,

            ExecuteReadWrite = 0x40,

            ExecuteWriteCopy = 0x80,

            NoAccess = 0x01,

            ReadOnly = 0x02,

            ReadWrite = 0x04,

            WriteCopy = 0x08,

            GuardModifierflag = 0x100,

            NoCacheModifierflag = 0x200,

            WriteCombineModifierflag = 0x400
        }

        [Flags]
        public enum ProcessAccessFlags
        {
            AllAccess = 0x1F0FFF,

            CreateProcess = 0x80,

            CreateThread = 0x2,

            DupHandle = 0x40,

            QueryInformation = 0x400,

            QueryLimitedInformation = 0x1000,

            SetInformation = 0x200,

            SetQuota = 0x100,

            SuspendResume = 0x800,

            Synchronize = 0x100000,

            Terminate = 0x1,

            VmOperation = 0x8,

            VmRead = 0x10,

            VmWrite = 0x20
        }

        #endregion

        #region Public Properties

        public ProcessModule MainModule
        {
            get
            {
                return this.NativeProcess.MainModule;
            }
        }

        public ProcessModuleCollection Modules
        {
            get
            {
                return this.NativeProcess.Modules;
            }
        }

        #endregion

        #region Properties

        private bool Is64 { get; }

        #endregion

        #region Public Methods and Operators

        public static (byte[], string) GetBytesAndMaskFromPattern(string pattern)
        {
            var split = pattern.Split(' ');
            var bytes = new byte[split.Length];
            var mask = new char[split.Length];
            for (var i = 0; i < split.Length; i++)
            {
                if (split[i][0] == '?' || split[i][1] == '?')
                {
                    bytes[i] = 0;
                    mask[i] = '?';
                }
                else
                {
                    bytes[i] = Convert.ToByte(new string(new char[2] { split[i][0], split[i][1] }), 16);
                    mask[i] = 'x';
                }
            }

            return (bytes, new string(mask));
        }

        public static byte[] GetBytesFromPattern(string pattern)
        {
            var split = pattern.Split(' ');
            var rtn = new byte[split.Length];
            for (var i = 0; i < split.Length; i++)
            {
                if (split[i][0] == '?' || split[i][1] == '?')
                {
                    rtn[i] = 0;
                }
                else
                {
                    rtn[i] = Convert.ToByte(new string(new char[2] { split[i][0], split[i][1] }), 16);
                }
            }

            return rtn;
        }

        public static string GetMaskFromPattern(string pattern)
        {
            var split = pattern.Split(' ');
            var rtn = new char[split.Length];
            for (var i = 0; i < split.Length; i++)
            {
                if (split[i][0] == '?' || split[i][1] == '?')
                {
                    rtn[i] = '?';
                }
                else
                {
                    rtn[i] = 'x';
                }
            }
            return new string(rtn);
        }

        public static bool Is64Bit(Process proc)
        {
            IsWow64Process(proc.Handle, out var iswow64);
            return !iswow64 && Environment.Is64BitOperatingSystem;
        }

        public static bool IsValid(Process process)
        {
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            try
            {
                Process.GetProcessById(process.Id);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            this.dumpRegion = null;
            this.ProcessHandle?.Close();
        }

        public IntPtr FindPatternInModule(byte[] btPattern, string strMask, int nOffset, ProcessModule procModule)
        {
            if (strMask.Length != btPattern.Length || btPattern.Length < 1)
            {
                throw new ArgumentException("length does not match");
            }

            try
            {
                var first = btPattern[strMask.IndexOf('x')];
                if (!this.DumpMemory(procModule.BaseAddress, procModule.ModuleMemorySize))
                {
                    return IntPtr.Zero;
                }

                for (var i = 0; i < this.dumpRegion.Length - btPattern.Length; i++)
                {
                    if (this.dumpRegion[i] != first)
                    {
                        continue;
                    }

                    if (this.CheckMask(i, btPattern, strMask))
                    {
                        return new IntPtr(procModule.BaseAddress.ToInt64()) + i + nOffset;
                    }
                }

                return IntPtr.Zero;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
            finally
            {
                this.dumpRegion = null;
            }
        }

        public IntPtr FindPatternInModule(string strPattern, int nOffset, ProcessModule procModule)
        {
            var (bytes, mask) = GetBytesAndMaskFromPattern(strPattern);
            return this.FindPatternInModule(bytes, mask, nOffset, procModule);
        }

        public T Read<T>(IntPtr address, bool relative = false)
        {
            int size;

            if (Self64 && !this.Is64 && typeof(T) == IntPtrType)
            {
                size = 4;
            }
            else
            {
                size = MarshalType<T>.Size;
            }
            return MarshalType<T>.ByteArrayToObject(this.ReadBytes(address, size, relative));
        }

        public T[] Read<T>(IntPtr address, int count, bool relative = false)
        {
            var array = new T[count];

            var bytes = this.ReadBytes(address, MarshalType<T>.Size * count, relative);

            if (typeof(T) != typeof(byte))
            {
                for (var i = 0; i < count; i++)
                {
                    array[i] = MarshalType<T>.ByteArrayToObject(bytes);
                }
            }
            else
            {
                Buffer.BlockCopy(bytes, 0, array, 0, count);
            }

            return array;
        }

        public string ReadString(IntPtr address, Encoding encoding, int maxLength = 512, bool relative = false)
        {
            var data = encoding.GetString(this.ReadBytes(address, maxLength, relative));

            var endOfStringPosition = data.IndexOf('\0');

            return endOfStringPosition == -1 ? data : data.Substring(0, endOfStringPosition);
        }

        public string ReadString(IntPtr address, int maxLength = 512, bool relative = false)
        {
            return this.ReadString(address, Encoding.UTF8, maxLength, relative);
        }

        public IntPtr ToAbsolute(IntPtr address)
        {
            if (address.ToInt64() > this.MainModule.ModuleMemorySize)
            {
                throw new ArgumentOutOfRangeException(nameof(address), @"relative address greater than main module size");
            }

            return new IntPtr(this.MainModule.BaseAddress.ToInt64() + address.ToInt64());
        }

        public void Write<T>(IntPtr address, T value, bool relative = false)
        {
            this.WriteBytes(address, MarshalType<T>.ObjectToByteArray(value), relative);
        }

        public void Write<T>(IntPtr address, T[] array, bool relative = false)
        {
            var bytes = new byte[MarshalType<T>.Size * array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                var offset = MarshalType<T>.Size * i;
                Buffer.BlockCopy(MarshalType<T>.ObjectToByteArray(array[i]), 0, bytes, offset, MarshalType<T>.Size);
            }

            this.WriteBytes(address, bytes, relative);
        }

        public void WriteString(IntPtr address, string text, Encoding encoding, bool relative = false)
        {
            this.WriteBytes(address, encoding.GetBytes(text + '\0'), relative);
        }

        public void WriteString(IntPtr address, string text, bool relative = false)
        {
            this.WriteString(address, text, Encoding.UTF8, relative);
        }

        #endregion

        #region Methods

        private static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(IntPtr process, out bool wow64Process);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(byte[] b1, byte[] b2, long count);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Handle OpenProcess(
            ProcessAccessFlags dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
            int dwProcessId);

        private static Handle OpenProcess(ProcessAccessFlags accessFlags, int processId)
        {
            var handle = OpenProcess(accessFlags, false, processId);

            if (!handle.IsInvalid && !handle.IsClosed)
            {
                return handle;
            }

            throw new Win32Exception($"Couldn't open the process {processId}");
        }

        private static byte[] ReadBytes(Handle processHandle, IntPtr address, int size)
        {
            if (!processHandle.IsInvalid && !processHandle.IsClosed && address != IntPtr.Zero)
            {
                var buffer = new byte[size];

                if (ReadProcessMemory(processHandle, address, buffer, size, out var numBytesRead) && size == numBytesRead)
                {
                    return buffer;
                }
            }

            throw new Win32Exception($"Couldn't read {size} byte(s) from 0x{address.ToString("X")}");
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReadProcessMemory(
            Handle hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool VirtualProtectEx(
            Handle hProcess,
            IntPtr lpAddress,
            int dwSize,
            MemoryProtection flNewProtect,
            out MemoryProtection lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WriteProcessMemory(
            Handle hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out int lpNumberOfBytesWritten);

        private bool CheckMask(int index, byte[] btPattern, string strMask)
        {
            for (var i = 0; i < btPattern.Length; i++)
            {
                if (strMask[i] == '?')
                {
                    continue;
                }

                if (strMask[i] == 'x' && btPattern[i] != this.dumpRegion[index + i])
                {
                    return false;
                }
            }

            return true;
        }

        private bool DumpMemory()
        {
            return this.dumpAddress != IntPtr.Zero && this.DumpMemory(this.dumpAddress, this.dumpSize);
        }

        private bool DumpMemory(IntPtr startAddress, int size)
        {
            try
            {
                if (this.NativeProcess == null)
                {
                    return false;
                }
                if (this.NativeProcess.HasExited)
                {
                    return false;
                }
                if (size == 0)
                {
                    return false;
                }

                this.dumpRegion = new byte[size];

                this.dumpRegion = this.ReadBytes(startAddress, size);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private byte[] ReadBytes(IntPtr address, int count, bool relative = false)
        {
            return ReadBytes(this.ProcessHandle, relative ? this.ToAbsolute(address) : address, count);
        }

        private void WriteBytes(IntPtr address, byte[] byteArray, bool relative = false)
        {
            this.WriteBytes(this.ProcessHandle, relative ? this.ToAbsolute(address) : address, byteArray);
        }

        private void WriteBytes(Handle processHandle, IntPtr address, byte[] byteArray)
        {
            if (processHandle.IsInvalid || processHandle.IsClosed || address == IntPtr.Zero)
            {
                return;
            }

            VirtualProtectEx(
                this.ProcessHandle,
                address,
                byteArray.Length,
                MemoryProtection.ExecuteReadWrite,
                out var oldProtect);
            var res = WriteProcessMemory(processHandle, address, byteArray, byteArray.Length, out var numBytesWritten);
            VirtualProtectEx(this.ProcessHandle, address, byteArray.Length, oldProtect, out oldProtect);

            if (!res || numBytesWritten != byteArray.Length)
            {
                throw new Win32Exception($"Couldn't write {byteArray.Length} bytes to 0x{address.ToString("X")}");
            }
        }

        #endregion

        public class Handle : SafeHandleZeroOrMinusOneIsInvalid
        {
            #region Constructors and Destructors

            public Handle()
                : base(true)
            {
            }

            public Handle(IntPtr handle)
                : base(true)
            {
                this.SetHandle(handle);
            }

            #endregion

            #region Methods

            protected override bool ReleaseHandle()
            {
                return this.handle != IntPtr.Zero && CloseHandle(this.handle);
            }

            #endregion
        }
    }
}