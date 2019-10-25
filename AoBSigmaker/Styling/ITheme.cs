using System.Windows.Media;

namespace AoBSigmaker.Styling
{
    public interface ITheme
    {
        Color BackgroundColor { get; set; }
        Color Buttons1 { get; set; }
        Color Buttons2 { get; set; }
        Color ButtonsMouseover { get; set; }
        Color ButtonsNormal { get; set; }
        Color ButtonsPressed { get; set; }
        Color DisabledColor { get; set; }
        Color DisabledHoverColor { get; set; }
        Color FlyoutColor { get; set; }
        Color ForegroundColor { get; set; }
        Color Highlighter1 { get; set; }
        Color Highlighter2 { get; set; }
        Color Highlighter3 { get; set; }
        Color Hover1 { get; set; }
        Color Theme6 { get; set; }
    }
}
