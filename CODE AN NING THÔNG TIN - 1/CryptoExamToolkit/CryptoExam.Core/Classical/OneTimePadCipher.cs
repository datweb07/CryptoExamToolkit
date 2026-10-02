using System;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    public static class OneTimePadCipher
    {
        public static string Encrypt(string plainText, string key)
        {
            plainText = AlphabetUtils.CleanString(plainText);
            key = AlphabetUtils.CleanString(key);
            
            if (plainText.Length != key.Length)
                throw new ArgumentException("Key length must be equal to plain text length for OTP.");

            var sb = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++)
            {
                int p = AlphabetUtils.CharToIndex(plainText[i]);
                int k = AlphabetUtils.CharToIndex(key[i]);
                int c = (p + k) % 26;
                sb.Append(AlphabetUtils.IndexToChar(c));
            }
            return sb.ToString();
        }

        public static string Decrypt(string cipherText, string key)
        {
            cipherText = AlphabetUtils.CleanString(cipherText);
            key = AlphabetUtils.CleanString(key);

            if (cipherText.Length != key.Length)
                throw new ArgumentException("Key length must be equal to cipher text length for OTP.");

            var sb = new StringBuilder();
            for (int i = 0; i < cipherText.Length; i++)
            {
                int cIndex = AlphabetUtils.CharToIndex(cipherText[i]);
                int kIndex = AlphabetUtils.CharToIndex(key[i]);
                int pIndex = (cIndex - kIndex) % 26;
                if (pIndex < 0) pIndex += 26;
                sb.Append(AlphabetUtils.IndexToChar(pIndex));
            }
            return sb.ToString();
        }
    }
}
