namespace AoBSigmaker
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    internal static class ProcessHandler
    {
        #region Static Fields

        internal static Process Proc;

        #endregion

        #region Public Methods and Operators

        public static List<Process> GetAllProcesses()
        {
            return Process.GetProcesses().OrderBy(x => x.Id).ToList();
        }

        #endregion
    }
}