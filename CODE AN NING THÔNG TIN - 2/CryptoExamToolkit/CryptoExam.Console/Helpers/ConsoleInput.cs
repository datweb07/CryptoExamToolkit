// File: CryptoExam.Console/Helpers/ConsoleInput.cs
using System;
using System.Numerics;
using CryptoExam.Core.Common;

namespace CryptoExam.Console.Helpers
{
    /// <summary>
    /// Helper nhập liệu từ Console - chống crash, tự validate.
    /// </summary>
    public static class ConsoleInput
    {
        /// <summary>
        /// Nhập chuỗi không rỗng.
        /// </summary>
        public static string ReadString(string prompt)
        {
            while (true)
            {
                System.Console.Write(prompt);
                string? input = System.Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(input)) return input;
                System.Console.WriteLine("  [!] Không được để trống. Nhập lại.");
            }
        }

        /// <summary>
        /// Nhập số nguyên dương.
        /// </summary>
        public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                System.Console.Write(prompt);
                string? input = System.Console.ReadLine()?.Trim();
                if (int.TryParse(input, out int val) && val >= min && val <= max)
                    return val;
                System.Console.WriteLine($"  [!] Nhập số nguyên ({min} - {max}). Nhập lại.");
            }
        }

        /// <summary>
        /// Nhập BigInteger.
        /// </summary>
        public static BigInteger ReadBigInt(string prompt)
        {
            while (true)
            {
                System.Console.Write(prompt);
                string? input = System.Console.ReadLine()?.Trim();
                if (BigInteger.TryParse(input, out BigInteger val))
                    return val;
                System.Console.WriteLine("  [!] Nhập số nguyên. Nhập lại.");
            }
        }

        /// <summary>
        /// Nhập chuỗi Hex hợp lệ, normalize thành uppercase, bỏ 0x và khoảng trắng.
        /// </summary>
        public static string ReadHex(string prompt, int? requiredLength = null)
        {
            while (true)
            {
                System.Console.Write(prompt);
                string? raw = System.Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(raw))
                {
                    System.Console.WriteLine("  [!] Không được để trống.");
                    continue;
                }
                string hex = HexUtils.CleanHex(raw);
                if (!HexUtils.IsValidHex(hex))
                {
                    System.Console.WriteLine("  [!] Hex không hợp lệ. Chỉ dùng ký tự 0-9, A-F.");
                    continue;
                }
                if (requiredLength.HasValue && hex.Length != requiredLength.Value)
                {
                    System.Console.WriteLine($"  [!] Cần đúng {requiredLength.Value} ký tự hex (hiện có {hex.Length}).");
                    continue;
                }
                return hex;
            }
        }

        /// <summary>
        /// Nhập danh sách số nguyên, phân cách bằng dấu phẩy hoặc khoảng trắng.
        /// Ví dụ: "64,112,97" hoặc "64 112 97"
        /// </summary>
        public static BigInteger[] ReadBigIntList(string prompt)
        {
            while (true)
            {
                System.Console.Write(prompt);
                string? raw = System.Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(raw))
                {
                    System.Console.WriteLine("  [!] Không được để trống.");
                    continue;
                }
                string[] parts = raw.Split(new char[] { ',', ' ', ';' },
                    StringSplitOptions.RemoveEmptyEntries);
                var list = new System.Collections.Generic.List<BigInteger>();
                bool ok = true;
                foreach (string p in parts)
                {
                    if (BigInteger.TryParse(p.Trim(), out BigInteger v))
                        list.Add(v);
                    else { ok = false; break; }
                }
                if (ok && list.Count > 0) return list.ToArray();
                System.Console.WriteLine("  [!] Nhập danh sách số nguyên, cách nhau bằng dấu phẩy hoặc khoảng trắng.");
            }
        }

        /// <summary>
        /// Nhập Y/N choice.
        /// </summary>
        public static bool ReadYesNo(string prompt)
        {
            while (true)
            {
                System.Console.Write($"{prompt} (y/n): ");
                string? input = System.Console.ReadLine()?.Trim().ToLower();
                if (input == "y" || input == "yes") return true;
                if (input == "n" || input == "no") return false;
                System.Console.WriteLine("  [!] Nhập y hoặc n.");
            }
        }

        /// <summary>
        /// Đọc lựa chọn menu. Trả về -1 nếu nhập không hợp lệ.
        /// </summary>
        public static int ReadMenuChoice(string prompt, int maxOption)
        {
            while (true)
            {
                System.Console.Write(prompt);
                string? input = System.Console.ReadLine()?.Trim();
                if (int.TryParse(input, out int val) && val >= 0 && val <= maxOption)
                    return val;
                System.Console.WriteLine($"  [!] Chọn từ 0 đến {maxOption}.");
            }
        }

        /// <summary>
        /// Nhấn Enter để tiếp tục.
        /// </summary>
        public static void PressEnterToContinue()
        {
            System.Console.WriteLine("\n  [Nhấn Enter để quay lại menu...]");
            System.Console.ReadLine();
        }
    }
}
