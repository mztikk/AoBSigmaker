namespace AoBSigmaker
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    internal static class UpdateHandler
    {
        #region Methods

        internal static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        internal static string GetGithubVersion(string githubInfo)
        {
            var wc = new WebClient();
            var webData =
                wc.DownloadString(githubInfo);
            var reg = Regex.Match(webData, @"n\(""\d.\d.\d.\d");
            var version = reg.Value.Remove(0, 3);
            return version;
        }

        internal static bool IsOnlineDiff(string githubInfo)
        {
            var currVersion = GetAssemblyVersion();
            var githubVersion = GetGithubVersion(githubInfo);
            return currVersion != githubVersion;
        }

        #endregion
    }
}