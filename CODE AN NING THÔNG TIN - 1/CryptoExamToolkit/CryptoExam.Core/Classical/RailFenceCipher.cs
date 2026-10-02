using System;
using System.Collections.Generic;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    public static class RailFenceCipher
    {
        public static string EncryptZigZag(string text, int rails)
        {
            text = AlphabetUtils.CleanString(text, true); // preserve spaces for some test cases, or remove depending on input
            if (rails <= 1) return text;
            
            var grid = new List<StringBuilder>();
            for (int i = 0; i < rails; i++) grid.Add(new StringBuilder());
            
            int r = 0;
            int dir = 1;
            foreach (char c in text)
            {
                grid[r].Append(c);
                r += dir;
                if (r == 0 || r == rails - 1) dir *= -1;
            }
            
            var sb = new StringBuilder();
            foreach (var row in grid) sb.Append(row.ToString());
            return sb.ToString();
        }

        public static string DecryptZigZag(string cipherText, int rails)
        {
            if (rails <= 1) return cipherText;
            
            int len = cipherText.Length;
            char[,] grid = new char[rails, len];
            for (int i = 0; i < rails; i++)
                for (int j = 0; j < len; j++)
                    grid[i, j] = '\n';
                    
            int r = 0, dir = 1;
            for (int i = 0; i < len; i++)
            {
                grid[r, i] = '*';
                r += dir;
                if (r == 0 || r == rails - 1) dir *= -1;
            }
            
            int index = 0;
            for (int i = 0; i < rails; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (grid[i, j] == '*' && index < len)
                    {
                        grid[i, j] = cipherText[index++];
                    }
                }
            }
            
            var sb = new StringBuilder();
            r = 0;
            dir = 1;
            for (int i = 0; i < len; i++)
            {
                sb.Append(grid[r, i]);
                r += dir;
                if (r == 0 || r == rails - 1) dir *= -1;
            }
            return sb.ToString();
        }
    }
}
