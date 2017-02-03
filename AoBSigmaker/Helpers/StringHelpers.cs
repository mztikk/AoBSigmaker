namespace AoBSigmaker.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal static class StringHelpers
    {
        #region Public Methods and Operators

        public static IEnumerable<string> SplitInParts(this string s, int partLength)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (partLength <= 0)
            {
                throw new ArgumentException("Part length has to be positive.", "partLength");
            }

            for (var i = 0; i < s.Length; i += partLength)
            {
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
            }
        }

        #endregion

        #region Methods

        internal static string RemoveWhitespace(this string str)
        {
            return Regex.Replace(str, @"\s+", string.Empty);
        }

        #endregion
    }
}