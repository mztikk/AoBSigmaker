using System.Windows.Media;

namespace AoBSigmaker.Styling
{
    public class Theme : ITheme
    {
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public Color Highlighter1 { get; set; }
        public Color Highlighter2 { get; set; }
        public Color Highlighter3 { get; set; }
        public Color Buttons1 { get; set; }
        public Color Buttons2 { get; set; }
        public Color Theme6 { get; set; }
        public Color ButtonsPressed { get; set; }
        public Color ButtonsMouseover { get; set; }
        public Color Hover1 { get; set; }
        public Color ButtonsNormal { get; set; }
        public Color DisabledColor { get; set; }
        public Color DisabledHoverColor { get; set; }
        public Color FlyoutColor { get; set; }
    }
}
