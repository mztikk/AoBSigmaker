using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RFReborn;

namespace AoBSigmaker.AoB
{
    public static class AobValidator
    {
        private const string AllowedChars = "abcdef0123456789?";

        public static bool IsValid(string? input, [NotNullWhen(false)] out string? errorMsg)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                errorMsg = "Input is null or empty";
                return false;
            }

            string trimmed = StringR.RemoveWhitespace(input);

            IEnumerable<char> illegalChars = trimmed.ToLowerInvariant().Except(AllowedChars);
            if (illegalChars.Any())
            {
                errorMsg = $"Illegal chars {RFReborn.Extensions.IEnumerableExtensions.ToObjectsString(illegalChars)}";
                return false;
            }

            if (trimmed.Length % 2 != 0)
            {
                errorMsg = "Bytes need to be full bytes, length of input has to be divisible by two";
                return false;
            }

            errorMsg = null;
            return true;
        }

        public static bool AreValid(IEnumerable<string> input, [NotNullWhen(false)] out AobError? aobError)
        {
            foreach (string aob in input)
            {
                if (!IsValid(aob, out string? errorMsg))
                {
                    aobError = new AobError(aob, errorMsg);
                    return false;
                }
            }

            aobError = null;
            return true;
        }
    }
}
