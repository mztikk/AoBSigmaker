namespace AoBSigmaker
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows;

    using Microsoft.Win32;

    public static class Files
    {
        #region Public Methods and Operators

        public static string ChooseFile()
        {
            var fileDialog = new OpenFileDialog { Title = @"Open Text File", Filter = @"TXT files|*.txt" };
            try
            {
                var res = fileDialog.ShowDialog();
                if (res.HasValue && res.Value)
                {
                    return fileDialog.FileName;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Could not read file from disk. Error:" + Environment.NewLine + e.Message);
            }

            return string.Empty;
        }

        public static string ReadFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return string.Empty;
            }

            return File.ReadAllText(path, Encoding.Default);
        }

        #endregion
    }
}