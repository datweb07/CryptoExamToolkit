using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    public static class CaesarCipher
    {
        public static string Encrypt(string plainText, int key, bool preserveSpaces = false)
        {
            plainText = AlphabetUtils.CleanString(plainText, preserveSpaces);
            var sb = new StringBuilder();

            foreach (char c in plainText)
            {
                if (c == ' ' && preserveSpaces)
                {
                    sb.Append(' ');
                    continue;
                }
                int p = AlphabetUtils.CharToIndex(c);
                int cIndex = (p + key) % 26;
                sb.Append(AlphabetUtils.IndexToChar(cIndex));
            }
            return sb.ToString();
        }

        public static string Decrypt(string cipherText, int key, bool preserveSpaces = false)
        {
            cipherText = AlphabetUtils.CleanString(cipherText, preserveSpaces);
            var sb = new StringBuilder();

            foreach (char c in cipherText)
            {
                if (c == ' ' && preserveSpaces)
                {
                    sb.Append(' ');
                    continue;
                }
                int cIndex = AlphabetUtils.CharToIndex(c);
                int pIndex = (cIndex - key) % 26;
                if (pIndex < 0) pIndex += 26;
                sb.Append(AlphabetUtils.IndexToChar(pIndex));
            }
            return sb.ToString();
        }
    }
}
