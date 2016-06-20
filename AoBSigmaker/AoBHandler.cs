namespace AoBSigmaker
{
    using System.Collections.Generic;
    using System.Linq;

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

                var loopPattern = pattern.Split(null);
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

                    if (loopPattern[i] == lastPattern[i])
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

                    if (loopPattern[i] != lastPattern[i] && i + 1 > patternWorking.Count)
                    {
                        patternWorking.Add("??");
                    }

                    if (i + 1 <= patternWorking.Count)
                    {
                        if (loopPattern[i] != lastPattern[i] && loopPattern[i] != patternWorking[i])
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
                rtnPattern += by + " ";
            }

            return rtnPattern;
        }

        #endregion
    }
}