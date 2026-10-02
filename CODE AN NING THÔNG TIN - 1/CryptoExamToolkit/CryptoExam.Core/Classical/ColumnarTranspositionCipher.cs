using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    public static class ColumnarTranspositionCipher
    {
        public static string Encrypt(string text, string keyword)
        {
            text = AlphabetUtils.CleanString(text, true).Replace(" ", "");
            keyword = AlphabetUtils.CleanString(keyword, true).Replace(" ", "");
            if (string.IsNullOrEmpty(keyword) || string.IsNullOrEmpty(text)) return text;

            int cols = keyword.Length;
            int rows = (int)Math.Ceiling((double)text.Length / cols);
            
            char[,] grid = new char[rows, cols];
            int index = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (index < text.Length)
                        grid[r, c] = text[index++];
                    else
                        grid[r, c] = 'X'; // Pad with X
                }
            }

            var order = GetColumnOrder(keyword);
            var sb = new StringBuilder();
            foreach (var colIndex in order)
            {
                for (int r = 0; r < rows; r++)
                {
                    sb.Append(grid[r, colIndex]);
                }
            }
            return sb.ToString();
        }

        public static string Decrypt(string cipherText, string keyword)
        {
            cipherText = AlphabetUtils.CleanString(cipherText, true).Replace(" ", "");
            keyword = AlphabetUtils.CleanString(keyword, true).Replace(" ", "");
            if (string.IsNullOrEmpty(keyword) || string.IsNullOrEmpty(cipherText)) return cipherText;

            int cols = keyword.Length;
            int rows = cipherText.Length / cols;
            
            char[,] grid = new char[rows, cols];
            var order = GetColumnOrder(keyword);
            
            int index = 0;
            foreach (var colIndex in order)
            {
                for (int r = 0; r < rows; r++)
                {
                    if (index < cipherText.Length)
                        grid[r, colIndex] = cipherText[index++];
                }
            }
            
            var sb = new StringBuilder();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    sb.Append(grid[r, c]);
                }
            }
            return sb.ToString().TrimEnd('X');
        }

        private static List<int> GetColumnOrder(string keyword)
        {
            var order = new List<int>();
            var sortedChars = keyword.Select((c, i) => new { Char = c, Index = i })
                                     .OrderBy(x => x.Char)
                                     .ThenBy(x => x.Index)
                                     .ToList();
            
            foreach (var item in sortedChars)
            {
                order.Add(item.Index);
            }
            return order;
        }
    }
}
