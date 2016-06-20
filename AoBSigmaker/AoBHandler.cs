namespace AoBSigmaker
{
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
            foreach (var pattern in aobs)
            {
                if (pattern.StartsWith("##") || pattern == string.Empty)
                {
                    continue;
                }

                /*var loopPattern = pattern.Split(null);
                if (loopPattern[0].Length != 2)
                {
                    loopPattern =
                        Enumerable.Range(0, pattern.Length / 2).Select(i => pattern.Substring(i * 2, 2)).ToArray();
                }*/
                /*var replaced = pattern.Replace(" ", string.Empty);
                replaced = replaced.Replace("\t", string.Empty);*/
                var replaced = Regex.Replace(pattern, @"\s+", string.Empty);
                var loopPattern =
                    Enumerable.Range(0, replaced.Length / 2).Select(i => replaced.Substring(i * 2, 2)).ToArray();

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

            string rtnPattern = null;
            foreach (var by in patternWorking)
            {
                rtnPattern += by.ToUpper() + " ";
            }

            return rtnPattern;
        }

        #endregion
    }
}