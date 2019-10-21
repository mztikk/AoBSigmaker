using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace AoBSigmaker.Converter
{
    public class ProcessToIdNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return "None";
            }

            if (value is Process proc)
            {
                return proc.ProcessName + "(" + proc.Id + ")";
            }

            throw new ArgumentException("Only " + typeof(Process) + " supported.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string idName)
            {
                string[] split = idName.Split(new[] { "(" }, StringSplitOptions.None);
                string procid_string = split[1].Remove(split[1].Length - 1);
                int procId = int.Parse(procid_string);
                Process rtn;
                try
                {
                    rtn = Process.GetProcessById(procId);
                }
                catch
                {
                    return null;
                }

                return rtn;
            }

            throw new ArgumentException("Only " + typeof(string) + " supported.");
        }

        //internal static Process? NameIdToProc(string proc)
        //{
        //    string[] split = proc.Split(new[] { "(" }, StringSplitOptions.None);
        //    string procid_string = split[1].Remove(split[1].Length - 1);
        //    int procId = int.Parse(procid_string);
        //    Process rtn;
        //    try
        //    {
        //        rtn = Process.GetProcessById(procId);
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //    return rtn;
        //}

        //internal static string ToNameId(Process proc) => proc.ProcessName + "(" + proc.Id + ")";
    }
}
