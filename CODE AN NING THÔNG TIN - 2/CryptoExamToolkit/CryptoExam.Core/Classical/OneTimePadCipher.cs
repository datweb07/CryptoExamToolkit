// File: CryptoExam.Core/Classical/OneTimePadCipher.cs
using System;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    /// <summary>
    /// One-Time Pad (OTP) trên alphabet A-Z:
    ///   Encrypt: Ci = (Pi + Ki) mod 26
    ///   Decrypt: Pi = (Ci - Ki + 26) mod 26
    /// QUAN TRỌNG: Key PHẢI cùng độ dài message. Không tự lặp key.
    /// </summary>
    public static class OneTimePadCipher
    {
        /// <summary>
        /// Mã hóa OTP.
        /// </summary>
        public static string Encrypt(string plaintext, string key)
        {
            string pt = AlphabetUtils.CleanUppercase(plaintext);
            string k = AlphabetUtils.CleanUppercase(key);

            ValidateLengths(pt, k);

            var sb = new StringBuilder();
            for (int i = 0; i < pt.Length; i++)
            {
                int pi = AlphabetUtils.ToIndex(pt[i]);
                int ki = AlphabetUtils.ToIndex(k[i]);
                sb.Append(AlphabetUtils.ToChar(pi + ki));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Giải mã OTP.
        /// </summary>
        public static string Decrypt(string ciphertext, string key)
        {
            string ct = AlphabetUtils.CleanUppercase(ciphertext);
            string k = AlphabetUtils.CleanUppercase(key);

            ValidateLengths(ct, k);

            var sb = new StringBuilder();
            for (int i = 0; i < ct.Length; i++)
            {
                int ci = AlphabetUtils.ToIndex(ct[i]);
                int ki = AlphabetUtils.ToIndex(k[i]);
                sb.Append(AlphabetUtils.ToChar(ci - ki + 26));
            }
            return sb.ToString();
        }

        private static void ValidateLengths(string text, string key)
        {
            if (text.Length != key.Length)
                throw new ArgumentException(
                    $"OTP: Key ({key.Length} ký tự) PHẢI cùng độ dài với message ({text.Length} ký tự). " +
                    "OTP không tự lặp key.");
        }
    }
}
