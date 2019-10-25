using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace AoBSigmaker.Styling
{
    public class ThemeManager
    {
        private const string RelativePathToStyles = "styles";
        private readonly Dictionary<string, ITheme> _themeDict = new Dictionary<string, ITheme>();
        private readonly Dictionary<string, ResourceDictionary> _themeResources = new Dictionary<string, ResourceDictionary>();
        private readonly HashSet<string> _injectedThemes = new HashSet<string>();
        private readonly IThemeBuilder _themeBuilder;

        public ThemeManager(IThemeBuilder themeBuilder) => _themeBuilder = themeBuilder;

        public async Task Load()
        {
            foreach (string themePath in GetAllThemeFiles())
            {
                FileInfo fi = new FileInfo(themePath);
                ITheme theme = await JsonHelper.LoadFromFile<ITheme>(fi).ConfigureAwait(false);
                _themeDict.Add(Path.GetFileNameWithoutExtension(fi.FullName), theme);
            }
        }

        public void AddTheme(ITheme theme, string name) => _themeDict.Add(name, theme);

        private static IEnumerable<string> GetAllThemeFiles() => Directory.EnumerateFiles(RelativePathToStyles, "*.json");

        public void LoadAndInjectTheme(string name)
        {
            if (!_themeDict.ContainsKey(name))
            {
                throw new Exception("Name doesn't exist.");
            }

            if (_injectedThemes.Contains(name))
            {
                return;
            }

            ResourceDictionary resourceDict = GenerateThemeByname(name);
            _themeResources.Add(name, resourceDict);
            MahApps.Metro.ThemeManager.AddTheme(resourceDict);
            _injectedThemes.Add(name);
        }

        public void ApplyTheme(string name)
        {
            if (!_themeDict.ContainsKey(name) || !_themeResources.ContainsKey(name))
            {
                throw new Exception("Name doesn't exist.");
            }

            MahApps.Metro.ThemeManager.ChangeTheme(Application.Current, name);
        }

        private ResourceDictionary GenerateThemeByname(string name)
        {
            if (!_themeDict.ContainsKey(name))
            {
                throw new Exception("Name doesn't exist.");
            }

            ITheme theme = _themeDict[name];

            return _themeBuilder.Make(theme, name);
        }
    }
}
