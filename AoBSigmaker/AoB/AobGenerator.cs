using System.Collections.Generic;
using System.Linq;
using RFReborn;

namespace AoBSigmaker.AoB
{
    public class AobGenerator : IAobGenerator
    {
        public string Make(IEnumerable<string> input)
        {
            // get the smallest
            IOrderedEnumerable<string> ordered = input.Select(StringR.RemoveWhitespace).Where(x => !string.IsNullOrWhiteSpace(x)).OrderBy(x => x.Length);

            char[] build = ordered.First().ToCharArray();

            // check for differences
            foreach (string aob in ordered)
            {
                // compare current to the first one
                for (int i = 0; i < build.Length; i++)
                {
                    if (build[i] == '?' || build[i] == aob[i])
                    {
                        continue;
                    }

                    build[i] = '?';
                }
            }

            // only allow full byte wildcards
            for (int i = 0; i < build.Length; i++)
            {
                if (build[i] == '?')
                {
                    if (i % 2 == 0)
                    {
                        build[i + 1] = '?';
                    }
                    else
                    {
                        build[i - 1] = '?';
                    }
                }
            }

            return new string(build);
        }
    }
}
