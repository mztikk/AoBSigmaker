namespace AoBSigmaker.Extensions
{
    using System;
    using System.Windows.Forms;

    // ReSharper disable once InconsistentNaming
    public static class RTBExts
    {
        #region Public Methods and Operators

        public static void AppendLine(this RichTextBox rtb, string text)
        {
            rtb.AppendText(text + Environment.NewLine);
        }

        #endregion
    }
}