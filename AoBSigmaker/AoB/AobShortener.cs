namespace AoBSigmaker.AoB
{
    public class AobShortener : IAobShortener
    {
        public string Shorten(string input) => input.Trim('?');
    }
}
