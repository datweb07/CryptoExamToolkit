// File: CryptoExam.Core/Classical/CaesarCipher.cs
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    /// <summary>
    /// Caesar Cipher: C = (P + k) mod 26, P = (C - k) mod 26
    /// </summary>
    public static class CaesarCipher
    {
        /// <summary>
        /// Mã hóa Caesar. preserveSpaces=true giữ nguyên khoảng trắng.
        /// </summary>
        public static string Encrypt(string plaintext, int k, bool preserveSpaces = false)
        {
            k = ((k % 26) + 26) % 26;
            var sb = new StringBuilder();
            foreach (char c in plaintext.ToUpper())
            {
                if (c >= 'A' && c <= 'Z')
                    sb.Append(AlphabetUtils.ToChar(AlphabetUtils.ToIndex(c) + k));
                else if (c == ' ' && preserveSpaces)
                    sb.Append(' ');
                // bỏ ký tự không phải A-Z
            }
            return sb.ToString();
        }

        /// <summary>
        /// Giải mã Caesar.
        /// </summary>
        public static string Decrypt(string ciphertext, int k, bool preserveSpaces = false)
        {
            // Giải mã = mã hóa với khóa âm
            return Encrypt(ciphertext, -k, preserveSpaces);
        }

        /// <summary>
        /// Brute force tất cả 25 khóa, trả về Dictionary key -> plaintext.
        /// </summary>
        public static Dictionary<int, string> BruteForce(string ciphertext, bool preserveSpaces = false)
        {
            var results = new Dictionary<int, string>();
            for (int k = 1; k <= 25; k++)
                results[k] = Decrypt(ciphertext, k, preserveSpaces);
            return results;
        }
    }
}
