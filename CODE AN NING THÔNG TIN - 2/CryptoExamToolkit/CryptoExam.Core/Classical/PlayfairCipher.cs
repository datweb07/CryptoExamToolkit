// File: CryptoExam.Core/Classical/PlayfairCipher.cs
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    /// <summary>
    /// Playfair Cipher - ma trận 5x5, I/J chung ô.
    /// Test: BALLOON + MONARCHY -> IBSUPMNA
    /// </summary>
    public class PlayfairCipher
    {
        private readonly char[,] _matrix = new char[5, 5];
        // Vị trí của mỗi ký tự trong ma trận
        private readonly Dictionary<char, (int row, int col)> _pos = new();

        public PlayfairCipher(string key)
        {
            BuildMatrix(key);
        }

        /// <summary>
        /// Xây dựng ma trận 5x5 từ key.
        /// </summary>
        private void BuildMatrix(string key)
        {
            // Chuẩn hóa key: uppercase, I/J -> I, bỏ ký tự lặp và non-alpha
            key = key.ToUpper().Replace("J", "I");
            var seen = new HashSet<char>();
            var sequence = new List<char>();

            foreach (char c in key)
            {
                if (c >= 'A' && c <= 'Z' && seen.Add(c))
                    sequence.Add(c);
            }
            // Điền các chữ cái còn lại của alphabet
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (c == 'J') continue; // J chung ô với I
                if (seen.Add(c)) sequence.Add(c);
            }

            // Điền vào ma trận 5x5
            for (int i = 0; i < 25; i++)
            {
                int row = i / 5, col = i % 5;
                _matrix[row, col] = sequence[i];
                _pos[sequence[i]] = (row, col);
            }
        }

        /// <summary>
        /// In ma trận 5x5.
        /// </summary>
        public void PrintMatrix()
        {
            Console.WriteLine("  Key Matrix (Playfair 5x5):");
            for (int r = 0; r < 5; r++)
            {
                Console.Write("    ");
                for (int c = 0; c < 5; c++)
                {
                    Console.Write(_matrix[r, c]);
                    if (c < 4) Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Chuẩn bị plaintext: chia digram, chèn X khi cần.
        /// Trả về list các cặp (pair) và chuỗi đã chuẩn bị.
        /// </summary>
        public (List<(char, char)> pairs, string prepared) PreparePlaintext(string plaintext)
        {
            // Uppercase, J->I, bỏ non-alpha
            string pt = plaintext.ToUpper().Replace("J", "I");
            var letters = new List<char>();
            foreach (char c in pt)
                if (c >= 'A' && c <= 'Z') letters.Add(c);

            var pairs = new List<(char, char)>();
            int idx = 0;
            while (idx < letters.Count)
            {
                char first = letters[idx];
                char second;
                if (idx + 1 >= letters.Count)
                {
                    // Chỉ còn 1 ký tự, thêm X
                    second = 'X';
                    idx++;
                }
                else if (letters[idx] == letters[idx + 1])
                {
                    // Hai ký tự giống nhau, chèn X vào giữa
                    second = (first == 'X') ? 'Q' : 'X';
                    idx++;
                }
                else
                {
                    second = letters[idx + 1];
                    idx += 2;
                }
                pairs.Add((first, second));
            }

            var sb = new StringBuilder();
            foreach (var (f, s) in pairs) { sb.Append(f); sb.Append(s); sb.Append(' '); }
            return (pairs, sb.ToString().Trim());
        }

        /// <summary>
        /// Mã hóa toàn bộ plaintext.
        /// </summary>
        public string Encrypt(string plaintext)
        {
            var (pairs, _) = PreparePlaintext(plaintext);
            var sb = new StringBuilder();
            foreach (var pair in pairs)
                sb.Append(EncryptPair(pair.Item1, pair.Item2));
            return sb.ToString();
        }

        /// <summary>
        /// Giải mã toàn bộ ciphertext.
        /// </summary>
        public string Decrypt(string ciphertext)
        {
            // Ciphertext không cần chuẩn bị digram, đã sẵn thành cặp
            string ct = ciphertext.ToUpper().Replace("J", "I");
            var letters = new List<char>();
            foreach (char c in ct)
                if (c >= 'A' && c <= 'Z') letters.Add(c);

            if (letters.Count % 2 != 0)
                throw new ArgumentException("Ciphertext Playfair phải có số ký tự chẵn.");

            var sb = new StringBuilder();
            for (int i = 0; i < letters.Count; i += 2)
                sb.Append(DecryptPair(letters[i], letters[i + 1]));
            return sb.ToString();
        }

        /// <summary>
        /// Mã hóa một cặp ký tự (digram).
        /// </summary>
        public string EncryptPair(char a, char b)
        {
            a = (a == 'J') ? 'I' : a;
            b = (b == 'J') ? 'I' : b;
            var (ra, ca) = _pos[a];
            var (rb, cb) = _pos[b];

            if (ra == rb)
            {
                // Cùng hàng: dịch phải 1
                return $"{_matrix[ra, (ca + 1) % 5]}{_matrix[rb, (cb + 1) % 5]}";
            }
            else if (ca == cb)
            {
                // Cùng cột: dịch xuống 1
                return $"{_matrix[(ra + 1) % 5, ca]}{_matrix[(rb + 1) % 5, cb]}";
            }
            else
            {
                // Hình chữ nhật: lấy hai góc còn lại (giữ nguyên hàng, đổi cột)
                return $"{_matrix[ra, cb]}{_matrix[rb, ca]}";
            }
        }

        /// <summary>
        /// Giải mã một cặp ký tự.
        /// </summary>
        public string DecryptPair(char a, char b)
        {
            a = (a == 'J') ? 'I' : a;
            b = (b == 'J') ? 'I' : b;
            var (ra, ca) = _pos[a];
            var (rb, cb) = _pos[b];

            if (ra == rb)
            {
                // Cùng hàng: dịch trái 1
                return $"{_matrix[ra, (ca + 4) % 5]}{_matrix[rb, (cb + 4) % 5]}";
            }
            else if (ca == cb)
            {
                // Cùng cột: dịch lên 1
                return $"{_matrix[(ra + 4) % 5, ca]}{_matrix[(rb + 4) % 5, cb]}";
            }
            else
            {
                // Hình chữ nhật: tương tự encrypt
                return $"{_matrix[ra, cb]}{_matrix[rb, ca]}";
            }
        }
    }
}
