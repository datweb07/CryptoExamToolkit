// File: CryptoExam.Core/Common/AlphabetUtils.cs
using System;

namespace CryptoExam.Core.Common
{
    /// <summary>
    /// Tiện ích cho alphabet A-Z (index 0-25).
    /// Dùng trong các cipher cổ điển.
    /// </summary>
    public static class AlphabetUtils
    {
        /// <summary>
        /// Chuyển ký tự A-Z thành index 0-25 (không phân biệt hoa/thường).
        /// </summary>
        public static int ToIndex(char c)
        {
            c = char.ToUpper(c);
            if (c < 'A' || c > 'Z')
                throw new ArgumentException($"Ký tự '{c}' không thuộc A-Z.");
            return c - 'A';
        }

        /// <summary>
        /// Chuyển index 0-25 thành ký tự 'A'-'Z'.
        /// </summary>
        public static char ToChar(int index)
        {
            return (char)('A' + ((index % 26 + 26) % 26));
        }

        /// <summary>
        /// Kiểm tra ký tự có thuộc A-Z không.
        /// </summary>
        public static bool IsLetter(char c)
        {
            return char.IsLetter(c) && char.IsAscii(c);
        }

        /// <summary>
        /// Chuẩn hóa text: uppercase, chỉ giữ A-Z (bỏ khoảng trắng, ký tự đặc biệt).
        /// </summary>
        public static string CleanUppercase(string text)
        {
            var sb = new System.Text.StringBuilder();
            foreach (char c in text.ToUpper())
                if (c >= 'A' && c <= 'Z') sb.Append(c);
            return sb.ToString();
        }

        /// <summary>
        /// Chuẩn hóa text: uppercase, giữ nguyên khoảng trắng, bỏ ký tự không phải A-Z hoặc space.
        /// </summary>
        public static string CleanPreserveSpaces(string text)
        {
            var sb = new System.Text.StringBuilder();
            foreach (char c in text.ToUpper())
                if ((c >= 'A' && c <= 'Z') || c == ' ') sb.Append(c);
            return sb.ToString();
        }
    }
}
