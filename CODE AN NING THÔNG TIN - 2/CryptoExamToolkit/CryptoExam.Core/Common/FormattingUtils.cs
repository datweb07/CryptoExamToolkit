// File: CryptoExam.Core/Common/FormattingUtils.cs
using System;
using System.Text;

namespace CryptoExam.Core.Common
{
    /// <summary>
    /// Tiện ích định dạng output cho Console - rõ ràng, có label.
    /// </summary>
    public static class FormattingUtils
    {
        /// <summary>
        /// In một dòng header phân cách.
        /// </summary>
        public static void PrintHeader(string title)
        {
            string line = new string('=', 50);
            Console.WriteLine(line);
            Console.WriteLine($" {title}");
            Console.WriteLine(line);
        }

        /// <summary>
        /// In một dòng sub-header.
        /// </summary>
        public static void PrintSubHeader(string title)
        {
            Console.WriteLine($"\n--- {title} ---");
        }

        /// <summary>
        /// In key-value có padding để căn thẳng hàng.
        /// Ví dụ: PrintKV("IP(M)", "CC00CCFFF0AAF0AA", 10)
        /// </summary>
        public static void PrintKV(string key, string value, int pad = 14)
        {
            Console.WriteLine($"  {key.PadRight(pad)} = {value}");
        }

        /// <summary>
        /// In separator nhỏ.
        /// </summary>
        public static void PrintSeparator()
        {
            Console.WriteLine(new string('-', 50));
        }

        /// <summary>
        /// Định dạng binary thành nhóm 8 bit để dễ đọc.
        /// Ví dụ: "0000111100001111" -> "00001111 00001111"
        /// </summary>
        public static string FormatBinary8(string binary)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < binary.Length; i++)
            {
                if (i > 0 && i % 8 == 0) sb.Append(' ');
                sb.Append(binary[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Định dạng hex thành nhóm 4 ký tự để dễ đọc.
        /// </summary>
        public static string FormatHex4(string hex)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < hex.Length; i++)
            {
                if (i > 0 && i % 4 == 0) sb.Append(' ');
                sb.Append(hex[i]);
            }
            return sb.ToString();
        }
    }
}
