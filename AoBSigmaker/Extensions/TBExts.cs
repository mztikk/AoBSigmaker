namespace AoBSigmaker.Extensions
{
    using System;
    using System.Windows.Forms;

    // ReSharper disable once InconsistentNaming
    public static class TBExts
    {
        #region Public Methods and Operators

        public static void AppendLine(this TextBox tb, string value)
        {
            tb.AppendText(value + Environment.NewLine);
        }

        #endregion
    }
}