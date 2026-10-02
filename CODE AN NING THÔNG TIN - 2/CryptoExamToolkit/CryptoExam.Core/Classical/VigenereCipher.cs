// File: CryptoExam.Core/Classical/VigenereCipher.cs
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    /// <summary>
    /// Vigenere Cipher:
    ///   Encrypt: Ci = (Pi + Ki) mod 26
    ///   Decrypt: Pi = (Ci - Ki + 26) mod 26
    /// Key tự lặp cho đủ độ dài plaintext.
    /// </summary>
    public static class VigenereCipher
    {
        /// <summary>
        /// Mã hóa Vigenere. Trả về ciphertext và expanded key.
        /// </summary>
        public static (string ciphertext, string expandedKey) Encrypt(string plaintext, string key)
        {
            string pt = AlphabetUtils.CleanUppercase(plaintext);
            string k = AlphabetUtils.CleanUppercase(key);
            if (k.Length == 0) throw new ArgumentException("Key không được rỗng.");

            var sb = new StringBuilder();
            var expandedKey = new StringBuilder();

            for (int i = 0; i < pt.Length; i++)
            {
                char keyChar = k[i % k.Length];
                expandedKey.Append(keyChar);
                int pi = AlphabetUtils.ToIndex(pt[i]);
                int ki = AlphabetUtils.ToIndex(keyChar);
                sb.Append(AlphabetUtils.ToChar(pi + ki));
            }
            return (sb.ToString(), expandedKey.ToString());
        }

        /// <summary>
        /// Giải mã Vigenere.
        /// </summary>
        public static (string plaintext, string expandedKey) Decrypt(string ciphertext, string key)
        {
            string ct = AlphabetUtils.CleanUppercase(ciphertext);
            string k = AlphabetUtils.CleanUppercase(key);
            if (k.Length == 0) throw new ArgumentException("Key không được rỗng.");

            var sb = new StringBuilder();
            var expandedKey = new StringBuilder();

            for (int i = 0; i < ct.Length; i++)
            {
                char keyChar = k[i % k.Length];
                expandedKey.Append(keyChar);
                int ci = AlphabetUtils.ToIndex(ct[i]);
                int ki = AlphabetUtils.ToIndex(keyChar);
                sb.Append(AlphabetUtils.ToChar(ci - ki + 26));
            }
            return (sb.ToString(), expandedKey.ToString());
        }
    }
}
