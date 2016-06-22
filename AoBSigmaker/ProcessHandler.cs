namespace AoBSigmaker
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Principal;

    internal static class ProcessHandler
    {
        #region Public Methods and Operators

        public static List<Process> GetAllProcesses()
        {
            return
                Process.GetProcesses()
                    .Where(x => x.ProcessName != "Idle" && x.ProcessName != "System")
                    .OrderBy(x => x.Id)
                    .ToList();
        }

        public static bool IsAdministrator()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
                   || Program.IsDebugMode;
        }

        #endregion
    }
}