namespace AoBSigmaker
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Principal;

    internal static class ProcessHandler
    {
        #region Static Fields

        internal static Process Proc;

        #endregion

        #region Public Methods and Operators

        public static List<Process> GetAllProcesses()
        {
            return
                Process.GetProcesses()
                    .Where(x => x.ProcessName != "Idle" && x.ProcessName != "System" && IntPtr.Size == 4)
                    .OrderBy(x => x.Id)
                    .ToList();
        }

        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator);
        }

        #endregion
    }
}