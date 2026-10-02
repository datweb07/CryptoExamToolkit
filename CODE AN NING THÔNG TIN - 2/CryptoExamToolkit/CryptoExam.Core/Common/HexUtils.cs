// File: CryptoExam.Core/Common/HexUtils.cs
using System;
using System.Text;

namespace CryptoExam.Core.Common
{
    /// <summary>
    /// Tiện ích chuyển đổi giữa Hex, Binary, và Bytes.
    /// </summary>
    public static class HexUtils
    {
        /// <summary>
        /// Chuẩn hóa chuỗi hex: bỏ prefix 0x, bỏ khoảng trắng, uppercase.
        /// Ví dụ: "0x1334 5779" -> "13345779"
        /// </summary>
        public static string CleanHex(string hex)
        {
            if (string.IsNullOrEmpty(hex)) return "";
            hex = hex.Trim();
            if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                hex = hex.Substring(2);
            hex = hex.Replace(" ", "").Replace("-", "").ToUpperInvariant();
            return hex;
        }

        /// <summary>
        /// Chuyển chuỗi hex sang chuỗi nhị phân, giữ nguyên leading zero.
        /// Ví dụ: "0F" -> "00001111"
        /// </summary>
        public static string HexToBinary(string hex)
        {
            hex = CleanHex(hex);
            var sb = new StringBuilder(hex.Length * 4);
            foreach (char c in hex)
            {
                int val = Convert.ToInt32(c.ToString(), 16);
                // Mỗi ký tự hex = 4 bit, giữ nguyên leading zero
                sb.Append(Convert.ToString(val, 2).PadLeft(4, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Chuyển chuỗi nhị phân sang hex, giữ nguyên độ dài.
        /// Ví dụ: "00001111" -> "0F"
        /// </summary>
        public static string BinaryToHex(string binary)
        {
            binary = binary.Replace(" ", "");
            // Pad left cho đủ bội số 4
            int remainder = binary.Length % 4;
            if (remainder != 0)
                binary = binary.PadLeft(binary.Length + (4 - remainder), '0');

            var sb = new StringBuilder(binary.Length / 4);
            for (int i = 0; i < binary.Length; i += 4)
            {
                string nibble = binary.Substring(i, 4);
                int val = Convert.ToInt32(nibble, 2);
                sb.Append(val.ToString("X"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Chuyển mảng byte sang chuỗi hex uppercase.
        /// </summary>
        public static string BytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// Chuyển chuỗi hex sang mảng byte.
        /// </summary>
        public static byte[] HexToBytes(string hex)
        {
            hex = CleanHex(hex);
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Hex string phải có số ký tự chẵn.");
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải hex hợp lệ không.
        /// </summary>
        public static bool IsValidHex(string hex)
        {
            hex = CleanHex(hex);
            if (string.IsNullOrEmpty(hex)) return false;
            foreach (char c in hex)
            {
                if (!Uri.IsHexDigit(c)) return false;
            }
            return true;
        }
    }
}
