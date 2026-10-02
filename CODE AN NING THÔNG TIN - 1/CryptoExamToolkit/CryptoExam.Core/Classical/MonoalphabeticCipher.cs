using System;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    public static class MonoalphabeticCipher
    {
        public static string Encrypt(string plainText, string plainAlphabet, string cipherAlphabet)
        {
            plainText = AlphabetUtils.CleanString(plainText);
            plainAlphabet = plainAlphabet.ToUpperInvariant();
            cipherAlphabet = cipherAlphabet.ToUpperInvariant();
            
            if (plainAlphabet.Length != cipherAlphabet.Length)
                throw new ArgumentException("Alphabets must be of the same length.");
                
            var sb = new StringBuilder();
            foreach (char c in plainText)
            {
                int index = plainAlphabet.IndexOf(c);
                if (index >= 0) sb.Append(cipherAlphabet[index]);
            }
            return sb.ToString();
        }

        public static string Decrypt(string cipherText, string plainAlphabet, string cipherAlphabet)
        {
            cipherText = AlphabetUtils.CleanString(cipherText);
            plainAlphabet = plainAlphabet.ToUpperInvariant();
            cipherAlphabet = cipherAlphabet.ToUpperInvariant();
            
            if (plainAlphabet.Length != cipherAlphabet.Length)
                throw new ArgumentException("Alphabets must be of the same length.");
                
            var sb = new StringBuilder();
            foreach (char c in cipherText)
            {
                int index = cipherAlphabet.IndexOf(c);
                if (index >= 0) sb.Append(plainAlphabet[index]);
            }
            return sb.ToString();
        }
    }
}
