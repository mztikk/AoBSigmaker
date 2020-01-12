namespace AoBSigmaker.AoB
{
    public class AobError
    {
        public readonly string Aob;
        public readonly string Message;

        public AobError(string aob, string message)
        {
            Aob = aob;
            Message = message;
        }

        public override string ToString() => $"{Message}";
    }
}
