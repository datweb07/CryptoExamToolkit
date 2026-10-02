// File: CryptoExam.Core/Classical/RailFenceCipher.cs
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    /// <summary>
    /// Rail Fence (Zigzag) Cipher.
    /// Ví dụ: SECURITY, rails=3 -> SREUIYCT
    /// </summary>
    public static class RailFenceCipher
    {
        /// <summary>
        /// Mã hóa Rail Fence Zigzag.
        /// </summary>
        public static string EncryptZigZag(string plaintext, int rails)
        {
            if (rails < 2) throw new ArgumentException("Số rail phải >= 2.");
            string text = AlphabetUtils.CleanUppercase(plaintext);

            // Tạo mảng các StringBuilder cho từng rail
            var railArrays = new StringBuilder[rails];
            for (int i = 0; i < rails; i++) railArrays[i] = new StringBuilder();

            int currentRail = 0;
            int direction = 1;

            foreach (char c in text)
            {
                railArrays[currentRail].Append(c);
                if (currentRail == 0) direction = 1;
                else if (currentRail == rails - 1) direction = -1;
                currentRail += direction;
            }

            var result = new StringBuilder();
            foreach (var rail in railArrays) result.Append(rail);
            return result.ToString();
        }

        /// <summary>
        /// Giải mã Rail Fence Zigzag.
        /// </summary>
        public static string DecryptZigZag(string ciphertext, int rails)
        {
            if (rails < 2) throw new ArgumentException("Số rail phải >= 2.");
            string text = AlphabetUtils.CleanUppercase(ciphertext);
            int n = text.Length;

            // Xác định vị trí mỗi ký tự thuộc rail nào
            int[] railIndex = new int[n];
            int currentRail = 0;
            int direction = 1;
            for (int i = 0; i < n; i++)
            {
                railIndex[i] = currentRail;
                if (currentRail == 0) direction = 1;
                else if (currentRail == rails - 1) direction = -1;
                currentRail += direction;
            }

            // Tính số ký tự trong mỗi rail
            int[] railLen = new int[rails];
            foreach (int r in railIndex) railLen[r]++;

            // Cắt ciphertext theo từng rail
            char[][] railChars = new char[rails][];
            int pos = 0;
            for (int r = 0; r < rails; r++)
            {
                railChars[r] = new char[railLen[r]];
                for (int j = 0; j < railLen[r]; j++)
                    railChars[r][j] = text[pos++];
            }

            // Đọc lại theo thứ tự zigzag
            int[] railPos = new int[rails];
            var result = new StringBuilder();
            for (int i = 0; i < n; i++)
                result.Append(railChars[railIndex[i]][railPos[railIndex[i]]++]);

            return result.ToString();
        }

        /// <summary>
        /// In pattern zigzag trực quan.
        /// </summary>
        public static void PrintZigZagPattern(string text, int rails)
        {
            text = AlphabetUtils.CleanUppercase(text);
            char[,] pattern = new char[rails, text.Length];
            for (int i = 0; i < rails; i++)
                for (int j = 0; j < text.Length; j++)
                    pattern[i, j] = '.';

            int currentRail = 0;
            int direction = 1;
            for (int i = 0; i < text.Length; i++)
            {
                pattern[currentRail, i] = text[i];
                if (currentRail == 0) direction = 1;
                else if (currentRail == rails - 1) direction = -1;
                currentRail += direction;
            }

            Console.WriteLine("  Zigzag pattern:");
            for (int r = 0; r < rails; r++)
            {
                Console.Write($"    Rail {r + 1}: ");
                for (int c = 0; c < text.Length; c++)
                    Console.Write(pattern[r, c]);
                Console.WriteLine();
            }
        }
    }
}
