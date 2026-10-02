using System;
using System.Text;

namespace CryptoExam.Core.Common
{
    public static class BinaryUtils
    {
        public static string CleanBinary(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input.Replace(" ", "");
        }

        public static string Xor(string bin1, string bin2)
        {
            if (bin1.Length != bin2.Length)
                throw new ArgumentException("Binary strings must have the same length for XOR.");
                
            var sb = new StringBuilder(bin1.Length);
            for (int i = 0; i < bin1.Length; i++)
            {
                sb.Append(bin1[i] == bin2[i] ? '0' : '1');
            }
            return sb.ToString();
        }
        
        public static string ShiftLeft(string binary, int amount)
        {
            amount = amount % binary.Length;
            return binary.Substring(amount) + binary.Substring(0, amount);
        }
    }
}
