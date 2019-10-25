using System;
using System.Windows;
using System.Windows.Media;

namespace AoBSigmaker.Styling
{
    public class ThemeBuilder : IThemeBuilder
    {
        public ResourceDictionary Make(ITheme theme, string name)
        {
            Color foregroundColor = theme.ForegroundColor;
            Color backgroundColor = theme.BackgroundColor;//["BackgroundColor"];
            Color highlighter1 = theme.Highlighter1;//["Highlighter1"];
            Color highlighter2 = theme.Highlighter2;//["Highlighter2"];
            Color highlighter3 = theme.Highlighter3;//["Highlighter3"];
            Color buttons1 = theme.Buttons1;//["Buttons1"];
            Color buttons2 = theme.Buttons2;//["Buttons2"];
            Color theme6 = theme.Theme6;//["Theme6"];
            Color buttonsPressed = theme.ButtonsPressed;//["ButtonsPressed"];
            Color buttonsMouseover = theme.ButtonsMouseover;//["ButtonsMouseover"];
            Color hover1 = theme.Hover1;//["Hover1"];
            Color buttonsNormal = theme.ButtonsNormal;//["ButtonsNormal"];
            Color disabled = theme.DisabledColor;//["DisabledColor"];
            Color disabledHover = theme.DisabledHoverColor;//["DisabledHoverColor"];
            Color flyout = theme.FlyoutColor;//["FlyoutColor"];

            return new ResourceDictionary
            {
                //<system:String x:Key="Theme.Name">Dark.Accent2</system:String>
                //<system:String x:Key="Theme.DisplayName">Accent2 (Dark)</system:String>
                //<system:String x:Key="Theme.BaseColorScheme">Dark</system:String>
                //<system:String x:Key="Theme.ColorScheme">Accent2</system:String>
                ["Theme.Name"] = name,
                ["Theme.DisplayName"] = name,
                ["MahApps.Colors.Black"] = foregroundColor,
                ["MahApps.Colors.White"] = backgroundColor,
                ["MahApps.Colors.Gray1"] = highlighter1,
                ["MahApps.Colors.Gray2"] = highlighter2,
                ["MahApps.Colors.Gray3"] = highlighter3,
                ["MahApps.Colors.Gray4"] = buttons1,
                ["MahApps.Colors.Gray5"] = buttons2,
                ["MahApps.Colors.Gray6"] = theme6,
                ["MahApps.Colors.Gray7"] = buttonsPressed,
                ["MahApps.Colors.Gray8"] = buttonsMouseover,
                ["MahApps.Colors.Gray9"] = hover1,
                ["MahApps.Colors.Gray10"] = buttonsNormal,
                ["MahApps.Colors.GrayNormal"] = disabled,
                ["MahApps.Colors.GrayHover"] = disabledHover,
                ["MahApps.Colors.FlyoutColor"] = flyout,

                ["MahApps.Brushes.Black"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.BlackColor"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.Text"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.Label.Text"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.TextBox.Border"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.TextBox.MouseOverBorder"] = GetSolidColorBrush(highlighter2),
                ["MahApps.Brushes.BlackColor"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.TextBox.MouseOverInnerBorder"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.TextBox.FocusBorder"] = GetSolidColorBrush(highlighter1),
                ["MahApps.Brushes.ComboBoxPopupBorder"] = GetSolidColorBrush(highlighter1),
                ["MahApps.Brushes.Button.MouseOverBorder"] = GetSolidColorBrush(highlighter1),
                ["MahApps.Brushes.Button.MouseOverInnerBorder"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.ComboBox.MouseOverBorder"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.ComboBox.MouseOverInnerBorder"] = GetSolidColorBrush(highlighter1),
                ["MahApps.Brushes.SystemColors.ControlTextBrushKey"] = GetSolidColorBrush(foregroundColor),

                ["MahApps.Brushes.Control.Background"] = GetSolidColorBrush(backgroundColor),
                ["MahApps.Brushes.WhiteColor"] = GetSolidColorBrush(backgroundColor),
                ["MahApps.Brushes.White"] = GetSolidColorBrush(backgroundColor),
                ["MahApps.Brushes.DisabledWhite"] = GetSolidColorBrush(backgroundColor),
                ["MahApps.Brushes.Window.Background"] = GetSolidColorBrush(backgroundColor),
                ["MahApps.Brushes.SystemColors.WindowBrushKey"] = GetSolidColorBrush(backgroundColor),

                ["MahApps.Brushes.Gray1"] = GetSolidColorBrush(highlighter1),
                ["MahApps.Brushes.Gray2"] = GetSolidColorBrush(highlighter2),
                ["MahApps.Brushes.Gray3"] = GetSolidColorBrush(highlighter3),
                ["MahApps.Brushes.Gray4"] = GetSolidColorBrush(buttons1),
                ["MahApps.Brushes.Gray5"] = GetSolidColorBrush(buttons2),
                ["MahApps.Brushes.Gray6"] = GetSolidColorBrush(theme6),
                ["MahApps.Brushes.Gray7"] = GetSolidColorBrush(buttonsPressed),
                ["MahApps.Brushes.Gray8"] = GetSolidColorBrush(buttonsMouseover),
                ["MahApps.Brushes.Gray9"] = GetSolidColorBrush(hover1),
                ["MahApps.Brushes.Gray10"] = GetSolidColorBrush(buttonsNormal),

                ["MahApps.Brushes.GrayNormal"] = GetSolidColorBrush(disabled),
                ["MahApps.Brushes.GrayHover"] = GetSolidColorBrush(disabledHover),

                ["MahApps.Brushes.Flyout.Background"] = GetSolidColorBrush(flyout),
                ["MahApps.Brushes.Flyout.Foreground"] = GetSolidColorBrush(foregroundColor),

                ["MahApps.Brushes.Menu.Background"] = GetSolidColorBrush(backgroundColor),
                ["MahApps.Brushes.ContextMenu.Background"] = GetSolidColorBrush(backgroundColor),
                ["MahApps.Brushes.SubMenu.Background"] = GetSolidColorBrush(backgroundColor),
                ["MahApps.Brushes.MenuItem.Background"] = GetSolidColorBrush(backgroundColor),

                ["MahApps.Brushes.ContextMenu.Border"] = GetSolidColorBrush(foregroundColor),
                ["MahApps.Brushes.SubMenu.Border"] = GetSolidColorBrush(foregroundColor),

                ["MahApps.Brushes.MenuItem.DisabledForeground"] = GetSolidColorBrush(highlighter2),
                ["SystemColors.MenuTextBrushKey"] = GetSolidColorBrush(foregroundColor),

                ["MahApps.Brushes.DataGrid.DisabledHighlight"] = GetSolidColorBrush(highlighter3),
                ["MahApps.Brushes.CleanWindowCloseButton.Background"] = GetSolidColorBrush(highlighter2),
                ["MahApps.Brushes.CloseButton.BackgroundHighlighted"] = GetSolidColorBrush(highlighter1),
                ["MahApps.Brushes.CloseButton.BackgroundPressed"] = GetSolidColorBrush(highlighter2),
                ["MahApps.Brushes.CleanWindowCloseButton.PressedBackground"] = GetSolidColorBrush(highlighter2),
                ["MahApps.Brushes.WindowTitle"] = GetSolidColorBrush(highlighter1),
                ["MahApps.Brushes.WindowTitle.NonActive"] = GetSolidColorBrush(disabled),
            };
        }

        /// <summary>
		/// Determining Ideal Text Color Based on Specified Background Color
		/// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
		/// </summary>
		/// <param name = "color">The bg.</param>
		private static Color IdealTextColor(Color color)
        {
            const int nThreshold = 105;
            int bgDelta = Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            return (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
        }

        private static SolidColorBrush GetSolidColorBrush(Color color, double opacity = 1d)
        {
            SolidColorBrush brush = new SolidColorBrush(color) { Opacity = opacity };
            brush.Freeze();
            return brush;
        }
    }
}
