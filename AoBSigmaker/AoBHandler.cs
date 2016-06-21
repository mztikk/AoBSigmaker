namespace AoBSigmaker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal static class AoBHandler
    {
        #region Public Methods and Operators

        public static string GenerateSigFromAoBs(string[] aobs)
        {
            var patternWorking = new List<string>();
            var lastPattern = new List<string>();
            var temp =
                (from aob in aobs
                 where !aob.StartsWith("##") && aob != string.Empty
                 select Regex.Replace(aob, @"\s+", string.Empty)).ToList();

            var checkedAoBs =
                temp.Select(aob => aob.Length > temp[0].Length ? aob.Remove(temp[0].Length) : aob).ToList();

            foreach (var pattern in checkedAoBs)
            {
                var loopPattern =
                    Enumerable.Range(0, pattern.Length / 2).Select(i => pattern.Substring(i * 2, 2)).ToArray();

                for (var i = 0; i < loopPattern.Length; i++)
                {
                    if (i + 1 > lastPattern.Count || !lastPattern.Any() || loopPattern[i] == string.Empty)
                    {
                        continue;
                    }

                    if (i + 1 <= patternWorking.Count && patternWorking[i] == "??")
                    {
                        continue;
                    }

                    var currentByte = loopPattern[i].ToLower();
                    var lastByte = lastPattern[i].ToLower();
                    var newByte = i + 1 <= patternWorking.Count ? patternWorking[i] : string.Empty;

                    if (currentByte == lastByte)
                    {
                        if (i + 1 <= patternWorking.Count)
                        {
                            patternWorking.RemoveAt(i);
                            patternWorking.Insert(i, loopPattern[i]);
                        }
                        else
                        {
                            patternWorking.Add(loopPattern[i]);
                        }
                    }

                    if (currentByte != lastByte && i + 1 > patternWorking.Count)
                    {
                        patternWorking.Add("??");
                    }

                    if (i + 1 <= patternWorking.Count)
                    {
                        if (currentByte != lastByte && currentByte != newByte)
                        {
                            patternWorking.RemoveAt(i);
                            patternWorking.Insert(i, "??");
                        }
                    }
                }

                lastPattern = loopPattern.ToList();
            }

            return patternWorking.Aggregate<string, string>(null, (current, @by) => current + (@by.ToUpper() + " "));
        }

        public static byte[] GetBytePattern(string pattern)
        {
            return
                Enumerable.Range(0, pattern.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(pattern.Substring(x, 2), 16))
                    .ToArray();
        }

        public static string GetMaskFromPattern(string pattern)
        {
            var reg = Regex.Replace(pattern, @"\s+", string.Empty);
            var splitThis = Enumerable.Range(0, reg.Length / 2).Select(i => reg.Substring(i * 2, 2)).ToArray();
            var mask = string.Empty;
            foreach (var by in splitThis)
            {
                if (by == string.Empty)
                {
                    continue;
                }

                if (by == "??")
                {
                    mask += "?";
                }
                else
                {
                    mask += "x";
                }
            }

            return mask;
        }

        #endregion
    }
}