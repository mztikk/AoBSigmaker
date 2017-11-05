namespace AoBSigmaker
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Principal;

    internal static class Processes
    {
        #region Public Methods and Operators

        public static bool IsRunning(this Process process)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
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

        #endregion

        #region Methods

        internal static IEnumerable<Process> GetAllProcs()
        {
            foreach (var proc in Process.GetProcesses())
            {
                RemoteMemory temp = null;
                try
                {
                    temp = new RemoteMemory(proc);
                }
                catch (Exception e)
                {
                    continue;
                }
                finally
                {
                    temp?.Dispose();
                }

                yield return proc;
            }
        }

        internal static bool IsAdministrator()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
                   || App.IsDebugMode;
        }

        internal static Process NameIdToProc(string proc)
        {
            var split = proc.Split(new[] { "(" }, StringSplitOptions.None);
            var procid_string = split[1].Remove(split[1].Length - 1);
            var procId = int.Parse(procid_string);
            Process rtn;
            try
            {
                rtn = Process.GetProcessById(procId);
            }
            catch (Exception e)
            {
                return null;
            }

            return rtn;
        }

        internal static string ToNameId(this Process proc)
        {
            return proc.ProcessName + "(" + proc.Id + ")";
        }

        #endregion
    }
}