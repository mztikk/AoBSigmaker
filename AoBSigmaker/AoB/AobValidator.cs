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

        public static bool AreValid(string[] input)
        {
            int? len = null;

            foreach (string aob in input)
            {
                string trimed = StringR.RemoveWhitespace(aob);

                if (len is null)
                {
                    len = trimed.Length;
                }

                if (len != trimed.Length)
                {
                    return false;
                }

                if (!IsValid(trimed))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
