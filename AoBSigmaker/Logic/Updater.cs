namespace AoBSigmaker
{
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;

    internal static class Updater
    {
        #region Properties

        internal static string GithubLink { get; } =
            "https://raw.githubusercontent.com/mztikk/AoBSigmaker/master/AoBSigmaker/Properties/AssemblyInfo.cs";

        #endregion

        #region Methods

        internal static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        internal static string GetGithubVersion(string githubInfo)
        {
            var wc = new WebClient();
            var webData = wc.DownloadString(githubInfo);
            var reg = Regex.Match(webData, @"n\(""\d.\d.\d.\d");
            var version = reg.Value.Remove(0, 3);
            return version;
        }

        internal static string GetGithubVersion()
        {
            return GetGithubVersion(GithubLink);
        }

        internal static bool IsOnlineDiff(string githubInfo)
        {
            var currVersion = GetAssemblyVersion();
            var githubVersion = GetGithubVersion(githubInfo);
            return currVersion != githubVersion;
        }

        internal static bool IsOnlineDiff()
        {
            return IsOnlineDiff(GithubLink);
        }

        #endregion
    }
}