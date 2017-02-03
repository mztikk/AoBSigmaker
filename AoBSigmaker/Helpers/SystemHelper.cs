namespace AoBSigmaker.Helpers
{
    using System.Security.Principal;

    internal static class SystemHelper
    {
        #region Methods

        internal static bool IsAdministrator()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
                   || Program.IsDebugMode;
        }

        #endregion
    }
}