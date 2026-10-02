using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CryptoExam.Core.Common
{
    public static class AlphabetUtils
    {
        public const string StandardAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string CleanString(string input, bool preserveSpaces = false)
        {
            if (string.IsNullOrEmpty(input)) return "";
            input = input.ToUpperInvariant();
            if (preserveSpaces)
            {
                return Regex.Replace(input, "[^A-Z ]", "");
            }
            return Regex.Replace(input, "[^A-Z]", "");
        }

        public static int CharToIndex(char c)
        {
            return c - 'A';
        }

        public static char IndexToChar(int index)
        {
            index = (index % 26 + 26) % 26; // handle negative mod
            return (char)('A' + index);
        }
    }
}
