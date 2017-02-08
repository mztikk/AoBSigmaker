namespace AoBSigmaker.Extensions
{
    using AoBSigmaker.Helpers;

    public static class ArrayExts
    {
        #region Public Methods and Operators

        public static string[] RemoveWhitespaces(this string[] self)
        {
            for (var i = 0; i < self.Length; i++)
            {
                self[i] = self[i].RemoveWhitespace();
            }

            return self;
        }

        #endregion
    }
}