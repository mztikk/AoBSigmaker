using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace AoBSigmaker.Converter
{
    public class ProcessModuleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return "None";
            }

            if (value is ProcessModule module)
            {
                return module.ModuleName;
            }

            throw new ArgumentException("Only " + typeof(ProcessModule) + " supported.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
