namespace AoBSigmaker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class AoBGen
    {
        #region Constants

        private const string AllowedChars = "abcdef0123456789?";

        #endregion

        #region Static Fields

        private static readonly HashSet<char> WhitespaceChars = new HashSet<char>
                                                                    {
                                                                        '\u0020', '\u00A0', '\u1680', '\u2000', '\u2001',
                                                                        '\u2002', '\u2003', '\u2004', '\u2005', '\u2006',
                                                                        '\u2007', '\u2008', '\u2009', '\u200A', '\u202F',
                                                                        '\u205F', '\u3000', '\u2028', '\u2029', '\u0009',
                                                                        '\u000A', '\u000B', '\u000C', '\u000D', '\u0085'
                                                                    };

        #endregion

        #region Enums

        public enum FileReadMode : byte
        {
            FullCopy,

            ReadLines
        }

        public enum ReturnStyle : byte
        {
            String,

            // ReSharper disable once InconsistentNaming
            EscapedHex
        }

        #endregion

        #region Public Methods and Operators

        public static string Generate(
            string[] aobs,
            bool halfByte = false,
            bool shortenWildcards = false,
            ReturnStyle returnStyle = ReturnStyle.String)
        {
            var stripped = aobs.RemoveWhitespaces();

            var temp = stripped.Where(x => Settings.TrustValidity || IsValid(x) && !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x.Length).ToArray();
            var checkedAoBs = temp.Select(
                aob => aob.Length > temp.First().Length
                           ? aob.Remove(temp.First().Length).ToUpperInvariant()
                           : aob.ToUpperInvariant()).ToArray();

            if (checkedAoBs.Length < 1)
            {
                return string.Empty;
            }
            if (checkedAoBs.Length < 2)
            {
                return aobs[0].ToUpperInvariant();
            }

            if (returnStyle == ReturnStyle.EscapedHex)
            {
                halfByte = false;
            }

            var build = new List<char>(checkedAoBs.First());

            foreach (var aob in checkedAoBs)
            {
                if (string.IsNullOrWhiteSpace(aob))
                {
                    continue;
                }

                if (Settings.TrustValidity || !IsValid(aob))
                {
                    continue;
                }

                for (var i = 0; i < aob.Length; i++)
                {
                    if (build[i] == '?' || build[i] == aob[i])
                    {
                        continue;
                    }

                    build[i] = '?';
                }
            }

            var beautify = new StringBuilder();
            while (build.Count > 0)
            {
                var get = TakeAndRemove(ref build, 2);
                if (!halfByte)
                {
                    if (get.Any(x => x == '?'))
                    {
                        get[0] = '?';
                        get[1] = '?';
                    }
                }
                beautify.Append(get);
                if (build.Count > 1)
                {
                    beautify.Append(" ");
                }
            }

            var rtn = beautify.ToString();
            if (shortenWildcards)
            {
                ShortenWildcards(ref rtn);
            }

            return returnStyle == ReturnStyle.String ? rtn : GetEscapedHexString(rtn);
        }

        public static bool IsValid(string aob)
        {
            aob = RemoveWhitespace(aob);

            return aob.Length % 2 == 0 && !aob.ToLower().Except(AllowedChars).Any();
        }

        public static string RemoveWhitespace(string input)
        {
            var len = input.Length;
            var src = input.ToCharArray();
            var dstIdx = 0;
            for (var i = 0; i < len; i++)
            {
                var ch = src[i];
                if (WhitespaceChars.Contains(ch))
                {
                    continue;
                }

                src[dstIdx++] = ch;
            }

            return new string(src, 0, dstIdx);
        }

        public static string[] RemoveWhitespaces(this string[] self)
        {
            self = (from t in self where !string.IsNullOrWhiteSpace(t) select RemoveWhitespace(t)).ToArray();
            return self;
        }

        #endregion

        #region Methods

        private static string GetEscapedHexString(string sig)
        {
            var splitThis = sig.Split(null);
            var res = new StringBuilder();
            var mask = new StringBuilder();
            foreach (var by in splitThis)
            {
                if (string.IsNullOrWhiteSpace(by))
                {
                    continue;
                }

                if (by == "??")
                {
                    res.Append("\\x" + "00");
                    mask.Append("?");
                }
                else
                {
                    res.Append("\\x" + by);
                    mask.Append("x");
                }
            }

            return res + Environment.NewLine + mask;
        }

        private static void ShortenWildcards(ref string sig)
        {
            while (sig.StartsWith("?") || sig.StartsWith(" "))
            {
                sig = sig.TrimStart('?', ' ');
            }

            while (sig.EndsWith("?") || sig.EndsWith(" "))
            {
                sig = sig.TrimEnd('?', ' ');
            }
        }

        private static T[] TakeAndRemove<T>(ref List<T> t, int count)
        {
            var rtn = new T[count];
            for (var i = 0; i < count; i++)
            {
                rtn[i] = t[i];
            }

            t.RemoveRange(0, count);
            return rtn;
        }

        #endregion
    }
}