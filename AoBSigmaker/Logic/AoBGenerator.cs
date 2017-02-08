namespace AoBSigmaker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AoBSigmaker.Helpers;

    internal static class AoBGenerator
    {
        #region Properties

        private static string[] Working { get; set; }

        private static char[] Working2 { get; set; }

        #endregion

        #region Methods

        internal static string GenerateSigFromAobFile(string path, bool halfByte)
        {
            using (var reader = new StreamReader(File.Open(path, FileMode.Open), Encoding.Default))
            {
                string line;
                var first = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!IsValid(line))
                    {
                        continue;
                    }

                    line = line.RemoveWhitespace();
                    if (first)
                    {
                        if (halfByte)
                        {
                            Working2 = line.ToCharArray();
                        }
                        else
                        {
                            Working = line.SplitInParts(2).ToArray();
                        }

                        first = false;
                    }

                    if (halfByte)
                    {
                        for (var i = 0; i < line.Length; i++)
                        {
                            if (Working2[i] == '?')
                            {
                                continue;
                            }

                            if (Working2[i] == line[i])
                            {
                                continue;
                            }

                            Working2[i] = '?';
                        }
                    }
                    else
                    {
                        var temp = line.SplitInParts(2).ToArray();
                        for (var i = 0; i < temp.Count(); i++)
                        {
                            if (Working[i] == "??")
                            {
                                continue;
                            }

                            if (Working[i] == temp[i])
                            {
                                continue;
                            }

                            Working[i] = "??";
                        }
                    }
                }

                reader.Close();
            }

            if (!halfByte)
            {
                return string.Join(" ", Working);
            }

            var str = new StringBuilder();
            var count = 0;
            foreach (var c in Working2)
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

        internal static string GenerateSigFromAobs(string[] aobs, bool halfByte)
        {
            if (aobs.Length < 2)
            {
                return aobs[0].ToUpper();
            }

            var checkedAoBs =
                aobs.Select(aob => aob.Length > aobs[0].Length ? aob.Remove(aobs[0].Length) : aob).ToArray();
            if (checkedAoBs.Length < 2)
            {
                return aobs[0].ToUpper();
            }

            if (halfByte)
            {
                Working2 = checkedAoBs[0].ToCharArray();
                GenerateHalfbyteSig(checkedAoBs);

                var str = new StringBuilder();
                var count = 0;
                foreach (var c in Working2)
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

            Working = checkedAoBs[0].SplitInParts(2).ToArray();
            GenerateFullbyteSig(checkedAoBs);

            return string.Join(" ", Working);
        }

        internal static bool IsValid(string aob)
        {
            aob = aob.RemoveWhitespace();

            return (aob.Length % 2 == 0) && !aob.ToLower().Except("abcdef0123456789?").Any();
        }

        internal static IEnumerable<string> TakeValidAoBs(string[] txt)
        {
            for (var i = 0; i < txt.Length; i++)
            {
                if (txt[i].StartsWith("##"))
                {
                    continue;
                }

                if (txt[i] == string.Empty)
                {
                    continue;
                }

                if (!IsValid(txt[i]))
                {
                    continue;
                }

                yield return txt[i].RemoveWhitespace();
            }
        }

        internal static IEnumerable<string> TakeValidAoBs(string txt)
        {
            return TakeValidAoBs(txt.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None));
        }

        private static void GenerateFullbyteSig(IReadOnlyList<string> aobs)
        {
            for (var j = 0; j < aobs.Count; j++)
            {
                if (j == 0)
                {
                    continue;
                }

                var temp = aobs[j].SplitInParts(2).ToArray();
                for (var i = 0; i < temp.Count(); i++)
                {
                    if (Working[i] == "??")
                    {
                        continue;
                    }

                    if (Working[i] == temp[i])
                    {
                        continue;
                    }

                    Working[i] = "??";
                }
            }
        }

        private static void GenerateHalfbyteSig(IReadOnlyList<string> aobs)
        {
            for (var j = 0; j < aobs.Count; j++)
            {
                if (j == 0)
                {
                    continue;
                }

                for (var i = 0; i < aobs[j].Length; i++)
                {
                    if (Working2[i] == '?')
                    {
                        continue;
                    }

                    if (Working2[i] == aobs[j][i])
                    {
                        continue;
                    }

                    Working2[i] = '?';
                }
            }
        }

        #endregion
    }
}