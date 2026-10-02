// File: CryptoExam.Core/Classical/MonoalphabeticCipher.cs
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    /// <summary>
    /// Monoalphabetic Substitution Cipher.
    /// Người dùng cung cấp bảng thay thế 26 ký tự.
    /// Ví dụ: Plain=ABCDE..., Cipher=DKVQI...
    /// </summary>
    public class MonoalphabeticCipher
    {
        // Mảng ánh xạ: plainIndex -> cipherChar
        private readonly char[] _encryptMap = new char[26];
        // Mảng ánh xạ ngược: cipherIndex -> plainChar
        private readonly char[] _decryptMap = new char[26];

        /// <summary>
        /// Khởi tạo với plain alphabet và cipher alphabet (26 ký tự).
        /// </summary>
        public MonoalphabeticCipher(string plainAlphabet, string cipherAlphabet)
        {
            plainAlphabet = plainAlphabet.Trim().ToUpper().Replace(" ", "");
            cipherAlphabet = cipherAlphabet.Trim().ToUpper().Replace(" ", "");

            ValidateAlphabet(plainAlphabet, "Plain alphabet");
            ValidateAlphabet(cipherAlphabet, "Cipher alphabet");

            for (int i = 0; i < 26; i++)
            {
                int plainIdx = AlphabetUtils.ToIndex(plainAlphabet[i]);
                int cipherIdx = AlphabetUtils.ToIndex(cipherAlphabet[i]);
                _encryptMap[plainIdx] = cipherAlphabet[i];
                _decryptMap[cipherIdx] = plainAlphabet[i];
            }
        }

        /// <summary>
        /// Khởi tạo nhanh với cipher alphabet (plain = ABCDEFG...).
        /// </summary>
        public MonoalphabeticCipher(string cipherAlphabet)
            : this("ABCDEFGHIJKLMNOPQRSTUVWXYZ", cipherAlphabet)
        { }

        public string Encrypt(string plaintext)
        {
            var sb = new StringBuilder();
            foreach (char c in plaintext.ToUpper())
            {
                if (c >= 'A' && c <= 'Z')
                    sb.Append(_encryptMap[AlphabetUtils.ToIndex(c)]);
                else if (c == ' ')
                    sb.Append(' ');
            }
            return sb.ToString();
        }

        public string Decrypt(string ciphertext)
        {
            var sb = new StringBuilder();
            foreach (char c in ciphertext.ToUpper())
            {
                if (c >= 'A' && c <= 'Z')
                    sb.Append(_decryptMap[AlphabetUtils.ToIndex(c)]);
                else if (c == ' ')
                    sb.Append(' ');
            }
            return sb.ToString();
        }

        /// <summary>
        /// In bảng ánh xạ A->X, B->Y, ...
        /// </summary>
        public void PrintMappingTable()
        {
            Console.WriteLine("  Bảng thay thế:");
            for (int i = 0; i < 26; i++)
                Console.WriteLine($"    {(char)('A' + i)} -> {_encryptMap[i]}");
        }

        private static void ValidateAlphabet(string alphabet, string name)
        {
            if (alphabet.Length != 26)
                throw new ArgumentException($"{name} phải có đúng 26 ký tự, hiện có {alphabet.Length}.");
            var seen = new HashSet<char>();
            foreach (char c in alphabet)
            {
                if (c < 'A' || c > 'Z')
                    throw new ArgumentException($"{name} chỉ được chứa A-Z, tìm thấy '{c}'.");
                if (!seen.Add(c))
                    throw new ArgumentException($"{name} có ký tự trùng: '{c}'.");
            }
        }
    }
}
