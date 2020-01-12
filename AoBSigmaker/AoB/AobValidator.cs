using System.Collections.Generic;
using System.Linq;
using RFReborn;

namespace AoBSigmaker.AoB
{
    public static class AobValidator
    {
        private const string AllowedChars = "abcdef0123456789?";

        public static bool IsValid(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string trimmed = StringR.RemoveWhitespace(input);

            return trimmed.Length % 2 == 0 && !trimmed.ToLowerInvariant().Except(AllowedChars).Any();
        }

        public static bool AreValid(IEnumerable<string> input) => AreValid(input, out string? _);

        public static bool AreValid(IEnumerable<string> input, out string? invalid)
        {
            int? len = null;
            invalid = null;
            foreach (string aob in input)
            {
                string trimmed = StringR.RemoveWhitespace(aob);

                if (len is null)
                {
                    len = trimmed.Length;
                }

                if (len != trimmed.Length)
                {
                    invalid = aob;
                    return false;
                }

                if (!IsValid(trimmed))
                {
                    invalid = aob;
                    return false;
                }
            }

            return true;
        }
    }
}
