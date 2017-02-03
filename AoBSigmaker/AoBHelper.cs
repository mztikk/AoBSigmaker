namespace AoBSigmaker
{
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using AoBSigmaker.Helpers;

    internal static class AoBHelper
    {
        #region Methods

        internal static string GenerateSigFromAobs(string[] aobs, bool halfByte)
        {
            if (aobs.Length < 2)
            {
                return aobs[0].ToUpper();
            }

            var temp =
                (from aob in aobs
                 where !aob.StartsWith("##") && (aob != string.Empty)
                 select Regex.Replace(aob, @"\s+", string.Empty)).ToArray();

            var checkedAoBs =
                temp.Select(aob => aob.Length > temp[0].Length ? aob.Remove(temp[0].Length) : aob).ToArray();
            if (checkedAoBs.Length < 2)
            {
                return aobs[0].ToUpper();
            }

            return halfByte ? GenerateHalfbyteSig(checkedAoBs) : GenerateFullbyteSig(checkedAoBs);
        }

        internal static bool IsValid(string aob)
        {
            aob = aob.RemoveWhitespace();

            if ((aob.Length % 2 != 0) || aob.ToLower().Except("abcdef0123456789?").Any())
            {
                return false;
            }

            return true;
        }

        private static string GenerateFullbyteSig(string[] aobs)
        {
            var working = aobs[0].SplitInParts(2).ToArray();
            for (var j = 0; j < aobs.Length; j++)
            {
                if (j == 0)
                {
                    continue;
                }

                var temp = aobs[j].SplitInParts(2).ToArray();
                for (var i = 0; i < temp.Count(); i++)
                {
                    if (working[i] == "??")
                    {
                        continue;
                    }

                    if (working[i] == temp[i])
                    {
                        continue;
                    }

                    working[i] = "??";
                }
            }

            var str = new StringBuilder();
            for (var i = 0; i < working.Length; i++)
            {
                str.Append(working[i]);
                if (i < working.Length - 1)
                {
                    str.Append(" ");
                }
            }

            return str.ToString();
        }

        private static string GenerateHalfbyteSig(string[] aobs)
        {
            var working = aobs[0].ToCharArray();

            for (var j = 0; j < aobs.Length; j++)
            {
                if (j == 0)
                {
                    continue;
                }

                for (var i = 0; i < aobs[j].Length; i++)
                {
                    if (working[i] == '?')
                    {
                        continue;
                    }

                    if (working[i] == aobs[j][i])
                    {
                        continue;
                    }

                    working[i] = '?';
                }
            }

            var str = new StringBuilder();
            var count = 0;
            foreach (var c in working)
            {
                if (count == 2)
                {
                    str.Append(' ');
                    count = 0;
                }

                str.Append(c);
                count++;
            }
            return str.ToString();
        }

        #endregion
    }
}