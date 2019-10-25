using System.Windows;

namespace AoBSigmaker.Styling
{
    public interface IThemeBuilder
    {
        ResourceDictionary Make(ITheme theme, string name);
    }
}