using System;
using System.Globalization;
using System.Windows.Data;

namespace AoBSigmaker.Converter
{
    public class MemoryTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return "None";
            }

            if (value is MemoryType t)
            {
                return t switch
                {
                    MemoryType.Byte => "Byte",
                    MemoryType.SByte => $"Byte Signed ({t})",
                    MemoryType.Short => $"2 Bytes Signed ({t})",
                    MemoryType.UShort => $"2 Bytes Unsigned ({t})",
                    MemoryType.Int => $"4 Bytes Signed ({t})",
                    MemoryType.Uint => $"4 Bytes Unsigned ({t})",
                    MemoryType.Long => $"8 Bytes Signed ({t})",
                    MemoryType.ULong => $"8 Bytes Unsigned ({t})",
                    MemoryType.Float => $"4 Bytes Float ({t})",
                    MemoryType.Double => $"8 Bytes Float ({t})",
                    MemoryType.IntPtr => $"Pointer ({t})",
                    MemoryType.String => "String",
                    _ => t.ToString(),
                };
            }

            throw new ArgumentException("Only " + typeof(MemoryType) + " supported.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
