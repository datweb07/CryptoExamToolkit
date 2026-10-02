using System;
using System.Text;
using System.Linq;

namespace CryptoExam.Core.Common
{
    public static class HexUtils
    {
        public static string CleanHex(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            input = input.Replace("0x", "", StringComparison.OrdinalIgnoreCase).Replace(" ", "");
            return input.ToUpperInvariant();
        }

        public static string HexToBinary(string hex)
        {
            hex = CleanHex(hex);
            var sb = new StringBuilder();
            foreach (char c in hex)
            {
                sb.Append(Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));
            }
            return sb.ToString();
        }

        public static string BinaryToHex(string binary)
        {
            if (binary.Length % 4 != 0)
                throw new ArgumentException("Binary length must be a multiple of 4.");
                
            var sb = new StringBuilder();
            for (int i = 0; i < binary.Length; i += 4)
            {
                string chunk = binary.Substring(i, 4);
                sb.Append(Convert.ToInt32(chunk, 2).ToString("X"));
            }
            return sb.ToString();
        }
        
        public static byte[] HexToBytes(string hex)
        {
            hex = CleanHex(hex);
            if (hex.Length % 2 != 0) throw new ArgumentException("Hex string length must be even");
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static string BytesToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
