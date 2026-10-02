// File: CryptoExam.Core/Classical/ColumnarTranspositionCipher.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    /// <summary>
    /// Columnar Transposition Cipher theo keyword.
    /// Thứ tự cột = thứ tự alphabet của các ký tự keyword.
    /// Ký tự lặp trong keyword được phân biệt theo vị trí (stable sort).
    /// </summary>
    public class ColumnarTranspositionCipher
    {
        private readonly string _keyword;
        private readonly int[] _columnOrder; // columnOrder[i] = thứ tự của cột i khi đọc

        public ColumnarTranspositionCipher(string keyword)
        {
            _keyword = keyword.ToUpper().Trim();
            _columnOrder = GetColumnOrder(_keyword);
        }

        /// <summary>
        /// Tính thứ tự cột từ keyword (stable sort A-Z).
        /// Ví dụ: MONARCHY -> [4,0,5,1,3,2,7,6] (cột 4 đọc trước, rồi cột 0, ...)
        /// </summary>
        public static int[] GetColumnOrder(string keyword)
        {
            int cols = keyword.Length;
            // Tạo danh sách (ký tự, vị trí gốc) rồi sort
            var indexed = keyword.Select((c, i) => (c, i)).ToList();
            indexed.Sort((a, b) =>
            {
                int cmp = a.c.CompareTo(b.c);
                if (cmp != 0) return cmp;
                return a.i.CompareTo(b.i); // stable: nếu bằng nhau, ưu tiên vị trí nhỏ hơn
            });
            // columnOrder[position_in_sorted] = original_column_index
            int[] order = new int[cols];
            for (int i = 0; i < cols; i++)
                order[i] = indexed[i].i;
            return order;
        }

        /// <summary>
        /// Mã hóa: xếp vào hàng, đọc theo thứ tự cột của keyword.
        /// </summary>
        public string Encrypt(string plaintext, char fillChar = 'X')
        {
            string text = AlphabetUtils.CleanUppercase(plaintext);
            int cols = _keyword.Length;
            int rows = (int)Math.Ceiling((double)text.Length / cols);

            // Điền vào matrix, padding với fillChar
            char[,] matrix = new char[rows, cols];
            int idx = 0;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    matrix[r, c] = (idx < text.Length) ? text[idx++] : fillChar;

            // Đọc cột theo thứ tự keyword sort
            var sb = new StringBuilder();
            foreach (int colIdx in _columnOrder)
                for (int r = 0; r < rows; r++)
                    sb.Append(matrix[r, colIdx]);

            return sb.ToString();
        }

        /// <summary>
        /// Giải mã: tính số ký tự mỗi cột, điền ngược lại.
        /// </summary>
        public string Decrypt(string ciphertext, char fillChar = 'X')
        {
            string text = AlphabetUtils.CleanUppercase(ciphertext);
            int cols = _keyword.Length;
            int rows = (int)Math.Ceiling((double)text.Length / cols);
            int extraCols = text.Length % cols; // số cột có rows ký tự
            // Số cột đầy (rows ký tự) vs. cột ngắn (rows-1 ký tự)
            // extraCols == 0 nghĩa là tất cả cột đều có rows ký tự

            // Tính độ dài mỗi cột theo thứ tự gốc
            int[] colLengths = new int[cols];
            for (int i = 0; i < cols; i++)
            {
                // Cột nào nằm trong nhóm đầu (< extraCols theo thứ tự sorted) thì có rows ký tự
                // Nếu extraCols == 0, tất cả có rows ký tự
                int sortedPos = Array.IndexOf(_columnOrder, i);
                if (extraCols == 0 || sortedPos < extraCols)
                    colLengths[i] = rows;
                else
                    colLengths[i] = rows - 1;
            }

            // Cắt ciphertext vào từng cột
            char[][] colData = new char[cols][];
            int pos = 0;
            foreach (int colIdx in _columnOrder)
            {
                colData[colIdx] = new char[colLengths[colIdx]];
                for (int r = 0; r < colLengths[colIdx]; r++)
                    colData[colIdx][r] = text[pos++];
            }

            // Đọc lại theo hàng
            var sb = new StringBuilder();
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    if (r < colData[c].Length)
                        sb.Append(colData[c][r]);

            return sb.ToString();
        }

        /// <summary>
        /// In matrix mã hóa và thứ tự cột.
        /// </summary>
        public void PrintMatrix(string plaintext, char fillChar = 'X')
        {
            string text = AlphabetUtils.CleanUppercase(plaintext);
            int cols = _keyword.Length;
            int rows = (int)Math.Ceiling((double)text.Length / cols);

            Console.WriteLine($"  Keyword: {_keyword}");
            Console.Write("  Col order (sorted): ");
            Console.WriteLine(string.Join(" -> ", _columnOrder.Select((c, i) => $"{c + 1}")));
            Console.WriteLine();

            // Header
            Console.Write("  Key: ");
            for (int c = 0; c < cols; c++) Console.Write($"{_keyword[c]} ");
            Console.WriteLine();

            char[,] matrix = new char[rows, cols];
            int idx = 0;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    matrix[r, c] = (idx < text.Length) ? text[idx++] : fillChar;

            for (int r = 0; r < rows; r++)
            {
                Console.Write("       ");
                for (int c = 0; c < cols; c++) Console.Write($"{matrix[r, c]} ");
                Console.WriteLine();
            }
        }

        public string Keyword => _keyword;
        public int[] ColumnOrder => _columnOrder;
    }
}
