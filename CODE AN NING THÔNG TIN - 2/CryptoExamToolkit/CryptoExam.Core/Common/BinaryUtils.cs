// File: CryptoExam.Core/Common/BinaryUtils.cs
using System;
using System.Text;

namespace CryptoExam.Core.Common
{
    /// <summary>
    /// Tiện ích thao tác trên chuỗi nhị phân (string '0'/'1').
    /// </summary>
    public static class BinaryUtils
    {
        /// <summary>
        /// XOR hai chuỗi nhị phân cùng độ dài.
        /// </summary>
        public static string Xor(string a, string b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException($"XOR: độ dài không khớp ({a.Length} vs {b.Length}).");
            var sb = new StringBuilder(a.Length);
            for (int i = 0; i < a.Length; i++)
                sb.Append(a[i] == b[i] ? '0' : '1');
            return sb.ToString();
        }

        /// <summary>
        /// Dịch vòng trái (circular left shift) một chuỗi nhị phân.
        /// Dùng trong DES key schedule.
        /// </summary>
        public static string ShiftLeft(string bits, int count)
        {
            count = count % bits.Length;
            if (count == 0) return bits;
            return bits.Substring(count) + bits.Substring(0, count);
        }

        /// <summary>
        /// Chuẩn hóa chuỗi nhị phân: bỏ khoảng trắng.
        /// </summary>
        public static string Clean(string binary)
        {
            return binary.Replace(" ", "").Trim();
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải binary hợp lệ không.
        /// </summary>
        public static bool IsValid(string binary)
        {
            binary = Clean(binary);
            if (string.IsNullOrEmpty(binary)) return false;
            foreach (char c in binary)
                if (c != '0' && c != '1') return false;
            return true;
        }
    }
}
