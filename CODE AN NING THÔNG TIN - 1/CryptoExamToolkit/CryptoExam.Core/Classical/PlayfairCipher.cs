using System;
using System.Collections.Generic;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    public class PlayfairCipher
    {
        public char[,] Matrix { get; private set; } = new char[5, 5];
        
        public PlayfairCipher(string key)
        {
            GenerateMatrix(key);
        }

        private void GenerateMatrix(string key)
        {
            key = AlphabetUtils.CleanString(key).Replace("J", "I");
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // no J
            
            var used = new HashSet<char>();
            var combined = key + alphabet;
            int r = 0, c = 0;
            
            foreach (char ch in combined)
            {
                if (!used.Contains(ch))
                {
                    used.Add(ch);
                    Matrix[r, c] = ch;
                    c++;
                    if (c == 5)
                    {
                        c = 0;
                        r++;
                    }
                    if (r == 5) break;
                }
            }
        }

        public string GetMatrixString()
        {
            var sb = new StringBuilder();
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    sb.Append(Matrix[r, c]).Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString().TrimEnd();
        }

        public List<string> PreparePairs(string text)
        {
            text = AlphabetUtils.CleanString(text).Replace("J", "I");
            var pairs = new List<string>();
            int i = 0;
            while (i < text.Length)
            {
                char c1 = text[i];
                char c2 = (i + 1 < text.Length) ? text[i + 1] : 'X';
                
                if (c1 == c2)
                {
                    pairs.Add(c1.ToString() + "X");
                    i++;
                }
                else
                {
                    pairs.Add(c1.ToString() + c2.ToString());
                    i += 2;
                }
            }
            return pairs;
        }

        private (int r, int c) FindPosition(char ch)
        {
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (Matrix[r, c] == ch) return (r, c);
                }
            }
            throw new Exception($"Character {ch} not found in matrix");
        }

        public string EncryptPair(string pair)
        {
            var p1 = FindPosition(pair[0]);
            var p2 = FindPosition(pair[1]);

            if (p1.r == p2.r)
            {
                return Matrix[p1.r, (p1.c + 1) % 5].ToString() + Matrix[p2.r, (p2.c + 1) % 5].ToString();
            }
            else if (p1.c == p2.c)
            {
                return Matrix[(p1.r + 1) % 5, p1.c].ToString() + Matrix[(p2.r + 1) % 5, p2.c].ToString();
            }
            else
            {
                return Matrix[p1.r, p2.c].ToString() + Matrix[p2.r, p1.c].ToString();
            }
        }

        public string DecryptPair(string pair)
        {
            var p1 = FindPosition(pair[0]);
            var p2 = FindPosition(pair[1]);

            if (p1.r == p2.r)
            {
                return Matrix[p1.r, (p1.c + 4) % 5].ToString() + Matrix[p2.r, (p2.c + 4) % 5].ToString();
            }
            else if (p1.c == p2.c)
            {
                return Matrix[(p1.r + 4) % 5, p1.c].ToString() + Matrix[(p2.r + 4) % 5, p2.c].ToString();
            }
            else
            {
                return Matrix[p1.r, p2.c].ToString() + Matrix[p2.r, p1.c].ToString();
            }
        }

        public string Encrypt(string plainText)
        {
            var pairs = PreparePairs(plainText);
            var sb = new StringBuilder();
            foreach (var pair in pairs)
            {
                sb.Append(EncryptPair(pair));
            }
            return sb.ToString();
        }

        public string Decrypt(string cipherText)
        {
            cipherText = AlphabetUtils.CleanString(cipherText).Replace("J", "I");
            var pairs = new List<string>();
            for (int i = 0; i < cipherText.Length; i += 2)
            {
                pairs.Add(cipherText.Substring(i, 2));
            }
            var sb = new StringBuilder();
            foreach (var pair in pairs)
            {
                sb.Append(DecryptPair(pair));
            }
            return sb.ToString();
        }
    }
}
