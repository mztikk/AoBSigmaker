namespace AoBSigmaker.Helpers
{
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    // ReSharper disable once InconsistentNaming
    public class TextBoxStreamWriter : TextWriter
    {
        #region Fields

        private readonly TextBox output;

        #endregion

        #region Constructors and Destructors

        public TextBoxStreamWriter(TextBox output)
        {
            this.output = output;
        }

        #endregion

        #region Public Properties

        public override Encoding Encoding => Encoding.UTF8;

        #endregion

        #region Public Methods and Operators

        public override void Write(char value)
        {
            MethodInvoker action = delegate { this.output.AppendText(value.ToString()); };
            this.output.BeginInvoke(action);
        }

        #endregion
    }
}